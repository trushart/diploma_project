using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.SchedulerData;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using TaskStatus = FIT.Diploma.Server.Database.SchedulerData.TaskStatus;

namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public class SchedulerForGames : ITaskExecutor
    {
        private SchedulerDataRepository taskRepo;
        private LeagueDataRepository gamesRepo;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken token;

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
                    var nextGameExist = ConfigurationManager.AppSettings["GameStatsGatheringDelay_NextGameExist"];
                    var nextGameNotExist = ConfigurationManager.AppSettings["GameStatsGatheringDelay_NextGameNotExist"];

                    var defaultHoursDelay_nextGameExist = string.IsNullOrEmpty(nextGameExist) ? 2 : int.Parse(nextGameExist);
                    var defaultHoursDelay_nextGameNotExist = string.IsNullOrEmpty(nextGameNotExist) ? 12 : int.Parse(nextGameNotExist);

                    var startTime = nextGame.HasValue 
                        ? nextGame.Value.AddHours(defaultHoursDelay_nextGameExist) 
                        : DateTime.Now.AddHours(defaultHoursDelay_nextGameNotExist);

                    //id could be changed, but name should be consistent
                    var taskExecutorId = taskRepo.GetTaskExecutor("GameStatsGathering").TaskExecutorId; 

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
