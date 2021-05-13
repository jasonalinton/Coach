using Coach.Service.Items;
using Coach.Service.Logging;
using Coach.Service.Planner;
using Coach.Site.Models.ViewModels.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class Planner1Controller : Controller
    {
        ITodoService _todoService;
        IPlannerService _plannerService;


        public Planner1Controller(ITodoService todoService, IPlannerService plannerService)
        {
            _todoService = todoService;
            _plannerService = plannerService;
        }

        public ActionResult Planner()
        {
            return View();
        }

        #region Todo Planner
        public ActionResult TodoPlanner()
        {
            return View();
        }

        public JsonResult GetGoogleAuthorizationUrl()
        {
            try
            {
                var googleAuthorizationURL = _plannerService.GetGoogleAuthorizationUrl();

                return Json(new { googleAuthorizationURL, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Google Authorization URL");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTodoPlannerVM(DateTime? date = null, int weekPadding = 4)
        {
            try
            {
                date = date?.Date ?? DateTime.Today;

                var days = TimelineVM.GetDays((DateTime)date, weekPadding);
                var todos = _todoService.GetTodosForDate((DateTime)date, weekPadding);
                var todoPlannerVM = new TodoPlannerVM(days, todos, (DateTime)date);

                return Json(new { todoPlannerVM, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving Events for date, {((DateTime)date).ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ScheduleTodo(int idTodo, string text, DateTime scheduledDateTime, string googleTaskList = null)
        {
            try
            {
                await _plannerService.ScheduleTodo(idTodo, text, scheduledDateTime, googleTaskList);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error scheduling Todo, {idTodo}-{text} for date, {scheduledDateTime.ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> SetTaskCompletionStatus(int idTodo, string text, DateTime scheduledDateTime, bool isComplete, string googleTaskList = null)
        {
            try
            {
                await _plannerService.SetTaskCompletionStatus(idTodo, scheduledDateTime, isComplete, googleTaskList);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error Marking Task complete, {idTodo}-{text} for date, {scheduledDateTime.ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> MarkTaskAttempted(int idTodo, DateTime scheduledDateTime, bool isAttempted, string googleTaskList = null)
        {
            try
            {
                await _plannerService.MarkTaskAttempted(idTodo, scheduledDateTime, isAttempted, googleTaskList);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error Marking Task complete, with ID:{idTodo} for date, {scheduledDateTime.ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> DeleteGoogleTaskEventTask(int idTodo, DateTime scheduledDateTime, string googleTaskList = null)
        {
            try
            {
                await _plannerService.DeleteGoogleTaskEventTask(idTodo, scheduledDateTime, googleTaskList);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error Event Task for Todo with ID {idTodo} on {scheduledDateTime.ToShortDateString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}