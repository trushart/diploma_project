using FIT.Diploma.REST.Host.TaskExecutors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.REST.Host.Helpers
{
    public static class TaskExtensions
    {
        //method for run task with timeout
        public static async Task<TaskResult> TimeoutAfter(this Task<TaskResult> task, int delay, ITaskExecutor executor)
        {
            try
            {
                await Task.WhenAny(task, Task.Delay(delay * 1000));

                if (!task.IsCompleted || task.IsCanceled)
                {
                    executor.StopExecuting();
                    throw new TimeoutException();
                }
                var result = await task;
                return result;
            }
            catch (Exception ex)
            {
                return new TaskResult()
                {
                    IsFailed = true,
                    FailedReason = ex.Message,
                    UpdateDb = false
                };
            }            
        }
    }
}
