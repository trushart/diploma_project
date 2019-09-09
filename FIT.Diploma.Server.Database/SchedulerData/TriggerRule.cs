using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SchedulerData
{
    public class TriggerRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RuleId { get; set; }

        [ForeignKey("FinishedExecutor")]
        public int FinishedExecutorId { get; set; }

        [ForeignKey("ToStartExecutor")]
        public int ToStartExecutorId { get; set; }

        public int DelayTime { get; set; }

        public bool CheckConfigs { get; set; }

        public bool? WhenFinishedExecutorUpdateDb { get; set; }

        public virtual TaskExecutor FinishedExecutor { get; set; }

        public virtual TaskExecutor ToStartExecutor { get; set; }
    }
}
