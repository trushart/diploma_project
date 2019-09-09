using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.SchedulerData;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using TaskStatus = FIT.Diploma.Server.Database.SchedulerData.TaskStatus;


namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public class SchedulerForBwin : ITaskExecutor
    {
        private SchedulerDataRepository taskRepo;
        private LeagueDataRepository gamesRepo;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken token;
        private const string BwinGatheringExecutor = "BwinCoefGathering";
        private const int DelayMinutesLong = 180;
        private const int DelayMinutesShort = 20;

        public Task<TaskResult> Execute()
        {
            token = tokenSource.Token;
            taskRepo = new SchedulerDataRepository();
            gamesRepo = new LeagueDataRepository();

            return Task.Run(() =>
            {
                var result = new TaskResult
                {
                    StartTime = DateTime.Now
                };

                try
                {
                    var nextGame = gamesRepo.GetNextUpcomingGameDateTime();
                    var lastGathering = taskRepo.GetTimeOfLastExecution(BwinGatheringExecutor);

                    var nextGameNotExist = ConfigurationManager.AppSettings["BwinCoefGatheringDelay_NextGameNotExist"];
                    var nextGameToday = ConfigurationManager.AppSettings["BwinCoefGatheringDelay_NextGameToday"];
                    var nextGameNotToday = ConfigurationManager.AppSettings["BwinCoefGatheringDelay_NextGameNotToday"];

                    var defaultHoursDelay = string.IsNullOrEmpty(nextGameNotExist) ? -2 : -int.Parse(nextGameNotExist);

                    var defaultMinutesShortDelay = string.IsNullOrEmpty(nextGameToday) ? DelayMinutesShort : int.Parse(nextGameToday);
                    var defaultMinutesLongDelay = string.IsNullOrEmpty(nextGameNotToday) ? DelayMinutesLong : int.Parse(nextGameNotToday);


                    if (DateTime.Now.AddHours(defaultHoursDelay) > lastGathering || !lastGathering.HasValue)
                        lastGathering = DateTime.Now.AddHours(defaultHoursDelay);

                    var startTime = DateTime.Now;
                    if(nextGame.HasValue)
                    {
                            startTime = (nextGame.Value.Date == DateTime.Now.Date)
                                ? lastGathering.Value.AddMinutes(defaultMinutesShortDelay)
                                : lastGathering.Value.AddMinutes(defaultMinutesLongDelay);
                    }

                    //id could be changed, but name should be consistent
                    var taskExecutorId = taskRepo.GetTaskExecutor(BwinGatheringExecutor).TaskExecutorId;

                    SchedulerTask newTask = new SchedulerTask
                    {
                        TaskExecutorId = taskExecutorId,
                        Status = TaskStatus.Wait,
                        PlanningTime = startTime,
                        UpdateDb = null,
                        Result = null,
                        Type = TaskType.Crawler
                    };
                    result.UpdateDb = taskRepo.AddSchedulerTask(newTask);
                }
                catch (Exception ex)
                {
                    result.IsFailed = true;
                    result.FailedReason = ex.Message;
                }

                result.IsFailed = false;
                result.EndTime = DateTime.Now;
                return result;
            }, token);
        }

        public void StopExecuting()
        {
            tokenSource.Cancel();
        }
    }
}
