using Coach.Data.DataAccess.Items;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Items
{
    public interface ITodoService
    {
        TodoModel GetTodo(int idTodo);
        List<TodoModel> GetTodos();
        List<TodoModel> GetTodosForDate(DateTime? date = null, int weekPadding = 4);
        int AddTodo(TodoModel todo);
        List<int> AddTodos(List<TodoModel> todos);
        TodoModel UpdateTodo(TodoModel todo);
        List<TodoModel> UpdateTodos(List<TodoModel> todos);
        void DeleteTodo(int todoID);
        void DeleteTodos(List<int> todoIDs);

        #region Planner
        void SetTodoCompletion(int idEventTask, bool isComplete);
        int HardCodeTimeFor_SUMMERMOVEOUT_Goals();
        #endregion
    }

    public class TodoService : ITodoService
    {
        ITodoDAO _todoDAO;

        public TodoService(ITodoDAO todoDAO)
        {
            _todoDAO = todoDAO;
        }

        #region CRUD
        public TodoModel GetTodo(int idTodo)
        {
            return _todoDAO.GetTodo(idTodo);
        }

        public List<TodoModel> GetTodos()
        {
            return _todoDAO.GetTodos();
        }

        public List<TodoModel> GetTodosForDate(DateTime? date = null, int weekPadding = 4)
        {
            date = date?.Date ?? DateTime.Today;

            var todos = _todoDAO.GetActiveTodos((DateTime)date, weekPadding);
            return todos;
        }

        public int AddTodo(TodoModel todo)
        {
            return _todoDAO.AddTodo(todo);
        }

        public List<int> AddTodos(List<TodoModel> todos)
        {
            return _todoDAO.AddTodos(todos);
        }

        public TodoModel UpdateTodo(TodoModel todo)
        {
            return _todoDAO.UpdateTodo(todo);
        }

        public List<TodoModel> UpdateTodos(List<TodoModel> todos)
        {
            return _todoDAO.UpdateTodos(todos);
        }

        public void DeleteTodo(int todoID)
        {
            _todoDAO.DeleteTodo(todoID);
        }

        public void DeleteTodos(List<int> todoIDs)
        {
            _todoDAO.DeleteTodos(todoIDs);
        }
        #endregion

        #region Planner
        public void SetTodoCompletion(int idEventTask, bool isComplete)
        {
            _todoDAO.SetTodoCompletion(idEventTask, isComplete);
        }

        public int HardCodeTimeFor_SUMMERMOVEOUT_Goals()
        {
            return _todoDAO.HardCodeTimeFor_SUMMERMOVEOUT_Goals();
        }
        #endregion
    }
}
