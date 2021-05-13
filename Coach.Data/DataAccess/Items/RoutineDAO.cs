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
using static Coach.Data.Mappping.MyMapper;

namespace Coach.Data.DataAccess.Items
{
    public interface IRoutineDAO
    {
        List<RoutineModel> GetRoutines();
        int AddRoutine(RoutineModel RoutineModel);
        List<int> AddRoutines(List<RoutineModel> RoutineModels);
        RoutineModel UpdateRoutine(RoutineModel RoutineModel);
        List<RoutineModel> UpdateRoutines(List<RoutineModel> RoutineModels);
        void DeleteRoutine(int id);
        void DeleteRoutines(List<int> ids);
        #region Planner
        RoutineModel GetRoutineForDate(DateTime datetime);
        bool DoesRoutineEventTaskExist(int routineID, DateTime startDatetime, DateTime? endDatetime = null, string timeframe = null);
        void CreateEventTasksForRoutine(int routineID, DateTime startDatetime, DateTime? endDatetime = null);
        void ReorderTodo(int routineID, int todoID, int newPosition);
        #endregion
    }

    public class RoutineDAO : IRoutineDAO
    {
        public List<RoutineModel> GetRoutines()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var routines = entities.routine_view.ToList();
                return CoachMapper.Map<List<RoutineModel>>(routines);
            }
        }

        public int AddRoutine(RoutineModel routineModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var routine = CoachMapper.Map<routine>(routineModel);
                routine = entities.routines.Add(routine);

                #region Mappings
                foreach (var idParent in routineModel.ParentIDs.Distinct())
                {
                    routine.routine_routine.Add(new routine_routine
                    {
                        idParent = idParent
                    });
                }
                foreach (var idInventoryItem in routineModel.InventoryItemIDs.Distinct())
                {
                    routine.inventoryitem_routine.Add(new inventoryitem_routine
                    {
                        idInventoryItem = idInventoryItem
                    });
                }
                foreach (var idGoal in routineModel.GoalIDs.Distinct())
                {
                    routine.goal_routine.Add(new goal_routine
                    {
                        idGoal = idGoal,
                    });
                }
                foreach (var idTodo in routineModel.TodoIDs.Distinct())
                {
                    routine.routine_todo.Add(new routine_todo
                    {
                        idTodo = idTodo
                    });
                }
                foreach (var idType in routineModel.TypeIDs.Distinct())
                {
                    routine.routine_type.Add(new routine_type
                    {
                        idType = idType
                    });
                }
                foreach (var idPhoto in routineModel.PhotoIDs.Distinct())
                {
                    routine.routine_type.Add(new routine_type
                    {
                        idType = idPhoto
                    });
                }
                foreach (var idLocation in routineModel.LocationIDs.Distinct())
                {
                    routine.routine_location.Add(new routine_location
                    {
                        idLocation = idLocation
                    });
                }
                foreach (var idNote in routineModel.NoteIDs.Distinct())
                {
                    routine.routine_note.Add(new routine_note
                    {
                        idNote = idNote
                    });
                }
                foreach (var idTag in routineModel.TagIDs.Distinct())
                {
                    routine.routine_tag.Add(new routine_tag
                    {
                        idTag = idTag,
                    });
                }
                #endregion

                entities.SaveChanges();

                return routine.id;
            }
        }

        public List<int> AddRoutines(List<RoutineModel> routineModels)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var newIDs = new List<int>();

                foreach (var routineModel in routineModels)
                {
                    try
                    {
                        newIDs.Add(AddRoutine(routineModel));
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("logMessage", $"Error adding routine: \"{routineModel.Text}");
                        LogDAO.AddQueuedLogError(ex);
                    }
                }

                return newIDs;
            }
        }

        public RoutineModel UpdateRoutine(RoutineModel routineModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var originalRoutine = entities.routines.SingleOrDefault(x => x.id == routineModel.ID);
                if (originalRoutine != null)
                {
                    var routine = CoachMapper.Map<routine>(routineModel);

                    #region Parents
                    /* Loop though all the current Routine-Routine(Parent-Routine) mappings. 
                     * If the mapping already exists, remove the ParentID from the list of ID's to add
                     * If the mapping no longer exists, add the Routine-Routine mapping to the list of mappings to be removed */
                    var parentIDs_Added = new List<int>(routineModel.ParentIDs.Distinct());
                    var routineroutine_Removed = new List<routine_routine>();
                    entities.routine_routine.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_routine =>
                    {
                        if (routineModel.ParentIDs.Contains(routine_routine.idParent))
                            parentIDs_Added.Remove(routine_routine.idParent); // Remove ParentID from list of IDs that need to be mapped
                        else
                            routineroutine_Removed.Add(routine_routine); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Routine mappings that don't already exist */
                    foreach (var idParent in parentIDs_Added)
                    {
                        var routine_routineNew = new routine_routine
                        {
                            idParent = idParent,
                            idRoutine = routineModel.ID
                        };
                        //entities.Entry(routine_routineNew).State = EntityState.Added;
                        routine.routine_routine.Add(routine_routineNew);
                    }
                    /* Remove Routine-Routine mapping that no longer exist */
                    entities.routine_routine.RemoveRange(routineroutine_Removed);
                    #endregion

                    #region Inventory Items
                    var inventoryitemIDs_Add = new List<int>(routineModel.InventoryItemIDs.Distinct());
                    var inventoryitemroutines_Remove = new List<inventoryitem_routine>();
                    entities.inventoryitem_routine.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(inventoryitem_routine =>
                    {
                        if (routineModel.InventoryItemIDs.Contains(inventoryitem_routine.idInventoryItem))
                            inventoryitemIDs_Add.Remove(inventoryitem_routine.idInventoryItem); // Remove InventoryItemID from list of IDs that need to be mapped
                        else
                            inventoryitemroutines_Remove.Add(inventoryitem_routine); // Queue mapping to be removed if it already exists
                    });
                    /* Add InventoryItem-Routine mappings that don't already exist */
                    foreach (var idInventoryItem in inventoryitemIDs_Add)
                    {
                        var inventoryitem_routineNew = new inventoryitem_routine
                        {
                            idInventoryItem = idInventoryItem,
                            idRoutine = routineModel.ID
                        };
                        entities.Entry(inventoryitem_routineNew).State = EntityState.Added;
                        routine.inventoryitem_routine.Add(inventoryitem_routineNew);
                    }
                    /* Remove Inventory Item - Routine mapping that no longer exist */
                    entities.inventoryitem_routine.RemoveRange(inventoryitemroutines_Remove);
                    #endregion

                    #region Goals
                    var goalroutineIDs_Added = new List<int>(routineModel.GoalIDs.Distinct());
                    var goalroutine_Removed = new List<goal_routine>();
                    entities.goal_routine.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(goal_routine =>
                    {
                        if (routineModel.GoalIDs.Contains(goal_routine.idGoal))
                            goalroutineIDs_Added.Remove(goal_routine.idGoal); // Remove GoalID from list of IDs that need to be mapped
                        else
                            goalroutine_Removed.Add(goal_routine); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Routine mappings that don't already exist */
                    foreach (var idGoal in goalroutineIDs_Added)
                    {
                        var goal_routineNew = new goal_routine
                        {
                            idGoal = idGoal,
                            idRoutine = routineModel.ID
                        };
                        entities.Entry(goal_routineNew).State = EntityState.Added;
                        routine.goal_routine.Add(goal_routineNew);
                    }
                    /* Remove Goal-Routine mapping that no longer exist */
                    entities.goal_routine.RemoveRange(goalroutine_Removed);
                    #endregion

                    #region Todo
                    var todoIDs_Added = new List<int>(routineModel.TodoIDs.Distinct());
                    var routinetodos_Removed = new List<routine_todo>();
                    entities.routine_todo.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_todo =>
                    {
                        if (routineModel.TodoIDs.Contains(routine_todo.idTodo))
                            todoIDs_Added.Remove(routine_todo.idTodo); // Remove TodoID from list of IDs that need to be mapped
                        else
                            routinetodos_Removed.Add(routine_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Todo mappings that don't already exist */
                    foreach (var idTodo in todoIDs_Added)
                    {
                        var routine_todoNew = new routine_todo
                        {
                            idRoutine = routineModel.ID,
                            idTodo = idTodo
                        };
                        entities.Entry(routine_todoNew).State = EntityState.Added;
                        routine.routine_todo.Add(routine_todoNew);
                    }
                    /* Remove Routine-Todo mapping that no longer exist */
                    entities.routine_todo.RemoveRange(routinetodos_Removed);
                    #endregion

                    #region Type
                    var typeIDs_Added = new List<int>(routineModel.TypeIDs.Distinct());
                    var routinetype_Removed = new List<routine_type>();
                    entities.routine_type.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_type =>
                    {
                        if (routineModel.TypeIDs.Contains(routine_type.idType))
                            typeIDs_Added.Remove(routine_type.idType); // Remove TypeID from list of IDs that need to be mapped
                        else
                            routinetype_Removed.Add(routine_type); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Type mappings that don't already exist */
                    foreach (var idType in typeIDs_Added)
                    {
                        var routine_typeNew = new routine_type
                        {
                            idRoutine = routineModel.ID,
                            idType = idType
                        };
                        entities.Entry(routine_typeNew).State = EntityState.Added;
                        routine.routine_type.Add(routine_typeNew);
                    }
                    /* Remove Routine-Type mapping that no longer exist */
                    entities.routine_type.RemoveRange(routinetype_Removed);
                    #endregion

                    #region Photo
                    var photoIDs_Added = new List<int>(routineModel.PhotoIDs.Distinct());
                    var routinephoto_Removed = new List<routine_photo>();
                    entities.routine_photo.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_photo =>
                    {
                        if (routineModel.PhotoIDs.Contains(routine_photo.idPhoto))
                            photoIDs_Added.Remove(routine_photo.idPhoto); // Remove PhotoID from list of IDs that need to be mapped
                        else
                            routinephoto_Removed.Add(routine_photo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Photo mappings that don't already exist */
                    foreach (var idPhoto in photoIDs_Added)
                    {
                        var routine_photoNew = new routine_photo
                        {
                            idRoutine = routineModel.ID,
                            idPhoto = idPhoto
                        };
                        entities.Entry(routine_photoNew).State = EntityState.Added;
                        routine.routine_photo.Add(routine_photoNew);
                    }
                    /* Remove Routine-Photo mapping that no longer exist */
                    entities.routine_photo.RemoveRange(routinephoto_Removed);
                    #endregion

                    #region Location
                    var locationIDs_Added = new List<int>(routineModel.LocationIDs.Distinct());
                    var routinelocation_Removed = new List<routine_location>();
                    entities.routine_location.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_location =>
                    {
                        if (routineModel.LocationIDs.Contains(routine_location.idLocation))
                            locationIDs_Added.Remove(routine_location.idLocation); // Remove LocationID from list of IDs that need to be mapped
                        else
                            routinelocation_Removed.Add(routine_location); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Location mappings that don't already exist */
                    foreach (var idLocation in locationIDs_Added)
                    {
                        var routine_locationNew = new routine_location
                        {
                            idRoutine = routineModel.ID,
                            idLocation = idLocation
                        };
                        entities.Entry(routine_locationNew).State = EntityState.Added;
                        routine.routine_location.Add(routine_locationNew);
                    }
                    /* Remove Routine-Location mapping that no longer exist */
                    entities.routine_location.RemoveRange(routinelocation_Removed);
                    #endregion

                    #region Note
                    var noteIDs_Added = new List<int>(routineModel.NoteIDs.Distinct());
                    var routinenote_Removed = new List<routine_note>();
                    entities.routine_note.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_note =>
                    {
                        if (routineModel.NoteIDs.Contains(routine_note.idNote))
                            noteIDs_Added.Remove(routine_note.idNote); // Remove NoteID from list of IDs that need to be mapped
                        else
                            routinenote_Removed.Add(routine_note); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Note mappings that don't already exist */
                    foreach (var idNote in noteIDs_Added)
                    {
                        var routine_noteNew = new routine_note
                        {
                            idRoutine = routineModel.ID,
                            idNote = idNote
                        };
                        entities.Entry(routine_noteNew).State = EntityState.Added;
                        routine.routine_note.Add(routine_noteNew);
                    }
                    /* Remove Routine-Note mapping that no longer exist */
                    entities.routine_note.RemoveRange(routinenote_Removed);
                    #endregion

                    #region Tag
                    var tagIDs_Added = new List<int>(routineModel.TagIDs.Distinct());
                    var routinetag_Removed = new List<routine_tag>();
                    entities.routine_tag.Where(x => x.idRoutine == routineModel.ID).ToList().ForEach(routine_tag =>
                    {
                        if (routineModel.TagIDs.Contains(routine_tag.idTag))
                            tagIDs_Added.Remove(routine_tag.idTag); // Remove TagID from list of IDs that need to be mapped
                        else
                            routinetag_Removed.Add(routine_tag); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Tag mappings that don't already exist */
                    foreach (var idTag in tagIDs_Added)
                    {
                        var routine_tagNew = new routine_tag
                        {
                            idRoutine = routineModel.ID,
                            idTag = idTag,
                        };
                        entities.Entry(routine_tagNew).State = EntityState.Added;
                        routine.routine_tag.Add(routine_tagNew);
                    }
                    /* Remove Routine-Tag mapping that no longer exist */
                    entities.routine_tag.RemoveRange(routinetag_Removed);
                    #endregion

                    entities.Entry(originalRoutine).CurrentValues.SetValues(routine);
                    entities.Entry(originalRoutine).State = EntityState.Modified;
                    entities.SaveChanges();

                    return CoachMapper.Map<RoutineModel>(originalRoutine);
                }
                else
                    return null;
            }
        }

        public List<RoutineModel> UpdateRoutines(List<RoutineModel> routineModels)
        {
            if (routineModels == null) return null;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var updatedModels = new List<RoutineModel>();

                foreach (var routineModel in routineModels)
                    updatedModels.Add(this.UpdateRoutine(routineModel));

                return updatedModels;
            }
        }

        public void DeleteRoutine(int id)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var routine = entities.routines.SingleOrDefault(x => x.id == id);

                if (routine != null)
                {
                    entities.routines.Remove(routine);
                    entities.SaveChanges();
                }
            }
        }

        public void DeleteRoutines(List<int> ids)
        {
            if (ids == null) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var routines = entities.routines.Where(x => ids.Contains(x.id));

                entities.routines.RemoveRange(routines);
                entities.SaveChanges();
            }
        }

        #region Planner
        public RoutineModel GetRoutineForDate(DateTime datetime)
        {
            if (datetime == null)
                throw new NullReferenceException("datetime NULL");

            using (coachdevEntities entities = new coachdevEntities())
            {
                var routines = entities.routines
                    .Include("inventoryitem_routine.inventoryitem")
                    .Include("goal_routine.goal")
                    .Include("routine_routine.routine")
                    .Include("routine_todo.todo.todo_child.child")
                    .Include("routine_todo.todo.todo_eventtask.eventtask")
                    .Include("routine_eventtask.eventtask")
                    .ToList()
                    .Where(x => x.isActive == 1)
                    .ToList();

                if (routines.Count() > 0)
                {
                    var routine = routines[0];
                    return CoachMapper.Map<RoutineModel>(routine);
                }
                else
                    return null;
            }
        }
        public bool DoesRoutineEventTaskExist(int routineID, DateTime startDatetime, DateTime? endDatetime = null, string timeframe = null)
        {
            startDatetime = startDatetime.StartOfDay();
            endDatetime = startDatetime.EndOfDay();

            using (coachdevEntities entities = new coachdevEntities())
            {
                //var task = entities.eventtasks.Where(x => x.startDateTime >= startDatetime && x.startDateTime < endDatetime).ToList();
                var task = entities.routine_eventtask.Where(x => x.routine.id == routineID && x.eventtask.startDateTime >= startDatetime && x.eventtask.startDateTime < endDatetime).ToList();

                return (task.Count > 0) ? true : false;
            }
        }

        public void CreateEventTasksForRoutine(int routineID, DateTime startDatetime, DateTime? endDatetime = null)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var routine = entities.routines.Include("routine_todo.todo").SingleOrDefault(x => x.id == routineID);
                if (routine == null) return;

                // Create EventTask for Routine
                var eventtaskRoutine = new eventtask
                {
                    text = routine.text,
                    startDateTime = startDatetime,
                    isVisible = (sbyte)routine.isVisible,
                    isActive = routine.isActive
                };
                entities.eventtasks.Add(eventtaskRoutine);

                var routineeventtask = new routine_eventtask
                {
                    idRoutine = routineID,
                    isVisible = (sbyte)routine.isVisible,
                    isActive = routine.isActive
                };
                eventtaskRoutine.routine_eventtask.Add(routineeventtask);

                // Create EventTasks for each Todo
                foreach (var todo in routine.routine_todo.Select(x => x.todo).ToList())
                {
                    var eventtaskTodo = new eventtask
                    {
                        text = todo.text,
                        startDateTime = startDatetime,
                        isVisible = (sbyte)todo.isVisible,
                        isActive = todo.isActive
                    };
                    entities.eventtasks.Add(eventtaskTodo);

                    var todoeventtask = new todo_eventtask
                    {
                        todo = todo,
                        isVisible = (sbyte)todo.isVisible,
                        isActive = todo.isActive
                    };
                    eventtaskTodo.todo_eventtask.Add(todoeventtask);
                }

                entities.SaveChanges();
            }
        }

        public void ReorderTodo(int routineID, int todoID, int newPosition)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                //var routine_todos = entities.routines.Where(x => x.id == routineID)
                //    .Include("routine_todo.todo")
                //    .Select(x => x.routine_todo)
                //    .OrderBy(x => x.position).ToList();
                var routine_todos = entities.routines.Where(x => x.id == routineID)
                    .Include("routine_todo.todo")
                    .Select(x => x.routine_todo).ToList()[0];

                foreach (var routine_todo in routine_todos)
                {
                    if (routine_todo.todo.id == todoID)
                    {
                        routine_todo.position = newPosition;
                        continue;
                    }

                    if (routine_todo.position >= newPosition)
                        routine_todo.position++;
                }

                entities.SaveChanges();
            }
        }
        #endregion
    }
}
