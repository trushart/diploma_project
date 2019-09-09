using FIT.Diploma.Server.Database.SystemData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT.Diploma.Server.DataAccess.Repositories
{
    public class LogRepository : BaseRepository
    {
        public void WriteLog(Severity severity, string message, string requestAddr, string IP_addr, string data,
            string application, string memberName = "", string sourceProcessId = "")
        {
            Db.Database.Connection.Open();

            Log log = new Log()
            {
                Severity = severity,
                Message = message,
                RequestAddress = requestAddr,
                IP = IP_addr,
                CallerMemberName = memberName,
                CallerProcessId = sourceProcessId,
                Data = DateTime.Now.ToString("yyyyMMdd HHmmss") + " | " + data,
                Application = application
            };

            Db.Log.Add(log);
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }

        public void WriteExceptionLog(Exception ex, string memberName = "", string sourceProcessId = "")
        {
            Db.Database.Connection.Open();

            Log log = new Log()
            {
                Severity = Severity.Error,
                Message = ex.Message,
                RequestAddress = ex.Source,
                IP = "localhost",
                CallerMemberName = memberName,
                CallerProcessId = sourceProcessId,
                Data = DateTime.Now.ToString("yyyyMMdd HHmmss") + " | " + ex.StackTrace
            };

            Db.Log.Add(log);
            Db.SaveChanges();

            Db.Database.Connection.Close();
        }
    }
}
