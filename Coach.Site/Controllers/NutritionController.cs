using Coach.Site.Areas.OG.Services;
using Coach.Site.Models.ViewModels.Nutrition;
using Coach.ServiceOG._Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coach.Service.Logging;

namespace Coach.Site.Controllers
{
    public class NutritionController : Controller
    {
        // GET: Nutrition
        public ActionResult Nutrition()
        {
            return View();
        }

        public JsonResult GetNutritionVM(DateTime? startDate = null, DateTime? endDate = null, DateTime? currentDate = null)
        {
            try
            {
                startDate = startDate?.Date;
                if (currentDate == null && startDate != null)
                    currentDate = startDate;

                var PhysicalService = new PhysicalService();
                var MealCharts = PhysicalService.GetMealCharts(startDate, endDate);

                var NutritionVM = new NutritionVM(MealCharts.Meals, startDate, endDate, currentDate);

                return Json(new { NutritionVM, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error getting Nutrition View Model");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMeals(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                    StartDT = DateTime.Parse(startDTString);
                if (endDTString != null)
                    EndDT = DateTime.Parse(endDTString);

                var PhysicalService = new PhysicalService();
                var Meals = PhysicalService.GetMeals(StartDT, EndDT);

                return Json(new { Meals, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error getting Meals");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetMealCharts(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                    StartDT = DateTime.Parse(startDTString);
                if (endDTString != null)
                    EndDT = DateTime.Parse(endDTString);

                var PhysicalService = new PhysicalService();
                var MealChart = PhysicalService.GetMealCharts(StartDT, EndDT);

                return Json(new { MealChart, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error getting Meal Charts");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}