using FIT.Diploma.Server.DataAnalysis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public class SeasonStatsAnalyzing : ITaskExecutor
    {
        private CancellationTokenSource tokenSource = new CancellationTokenSource();
        private CancellationToken token;

        public Task<TaskResult> Execute()
        {
            token = tokenSource.Token;

            return Task.Run(() =>
            {
                var result = new TaskResult
                {
                    StartTime = DateTime.Now
                };

                try
                {
                    SeasonStatsAnalyzer analyzer = new SeasonStatsAnalyzer();
                    analyzer.RunAnalyzing();

                    StandingTableAnalyzer analyzer2 = new StandingTableAnalyzer();
                    analyzer2.RunAnalyzing();
                    result.UpdateDb = true;
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
