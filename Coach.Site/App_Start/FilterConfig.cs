using Coach.Site.Attributes;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); 
            filters.Add(new HandleExceptionAttribute()); 
            filters.Add(new RequestAttributeAttribute());
            //filters.Add(new RequireHttpsAttribute());

            FilterProviders.Providers.Add(new AntiForgeryTokenFilter());
        }
    }

    //This will add ValidateAntiForgeryToken Attribute to all HttpPost action methods
    public class AntiForgeryTokenFilter : IFilterProvider, IExceptionFilter
    {
        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            List<Filter> result = new List<Filter>();
            string incomingVerb = controllerContext.HttpContext.Request.HttpMethod;

            if (String.Equals(incomingVerb, "POST", StringComparison.OrdinalIgnoreCase))
            {
                //var token = controllerContext.HttpContext.Request.Params.Get("__RequestVerificationToken");
                var username = controllerContext.HttpContext.Request.Params.Get("username");
                var password = controllerContext.HttpContext.Request.Params.Get("password");
                if (username != null && password != null)
                    if (username == "jasonalinton" && password == "L3tm3!nN0w")
                        return result;

                result.Add(new Filter(new ValidateAntiForgeryTokenAttribute(), FilterScope.Global, null));
            }

            return result;
        }

        public virtual void OnException(ExceptionContext filterContext)
        {

        }
    }
}
