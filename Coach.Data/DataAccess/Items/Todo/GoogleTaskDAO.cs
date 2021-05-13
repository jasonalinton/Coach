using Coach.Model.Items.GoogleTask;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using static Coach.Data.Mappping.MyMapper;
using Coach.Data.DataAccess.Logging;
using Google.Apis.Auth.OAuth2.Requests;

namespace Coach.Data.DataAccess.Items.Todo
{
    public interface IGoogleTaskDAO
    {
        string GetGoogleAuthorizationUrl();
        Task<List<GoogleTaskModel>> GetTasks(string taskList, bool showCompleted = true, bool showHidden = true, bool showDeleted = true);
        Task<List<Google.Apis.Tasks.v1.Data.Task>> CreateExtentionJSON(List<Google.Apis.Tasks.v1.Data.Task> tasks, string taskList = null);
        void GetAllTasksInLists();
        Task<string> GetListID(string taskList = null);
        Task<GoogleTaskModel> AddTask(GoogleTaskModel googleTaskModel, string taskList);
        Task<GoogleTaskModel> GetTask(string idGoogleTask, string taskList = null);
        Task<GoogleTaskModel> UpdateTask(GoogleTaskModel task, string taskList = null);
        System.Threading.Tasks.Task DeleteTask(string idGoogleTask, string listID);
        Task<GoogleTaskModel> MarkTaskAttempted(GoogleTaskModel attemptedTask, string taskList = null);
        Task<GoogleTaskModel> MarkTaskAttempted1(GoogleTaskModel attemptedTask, string taskList = null);
        void MarkTaskComplete(GoogleTaskModel task);
        System.Threading.Tasks.Task MarkTaskComplete(Google.Apis.Tasks.v1.Data.Task task, string taskList = null);
    }

    public class GoogleTaskDAO : IGoogleTaskDAO
    {
        public UserCredential credential;
        public TasksService taskService;

        public GoogleTaskDAO()
        {
            InitializeTaskService();
        }

        public void InitializeTaskService()
        {
            this.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "687163156188-les26n01rcp6b2hhkibte1qknqit1gkd.apps.googleusercontent.com",
                    ClientSecret = "yYOgawnfQc6-w0mBHEnk2M1i"
                },
                new[] { TasksService.Scope.Tasks },
                "user",
                CancellationToken.None,
                new FileDataStore(AppDomain.CurrentDomain.BaseDirectory + "App_Data//GoogleAPI//Task")
                //,new LocalServerCodeReceiver2()
                ).Result;

