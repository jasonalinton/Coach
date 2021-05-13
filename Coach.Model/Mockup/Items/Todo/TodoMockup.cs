using Coach.Model.Items;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Model.Mockup.Items.Todo
{
    public class TodoMockup
    {
        const int _maxChildren = 3;

        public TodoMockup(List<TodoModel> todoModels)
        {
            Todos = new List<Todo>();
            InitializeTodos(todoModels);
        }

        public void InitializeTodos(List<TodoModel> todoModels)
        {
            foreach(var todoModel in todoModels)
            {
                var todo = InitializeTodo(todoModel);
                Todos.Add(todo);
            }
        }

        public Todo InitializeTodo(TodoModel todoModel)
        {
            var todo = new Todo(todoModel.Text);
            var childCount = todoModel.Children.Count();

            for (int i = 0; i < childCount || i < _maxChildren; i++)
            {
                var childTodoModel = todoModel.Children.ElementAtOrDefault(i);

                if (childTodoModel != null)
                {
                    var childTodo = InitializeTodo(childTodoModel);
                    todo.Children.Add(childTodo);

                    if (i == 0)
                        todo.Child1 = childTodo;
                    else if (i == 1)
                        todo.Child2 = childTodo;
                    else if (i == 2)
                        todo.Child3 = childTodo;
                }
                else
                    break;
            }
            return todo;
        }


        public List<Todo> Todos { get; set; }

        public class Todo
        {
            public Todo()
            {

            }

            public Todo(string text)
            {
                Text = text;
                Children = new List<Todo>();
            }

            public string Text { get; set; }
            [JsonIgnore]
            public List<Todo> Children { get; set; }
            public int ChildCount => (Children.Count() + 1 <= _maxChildren) ? Children.Count() : _maxChildren;
            
            public Todo Child1 { get; set; }
            public Todo Child2 { get; set; }
            public Todo Child3 { get; set; }
        }

    }
}
