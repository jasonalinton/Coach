using Coach.Service.Logging;
using Coach.Service.Planner;
using Coach.Site.Models.ViewModels.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class MobilePlannerController : Controller
    {
        IMobilePlannerService _mobilePlannerService;

        public MobilePlannerController(IMobilePlannerService mobilePlannerService)
        {
            _mobilePlannerService = mobilePlannerService;
        }

        public ActionResult Todo()
        {
            return View();
        }

        public JsonResult GetEventsForDate(DateTime date, int weekPadding = 4)
        {
            try
            {
                var todos =_mobilePlannerService.GetTodosForDate(date, weekPadding);
                var todoVM = new TodoPlannerVM(todos);

                return Json(new { todoVM, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Events for date, {date.ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}