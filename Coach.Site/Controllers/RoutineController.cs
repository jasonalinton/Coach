using Coach.Model.Items;
using Coach.Service.Items;
using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Routine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class RoutineController : BaseController
    {
        IRoutineService _routineService;

        public RoutineController(IRoutineService routineService)
        {
            _routineService = routineService;
        }
        
        public ActionResult Index()
        {
            return View();
        }

        #region Routine View
        public ActionResult Routine()
        {
            return View();
        }

        public JsonResult GetRoutineForDate(int routineID, DateTime date)
        {
            try
            {
                _routineService.CreateEventTasksForRoutine(routineID, date);
                var routine = _routineService.GetRoutineForDate(date);
                var routineViewModel = new RoutineVM(routine, date);

                return Json(new { RoutineViewModel = routineViewModel, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving routines");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ReorderTodo(int routineID, int todoID, int newPosition)
        {
            try
            {
                _routineService.ReorderTodo(routineID, todoID, newPosition);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving routines");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Routines
        public JsonResult GetRoutines()
        {
            try
            {
                var routines = _routineService.GetRoutines();

                return Json(new { routines, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving routines");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddRoutine(RoutineModel routine)
        {
            try
            {
                var routines = _routineService.AddRoutine(routine);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding routine: {routine.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddRoutines(List<RoutineModel> routines)
        {
            try
            {
                _routineService.AddRoutines(routines);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding routines");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRoutine(RoutineModel routine)
        {
            try
            {
                _routineService.UpdateRoutine(routine);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating routine: {routine.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateRoutines(List<RoutineModel> routines)
        {
            try
            {
                _routineService.UpdateRoutines(routines);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating routines");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRoutine(int ID)
        {
            try
            {
                _routineService.DeleteRoutine(ID);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting routine: {ID}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRoutines(List<int> IDs)
        {
            try
            {
                _routineService.DeleteRoutines(IDs);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                var errors = LogService.LogError(ex, $"Error deleting routines: {IDs.ToString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}