using Coach.Data.DataAccess.Logging;
using Coach.Data.Extension;
using Coach.Data.Model;
using Coach.Model.Items;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static Coach.Data.Mappping.MyMapper;

namespace Coach.Data.DataAccess.Items
{
    public interface IGoalDAO
    {
        List<GoalModel> GetGoals();
        int AddGoal(GoalModel GoalModel);
        List<int> AddGoals(List<GoalModel> GoalModels);
        GoalModel UpdateGoal(GoalModel GoalModel);
        List<GoalModel> UpdateGoals(List<GoalModel> GoalModels);
        void DeleteGoal(int id);
        void DeleteGoals(List<int> ids);
    }

    public class GoalDAO : IGoalDAO
    {
        public List<GoalModel> GetGoals()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var goals = entities.goal_view.ToList();
                return CoachMapper.Map<List<GoalModel>>(goals); 
            }
        }

        public int AddGoal(GoalModel goalModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var goal = CoachMapper.Map<goal>(goalModel);
                entities.goals.Add(goal);

                #region Mappings
                foreach (var idParent in goalModel.ParentIDs.Distinct())
                {
                    goal.goal_goal.Add(new goal_goal
                    {
                        idGoal = goalModel.ID,
                        idParent = idParent
                    });
                }
                foreach (var idInventoryItem in goalModel.InventoryItemIDs.Distinct())
                {
                    goal.inventoryitem_goal.Add(new inventoryitem_goal
                    {
                        idInventoryItem = idInventoryItem
                    });
                }
                foreach (var idTodo in goalModel.TodoIDs.Distinct())
                {
                    goal.goal_todo.Add(new goal_todo
                    {
                        idGoal = goal.id,
                        idTodo = idTodo
                    });
                }
                foreach (var idRoutine in goalModel.RoutineIDs.Distinct())
                {
                    goal.goal_routine.Add(new goal_routine
                    {
                        idGoal = goal.id,
                        idRoutine = idRoutine
                    });
                }
                foreach (var idType in goalModel.TypeIDs.Distinct())
                {
                    goal.goal_type.Add(new goal_type
                    {
                        idGoal = goal.id,
                        idType = idType
                    });
                }
                foreach (var idPhoto in goalModel.PhotoIDs.Distinct())
                {
                    goal.goal_type.Add(new goal_type
                    {
                        idGoal = goal.id,
                        idType = idPhoto
                    });
                }
                foreach (var idLocation in goalModel.LocationIDs.Distinct())
                {
                    goal.goal_location.Add(new goal_location
                    {
                        idGoal = goalModel.ID,
                        idLocation = idLocation
                    });
                }
                foreach (var idNote in goalModel.NoteIDs.Distinct())
                {
                    goal.goal_note.Add(new goal_note
                    {
                        idGoal = goal.id,
                        idNote = idNote
                    });
                }
                foreach (var idTag in goalModel.TagIDs.Distinct())
                {
                    goal.goal_tag.Add(new goal_tag
                    {
                        idGoal = goal.id,
                        idTag = idTag
                    });
                }
                #endregion

                entities.SaveChanges();

                return goal.id; 
            }
        }

        public List<int> AddGoals(List<GoalModel> goalModels)
        {
            var newIDs = new List<int>();

            foreach (var goalModel in goalModels)
            {
                try
                {
                    newIDs.Add(AddGoal(goalModel));
                }
                catch (Exception ex)
                {
                    ex.Data.Add("logMessage", $"Error adding goal: \"{goalModel.Text}");
                    LogDAO.AddQueuedLogError(ex);
                }
            }

            return newIDs;
        }

        public GoalModel UpdateGoal(GoalModel goalModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var originalGoal = entities.goals.SingleOrDefault(x => x.id == goalModel.ID);
                if (originalGoal != null)
                {
                    var goal = CoachMapper.Map<goal>(goalModel);

                    #region Parents
                    /* Loop though all the current Goal-Goal(Parent-Goal) mappings. 
                     * If the mapping already exists, remove the ParentID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Goal mapping to the list of mappings to be removed */
                    var parentIDs_Added = new List<int>(goalModel.ParentIDs.Distinct());
                    var goalgoal_Removed = new List<goal_goal>();
                    entities.goal_goal.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_goal =>
                    {
                        if (goalModel.ParentIDs.Contains(goal_goal.idParent))
                            parentIDs_Added.Remove(goal_goal.idParent); // Remove ParentID from list of IDs that need to be mapped
                    else
                            goalgoal_Removed.Add(goal_goal); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Goal mappings that don't already exist */
                    foreach (var idParent in parentIDs_Added)
                    {
                        var goal_goalNew = new goal_goal
                        {
                            idGoal = goalModel.ID,
                            idParent = idParent
                        };
                        entities.Entry(goal_goalNew).State = EntityState.Added;
                        goal.goal_goal.Add(goal_goalNew);
                    }
                    /* Remove Goal-Goal mapping that no longer exist */
                    entities.goal_goal.RemoveRange(goalgoal_Removed);
                    #endregion

                    #region Inventory Items
                    /* Loop though all the current InventoryItem-Goal mappings. 
                     * If the mapping already exists, remove the InventoryItemID from the list of ID's to add
                     * If the mapping no longer exists, add the InentoryItem-Goal mapping to the list of mappings to be removed */
                    var inventoryitemIDs_Add = new List<int>(goalModel.InventoryItemIDs.Distinct());
                    var inventoryitemgoals_Remove = new List<inventoryitem_goal>();
                    entities.inventoryitem_goal.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(inventoryitem_goal =>
                    {
                        if (goalModel.InventoryItemIDs.Contains(inventoryitem_goal.idInventoryItem))
                            inventoryitemIDs_Add.Remove(inventoryitem_goal.idInventoryItem); // Remove InventoryItemID from list of IDs that need to be mapped
                        else
                            inventoryitemgoals_Remove.Add(inventoryitem_goal); // Queue mapping to be removed if it already exists
                    });
                    /* Add InventoryItem-Goal mappings that don't already exist */
                    foreach (var idInventoryItem in inventoryitemIDs_Add)
                    {
                        var inventoryitem_goalNew = new inventoryitem_goal
                        {
                            idGoal = goalModel.ID,
                            idInventoryItem = idInventoryItem
                        };
                        entities.Entry(inventoryitem_goalNew).State = EntityState.Added;
                        goal.inventoryitem_goal.Add(inventoryitem_goalNew);
                    }
                    /* Remove Inventory Item - Goal mapping that no longer exist */
                    entities.inventoryitem_goal.RemoveRange(inventoryitemgoals_Remove);
                    #endregion

                    #region Todos
                    /* Loop though all the current Goal-Todo mappings. 
                     * If the mapping already exists, remove the TodoID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Todo mapping to the list of mappings to be removed */
                    var todoIDs_Added = new List<int>(goalModel.TodoIDs.Distinct());
                    var goaltodo_Removed = new List<goal_todo>();
                    entities.goal_todo.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_todo =>
                    {
                        if (goalModel.TodoIDs.Contains(goal_todo.idTodo))
                            todoIDs_Added.Remove(goal_todo.idTodo); // Remove TodoID from list of IDs that need to be mapped
                        else
                            goaltodo_Removed.Add(goal_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Todo mappings that don't already exist */
                    foreach (var idTodo in todoIDs_Added)
                    {
                        var goal_todoNew = new goal_todo
                        {
                            idGoal = goalModel.ID,
                            idTodo = idTodo
                        };
                        entities.Entry(goal_todoNew).State = EntityState.Added;
                        goal.goal_todo.Add(goal_todoNew);
                    }
                    /* Remove Goal-Todo mapping that no longer exist */
                    entities.goal_todo.RemoveRange(goaltodo_Removed);
                    #endregion

                    #region Routine
                    /* Loop though all the current Goal-Routine mappings. 
                     * If the mapping already exists, remove the RoutineID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Routine mapping to the list of mappings to be removed */
                    var routineIDs_Added = new List<int>(goalModel.RoutineIDs.Distinct());
                    var goalroutine_Removed = new List<goal_routine>();
                    entities.goal_routine.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_routine =>
                    {
                        if (goalModel.RoutineIDs.Contains(goal_routine.idRoutine))
                            routineIDs_Added.Remove(goal_routine.idRoutine); // Remove RoutineID from list of IDs that need to be mapped
                        else
                            goalroutine_Removed.Add(goal_routine); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Routine mappings that don't already exist */
                    foreach (var idRoutine in routineIDs_Added)
                    {
                        var goal_routineNew = new goal_routine
                        {
                            idGoal = goalModel.ID,
                            idRoutine = idRoutine
                        };
                        entities.Entry(goal_routineNew).State = EntityState.Added;
                        goal.goal_routine.Add(goal_routineNew);
                    }
                    /* Remove Goal-Routine mapping that no longer exist */
                    entities.goal_routine.RemoveRange(goalroutine_Removed);
                    #endregion

                    #region Type
                    /* Loop though all the current Goal-Type mappings. 
                     * If the mapping already exists, remove the TypeID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Type mapping to the list of mappings to be removed */
                    var typeIDs_Added = new List<int>(goalModel.TypeIDs.Distinct());
                    var goaltype_Removed = new List<goal_type>();
                    entities.goal_type.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_type =>
                    {
                        if (goalModel.TypeIDs.Contains(goal_type.idType))
                            typeIDs_Added.Remove(goal_type.idType); // Remove TypeID from list of IDs that need to be mapped
                        else
                            goaltype_Removed.Add(goal_type); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Type mappings that don't already exist */
                    foreach (var idType in typeIDs_Added)
                    {
                        var goal_typeNew = new goal_type
                        {
                            idGoal = goalModel.ID,
                            idType = idType
                        };
                        entities.Entry(goal_typeNew).State = EntityState.Added;
                        goal.goal_type.Add(goal_typeNew);
                    }
                    /* Remove Goal-Type mapping that no longer exist */
                    entities.goal_type.RemoveRange(goaltype_Removed);
                    #endregion

                    #region Photo
                    /* Loop though all the current Goal-Photo mappings. 
                     * If the mapping already exists, remove the PhotoID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Photo mapping to the list of mappings to be removed */
                    var photoIDs_Added = new List<int>(goalModel.PhotoIDs.Distinct());
                    var goalphoto_Removed = new List<goal_photo>();
                    entities.goal_photo.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_photo =>
                    {
                        if (goalModel.PhotoIDs.Contains(goal_photo.idPhoto))
                            photoIDs_Added.Remove(goal_photo.idPhoto); // Remove PhotoID from list of IDs that need to be mapped
                        else
                            goalphoto_Removed.Add(goal_photo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Photo mappings that don't already exist */
                    foreach (var idPhoto in photoIDs_Added)
                    {
                        var goal_photoNew = new goal_photo
                        {
                            idGoal = goalModel.ID,
                            idPhoto = idPhoto
                        };
                        entities.Entry(goal_photoNew).State = EntityState.Added;
                        goal.goal_photo.Add(goal_photoNew);
                    }
                    /* Remove Goal-Photo mapping that no longer exist */
                    entities.goal_photo.RemoveRange(goalphoto_Removed);
                    #endregion

                    #region Location
                    /* Loop though all the current Goal-Location mappings. 
                     * If the mapping already exists, remove the LocationID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Location mapping to the list of mappings to be removed */
                    var locationIDs_Added = new List<int>(goalModel.LocationIDs.Distinct());
                    var goallocation_Removed = new List<goal_location>();
                    entities.goal_location.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_location =>
                    {
                        if (goalModel.LocationIDs.Contains(goal_location.idLocation))
                            locationIDs_Added.Remove(goal_location.idLocation); // Remove LocationID from list of IDs that need to be mapped
                        else
                            goallocation_Removed.Add(goal_location); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Location mappings that don't already exist */
                    foreach (var idLocation in locationIDs_Added)
                    {
                        var goal_locationNew = new goal_location
                        {
                            idGoal = goalModel.ID,
                            idLocation = idLocation
                        };
                        entities.Entry(goal_locationNew).State = EntityState.Added;
                        goal.goal_location.Add(goal_locationNew);
                    }
                    /* Remove Goal-Location mapping that no longer exist */
                    entities.goal_location.RemoveRange(goallocation_Removed);
                    #endregion

                    #region Note
                    /* Loop though all the current Goal-Note mappings. 
                     * If the mapping already exists, remove the NoteID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Note mapping to the list of mappings to be removed */
                    var noteIDs_Added = new List<int>(goalModel.NoteIDs.Distinct());
                    var goalnote_Removed = new List<goal_note>();
                    entities.goal_note.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_note =>
                    {
                        if (goalModel.NoteIDs.Contains(goal_note.idNote))
                            noteIDs_Added.Remove(goal_note.idNote); // Remove NoteID from list of IDs that need to be mapped
                        else
                            goalnote_Removed.Add(goal_note); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Note mappings that don't already exist */
                    foreach (var idNote in noteIDs_Added)
                    {
                        var goal_noteNew = new goal_note
                        {
                            idGoal = goalModel.ID,
                            idNote = idNote
                        };
                        entities.Entry(goal_noteNew).State = EntityState.Added;
                        goal.goal_note.Add(goal_noteNew);
                    }
                    /* Remove Goal-Note mapping that no longer exist */
                    entities.goal_note.RemoveRange(goalnote_Removed);
                    #endregion

                    #region Tag
                    /* Loop though all the current Goal-Tag mappings. 
                     * If the mapping already exists, remove the TagID from the list of ID's to add
                     * If the mapping no longer exists, add the Goal-Tag mapping to the list of mappings to be removed */
                    var tagIDs_Added = new List<int>(goalModel.TagIDs.Distinct());
                    var goaltag_Removed = new List<goal_tag>();
                    entities.goal_tag.Where(x => x.idGoal == goalModel.ID).ToList().ForEach(goal_tag =>
                    {
                        if (goalModel.TagIDs.Contains(goal_tag.idTag))
                            tagIDs_Added.Remove(goal_tag.idTag); // Remove TagID from list of IDs that need to be mapped
                        else
                            goaltag_Removed.Add(goal_tag); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Tag mappings that don't already exist */
                    foreach (var idTag in tagIDs_Added)
                    {
                        var goal_tagNew = new goal_tag
                        {
                            idGoal = goalModel.ID,
                            idTag = idTag
                        };
                        entities.Entry(goal_tagNew).State = EntityState.Added;
                        goal.goal_tag.Add(goal_tagNew);
                    }
                    /* Remove Goal-Tag mapping that no longer exist */
                    entities.goal_tag.RemoveRange(goaltag_Removed);
                    #endregion
                    
                    entities.Entry(originalGoal).CurrentValues.SetValues(goal);
                    entities.Entry(originalGoal).State = EntityState.Modified;
                    entities.SaveChanges();

                    return CoachMapper.Map<GoalModel>(originalGoal);
                }
                else
                    return null; 
            }
        }

        public List<GoalModel> UpdateGoals(List<GoalModel> goalModels)
        {
            if (goalModels == null) return null;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var updatedModels = new List<GoalModel>();

                foreach (var goalModel in goalModels)
                    updatedModels.Add(this.UpdateGoal(goalModel));

                return updatedModels; 
            }
        }

        public void DeleteGoal(int id)
        {
            if (id <= 0) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var goal = entities.goals.SingleOrDefault(x => x.id == id);

                if (goal != null)
                {
                    entities.goals.Remove(goal);
                    entities.SaveChanges();
                } 
            }
        }

        public void DeleteGoals(List<int> ids)
        {
            if (ids == null) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var goals = entities.goals.Where(x => ids.Contains(x.id));

                entities.goals.RemoveRange(goals);
                entities.SaveChanges(); 
            }
        }
    }
}
