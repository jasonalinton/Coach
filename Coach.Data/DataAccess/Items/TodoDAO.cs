using Coach.Data.DataAccess.Logging;
using Coach.Data.Model;
using Coach.Model.Items;
using Coach.Model.Items.GoogleTask;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Coach.Data.Mappping.MyMapper;
using static Coach.Model.Planner.Time.Types;

namespace Coach.Data.DataAccess.Items
{
    public interface ITodoDAO
    {
        TodoModel GetTodo(int idTodo);
        List<TodoModel> GetTodos();
        int AddTodo(TodoModel todoModel);
        List<int> AddTodos(List<TodoModel> todoModels);
        TodoModel UpdateTodo(TodoModel todoModel);
        List<TodoModel> UpdateTodos(List<TodoModel> todoModels);
        void DeleteTodo(int id);
        void DeleteTodos(List<int> ids);

        #region Planner
        string GetGoogleTaskIDForTodo(int idTodo, DateTime dueDate);
        void ScheduleTodo(int idTodo, DateTime scheduledDateTime, string idGoogleTask = null);
        void SetTodoCompletion(int idEventTask, bool isComplete);
        bool SyncGoogleTaskWithEventTask(int idTodo, GoogleTaskModel googleTaskModel, bool isAttempted = false);
        string DeleteGoogleTaskEventTask(int idTodo, DateTime date);
        #endregion

        #region TEMP - Planner Day
        List<TodoModel> GetActiveTodos();
        List<TodoModel> GetActiveTodos(DateTime date, int paddingWeeks = 4);
        int HardCodeTimeFor_SUMMERMOVEOUT_Goals();
        #endregion
    }

    public class TodoDAO : ITodoDAO
    {
        public TodoModel GetTodo(int idTodo)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todo = entities.todoes
                    .Include("inventoryitem_todo.inventoryitem")
                    .Include("goal_todo.goal")
                    .Include("routine_todo.routine")
                    .Include("todo_parent.parent")
                    .Include("todo_child.child")
                    .Include("todo_time.time")
                    .Include("todo_repeat.repeat")
                    .Include("todo_eventtask.eventtask")
                    .ToList()
                    .SingleOrDefault(x => x.id == idTodo);

