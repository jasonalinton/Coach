using Coach.Data.DataAccess.Items;
using Coach.Model.Items.GoogleTask;
using Coach.Service.Items.Todo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Planner
{
    public interface IPlannerService
    {
        string GetGoogleAuthorizationUrl();
        Task ScheduleTodo(int idTodo, string text, DateTime scheduledDateTime, string googleTaskList = null);
        Task SetTaskCompletionStatus(int idTodo, DateTime dueDate, bool isComplete, string taskList = null);
        Task<GoogleTaskModel> MarkTaskAttempted(int idTodo, DateTime dueDate, bool isComplete, string taskList = null);
        Task DeleteGoogleTaskEventTask(int idTodo, DateTime datetime, string googleTaskList = null);
    }
    public class PlannerService : IPlannerService
    {
        ITodoDAO _todoDAO;
        IGoogleTaskService _googleTaskService;

        public PlannerService(ITodoDAO todoDAO, IGoogleTaskService googleTaskService)
        {
            _todoDAO = todoDAO;
            _googleTaskService = googleTaskService;
        }

        public string GetGoogleAuthorizationUrl()
        {
            return _googleTaskService.GetGoogleAuthorizationUrl();
        }

        public async Task ScheduleTodo(int idTodo, string text, DateTime scheduledDateTime, string googleTaskList = null)
        {
            var googleTaskID = _todoDAO.GetGoogleTaskIDForTodo(idTodo, scheduledDateTime);
            if (googleTaskID == null)
            {
                var googleTask = await _googleTaskService.AddTask(text, scheduledDateTime, googleTaskList);
                googleTaskID = googleTask.Id;
            }
            else
            {
                var googleTaskModel = new GoogleTaskModel(googleTaskID, text, scheduledDateTime);
                await _googleTaskService.UpdateTask(googleTaskModel);
            }

            _todoDAO.ScheduleTodo(idTodo, scheduledDateTime, googleTaskID);
        }

        public async Task SetTaskCompletionStatus(int idTodo, DateTime dueDate, bool isComplete, string taskList = null)
        {
            await _googleTaskService.SetTaskCompletionStatus(idTodo, dueDate, isComplete, taskList);
        }

        public async Task<GoogleTaskModel> MarkTaskAttempted(int idTodo, DateTime dueDate, bool isComplete, string taskList = null)
        {
            return await _googleTaskService.MarkTaskAttempted(idTodo, dueDate, isComplete, taskList);
        }

        public async Task DeleteGoogleTaskEventTask(int idTodo, DateTime datetime, string googleTaskList = null)
        {
            var idGoogleTask = _todoDAO.DeleteGoogleTaskEventTask(idTodo, datetime);
            await _googleTaskService.DeleteTask(idGoogleTask, googleTaskList);
        }
    }
}
