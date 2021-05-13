using Coach.Model.Briefing;
using Coach.Service.Briefing;
using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Briefing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class BriefingController : BaseController
    {
        IBriefingService _briefingService;

        public BriefingController(IBriefingService briefingService)
        {
            _briefingService = briefingService;
        }

        public ActionResult Briefing()
        {
            return View();
        }

        public JsonResult GetBriefingsForDate(DateTime date)
        {
            try
            {
                var briefing = _briefingService.GetBriefingForDate(date);
                var briefingViewModel = new BriefingVM(briefing, date);

                return Json(new { BriefingViewModel = briefingViewModel, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving briefings");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddBriefing(BriefingModel briefing)
        {
            try
            {
                _briefingService.AddBriefing(briefing);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving briefings");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}