                return CoachMapper.Map<TodoModel>(todo);
            }
        }

        public List<TodoModel> GetTodos()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todo = entities.todoes
                    .Include("inventoryitem_todo.inventoryitem")
                    .Include("goal_todo.goal")
                    .Include("routine_todo.routine")
                    .Include("todo_parent.parent")
                    .Include("todo_child.child")
                    .Include("todo_time.time")
                    .Include("todo_repeat.repeat")
                    .Include("todo_eventtask.eventtask")
                    .ToList();

                return CoachMapper.Map<List<TodoModel>>(todo);
            }
        }

        public int AddTodo(TodoModel todoModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todo = CoachMapper.Map<todo>(todoModel);
                todo = entities.todoes.Add(todo);

                #region Mappings
                foreach (var idParent in todoModel.ParentIDs.Distinct())
                {
                    todo.todo_child.Add(new todo_todo
                    {
                        idParent = idParent
                    });
                }
                foreach (var idInventoryItem in todoModel.InventoryItemIDs.Distinct())
                {
                    todo.inventoryitem_todo.Add(new inventoryitem_todo
                    {
                        idInventoryItem = idInventoryItem
                    });
                }
                foreach (var idGoal in todoModel.GoalIDs.Distinct())
                {
                    todo.goal_todo.Add(new goal_todo
                    {
                        idGoal = idGoal,
                    });
                }
                foreach (var idRoutine in todoModel.RoutineIDs.Distinct())
                {
                    todo.routine_todo.Add(new routine_todo
                    {
                        idRoutine = idRoutine
                    });
                }
                foreach (var idType in todoModel.TypeIDs.Distinct())
                {
                    todo.todo_type.Add(new todo_type
                    {
                        idType = idType
                    });
                }
                foreach (var idPhoto in todoModel.PhotoIDs.Distinct())
                {
                    todo.todo_type.Add(new todo_type
                    {
                        idType = idPhoto
                    });
                }
                foreach (var idLocation in todoModel.LocationIDs.Distinct())
                {
                    todo.todo_location.Add(new todo_location
                    {
                        idLocation = idLocation
                    });
                }
                foreach (var idNote in todoModel.NoteIDs.Distinct())
                {
                    todo.todo_note.Add(new todo_note
                    {
                        idNote = idNote
                    });
                }
                foreach (var idTag in todoModel.TagIDs.Distinct())
                {
                    todo.todo_tag.Add(new todo_tag
                    {
                        idTag = idTag,
                    });
                }
                #endregion

                entities.SaveChanges();

                return todo.id;
            }
        }

        public List<int> AddTodos(List<TodoModel> todoModels)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var newIDs = new List<int>();

                foreach (var todoModel in todoModels)
                {
                    try
                    {
                        newIDs.Add(AddTodo(todoModel));
                    }
                    catch (Exception ex)
                    {
                        ex.Data.Add("logMessage", $"Error adding todo: \"{todoModel.Text}");
                        LogDAO.AddQueuedLogError(ex);
                    }
                }

                return newIDs;
            }
        }

        public TodoModel UpdateTodo(TodoModel todoModel)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var originalTodo = entities.todoes.SingleOrDefault(x => x.id == todoModel.ID);
                if (originalTodo != null)
                {
                    var todo = CoachMapper.Map<todo>(todoModel);

                    #region Parents
                    /* Loop though all the current Todo-Todo(Parent-Todo) mappings. 
                     * If the mapping already exists, remove the ParentID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Todo mapping to the list of mappings to be removed */
                    var parentIDs_Added = new List<int>(todoModel.ParentIDs.Distinct());
                    var todotodo_Removed = new List<todo_todo>();
                    entities.todo_todo.Where(x => x.idChild == todoModel.ID).ToList().ForEach(todo_todo =>
                    {
                        if (todoModel.ParentIDs.Contains(todo_todo.idParent))
                            parentIDs_Added.Remove(todo_todo.idParent); // Remove ParentID from list of IDs that need to be mapped
                        else
                            todotodo_Removed.Add(todo_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Todo mappings that don't already exist */
                    foreach (var idParent in parentIDs_Added)
                    {
                        var todo_todoNew = new todo_todo
                        {
                            idParent = idParent,
                            idChild = todoModel.ID
                        };
                        //entities.Entry(todo_todoNew).State = EntityState.Added;
                        todo.todo_child.Add(todo_todoNew);
                    }
                    /* Remove Todo-Todo mapping that no longer exist */
                    entities.todo_todo.RemoveRange(todotodo_Removed);
                    #endregion

                    #region Inventory Items
                    /* Loop though all the current InventoryItem-Todo mappings. 
                     * If the mapping already exists, remove the InventoryItemID from the list of ID's to add
                     * If the mapping no longer exists, add the InentoryItem-Todo mapping to the list of mappings to be removed */
                    var inventoryitemIDs_Add = new List<int>(todoModel.InventoryItemIDs.Distinct());
                    var inventoryitemtodos_Remove = new List<inventoryitem_todo>();
                    entities.inventoryitem_todo.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(inventoryitem_todo =>
                    {
                        if (todoModel.InventoryItemIDs.Contains(inventoryitem_todo.idInventoryItem))
                            inventoryitemIDs_Add.Remove(inventoryitem_todo.idInventoryItem); // Remove InventoryItemID from list of IDs that need to be mapped
                        else
                            inventoryitemtodos_Remove.Add(inventoryitem_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add InventoryItem-Todo mappings that don't already exist */
                    foreach (var idInventoryItem in inventoryitemIDs_Add)
                    {
                        var inventoryitem_todoNew = new inventoryitem_todo
                        {
                            idInventoryItem = idInventoryItem,
                            idTodo = todoModel.ID
                        };
                        entities.Entry(inventoryitem_todoNew).State = EntityState.Added;
                        todo.inventoryitem_todo.Add(inventoryitem_todoNew);
                    }
                    /* Remove Inventory Item - Todo mapping that no longer exist */
                    entities.inventoryitem_todo.RemoveRange(inventoryitemtodos_Remove);
                    #endregion

                    #region Goals
                    /* Loop though all the current Goal-Todo mappings. 
                     * If the mapping already exists, remove the TodoID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Todo mapping to the list of mappings to be removed */
                    var goaltodoIDs_Added = new List<int>(todoModel.GoalIDs.Distinct());
                    var goaltodo_Removed = new List<goal_todo>();
                    entities.goal_todo.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(goal_todo =>
                    {
                        if (todoModel.GoalIDs.Contains(goal_todo.idGoal))
                            goaltodoIDs_Added.Remove(goal_todo.idGoal); // Remove GoalID from list of IDs that need to be mapped
                        else
                            goaltodo_Removed.Add(goal_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Goal-Todo mappings that don't already exist */
                    foreach (var idGoal in goaltodoIDs_Added)
                    {
                        var goal_todoNew = new goal_todo
                        {
                            idGoal = idGoal,
                            idTodo = todoModel.ID
                        };
                        entities.Entry(goal_todoNew).State = EntityState.Added;
                        todo.goal_todo.Add(goal_todoNew);
                    }
                    /* Remove Todo-Todo mapping that no longer exist */
                    entities.goal_todo.RemoveRange(goaltodo_Removed);
                    #endregion

                    #region Routine
                    /* Loop though all the current Routine_Todo mappings. 
                     * If the mapping already exists, remove the RoutineID from the list of ID's to add
                     * If the mapping no longer exists, add the Routine-Todo mapping to the list of mappings to be removed */
                    var routineIDs_Added = new List<int>(todoModel.RoutineIDs.Distinct());
                    var routinetodos_Removed = new List<routine_todo>();
                    entities.routine_todo.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(routine_todo =>
                    {
                        if (todoModel.RoutineIDs.Contains(routine_todo.idRoutine))
                            routineIDs_Added.Remove(routine_todo.idRoutine); // Remove RoutineID from list of IDs that need to be mapped
                        else
                            routinetodos_Removed.Add(routine_todo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Routine-Todo mappings that don't already exist */
                    foreach (var idRoutine in routineIDs_Added)
                    {
                        var routine_todoNew = new routine_todo
                        {
                            idRoutine = idRoutine,
                            idTodo = todoModel.ID
                        };
                        entities.Entry(routine_todoNew).State = EntityState.Added;
                        todo.routine_todo.Add(routine_todoNew);
                    }
                    /* Remove Routine-Todo mapping that no longer exist */
                    entities.routine_todo.RemoveRange(routinetodos_Removed);
                    #endregion

                    #region Type
                    /* Loop though all the current Todo-Type mappings. 
                     * If the mapping already exists, remove the TypeID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Type mapping to the list of mappings to be removed */
                    var typeIDs_Added = new List<int>(todoModel.TypeIDs.Distinct());
                    var todotype_Removed = new List<todo_type>();
                    entities.todo_type.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(todo_type =>
                    {
                        if (todoModel.TypeIDs.Contains(todo_type.idType))
                            typeIDs_Added.Remove(todo_type.idType); // Remove TypeID from list of IDs that need to be mapped
                        else
                            todotype_Removed.Add(todo_type); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Type mappings that don't already exist */
                    foreach (var idType in typeIDs_Added)
                    {
                        var todo_typeNew = new todo_type
                        {
                            idTodo = todoModel.ID,
                            idType = idType
                        };
                        entities.Entry(todo_typeNew).State = EntityState.Added;
                        todo.todo_type.Add(todo_typeNew);
                    }
                    /* Remove Todo-Type mapping that no longer exist */
                    entities.todo_type.RemoveRange(todotype_Removed);
                    #endregion

                    #region Photo
                    /* Loop though all the current Todo-Photo mappings. 
                     * If the mapping already exists, remove the PhotoID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Photo mapping to the list of mappings to be removed */
                    var photoIDs_Added = new List<int>(todoModel.PhotoIDs.Distinct());
                    var todophoto_Removed = new List<todo_photo>();
                    entities.todo_photo.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(todo_photo =>
                    {
                        if (todoModel.PhotoIDs.Contains(todo_photo.idPhoto))
                            photoIDs_Added.Remove(todo_photo.idPhoto); // Remove PhotoID from list of IDs that need to be mapped
                        else
                            todophoto_Removed.Add(todo_photo); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Photo mappings that don't already exist */
                    foreach (var idPhoto in photoIDs_Added)
                    {
                        var todo_photoNew = new todo_photo
                        {
                            idTodo = todoModel.ID,
                            idPhoto = idPhoto
                        };
                        entities.Entry(todo_photoNew).State = EntityState.Added;
                        todo.todo_photo.Add(todo_photoNew);
                    }
                    /* Remove Todo-Photo mapping that no longer exist */
                    entities.todo_photo.RemoveRange(todophoto_Removed);
                    #endregion

                    #region Location
                    /* Loop though all the current Todo-Location mappings. 
                     * If the mapping already exists, remove the LocationID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Location mapping to the list of mappings to be removed */
                    var locationIDs_Added = new List<int>(todoModel.LocationIDs.Distinct());
                    var todolocation_Removed = new List<todo_location>();
                    entities.todo_location.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(todo_location =>
                    {
                        if (todoModel.LocationIDs.Contains(todo_location.idLocation))
                            locationIDs_Added.Remove(todo_location.idLocation); // Remove LocationID from list of IDs that need to be mapped
                        else
                            todolocation_Removed.Add(todo_location); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Location mappings that don't already exist */
                    foreach (var idLocation in locationIDs_Added)
                    {
                        var todo_locationNew = new todo_location
                        {
                            idTodo = todoModel.ID,
                            idLocation = idLocation
                        };
                        entities.Entry(todo_locationNew).State = EntityState.Added;
                        todo.todo_location.Add(todo_locationNew);
                    }
                    /* Remove Todo-Location mapping that no longer exist */
                    entities.todo_location.RemoveRange(todolocation_Removed);
                    #endregion

                    #region Note
                    /* Loop though all the current Todo-Note mappings. 
                     * If the mapping already exists, remove the NoteID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Note mapping to the list of mappings to be removed */
                    var noteIDs_Added = new List<int>(todoModel.NoteIDs.Distinct());
                    var todonote_Removed = new List<todo_note>();
                    entities.todo_note.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(todo_note =>
                    {
                        if (todoModel.NoteIDs.Contains(todo_note.idNote))
                            noteIDs_Added.Remove(todo_note.idNote); // Remove NoteID from list of IDs that need to be mapped
                        else
                            todonote_Removed.Add(todo_note); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Note mappings that don't already exist */
                    foreach (var idNote in noteIDs_Added)
                    {
                        var todo_noteNew = new todo_note
                        {
                            idTodo = todoModel.ID,
                            idNote = idNote
                        };
                        entities.Entry(todo_noteNew).State = EntityState.Added;
                        todo.todo_note.Add(todo_noteNew);
                    }
                    /* Remove Todo-Note mapping that no longer exist */
                    entities.todo_note.RemoveRange(todonote_Removed);
                    #endregion

                    #region Tag
                    /* Loop though all the current Todo-Tag mappings. 
                     * If the mapping already exists, remove the TagID from the list of ID's to add
                     * If the mapping no longer exists, add the Todo-Tag mapping to the list of mappings to be removed */
                    var tagIDs_Added = new List<int>(todoModel.TagIDs.Distinct());
                    var todotag_Removed = new List<todo_tag>();
                    entities.todo_tag.Where(x => x.idTodo == todoModel.ID).ToList().ForEach(todo_tag =>
                    {
                        if (todoModel.TagIDs.Contains(todo_tag.idTag))
                            tagIDs_Added.Remove(todo_tag.idTag); // Remove TagID from list of IDs that need to be mapped
                        else
                            todotag_Removed.Add(todo_tag); // Queue mapping to be removed if it already exists
                    });
                    /* Add Todo-Tag mappings that don't already exist */
                    foreach (var idTag in tagIDs_Added)
                    {
                        var todo_tagNew = new todo_tag
                        {
                            idTodo = todoModel.ID,
                            idTag = idTag,
                        };
                        entities.Entry(todo_tagNew).State = EntityState.Added;
                        todo.todo_tag.Add(todo_tagNew);
                    }
                    /* Remove Todo-Tag mapping that no longer exist */
                    entities.todo_tag.RemoveRange(todotag_Removed);
                    #endregion

                    entities.Entry(originalTodo).CurrentValues.SetValues(todo);
                    entities.Entry(originalTodo).State = EntityState.Modified;
                    entities.SaveChanges();

                    return CoachMapper.Map<TodoModel>(originalTodo);
                }
                else
                    return null;
            }
        }

        public List<TodoModel> UpdateTodos(List<TodoModel> todoModels)
        {
            if (todoModels == null) return null;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var updatedModels = new List<TodoModel>();

                foreach (var todoModel in todoModels)
                    updatedModels.Add(this.UpdateTodo(todoModel));

                return updatedModels;
            }
        }

        public void DeleteTodo(int id)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todo = entities.todoes.SingleOrDefault(x => x.id == id);

                if (todo != null)
                {
                    entities.todoes.Remove(todo);
                    entities.SaveChanges();
                }
            }
        }

        public void DeleteTodos(List<int> ids)
        {
            if (ids == null) return;

            using (coachdevEntities entities = new coachdevEntities())
            {
                var todos = entities.todoes.Where(x => ids.Contains(x.id));

                entities.todoes.RemoveRange(todos);
                entities.SaveChanges();
            }
        }

        #region Planner
        public string GetGoogleTaskIDForTodo(int idTodo, DateTime dueDate)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                //var sfs = entities.todo_eventtask.ToList();
                //var todo_eventtask = entities.todo_eventtask.SingleOrDefault(x => x.idTodo == idTodo);
                //var todo_eventtask2 = entities.todo_eventtask.SingleOrDefault(x => x.idTodo == idTodo)?.eventtask.idGoogleTask;
                return entities.todo_eventtask.SingleOrDefault(x => x.idTodo == idTodo && x.eventtask.startDateTime == dueDate)?.eventtask.idGoogleTask;
            }
        }

        public void ScheduleTodo(int idTodo, DateTime scheduledDateTime, string idGoogleTask = null)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todo = entities.todoes.SingleOrDefault(x => x.id == idTodo);
                if (todo == null)
                    throw new ArgumentNullException("No todo exists with the ID " + idTodo);

                /* If Event Task exists, update datetime, else create new Event Task */
                eventtask eventtask = entities.todo_eventtask.SingleOrDefault(x => x.idTodo == idTodo && x.eventtask.idGoogleTask == idGoogleTask)?.eventtask;
                if (eventtask == null)
                {
                    eventtask = new eventtask
                    {
                        text = todo.text,
                        startDateTime = scheduledDateTime,
                        idGoogleTask = idGoogleTask,
                        isVisible = 1,
                        isActive = 1
                    };
                    entities.eventtasks.Add(eventtask);

                    var todo_eventtask = new todo_eventtask
                    {
                        idTodo = idTodo,
                        eventtask = eventtask,
                        isVisible = 1,
                        isActive = 1
                    };
                    entities.todo_eventtask.Add(todo_eventtask);
                }
                else
                {
                    eventtask.startDateTime = scheduledDateTime;
                    entities.Entry(eventtask).State = EntityState.Modified;
                }

                entities.SaveChanges();
            }
        }

        public void SetTodoCompletion(int idEventTask, bool isComplete)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var eventtask = entities.eventtasks.SingleOrDefault(x => x.id == idEventTask);
                if (eventtask == null)
                    throw new ArgumentNullException("No event task exists with the ID " + idEventTask);

                eventtask.isComplete = Convert.ToSByte(isComplete);
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Google Tasks can be marked complete in other applications so it needs to update Event Task for any potential changes in Due Date or Completion Status
        /// </summary>
        /// <param name="idTodo"></param>
        /// <param name="googleTaskModel"></param>
        /// <returns>Whether or not there was a change</returns>
        public bool SyncGoogleTaskWithEventTask(int idTodo, GoogleTaskModel googleTaskModel, bool isAttempted = false)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var eventtask = entities.todo_eventtask.FirstOrDefault(x => x.todo.id == idTodo 
                                                                            && x.eventtask.idGoogleTask == googleTaskModel.Id 
                                                                            && x.eventtask.startDateTime == googleTaskModel.ScheduledDate)?.eventtask;
                if (eventtask == null)
                    throw new ArgumentNullException($"No event task exists for Todo - {googleTaskModel.Text} with ID: {idTodo} and idGoogleTask: {googleTaskModel.Id}");

                var updated = false;
                /* Check that the Due Date is correct, updated if not */
                if (eventtask.text != googleTaskModel.Title)
                {
                    eventtask.text = googleTaskModel.Title;
                    updated = true;
                }
                /* Check that the Due Date is correct, updated if not */
                if (eventtask.startDateTime != googleTaskModel.ScheduledDate)
                {
                    eventtask.startDateTime = googleTaskModel.ScheduledDate;
                    updated = true;
                }
                /* Check that the Completion Status is correct, update if not */
                if (Convert.ToBoolean(eventtask.isComplete) != googleTaskModel.IsComplete)
                {
                    eventtask.isComplete = Convert.ToSByte(googleTaskModel.IsComplete);
                    updated = true;
                }
                /* Check that the Completion Status is correct, update if not */
                if (isAttempted)
                {
                    eventtask.text = "[Attempted] " + eventtask.text;
                    eventtask.isAttempted = 1;
                    updated = true;
                }

                /* If updated, save changes */
                if (updated)
                {
                    entities.Entry(eventtask).State = EntityState.Modified;
                    entities.SaveChanges();
                }

                return updated;
            }
        }

        /// <summary>
        /// Delete Event Task for Todo that is mapped to a Google Task and return thre ID of the Google Task
        /// </summary>
        /// <param name="idTodo"></param>
        /// <returns>ID of Google Task mapped to Todo</returns>
        public string DeleteGoogleTaskEventTask(int idTodo, DateTime date)
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var eventtask = entities.todo_eventtask.FirstOrDefault(x => x.todo.id == idTodo && x.eventtask.startDateTime == date).eventtask;
                if (eventtask == null)
                    throw new ArgumentNullException($"No event task exists for Todo with ID {idTodo} on {date.ToShortDateString()}");

                var idGoogleTask = eventtask.idGoogleTask;

                entities.eventtasks.Remove(eventtask);
                entities.SaveChanges();

                return idGoogleTask;
            }
        }
        #endregion


        #region TEMP - Planner Day
        public List<TodoModel> GetActiveTodos()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var todos = entities.todoes
                    .Include("inventoryitem_todo.inventoryitem")
                    .Include("goal_todo.goal")
                    .Include("routine_todo.routine")
                    .Include("todo_parent.parent")
                    .Include("todo_child.child")
                    .Include("todo_time.time")
                    .Include("todo_repeat.repeat")
                    .Include("todo_eventtask.eventtask")
                    .ToList()
                    .Where(x => x.isActive == 1).ToList();

                return CoachMapper.Map<List<TodoModel>>(todos);
            }
        }

        public List<TodoModel> GetActiveTodos(DateTime date, int paddingWeeks = 4)
        {
            //var startDate = date.AddDays(-paddingWeeks * 7).Date;
            //var endDate = date.AddDays(paddingWeeks * 7).Date;


            using (coachdevEntities entities = new coachdevEntities())
            {
                var todoQuery = entities.todoes
                    .Include("inventoryitem_todo.inventoryitem")
                    .Include("goal_todo.goal")
                    .Include("routine_todo.routine")
                    .Include("todo_parent.parent")
                    .Include("todo_child.child")
                    .Include("todo_time.time")
                    .Include("todo_repeat.repeat")
                    .Include("todo_eventtask.eventtask");
                
                var todos = (from todo in todoQuery
                              join todo_time in entities.todo_time on todo equals todo_time.todo
                              join time in entities.times on todo_time.time equals time
                              join endpoint in entities.types on time.idEndpoint equals endpoint.id
                              //where time.datetime >= startDate && time.datetime <= endDate
                              where time.datetime <= date && endpoint.id == (int)Endpoints.Start
                             select todo).ToList();

                return CoachMapper.Map<List<TodoModel>>(todos);
            }
        }

        // Todo: THIS SHOULDN'T BE USED AFTER MOVING OUT OF CENTURY SKYLINE AUG 2020
        public int HardCodeTimeFor_SUMMERMOVEOUT_Goals()
        {
            using (coachdevEntities entities = new coachdevEntities())
            {
                var newTime = new time
                {
                    datetime = new DateTime(2020, 8, 1, 0, 0, 0),
                    idType = 80,
                    idEndpoint = 84,
                    idMoment = 87,
                    isActive = 1
                };

                entities.times.Add(newTime);
                entities.SaveChanges();

                return newTime.id;
            }
        }
        #endregion
    }
}
