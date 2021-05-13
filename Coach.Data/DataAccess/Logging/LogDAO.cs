using Coach.Data.Extension;
using Coach.Data.Model;
using Coach.Model.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Coach.Data.DataAccess.Logging
{
    public class LogDAO
    {
        public static void LogInfo(EventLogModel logModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var eventLog = new eventlog
                {
                    logMessage = logModel.LogMessage,
                    dataKey = logModel.DataKey,
                    dataTable = logModel.DataTable,
                    dataValue = logModel.DataValue,
                    crud = logModel.CRUD,
                    endpoint = (logModel.Endpoint == null && HttpContext.Current != null) ? HttpContext.Current.Request.Path : logModel.Endpoint,
                    username = (logModel.Username == null && HttpContext.Current != null) ? HttpContext.Current.User.Identity.Name : "jasonalinton",
                    machineName = Environment.MachineName,
                    eventLevel = "Info"
                };

                entities.eventlogs.Add(eventLog);
                entities.SaveChanges(false);
            }
        }
        public static void LogInfo(string message, int dataKey, string dataTable, string dataValue, string crud, string endpoint = null, string username = null)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var eventLog = new eventlog
                {
                    logMessage = message,
                    dataKey = dataKey,
                    dataTable = dataTable,
                    dataValue = dataValue,
                    crud = crud,
                    endpoint = (endpoint == null && HttpContext.Current != null) ? HttpContext.Current.Request.Path : endpoint,
                    username = (username == null && HttpContext.Current != null) ? HttpContext.Current.User.Identity.Name : "jasonalinton",
                    machineName = Environment.MachineName,
                    eventLevel = "Info"
                };

                entities.eventlogs.Add(eventLog);
                entities.SaveChanges(false);
            }
        }

        public static string[] LogError(Exception ex, string message, string endpoint = null, string username = null)
        {
            try
            {
                using (coachdevEntities entities = new coachdevEntities())
                {
                    var errorLog = new errorlog
                    {
                        logMessage = message,
                        errorSource = ex.Source,
                        errorMessage = ex.GetFullMessageString(),
                        stackTrace = ex.GetFullStackTraceString(),
                        endpoint = (endpoint == null && HttpContext.Current != null) ? HttpContext.Current.Request.Path : endpoint,
                        username = (username == null && HttpContext.Current != null) ? HttpContext.Current.User.Identity.Name : "jasonalinton",
                        machineName = Environment.MachineName,
                        eventLevel = "Error"
                    };

                    entities.errorlogs.Add(errorLog);
                    entities.SaveChanges(false);
                }

                return new string[]
                {
                    message,
                    ex.GetFullMessageString()
                };
            }
            catch (Exception ex2)
            {

                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ErrorLog.txt";
                var errorString = String.Empty;

                errorString += "\r\n";
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += DateTime.Now.ToLongDateString();
                errorString += "\r\n";
                errorString += DateTime.Now.ToLongTimeString();
                errorString += "\r\n";
                errorString += ex2.GetFullMessageString();
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += ex2.GetFullStackTraceString();

                if (File.Exists(path))
                    errorString += System.IO.File.ReadAllText(path);

                System.IO.File.WriteAllText(path, errorString);

                return new string[]
                {
                    message,
                    ex2.GetFullMessageString()
                };
            }
        }

        public static string[] LogError(string message, string errorSource, string errorMessage, string stackTrace, string endpoint = null, string username = null)
        {
            try
            {
                using (coachdevEntities entities = new coachdevEntities())
                {
                    var errorLog = new errorlog
                    {
                        logMessage = message,
                        errorSource = errorSource,
                        errorMessage = errorMessage,
                        stackTrace = stackTrace,
                        endpoint = (endpoint == null && HttpContext.Current != null) ? HttpContext.Current.Request.Path : endpoint,
                        username = (username == null && HttpContext.Current != null) ? HttpContext.Current.User.Identity.Name : "jasonalinton",
                        machineName = Environment.MachineName,
                        eventLevel = "Error"
                    };

                    entities.errorlogs.Add(errorLog);
                    entities.SaveChanges(false);
                }

                return new string[]
                {
                    message,
                    errorMessage
                };
            }
            catch (Exception ex2)
            {
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ErrorLog.txt";
                var errorString = String.Empty;

                errorString += "\r\n";
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += DateTime.Now.ToLongDateString();
                errorString += "\r\n";
                errorString += DateTime.Now.ToLongTimeString();
                errorString += "\r\n";
                errorString += ex2.GetFullMessageString();
                errorString += "\r\n";
                errorString += "\r\n";
                errorString += ex2.GetFullStackTraceString();

                if (File.Exists(path))
                    errorString += System.IO.File.ReadAllText(path);

                System.IO.File.WriteAllText(path, errorString);

                return new string[]
                {
                    message,
                    ex2.GetFullMessageString()
                };
            }
        }

        public static void AddQueuedLogError(Exception ex)
        {
            if (HttpContext.Current != null)
            {
                var httpContext = HttpContext.Current;

                if (httpContext.Items.Contains("QueuedExceptions"))
                    (httpContext.Items["QueuedExceptions"] as List<Exception>).Add(ex);
                else
                    httpContext.Items.Add("QueuedExceptions", new List<Exception> { ex });
            }
        }
    }
}