            // Create the service.
            taskService = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Planner",
            });

            //// Define parameters of request.
            //TasklistsResource.ListRequest listRequest = taskService.Tasklists.List();
            //listRequest.MaxResults = 10;

            //// List task lists.
            //IList<TaskList> taskLists = listRequest.Execute().Items;
            //Console.WriteLine("Task Lists:");
            //if (taskLists != null && taskLists.Count > 0)
            //{
            //    foreach (var taskList in taskLists)
            //    {
            //        Console.WriteLine("{0} ({1})", taskList.Title, taskList.Id);
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("No task lists found.");
            //}
            ////Console.Read();

        }

        public string GetGoogleAuthorizationUrl()
        {
            var uri = new Uri("https://accounts.google.com/o/oauth2/v2/auth");
            var url = new GoogleAuthorizationCodeRequestUrl(uri)
            {
                RedirectUri = LocalServerCodeReceiver2.RedirectUriStatic,
                Scope = TasksService.Scope.Tasks,
                ClientId = "687163156188-les26n01rcp6b2hhkibte1qknqit1gkd.apps.googleusercontent.com"
            };

            return url.Build().ToString();
        }

        public async Task<List<GoogleTaskModel>> GetTasks(string taskList = null, bool showCompleted = true, bool showHidden = true, bool showDeleted = true)
        {
            string listID = await GetListID(taskList);

            /* Define parameters of request */
            TasksResource.ListRequest listRequest = taskService.Tasks.List(listID);
            listRequest.MaxResults = 100;
            listRequest.ShowCompleted = showCompleted;
            listRequest.ShowHidden = showHidden;
            listRequest.ShowDeleted = showDeleted;

            var googleTaskObject = await listRequest.ExecuteAsync();
            var tasks = googleTaskObject.Items.ToList();
            while (googleTaskObject.NextPageToken != null)
            {
                listRequest.PageToken = googleTaskObject.NextPageToken;
                googleTaskObject = await listRequest.ExecuteAsync();
                tasks.AddRange(googleTaskObject.Items);
            }

            tasks = tasks.OrderBy(x => x.Position).ToList();
            await this.CreateExtentionJSON(tasks);
            //var taskTitles = tasks.Select(x => x.Title).ToList();

            return CoachMapper.Map<List<GoogleTaskModel>>(tasks);
        }

        public async Task<List<Google.Apis.Tasks.v1.Data.Task>> CreateExtentionJSON(List<Google.Apis.Tasks.v1.Data.Task> tasks, string taskList = null)
        {
            var listID = await GetListID(taskList);

            foreach (var task in tasks)
            {
                if (task.Notes != null && task.Notes != "")
                {
                    try
                    {
                        var extentionModel = JsonConvert.DeserializeObject<GoogleTaskModel.GoogleMapExtension>(task.Notes);
                    }
                    catch
                    {
                        var extentionModel = new GoogleTaskModel.GoogleMapExtension { Note = task.Notes };
                        task.Notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                        this.UpdateTask(task, listID);
                    }
                }
                else
                {
                    var extentionModel = new GoogleTaskModel.GoogleMapExtension();
                    task.Notes = JsonConvert.SerializeObject(extentionModel, Formatting.Indented);
                    this.UpdateTask(task, listID);
                }
            }

            return tasks;
        }

        public async void GetAllTasksInLists()
        {
            var tasklists = await taskService.Tasklists.List().ExecuteAsync();
            foreach (TaskList list in tasklists.Items)
            {
                var taskss = await taskService.Tasks.List(list.Id).ExecuteAsync();
            }
        }

        /// <summary>
        /// Get ID for list
        /// </summary>
        /// <param name="taskList">Name of the Google Task list. Default: "My List"</param>
        /// <returns></returns>
        public async Task<string> GetListID(string taskList = null)
        {
            taskList = taskList ?? "My Tasks";
            var tasklists = await taskService.Tasklists.List().ExecuteAsync();

            string listID;
            try
            {
                listID = tasklists.Items.SingleOrDefault(x => x.Title == taskList).Id;
            }
            catch (ArgumentNullException ex)
            {
                throw new Exception($"Error getting ListID for list, {taskList ?? "null"}, from Google TaskAPI", ex);
            }

            return listID;
        }

        public async Task<GoogleTaskModel> AddTask(GoogleTaskModel googleTaskModel, string taskList = null)
        {
            var task = CoachMapper.Map<Google.Apis.Tasks.v1.Data.Task>(googleTaskModel);
            var listID = await GetListID(taskList);

            var insertRequest = new TasksResource.InsertRequest(taskService, task, listID);
            task = await insertRequest.ExecuteAsync();

            return CoachMapper.Map<GoogleTaskModel>(task);
        }

        private async Task<Google.Apis.Tasks.v1.Data.Task> AddTask(Google.Apis.Tasks.v1.Data.Task task, string listID)
        {
            var insertRequest = new TasksResource.InsertRequest(taskService, task, listID);
            return await insertRequest.ExecuteAsync();
        }

        public async Task<GoogleTaskModel> GetTask(string idGoogleTask, string taskList = null)
        {
            var listID = await GetListID(taskList);

            var getRequest = new TasksResource.GetRequest(taskService, listID, idGoogleTask);
            var task = await getRequest.ExecuteAsync();

            return CoachMapper.Map<GoogleTaskModel>(task);
        }

        public async Task<GoogleTaskModel> UpdateTask(GoogleTaskModel task, string taskList = null)
        {
            var googleTaskModel = CoachMapper.Map<Google.Apis.Tasks.v1.Data.Task>(task);
            var listID = await GetListID(taskList);
            var googleTask = await UpdateTask(googleTaskModel, listID);

            return CoachMapper.Map<GoogleTaskModel>(googleTask);
        }

        private async Task<Google.Apis.Tasks.v1.Data.Task> UpdateTask(Google.Apis.Tasks.v1.Data.Task task, string listID)
        {
            var updateRequest = new TasksResource.UpdateRequest(taskService, task, listID, task.Id);
            return await updateRequest.ExecuteAsync();
        }

        public async System.Threading.Tasks.Task DeleteTask(string idGoogleTask, string taskList = null)
        {
            var listID = await GetListID(taskList);
            var deleteRequest = new TasksResource.DeleteRequest(taskService, listID, idGoogleTask);
            await deleteRequest.ExecuteAsync();
        }

        public async Task<GoogleTaskModel> MarkTaskAttempted(GoogleTaskModel attemptedTask, string taskList = null)
        {
            //attemptedTask.WasAttempted = false;
            var googleTask = CoachMapper.Map<Google.Apis.Tasks.v1.Data.Task>(attemptedTask);

            /* Clone attempted task */
            var taskJSON = JsonConvert.SerializeObject(googleTask);
            var clone = JsonConvert.DeserializeObject<Google.Apis.Tasks.v1.Data.Task>(taskJSON);
            clone.Id = null;
            clone.Due = clone.Due?.AddDays(1);
            clone.Status = "needsAction";
            if (attemptedTask.NewName != null) clone.Title = attemptedTask.NewName;

            googleTask.Title = "[Attempted] " + attemptedTask.Title; // Add [Attempted] to title
            await this.MarkTaskComplete(googleTask); // Mark attempted task as complete

            var listID = await GetListID(taskList);
            clone = await this.AddTask(clone, listID); // Add clone to Google Task list
            clone.Position = attemptedTask.Position;
            UpdateTask(clone, listID); // Set clone to the same position as the updated task

            return CoachMapper.Map<GoogleTaskModel>(clone);
        }

        public async Task<GoogleTaskModel> MarkTaskAttempted1(GoogleTaskModel attemptedTask, string listID)
        {
            attemptedTask.WasAttempted = false;
            var googleTask = CoachMapper.Map<Google.Apis.Tasks.v1.Data.Task>(attemptedTask);

            /* Clone attempted task */
            var taskJSON = JsonConvert.SerializeObject(googleTask);
            var clone = JsonConvert.DeserializeObject<Google.Apis.Tasks.v1.Data.Task>(taskJSON);
            clone.Id = null;
            clone.Status = "needsAction";
            if (attemptedTask.NewName != null) clone.Title = attemptedTask.NewName;

            googleTask.Title = "[Attempted] " + attemptedTask.Title; // Add [Attempted] to title
            await this.MarkTaskComplete(googleTask); // Mark attempted task as complete

            clone = await this.AddTask(clone, listID);
            clone.Position = attemptedTask.Position;
            UpdateTask(clone, listID); // Set clone to the same position as the updated task

            return CoachMapper.Map<GoogleTaskModel>(clone);
        }

        public async void MarkTaskComplete(GoogleTaskModel task)
        {
            var googleTask = CoachMapper.Map<Google.Apis.Tasks.v1.Data.Task>(task);
            await MarkTaskComplete(googleTask);
        }

        public async System.Threading.Tasks.Task MarkTaskComplete(Google.Apis.Tasks.v1.Data.Task task, string taskList = null)
        {
            task.Status = "completed";
            var listID = await GetListID(taskList);

            await this.UpdateTask(task, listID);
        }
    }
}
