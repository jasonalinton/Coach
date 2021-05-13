using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Coach.ServiceOG._Planner;
using Coach.Site.Areas.OG.Services;
using CoachModel._App._Task;
using CoachModel._Planner;
using CoachModel._ViewModel._Planner;

namespace Coach.Site.Areas.OG.Controllers
{
    public class PlannerController : Controller
    {
        // GET: Planner
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Planner()
        {
            return View();
        }

        public JsonResult GetPlannerViewModel()
        {
            try
            {
                var PlannerService = new PlannerService();
                PlannerVM PlannerVM = PlannerService.GetPlannerViewModel();

                return new CustomJsonResult()
                {
                    Data = new { model = PlannerVM, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult RefreshTasks(string startDateString, string endDateString)
        {
            try
            {
                DateTime StartDate = (startDateString != null) ? DateTime.Parse(startDateString) : new DateTime(2018, 11, 27);
                DateTime EndDate = (endDateString != null) ? DateTime.Parse(endDateString) : new DateTime(2019, 1, 31);

                var PlannerService = new PlannerService();
                PlannerService.RefreshTasks(StartDate, EndDate, true);

                return new CustomJsonResult()
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*--------------------------------------------- Get Items --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public JsonResult GetInventoryItems()
        {
            try
            {
                var PlannerService = new PlannerService();
                var InventoryItems = PlannerService.GetInventoryItems();

                return new CustomJsonResult()
                {
                    Data = new { InventoryItems = InventoryItems, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetGoals()
        {
            try
            {
                var PlannerService = new PlannerService();
                var Goals = PlannerService.GetGoals();

                return new CustomJsonResult()
                {
                    Data = new { Goals = Goals, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetTodoItems(bool isActive = true)
        {
            try
            {
                var PlannerService = new PlannerService();
                var TodoItems = PlannerService.GetTodoItems(isActive);

                return new CustomJsonResult()
                {
                    Data = new { TodoItems = TodoItems, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetRoutines()
        {
            try
            {
                var PlannerService = new PlannerService();
                var Routines = PlannerService.GetRoutines();

                return new CustomJsonResult()
                {
                    Data = new { Routines = Routines, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetTasks(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                {
                    StartDT = DateTime.Parse(startDTString);
                }
                if (endDTString != null)
                {
                    EndDT = DateTime.Parse(endDTString);
                }

                var PlannerService = new PlannerService();
                var Tasks = PlannerService.GetTasks(StartDT, EndDT);

                return new CustomJsonResult()
                {
                    Data = new { Tasks = Tasks, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetEvents(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                {
                    StartDT = DateTime.Parse(startDTString);
                }
                if (endDTString != null)
                {
                    EndDT = DateTime.Parse(endDTString);
                }

                var PlannerService = new PlannerService();
                var Events = PlannerService.GetEvents(StartDT, EndDT);

                return new CustomJsonResult()
                {
                    Data = new { Events = Events, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*------------------------------------------ Create Items --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public JsonResult CreateGoals(List<Goal> Goals)
        {
            try
            {
                var PlannerService = new PlannerService();
                //PlannerService.CreateGoals(Goals);

                return Json(new { goals = Goals, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult CreateTodoItems(List<TodoItem> TodoItems)
        {
            try
            {
                var PlannerService = new PlannerService();
                //PlannerService.CreateTodoItems(TodoItems);

                return Json(new { todoItems = TodoItems, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult CreateRoutines(List<Routine> Routines)
        {
            try
            {
                var PlannerService = new PlannerService();
                //PlannerService.CreateRoutines(Routines);

                return Json(new { routines = Routines, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult CreateTasks(List<Task> Tasks)
        {
            try
            {
                var PlannerService = new PlannerService();
                //PlannerService.CreateTasks(Tasks);

                return Json(new { tasks = Tasks, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult CreateEvents(List<Event> Events)
        {
            try
            {
                var PlannerService = new PlannerService();
                PlannerService.CreateEvents_Errands(Events, true);

                return Json(new { events = Events, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*------------------------------------------ Update Items --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public JsonResult UpdateEvents(List<Event> Events)
        {
            try
            {
                var PlannerService = new PlannerService();
                PlannerService.UpdateEvents(Events);

                return Json(new { events = Events, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*------------------------------------------ Delete Items --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public JsonResult DeleteEvents(List<Event> Events)
        {
            try
            {
                var PlannerService = new PlannerService();
                PlannerService.DeleteEvents(Events);

                return Json(new { events = Events, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult DeleteTasks(List<int> TaskIDs)
        {
            try
            {
                var PlannerService = new PlannerService();
                PlannerService.DeleteTasks(TaskIDs);

                return Json(new { tasks = TaskIDs, success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public ActionResult SetupMonth()

        {
            return View();
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------- Tasks ----------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public JsonResult MarkTaskCompletion(Task Task)
        {
            try
            {
                var PlannerService = new PlannerService();
                var Events_Task = PlannerService.MarkTaskCompletion(Task);

                return Json(new { success = true, Events_Task }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        //public JsonResult CatchControllerError1(Exception ex)
        //{
        //    var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ErrorLog.txt";
        //    var errorString = System.IO.File.ReadAllText(path);

        //    errorString += "\r\n";
        //    errorString += ex.Message;
        //    if (ex.InnerException != null)
        //        errorString += ex.InnerException.Message;

        //    System.IO.File.WriteAllText(path, errorString);

        //    return Json(new { success = false, error = ex.Message }, "application/json", JsonRequestBehavior.AllowGet);
        //}

        public JsonResult CatchControllerError(Exception ex)
        {
            Temp.SaveExceptionToDatabase(ex);

            return Json(new { success = false, error = ex.Message }, "application/json", JsonRequestBehavior.AllowGet);
        }
    }
}