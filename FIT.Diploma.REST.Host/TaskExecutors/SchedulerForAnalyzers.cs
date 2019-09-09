using FIT.Diploma.Server.DataAccess.Repositories;
using FIT.Diploma.Server.Database.SchedulerData;
using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using TaskStatus = FIT.Diploma.Server.Database.SchedulerData.TaskStatus;

namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public class SchedulerForAnalyzers : ITaskExecutor
    {
        private SchedulerDataRepository taskRepo;
        private LeagueDataRepository gamesRepo;
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken token;
        private const string BookmakerOddsStatsAnalyzingExecutor = "BookmakerOddsStatsAnalyzing";
        private const string FootballTeamFormAnalyzingExecutor = "FootballTeamFormAnalyzing";
        private const string GamePredictionAnalyzingExecutor = "GamePredictionAnalyzing";
        private const string HeadToHeadStatsAnalyzingExecutor = "HeadToHeadStatsAnalyzing";
        private const string RoundStatsAnalyzingExecutor = "RoundStatsAnalyzing";
        private const string SeasonStatsAnalyzingExecutor = "SeasonStatsAnalyzing";

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
                    result.UpdateDb = taskRepo.AddSchedulerTask(AddTaskForAnalyzer(BookmakerOddsStatsAnalyzingExecutor, -1, 60));
                    result.UpdateDb |= taskRepo.AddSchedulerTask(AddTaskForAnalyzer(FootballTeamFormAnalyzingExecutor, -1, 60));
                    result.UpdateDb |= taskRepo.AddSchedulerTask(AddTaskForAnalyzer(HeadToHeadStatsAnalyzingExecutor, -1, 65));                   
                    result.UpdateDb |= taskRepo.AddSchedulerTask(AddTaskForAnalyzer(RoundStatsAnalyzingExecutor, -1, 70));
                    result.UpdateDb |= taskRepo.AddSchedulerTask(AddTaskForAnalyzer(SeasonStatsAnalyzingExecutor, -1, 70));
                    result.UpdateDb |= taskRepo.AddSchedulerTask(AddTaskForAnalyzer(GamePredictionAnalyzingExecutor, -1, 90));
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

        private SchedulerTask AddTaskForAnalyzer(string analyzerName, int defaultHoursDelay, int defaultMinutesDelay)
        {
            var lastGathering = taskRepo.GetTimeOfLastExecution(analyzerName);

            if (DateTime.Now.AddHours(defaultHoursDelay) > lastGathering || !lastGathering.HasValue)
                lastGathering = DateTime.Now.AddHours(defaultHoursDelay);

            var startTime = lastGathering.Value.AddMinutes(defaultMinutesDelay);

            //id could be changed, but name should be consistent
            var taskExecutorId = taskRepo.GetTaskExecutor(analyzerName).TaskExecutorId;

            SchedulerTask newTask = new SchedulerTask
            {
                TaskExecutorId = taskExecutorId,
                Status = TaskStatus.Wait,
                PlanningTime = startTime,
                UpdateDb = null,
                Result = null,
                Type = TaskType.Analyzer
            };

            return newTask;
        }

        public void StopExecuting()
        {
            tokenSource.Cancel();
        }
    }
}
