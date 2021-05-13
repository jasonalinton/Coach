using Coach.Service.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Modules
{
    public class LoggingModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            //context.EndRequest += Context_EndRequest;
            //context.Error += OnUnhandleError_Error;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {

        }

        private void OnUnhandleError_Error(object sender, EventArgs e)
        {

        }

        public void Dispose()
        {

        }

    }
}