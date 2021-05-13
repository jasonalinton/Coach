using Coach.Data.Extension;
using Coach.Model.Items;
using Coach.Service.Items;
using Coach.Service.Items.Todo;
using Coach.Service.Logging;
using Coach.Service.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class ItemsController : BaseController
    {
        ITodoService _todoService;
        IGoalService _goalService;
        IRoutineService _routineService;
        IGoogleTaskService _googleTaskService;
        IPlannerService _plannerService;

        public ItemsController(ITodoService todoService, IGoalService goalService, IRoutineService routineService, IGoogleTaskService googleTaskService, IPlannerService plannerService)
        {
            _todoService = todoService;
            _goalService = goalService;
            _routineService = routineService;
            _googleTaskService = googleTaskService;
            _plannerService = plannerService;
        }

        // GET: Item
        public ActionResult Index()
        {
            return View();
        }

        #region Goals
        public JsonResult GetGoals()
        {
            try
            {
                var goals = _goalService.GetGoals();

                return Json(new { goals, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving goals");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddGoal(GoalModel goal)
        {
            try
            {
                var goals = _goalService.AddGoal(goal);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding goal: {goal.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddGoals(List<GoalModel> goals)
        {
            try
            {
                _goalService.AddGoals(goals);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding goals");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateGoal(GoalModel goal)
        {
            try
            {
                _goalService.UpdateGoal(goal);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating goal: {goal.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateGoals(List<GoalModel> goals)
        {
            try
            {
                _goalService.UpdateGoals(goals);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating goals");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteGoal(int ID)
        {
            try
            {
                _goalService.DeleteGoal(ID);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting goal: {ID}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteGoals(List<int> IDs)
        {
            try
            {
                _goalService.DeleteGoals(IDs);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                var errors = LogService.LogError(ex, $"Error deleting goals: {IDs.ToString()}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region Todos
        public JsonResult GetTodos()
        {
            try
            {
                var todos = _todoService.GetTodos();

                return Json(new { todos, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving todos");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddTodo(TodoModel todo)
        {
            try
            {
                var todos = _todoService.AddTodo(todo);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding todo: {todo.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult AddTodos(List<TodoModel> todos)
        {
            try
            {
                _todoService.AddTodos(todos);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error adding todos");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTodo(TodoModel todo)
        {
            try
            {
                _todoService.UpdateTodo(todo);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating todo: {todo.Text}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateTodos(List<TodoModel> todos)
        {
            try
            {
                _todoService.UpdateTodos(todos);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error updating todos");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteTodo(int ID)
        {
            try
            {
                _todoService.DeleteTodo(ID);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error deleting todo: {ID}");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteTodos(List<int> IDs)
        {
            try
            {
                _todoService.DeleteTodos(IDs);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                var errors = LogService.LogError(ex, $"Error deleting todos: {IDs.ToString()}");
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

        public async Task<JsonResult> AddTask(string title, DateTime? dueDate = null, string taskList = null)
        {
            try
            {
                await _googleTaskService.AddTask(title, dueDate, taskList);
                var tasks = await _googleTaskService.GetIncompleteTasks();

                return Json(new { tasks, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> ScheduleTodo(int idTodo, string text, DateTime scheduledDateTime, string googleTaskList = null)
        {
            try
            {
                await _plannerService.ScheduleTodo(idTodo, text, scheduledDateTime, googleTaskList);
                var todo = _todoService.GetTodo(idTodo);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> GetTasks()
        {
            try
            {
                _googleTaskService.RescheduleTasks();
                var tasks = await _googleTaskService.GetIncompleteTasks();

                return Json(new { tasks, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}