using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class TimelineController : Controller
    {
        // GET: Timeline
        public ActionResult Timeline()
        {
            return View();
        }

        public JsonResult GetTimelineVM(DateTime date, int padding = 4)
        {
            try
            {
                var timelineVM = new TimelineVM(date, padding);

                return Json(new { timelineVM, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Timeline View Model");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}