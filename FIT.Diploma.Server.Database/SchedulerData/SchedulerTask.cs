using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SchedulerData
{
    public class SchedulerTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [ForeignKey("TaskExecutor")]
        public int TaskExecutorId { get; set; }

        public TaskType Type { get; set; }

        public DateTime PlanningTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public TaskStatus Status { get; set; }

        public TaskResult? Result { get; set; }

        public string FailReason { get; set; }

        public bool? UpdateDb { get; set; }

        public virtual TaskExecutor TaskExecutor { get; set; }

        public void CopuFrom(SchedulerTask task)
        {
            Type = task.Type;
            StartTime = task.StartTime;
            EndTime = task.EndTime;
            Status = task.Status;
            Result = task.Result;
            FailReason = task.FailReason;
            UpdateDb = task.UpdateDb;
        }
    }

    public enum TaskType
    {
        Analyzer,
        Crawler,
        Helper
    }

    public enum TaskStatus
    {
        Wait,
        InProgress,
        Finished
    }

    public enum TaskResult
    {
        Succeed,
        Failed
    }
}
