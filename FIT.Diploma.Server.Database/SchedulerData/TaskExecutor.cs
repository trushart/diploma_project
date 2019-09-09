using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SchedulerData
{
    public class TaskExecutor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskExecutorId { get; set; }

        public string Name { get; set; }

        public string CodeClass { get; set; }

        public string Description { get; set; }

        public int TimeOut { get; set; }

        public int MaxRetries { get; set; }
    }
}
