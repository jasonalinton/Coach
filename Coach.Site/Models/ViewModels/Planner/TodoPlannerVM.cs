using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Models.ViewModels.Planner
{
    public class TodoPlannerVM
    {

        public TodoPlannerVM(List<TodoModel> todoModels)
        {
            foreach (var todoModel in todoModels)
                Todos.Add(new Todo(todoModel));

            foreach (var todoModel in todoModels.Where(x => x.EventTasks.Count == 0))
                UnplannerTodos.Add(new Todo(todoModel));

            foreach (var todoModel in todoModels.Where(x => x.Parents.Count == 0))
                TodoHierarchy.Add(new Todo(todoModel));
        }

        public TodoPlannerVM(List<Day> days, List<TodoModel> todoModels, DateTime currentDate)
            : this(todoModels)
        {
            foreach (var day in days)
            {
                var tasks = todoModels.Where(x => x.EventTasks.Exists(y => y.idGoogleTask != null && y.StartDateTime == day.Date)).ToList();
                Days.Add(new TodoDay(day.Date.Date, currentDate.Date, tasks));
            }
        }

        public List<Todo> Todos { get; set; } = new List<Todo>();
        public List<Todo> UnplannerTodos { get; set; } = new List<Todo>();
        public List<Todo> TodoHierarchy { get; set; } = new List<Todo>();
        public List<Goal> Goals => Todos.SelectMany(x => x.Goals).Distinct().ToList();
        public List<TodoDay> Days { get; set; } = new List<TodoDay>();
    }

    public class TodoDay : Day
    {
        public TodoDay(DateTime date, DateTime currentDate, List<TodoModel> tasks)
            : base(date, currentDate)
        {
            foreach (var task in tasks)
                Tasks.Add(new Task(task, date));
        }

        public List<Task> Tasks { get; set; } = new List<Task>();
    }

    public class Todo
    {
        public Todo(TodoModel todoModel)
        {
            ID = todoModel.ID;
            Text = todoModel.Text;
            HasGoogleTask = todoModel.EventTasks.Exists(x => x.idGoogleTask != null);
            IsComplete = Convert.ToBoolean(todoModel.EventTasks.Exists(x => x.idGoogleTask != null && x.IsComplete == 1));

            foreach (var goal in todoModel.Goals)
                Goals.Add(new Goal(goal.ID, goal.Text));

            foreach (var childTodoModel in todoModel.Children)
                Children.Add(new Todo(childTodoModel));
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public List<Goal> Goals { get; set; } = new List<Goal>();
        public List<Todo> Children { get; set; } = new List<Todo>();
        public bool HasGoogleTask { get; set; }
        public bool IsComplete { get; set; }
        //public string Class => (HasGoogleTask) ? "scheduled" : "";
        public string Class
        {
            get
            {
                var classNames = String.Empty;

                if (HasGoogleTask)
                    classNames += "scheduled ";
                if (IsComplete)
                    classNames += "complete";

                return classNames;
            }
        }

        public override string ToString() => $"Todo: {Text}";
    }

    public class Goal : IEquatable<Goal>
    {
        public Goal(int iD, string text)
        {
            ID = iD;
            Text = text;
        }

        public int ID { get; set; }
        public String Text { get; set; }

        public bool Equals(Goal other)
        {
            if (Object.ReferenceEquals(other, null)) return false; //Check whether the compared object is null.
            if (Object.ReferenceEquals(this, other)) return true; //Check whether the compared object references the same data.
            return ID == other.ID; //Check whether the products' properties are equal.
        }
        public override int GetHashCode() => ID.GetHashCode();

        public override string ToString() => $"Goal: {Text}";
    }

    public class Task
    {
        public Task(TodoModel task, DateTime scheduledDateTime)
        {
            var eventTask = task.EventTasks.FirstOrDefault(x => x.StartDateTime == scheduledDateTime);

            ID = task.ID;
            Text = eventTask.Text;
            ScheduledDateTime = scheduledDateTime;
            IsComplete = Convert.ToBoolean(eventTask.IsComplete);
            IsAttempted = Convert.ToBoolean(eventTask.IsAttempted);
        }

        public Task(int iD, string text, DateTime scheduledDateTime)
        {
            ID = iD;
            Text = text;
            ScheduledDateTime = scheduledDateTime;
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime ScheduledDateTime { get; set; }
        public bool IsComplete { get; set; }
        public bool IsAttempted { get; set; }
        public string Class
        {
            get
            {
                if (IsComplete)
                    return "complete";
                if (IsAttempted)
                    return "attempted";
                else
                    return "";
            }
        }
    }
}