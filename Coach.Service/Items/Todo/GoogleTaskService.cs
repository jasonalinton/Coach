using Coach.Data.DataAccess.Items;
using Coach.Data.DataAccess.Items.Todo;
using Coach.Model.Items.GoogleTask;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Items.Todo
{
    public interface IGoogleTaskService
    {
        string GetGoogleAuthorizationUrl();
        Task<List<GoogleTaskModel>> GetTasks(string taskList = null);
        Task<List<GoogleTaskModel>> GetIncompleteTasks(string taskList = null);
        Task<GoogleTaskModel> AddTask(string title, DateTime? dueDate, string taskList = null);
        Task UpdateTask(GoogleTaskModel googleTaskModel, string taskList = null);
        bool SyncGoogleTaskWithEventTask(int idTodo, GoogleTaskModel googleTaskModel);
        Task SetTaskCompletionStatus(int idTodo, DateTime dueDate, bool isComplete, string taskList = null);
        Task<GoogleTaskModel> MarkTaskAttempted(int idTodo, DateTime dueDate, bool isComplete, string taskList = null);
        Task DeleteTask(string idGoogleTask, string taskList = null);
        void RescheduleTasks(int completionDateCusion = 5, int numberOfTaskPerDay = 3, string taskList = null);
    }

    public class GoogleTaskService : IGoogleTaskService
    {
        GoogleTaskDAO _googleTaskDAO;
        TodoDAO _todoDAO;

        public GoogleTaskService(GoogleTaskDAO googleTaskDAO, TodoDAO todoDAO)
        {
            _googleTaskDAO = googleTaskDAO;
            _todoDAO = todoDAO;
        }

        public string GetGoogleAuthorizationUrl()
        {
            return _googleTaskDAO.GetGoogleAuthorizationUrl();
        }

        public async Task<List<GoogleTaskModel>> GetTasks(string taskList = null)
        {
            return await _googleTaskDAO.GetTasks(taskList);
        }

        public async Task<List<GoogleTaskModel>> GetIncompleteTasks(string taskList = null)
        {
            return await _googleTaskDAO.GetTasks(taskList, showCompleted: false, showDeleted: false);
        }

        public async Task<GoogleTaskModel> AddTask(string title, DateTime? dueDate, string taskList = null)
        {
            var googleTaskModel = new GoogleTaskModel(title, dueDate);
            return await _googleTaskDAO.AddTask(googleTaskModel, taskList);
        }

        public async Task UpdateTask(GoogleTaskModel googleTaskModel, string taskList = null)
        {
            await _googleTaskDAO.UpdateTask(googleTaskModel, taskList);
        }

        public bool SyncGoogleTaskWithEventTask(int idTodo, GoogleTaskModel googleTaskModel)
        {
            return _todoDAO.SyncGoogleTaskWithEventTask(idTodo, googleTaskModel);
        }

        public async Task SetTaskCompletionStatus(int idTodo, DateTime dueDate, bool isComplete, string taskList = null)
        {
            /* Get Google Task ID so that you can get the Google Task Model */
            var idGoogleTask = _todoDAO.GetGoogleTaskIDForTodo(idTodo, dueDate);
            GoogleTaskModel googleTaskModel;
            if (idGoogleTask != null)
            {
                /* Get Google Task and Set to Complete */
                googleTaskModel = await _googleTaskDAO.GetTask(idGoogleTask, taskList);
                googleTaskModel.IsComplete = isComplete;

                /* Update Google Task to Mark Complete */
                googleTaskModel = await _googleTaskDAO.UpdateTask(googleTaskModel, taskList);
                
                /* Syncronize Google Task with Event Task on Server */
                _todoDAO.SyncGoogleTaskWithEventTask(idTodo, googleTaskModel);

            }
        }

        public async Task<GoogleTaskModel> MarkTaskAttempted(int idTodo, DateTime dueDate, bool isComplete, string taskList = null)
        {
            /* Get Google Task ID so that you can get the Google Task Model */
            var idGoogleTask = _todoDAO.GetGoogleTaskIDForTodo(idTodo, dueDate);
            GoogleTaskModel attemptedGoogleTaskModel = null;
            GoogleTaskModel newGoogleTaskModel = null;
            if (idGoogleTask != null)
            {
                /* Get Google Task and Set to Complete */
                attemptedGoogleTaskModel = await _googleTaskDAO.GetTask(idGoogleTask, taskList);
                newGoogleTaskModel = await _googleTaskDAO.MarkTaskAttempted(attemptedGoogleTaskModel, taskList);

                /* Syncronize Attempted Google Task with Event Task on Server */
                _todoDAO.SyncGoogleTaskWithEventTask(idTodo, attemptedGoogleTaskModel, true);
                /* Schedule Google Task with Event Task on Server */
                _todoDAO.ScheduleTodo(idTodo, (DateTime)newGoogleTaskModel.ScheduledDate, newGoogleTaskModel.Id);
            }
            return newGoogleTaskModel;
        }

        public async Task DeleteTask(string idGoogleTask, string taskList = null)
        {
            await _googleTaskDAO.DeleteTask(idGoogleTask, taskList);
        }

        /* 
         * 1. If a task is marked attempted, mark it complete, add "[Attempted]" tag, clone, add clone to queue
         * 2. Order by tasks that will be due within the completion date cusion then order the rest by position
         */
        public async void RescheduleTasks(int completionDateCusion = 5, int numberOfTaskPerDay = 3, string taskList = null)
        {
            var tasks = await this.GetIncompleteTasks(); // Get all incomplete tasks
            var listID = await _googleTaskDAO.GetListID(taskList);

            /* Recreate attempted tasks (foreach attempted Task) */
            var attemptedTasks = tasks.Where(x => x.WasAttempted == true).ToList();
            foreach (var attemptedTask in attemptedTasks)
            {
                var clone = await _googleTaskDAO.MarkTaskAttempted1(attemptedTask, listID);
                tasks[tasks.IndexOf(attemptedTask)] = clone; // Replace attemped task with its clone
            }

            var dueSoon = tasks.Where(x => x.CompletionDate != null && x.CompletionDate < DateTime.Now.AddDays(completionDateCusion))
                .OrderBy(x => x.CompletionDate).ToList(); // Get tasks who have a deadline, due date, or recommended date within {(int)CompletionDateCushin} days
            var orderedTasks = new List<GoogleTaskModel>(dueSoon);

            tasks.OrderBy(x => x.Position);
            orderedTasks.AddRange(tasks.Where(x => !dueSoon.Contains(x)));

            var date = DateTime.Now.AddDays(-1).Date;
            for (int i = 0; i < orderedTasks.Count; i++)
            {
                var orderedTask = orderedTasks[i];
                var remain = i % numberOfTaskPerDay;
                if (remain == 0)
                    date = date.AddDays(1);

                orderedTask.Due = DateTime.Parse(date.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ssZ"));
                orderedTask.DueRaw = null;
                _googleTaskDAO.UpdateTask(orderedTasks[i]);
            }
        }
    }
}
