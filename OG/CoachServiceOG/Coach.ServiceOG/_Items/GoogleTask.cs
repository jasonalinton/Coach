using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Tasks.v1;
using Google.Apis.Tasks.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Coach.ServiceOG._Items
{
    public class GoogleTask
    {
        public UserCredential credential;
        public TasksService taskService;

        public GoogleTask()
        {
            this.credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "810156924312-btrvmj2ksfqnt0t215egler7tbinn8cm.apps.googleusercontent.com",
                    ClientSecret = "jA1oyEsrY2cPi3xyN7U6yeMN",
                },
                new[] { TasksService.Scope.Tasks },
                "user",
                CancellationToken.None).Result;

            // Create the service.
            taskService = new TasksService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Planner",
            });

            // Define parameters of request.
            TasklistsResource.ListRequest listRequest = taskService.Tasklists.List();
            listRequest.MaxResults = 10;

            // List task lists.
            IList<TaskList> taskLists = listRequest.Execute().Items;
            Console.WriteLine("Task Lists:");
            if (taskLists != null && taskLists.Count > 0)
            {
                foreach (var taskList in taskLists)
                {
                    Console.WriteLine("{0} ({1})", taskList.Title, taskList.Id);
                }
            }
            else
            {
                Console.WriteLine("No task lists found.");
            }
            //Console.Read();

            var tasks = taskService.Tasks.List("My Tasks");

        }

        public async void GetTasks()
        {
            var mylistID = "MDQ0MTI0ODk4OTk2NTY0MDYzMDI6MDow";
            var tasks = await taskService.Tasks.List(mylistID).ExecuteAsync();

            var tasklists = await taskService.Tasklists.List().ExecuteAsync();
            foreach (TaskList list in tasklists.Items)
            {
                var taskss = await taskService.Tasks.List(list.Id).ExecuteAsync();
            }

            var lists = tasks.Items.Select(x => x.Title).ToList();

        }
    }
}
