using Coach.Data.DataAccess.Items;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Planner
{
    public interface IMobilePlannerService
    {
        List<TodoModel> GetTodosForDate(DateTime date, int weekPadding = 4);
    }

    public class MobilePlannerService : IMobilePlannerService
    {
        ITodoDAO _todoDAO;

        public MobilePlannerService(ITodoDAO todoDAO)
        {
            _todoDAO = todoDAO;
        }

        public List<TodoModel> GetTodosForDate(DateTime date, int weekPadding = 4)
        {
            var todos = _todoDAO.GetActiveTodos(date, weekPadding);
            return todos;
        }
    }
}
