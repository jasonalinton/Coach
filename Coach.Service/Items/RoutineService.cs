using Coach.Data.DataAccess.Items;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Items
{
    public interface IRoutineService
    {
        List<RoutineModel> GetRoutines();
        int AddRoutine(RoutineModel routine);
        List<int> AddRoutines(List<RoutineModel> routines);
        RoutineModel UpdateRoutine(RoutineModel routine);
        List<RoutineModel> UpdateRoutines(List<RoutineModel> routines);
        void DeleteRoutine(int routineID);
        void DeleteRoutines(List<int> routineIDs);

        #region Planner
        RoutineModel GetRoutineForDate(DateTime datetime);
        void CreateEventTasksForRoutine(int routineID, DateTime? startDatetime = null, DateTime? endDatetime = null);
        void ReorderTodo(int routineID, int todoID, int newPosition);
        #endregion
    }

    public class RoutineService : IRoutineService
    {
        IRoutineDAO _routineDAO;

        public RoutineService(IRoutineDAO routineDAO)
        {
            _routineDAO = routineDAO;
        }

        #region CRUD
        public List<RoutineModel> GetRoutines()
        {
            return _routineDAO.GetRoutines();
        }

        public int AddRoutine(RoutineModel routine)
        {
            return _routineDAO.AddRoutine(routine);
        }

        public List<int> AddRoutines(List<RoutineModel> routines)
        {
            return _routineDAO.AddRoutines(routines);
        }

        public RoutineModel UpdateRoutine(RoutineModel routine)
        {
            return _routineDAO.UpdateRoutine(routine);
        }

        public List<RoutineModel> UpdateRoutines(List<RoutineModel> routines)
        {
            return _routineDAO.UpdateRoutines(routines);
        }

        public void DeleteRoutine(int routineID)
        {
            _routineDAO.DeleteRoutine(routineID);
        }

        public void DeleteRoutines(List<int> routineIDs)
        {
            _routineDAO.DeleteRoutines(routineIDs);
        }
        #endregion

        #region Routine View
        public RoutineModel GetRoutineForDate(DateTime datetime)
        {
            return _routineDAO.GetRoutineForDate(datetime);
        }
        #endregion

        #region Planner
        public void CreateEventTasksForRoutine(int routineID, DateTime? startDatetime = null, DateTime? endDatetime = null)
        {
            if (startDatetime == null)
                startDatetime = DateTime.Today;

            /* Check if EventTask exist for Routine */
            bool eventtastExist =_routineDAO.DoesRoutineEventTaskExist(routineID, (DateTime)startDatetime);
            if (eventtastExist) return;

            _routineDAO.CreateEventTasksForRoutine(routineID, (DateTime)startDatetime, endDatetime);
        }

        public void ReorderTodo(int routineID, int todoID, int newPosition)
        {
            _routineDAO.ReorderTodo(routineID, todoID, newPosition);
        }
        #endregion
    }
}