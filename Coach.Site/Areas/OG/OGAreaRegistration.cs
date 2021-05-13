using System.Web.Mvc;

namespace Coach.Site.Areas.OG
{
    public class OGAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OG";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OG_default",
                "OG/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}