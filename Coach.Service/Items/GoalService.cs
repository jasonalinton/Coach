using Coach.Data.DataAccess.Items;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Service.Items
{
    public interface IGoalService
    {
        List<GoalModel> GetGoals();
        int AddGoal(GoalModel goal);
        List<int> AddGoals(List<GoalModel> goals);
        GoalModel UpdateGoal(GoalModel goal);
        List<GoalModel> UpdateGoals(List<GoalModel> goals);
        void DeleteGoal(int goalID);
        void DeleteGoals(List<int> goalIDs);
    }

    public class GoalService : IGoalService
    {
        IGoalDAO _goalDAO;

        public GoalService(GoalDAO goalDAO)
        {
            _goalDAO = goalDAO;
        }

        #region CRUD
        public List<GoalModel> GetGoals()
        {
            return _goalDAO.GetGoals();
        }

        public int AddGoal(GoalModel goal)
        {
            return _goalDAO.AddGoal(goal);
        }

        public List<int> AddGoals(List<GoalModel> goals)
        {
            return _goalDAO.AddGoals(goals);
        }

        public GoalModel UpdateGoal(GoalModel goal)
        {
            return _goalDAO.UpdateGoal(goal);
        }

        public List<GoalModel> UpdateGoals (List<GoalModel> goals)
        {
            return _goalDAO.UpdateGoals(goals);
        }

        public void DeleteGoal(int goalID)
        {
            _goalDAO.DeleteGoal(goalID);
        }

        public void DeleteGoals(List<int> goalIDs)
        {
            _goalDAO.DeleteGoals(goalIDs);
        }
        #endregion
    }
}
