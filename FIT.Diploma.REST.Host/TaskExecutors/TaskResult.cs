using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.REST.Host.TaskExecutors
{
    public class TaskResult
    {
        public bool UpdateDb { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsFailed { get; set; }

        public string FailedReason { get; set; }
    }
}
