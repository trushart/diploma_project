using System.Threading.Tasks;

namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public interface ITaskExecutor
    {
        Task<TaskResult> Execute();
        void StopExecuting();
    }
}
