using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class UtilitiesController : BaseController
    {
        // GET: Utilities
        public ActionResult ExcelToJSON()
        {
            return View();
        }
    }
}