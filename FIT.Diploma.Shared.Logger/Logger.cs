using FIT.Diploma.Shared.DataAccess;
using FIT.Diploma.Shared.Database.Models;
using System;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;

namespace FIT.Diploma.Shared.Logger
{
    public class Logger
    {
        #region Singleton
        private static volatile Logger currentInstance;
        private static object syncRoot = new Object();

        public static Logger Instance
        {
            get
            {
                if (currentInstance == null)
                {
                    lock (syncRoot)
                    {
                        if (currentInstance == null)
                            currentInstance = new Logger();
                    }
                }

                return currentInstance;
            }
        }
        #endregion

        private static object fileLock = new Object();

        public void WriteErrorLog(string message, string user, string requestAddr, string IPAddress, string app, string siteUrl, string data = "",
            [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFilePath = "")
        {
            WriteLog(SeverityEnum.Error, message, user, requestAddr, IPAddress, app, siteUrl, data, callerMethod, callerFilePath);
        }

        public void WriteVerboseLog(string message, string user, string requestAddr, string IPAddress, string app, string siteUrl, string data = "",
            [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFilePath = "")
        {
            WriteLog(SeverityEnum.Verbose, message, user, requestAddr, IPAddress, app, siteUrl, data, callerMethod, callerFilePath);
        }

        public void WriteInfoLog(string message, string user, string requestAddr, string IPAddress, string app, string siteUrl, string data = "",
            [CallerMemberName] string callerMethod = "", [CallerFilePath] string callerFilePath = "")
        {
            WriteLog(SeverityEnum.Information, message, user, requestAddr, IPAddress, app, siteUrl, data, callerMethod, callerFilePath);
        }

        private void WriteLog(SeverityEnum severity, string logMessage, string user, string requestAddr, string IPAddress, string app, string siteUrl, string data, string callerMemberName = "", string callerFilePath = "")
        {
            //Task.Run(() =>{});

            if (int.Parse(ConfigurationManager.AppSettings["LogSeverity"]) <= (int)severity)
            {
                bool _logFile = bool.Parse(ConfigurationManager.AppSettings["LogFileEnabled"]);

                //string logfilePath = siteUrl; 
                string logfilePath = ConfigurationManager.AppSettings["LogFilePath"];
                string fileName = DateTime.Today.ToString("yyyyMMdd") + ".txt";
                string filePath = Path.Combine(@logfilePath, fileName);

                if (!File.Exists(filePath))
                {
                    lock (fileLock)
                    {
                        System.IO.File.AppendAllText(filePath, "Date"
                           + " | Error message"
                           + " | User"
                           + " | Request address"
                           + " | IP address"
                           + " | App"
                           + " | Data"
                           + " | Caller Member Name"
                           + " | Caller File Path"
                           + "|" + Environment.NewLine + Environment.NewLine);
                    }

                }

                if (_logFile)
                {
                    lock (fileLock)
                    {
                        System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                        + "|" + logMessage
                        + "|" + user
                        + "|" + requestAddr
                        + "|" + IPAddress
                        + "|" + app
                        + "|" + data
                        + "|" + callerMemberName
                        + "|" + callerFilePath
                        + "|" + Environment.NewLine);
                    }
                }

                try
                {
                    var repository = new LogRepository();

                    repository.WriteExceptionLog(severity, logMessage, user, requestAddr, IPAddress, data, app, callerMemberName, callerFilePath);

                }
                catch (Exception ex)
                {
                    //to do log
                    lock (fileLock)
                    {
                        System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                        + "|Log to DB exception"
                        + "|" + ex.Message
                        + "|" + user
                        + "|" + requestAddr
                        + "|" + IPAddress
                        + "|" + app
                        + "|" + ex.StackTrace
                        + "|" + callerMemberName
                        + "|" + callerFilePath
                        + "|" + Environment.NewLine);


                        try
                        {

                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                             + "|WindowsIdentity" + System.Security.Principal.WindowsIdentity.GetCurrent().Name
                                             + "|" + user
                                             + "|" + requestAddr
                                             + "|" + IPAddress
                                             + "|" + app
                                             + "|" + callerMemberName
                                             + "|" + callerFilePath
                                             + "|" + Environment.NewLine);

                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                           + "|HttpContext User" + HttpContext.Current.User.Identity.Name
                                           + "|" + user
                                           + "|" + requestAddr
                                           + "|" + IPAddress
                                           + "|" + app
                                           + "|" + callerMemberName
                                           + "|" + callerFilePath
                                           + "|" + Environment.NewLine);
                        }
                        catch
                        { }

                        try
                        {
                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                             + "|Try Impersonate with HttpContext User: " + System.Web.HttpContext.Current.User.Identity.Name
                                             + "|" + user
                                             + "|" + requestAddr
                                             + "|" + IPAddress
                                             + "|" + app
                                             + "|" + callerMemberName
                                             + "|" + callerFilePath
                                             + "|" + Environment.NewLine);

                            using (((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).Impersonate())
                            {
                                var repository = new LogRepository();
                                repository.WriteExceptionLog(severity, logMessage, user, requestAddr, IPAddress, data, app, callerMemberName, callerFilePath);
                            }

                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                 + "|OK: Try Impersonate with HttpContext User: " + System.Web.HttpContext.Current.User.Identity.Name
                                 + "|" + user
                                 + "|" + requestAddr
                                 + "|" + IPAddress
                                 + "|" + app
                                 + "|" + callerMemberName
                                 + "|" + callerFilePath
                                 + "|" + Environment.NewLine);
                        }
                        catch (Exception ex2)
                        {
                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                               + "|ERROR: Try Impersonate with HttpContext User: " + System.Web.HttpContext.Current.User.Identity.Name
                               + "|" + ex2.Message
                               + "|" + user
                               + "|" + requestAddr
                               + "|" + IPAddress
                               + "|" + app
                               + "|" + ex2.StackTrace
                               + "|" + callerMemberName
                               + "|" + callerFilePath
                               + "|" + Environment.NewLine);
                        }


