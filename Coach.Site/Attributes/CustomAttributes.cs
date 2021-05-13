using Coach.Service.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Coach.Site.Attributes
{
    public class RequestAttributeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var endpoint = filterContext.HttpContext.Request.Path;
            LogService.LogInfo("Endpoint Hit: " + endpoint, -1, String.Empty, String.Empty, String.Empty);

            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;

            /* Log erros that haven't been logged yet */
            if (httpContext.Items.Contains("QueuedExceptions"))
            {
                httpContext.Response.StatusCode = 218;

                foreach (var ex in (List<Exception>)httpContext.Items["QueuedExceptions"])
                {
                    var logMessage = (ex.Data != null && ex.Data["logMessage"] != null) ? (string)ex.Data["logMessage"] : "Unhandled exception";
                    var logErrors = LogService.LogError(ex, logMessage);
                }
                ((List<Exception>)httpContext.Items["QueuedExceptions"]).Clear();
            }

            base.OnResultExecuting(filterContext);
        }

        //public void TestExceptions()
        //{
        //    var ex2 = new Exception("Test all error exception");
        //    ex2.Data.Add("logMessage", "Error getting goals");
        //    LogService.AddQueriedLogError(ex2);
        //    var ex = new Exception("This is a test exception in the get goals area");
        //    throw ex;
        //}
    }

    public class HandleExceptionAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext exceptionContext)
        {
            var httpContext = exceptionContext.HttpContext;

            var logErrors = LogService.LogError(exceptionContext.Exception, "Unhandled Exception");
            var errors = new List<string>(logErrors);

            /* Log erros that haven't been logged yet */
            if (httpContext.Items.Contains("QueuedExceptions"))
            {
                foreach (var ex in (List<Exception>)httpContext.Items["QueuedExceptions"])
                {
                    var logMessage = (ex.Data != null && ex.Data["logMessage"] != null) ? (string)ex.Data["logMessage"] : "Unhandled exception";
                    logErrors = LogService.LogError(ex, logMessage);
                    errors.AddRange(logErrors);
                }
                ((List<Exception>)httpContext.Items["QueuedExceptions"]).Clear();
            }

            exceptionContext.ExceptionHandled = true;
            exceptionContext.Result = new JsonResult { Data = new { success = false, errors }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}