using Coach.Data.DataAccess.Briefing;
using Coach.Data.DataAccess.Items.Todo;
using Coach.Data.DataAccess.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Coach.Service.Items.Todo;

namespace Coach.Site.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
    }
}
