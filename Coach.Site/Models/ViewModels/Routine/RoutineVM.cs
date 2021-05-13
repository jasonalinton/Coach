using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Models.ViewModels.Routine
{
    public class RoutineVM
    {
        public RoutineVM(RoutineModel routineModel, DateTime dateTime)
        {
            Routine = new Routine(routineModel, dateTime);
        }
        public Routine Routine { get; set; }
    }

    public class Routine
    {
        public Routine()
        {
            Todos = new List<Todo>();
            TodoGroups = new List<TodoGroup>();
        }

        public Routine(RoutineModel routineModel, DateTime dateTime)
            : this()
        {
            ID = routineModel.ID;
            Text = routineModel.Text;

            /* Add Todos */
            foreach (var todoModel in routineModel.Todos.Where(x => x.IsGroup == 0))
            {
                var todo = new Todo(routineModel, todoModel, dateTime);
                Todos.Add(todo);
            }
            Todos = Todos.OrderBy(x => x.Position).ToList();

            CreateTodoGroups(Todos);
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public string RoutineCompetionFractionString => 
            $"{Todos.Count(x => x.IsComplete)}/{Todos.Count()}";
        public string RoutineCompetionPercentageString => 
            $"{(int)((double)Todos.Count(x => x.IsComplete) / Todos.Count() * 100)}%";
        public List<Todo> Todos { get; set; }
        public List<TodoGroup> TodoGroups { get; set; }

        public void CreateTodoGroups(List<Todo> todos)
        {
            var todoGroup = new TodoGroup();

            foreach (var todo in todos.Where(x => x.IsGroup == false))
            {
                if (todo.ParentText != null && todoGroup.Name != todo.ParentText)
                {
                    todoGroup.TotalCompleted = Todos.Count((x => x.ParentText == todoGroup.Name && x.IsComplete == true));
                    todoGroup.TodoCount = Todos.Count((x => x.ParentText == todoGroup.Name));
                    todoGroup = new TodoGroup(todo.ParentText);
                    TodoGroups.Add(todoGroup);
                }
                else if (todo.ParentText == null)
                {
                    todoGroup = new TodoGroup("");
                    TodoGroups.Add(todoGroup);
                }
                todoGroup.Todos.Add(todo);
            }
        }

        public override string ToString()
        {
            return $"Routine: {Text}";
        }
    }

    public class TodoGroup
    {
        public TodoGroup()
        {
            Todos = new List<Todo>();
        }
        public TodoGroup(string name)
            : this()
        {
            Name = name;
        }

        public int TotalCompleted { get; set; }
        public int TodoCount { get; set; }
        public string Name { get; set; }
        public string CompletionString
        {
            get
            {
                return $"{TotalCompleted}/{TodoCount}";
            }
        }
        public List<Todo> Todos { get; set; }
    }

    public class Todo
    {
        public Todo(RoutineModel routineModel, TodoModel todoModel, DateTime dateTime)
        {
            ID = todoModel.ID;
            Position = todoModel.RoutinePosition;
            Text = todoModel.Text;
            IsGroup = Convert.ToBoolean(todoModel.IsGroup);

            var parentTodo = todoModel.Parents.SingleOrDefault(x => routineModel.Todos.Where(y => x.IsGroup == 1).Contains(x));
            ParentID = parentTodo.ID;
            ParentText = parentTodo.Text;

            EventTask = new EventTask(todoModel, dateTime);
        }

        public int ID { get; set; }
        public int ParentID { get; set; }
        public int? Position { get; set; }
        public string Text { get; set; }
        public string ParentText { get; set; }
        public string LineItemID
        {
            get
            {
                return $"todo-{ID}";
            }
        }
        public string SwitchID
        {
            get
            {
                return $"todo-{ID}-switch";
            }
        }
        public bool? IsGroup { get; set; }
        public bool IsComplete
        {
            get { return EventTask.IsComplete; }
        }
        public string CheckedProperty
        {
            get
            {
                if (IsComplete)
                    return "checked=checked";
                else
                    return String.Empty;
            }
        }
        public EventTask EventTask { get; set; }

        public override string ToString()
        {
            return $"Todo: {this.Text}";
        }

    }

    public class EventTask
    {
        public EventTask(TodoModel todoModel, DateTime dateTime)
        {
            var eventTaskModel = todoModel.EventTasks.SingleOrDefault(x => x.StartDateTime.Date == dateTime.Date);

            ID = eventTaskModel.ID;
            Text = eventTaskModel.Text;
            IsComplete = Convert.ToBoolean(eventTaskModel.IsComplete);
            StartDateTime = eventTaskModel.StartDateTime;
            EndDateTime = eventTaskModel.EndDateTime;
            DateTimeCompleted = eventTaskModel.DateTimeCompleted;
        }

        public int ID { get; set; }
        public string Text { get; set; }
        public bool IsComplete {get;set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime DateTimeCompleted { get; set; }

        public override string ToString()
        {
            return $"EventTask: {this.Text}";
        }
    }
}