                        try
                        {
                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                + "|Try Impersonate with WindowsIdentity: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name
                                + "|" + user
                                + "|" + requestAddr
                                + "|" + IPAddress
                                + "|" + app
                                + "|" + callerMemberName
                                + "|" + callerFilePath
                                + "|" + Environment.NewLine);

                            using (((System.Security.Principal.WindowsIdentity)System.Security.Principal.WindowsIdentity.GetCurrent()).Impersonate())
                            {
                                var repository = new LogRepository();
                                repository.WriteExceptionLog(severity, logMessage, user, requestAddr, IPAddress, data, app, callerMemberName, callerFilePath);
                            }

                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                + "|OK: Try Impersonate with WindowsIdentity: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name
                                + "|" + user
                                + "|" + requestAddr
                                + "|" + IPAddress
                                + "|" + app
                                + "|" + callerMemberName
                                + "|" + callerFilePath
                                + "|" + Environment.NewLine);
                        }
                        catch (Exception ex3)
                        {
                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                               + "|ERROR: Try Impersonate with WindowsIdentity: " + System.Security.Principal.WindowsIdentity.GetCurrent().Name
                               + "|" + ex3.Message
                               + "|" + user
                               + "|" + requestAddr
                               + "|" + IPAddress
                               + "|" + app
                               + "|" + ex3.StackTrace
                               + "|" + callerMemberName
                               + "|" + callerFilePath
                               + "|" + Environment.NewLine);
                        }

                        try
                        {
                            bool willFlowAcrossNetwork = false;
                            foreach (System.Security.Principal.SecurityIdentifier s in System.Security.Principal.WindowsIdentity.GetCurrent().Groups)
                            {
                                if (s.IsWellKnown(System.Security.Principal.WellKnownSidType.InteractiveSid))
                                {
                                    willFlowAcrossNetwork = true;
                                    return;
                                }
                                if (s.IsWellKnown(System.Security.Principal.WellKnownSidType.BatchSid))
                                {
                                    willFlowAcrossNetwork = true;
                                    return;
                                }
                                if (s.IsWellKnown(System.Security.Principal.WellKnownSidType.ServiceSid))
                                {
                                    willFlowAcrossNetwork = true;
                                    return;
                                }
                            }

                            System.IO.File.AppendAllText(filePath, DateTime.Now.ToString("yyyyMMddHHmmss")
                                             + "|WillFlowAcrossNetwork" + willFlowAcrossNetwork
                                             + "|" + user
                                             + "|" + requestAddr
                                             + "|" + IPAddress
                                             + "|" + app
                                             + "|" + callerMemberName
                                             + "|" + callerFilePath
                                             + "|" + Environment.NewLine);
                        }
                        catch
                        {

                        }
                    }
                }
            }
        }
    }
}
