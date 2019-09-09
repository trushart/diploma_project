using FIT.Diploma.Shared.Database.Models;
using System;

namespace FIT.Diploma.Shared.DataAccess
{
    public class LogRepository : BaseRepository
    {
        //Constructor - initialize _severity from app.config
        public LogRepository()
        {
        }


        public void WriteExceptionLog(SeverityEnum severity, string message, string user, string requestAddr, string IP_addr, string data,
            string application, string memberName = "", string sourceFilePath = "")
        {
            Db.Database.Connection.Open();

            Log log = new Log()
            {
                Severity = severity,
                Message = message,
                User = user,
                RequestAddress = requestAddr,
                IP = IP_addr,
                CallerMemberName = memberName,
                CallerFilePath = sourceFilePath,
                Data = DateTime.Now.ToString("yyyyMMdd HHmmss") + " | " + data,
                Application = application
            };

            Db.Logs.Add(log);
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }
    }
}
