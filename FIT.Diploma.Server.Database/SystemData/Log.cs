using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIT.Diploma.Server.Database.SystemData
{
    public class Log
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LogId { get; set; }

        public Severity Severity { get; set; }

        public string Message { get; set; }

        public string RequestAddress { get; set; }

        public string IP { get; set; }

        public string CallerMemberName { get; set; }

        public string CallerProcessId { get; set; }

        public string Data { get; set; }

        public string Application { get; set; }

        //public DateTime DateCreated { get; set; }

        //for log File
        public string GetText()
        {
            return "[Severity: " + Severity + "] "
                + "[Message: " + Message + "] "
                + "[RequestAddress: " + RequestAddress + "] "
                + "[IP: " + IP + "] "
                + "[CallerMemberName: " + CallerMemberName + "] "
                + "[CallerProcessId: " + CallerProcessId + "] "
                + "[Data: " + Data + "] "
                + "[Application: " + Application + "]\n";
        }
    }

    public enum Severity
    {
        Verbose = 0,
        Information = 1,
        Warning = 2,
        Error = 3,
        Fatal = 4
    }
}
