using Coach.Model.Mockup.Items.Todo;
using Coach.Service.Items;
using Coach.Service.Logging;
using Coach.Site.Models.ViewModels.Planner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Coach.Site.Controllers
{
    public class TodoController : BaseController
    {
        ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

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

        public JsonResult GetTodoMockupModel()
        {
            try
            {
                var todoModels = _todoService.GetTodos();
                var todoMockup = new TodoMockup(todoModels);

                return Json(new { todoMockup, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error retrieving todos");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Planner

        public JsonResult SetTodoCompletion(int idEventTask, bool isComplete)
        {
            try
            {
                _todoService.SetTodoCompletion(idEventTask, isComplete);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errors = LogService.LogError(ex, $"Error setting Todo completion");
                return Json(new { success = false, errors }, JsonRequestBehavior.AllowGet);
            }

            #endregion
        }
    }
}