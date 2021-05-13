using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoachModel._App._Universal;
using Coach.ServiceOG._Data._CoachModel;
using AutoMapper;
using CoachModel._ViewModel._Items;
using CoachModel._App._Time;
using CoachModel._Planner;
using Newtonsoft.Json;

namespace Coach.ServiceOG._Items
{
    public class ItemsService
    {
        coachEntities entities;

        public ItemsService()
        {
            this.entities = new coachEntities();
        }

        public ItemsVM GetItemsVM()
        {
            ItemsVM ItemsVM = new ItemsVM
            {
                Types = GetTypes(),
                Mediums = GetMediums(),
                Deadlines = GetDeadlines(),
                Timeframes = GetTimeframes()
            };

            return ItemsVM;
        }

        public List<CoachModel._App._Universal.Type> GetTypes()
        {
            var Types = new List<CoachModel._App._Universal.Type>();
            foreach (var type in entities.types)
            {
                var Type = new CoachModel._App._Universal.Type();
                Types.Add(Type);

                /* Map database model to app model */
                Mapper.Map(type, Type);
            }
            return Types;
        }

        public List<Medium> GetMediums()
        {
            var Mediums = new List<Medium>();
            foreach (var medium in entities.media)
            {
                var Medium = new Medium();
                Mediums.Add(Medium);

                /* Map database model to app model */
                Mapper.Map(medium, Medium);
            }
            return Mediums;
        }

        public List<Deadline> GetDeadlines()
        {
            var Deadlines = new List<Deadline>();
            foreach (var deadline in entities.deadlines)
            {
                var Deadline = new Deadline();
                Deadlines.Add(Deadline);

                /* Map database model to app model */
                Mapper.Map(deadline, Deadline);
            }
            return Deadlines;
        }

        public List<Timeframe> GetTimeframes()
        {
            var Timeframes = new List<Timeframe>();
            foreach (var timeframe in entities.timeframes)
            {
                var Timeframe = new Timeframe();
                Timeframes.Add(Timeframe);

                /* Map database model to app model */
                Mapper.Map(timeframe, Timeframe);
            }
            return Timeframes;
        }

        public int SaveTodoItem(TodoItem TodoItem)
        {
            var UpdateDT = (TodoItem.UpdatedAt != null) ? TodoItem.UpdatedAt : DateTime.Now; // Update time

            /* Create todo item */
            var todoitem_New = new todoitem
            {
                title = TodoItem.Title,
                description = TodoItem.Description,
                points = TodoItem.Points,
                //position = 
                percentComplete = 0,
                estimatedTime = TodoItem.EstimatedTime,
                idDeadline = TodoItem.idDeadline,
                //idTime = 
                isGroup = TodoItem.IsGroup,
                isComplete = TodoItem.IsComplete,
                isVisible = TodoItem.IsVisible,
                isActive = (bool)TodoItem.IsActive,
                updatedAt = UpdateDT
            };
            entities.todoitems.Add(todoitem_New);

            /* Map helper relationships */
            foreach (var typeID in TodoItem.TypeIDs)
            {
                var todoitem_type_New = new todoitem_type
                {
                    todoitem = todoitem_New,
                    idType = typeID,
                    updatedAt = UpdateDT,
                };
                entities.todoitem_type.Add(todoitem_type_New);
            }
            foreach (var mediumID in TodoItem.MediumIDs)
            {
                var todoitem_medium_New = new todoitem_medium
                {
                    todoitem = todoitem_New,
                    idMedium = mediumID,
                    updatedAt = UpdateDT
                };
                entities.todoitem_medium.Add(todoitem_medium_New);
            }

            /* Create new time */
            var time_New = new time
            {
                startDate = TodoItem.TimeClass.StartDate,
                startTime = TodoItem.TimeClass.StartTime,
                endDate = TodoItem.TimeClass.EndDate,
                endTime = TodoItem.TimeClass.EndTime,
                updatedAt = UpdateDT,
            };
            todoitem_New.time = time_New;
            entities.times.Add(time_New);

            /* Create new repeats */
            foreach (var repeat in TodoItem.Repeats)
            {
                var repeat_New = new repeat
                {
                    idTimeframe = repeat.idTimeframe,
                    interval = repeat.interval,
                    frequency = repeat.frequency,
                    updatedAt = UpdateDT,
                };
                entities.repeats.Add(repeat_New);

                foreach (var repeat_DayOfWeek in repeat.Repeat_DayOfWeeks)
                {
                    var repeat_dayofweek_New = new repeat_dayofweek
                    {
                        repeat = repeat_New,
                        dayOfWeek = repeat_DayOfWeek.DayOfWeek,
                        updatedAt = UpdateDT
                    };
                    entities.repeat_dayofweek.Add(repeat_dayofweek_New);
                }

                foreach (var repeat_DayOfMonth in repeat.Repeat_DayOfMonths)
                {
                    var repeat_dayofmonth_New = new repeat_dayofmonth
                    {
                        repeat = repeat_New,
                        dayOfMonth = repeat_DayOfMonth.DayOfMonth,
                        updatedAt = UpdateDT
                    };
                    entities.repeat_dayofmonth.Add(repeat_dayofmonth_New);
                }

                foreach (var repeat_Month in repeat.Repeat_Months)
                {
                    var repeat_month_New = new repeat_month
                    {
                        repeat = repeat_New,
                        month = repeat_Month.Month,
                        updatedAt = UpdateDT
                    };
                    entities.repeat_month.Add(repeat_month_New);
                }
            }

            /* Map TodoItem to Inventory Items */
            foreach (var inventoryItemID in TodoItem.InventoryItemIDs)
            {
                var inventoryitem_todoitem_New = new inventoryitem_todoitem
                {
                    idInventoryItem = inventoryItemID,
                    idToDoItem = TodoItem.ID,
                    updatedAt = UpdateDT
                };
                entities.inventoryitem_todoitem.Add(inventoryitem_todoitem_New);
            }

            /* Map TodoItem to Goals */
            foreach (var goalID in TodoItem.GoalIDs)
            {
                var goal_todoitem_New = new goal_todoitem
                {
                    idGoal = goalID,
                    idToDoItem = TodoItem.ID,
                    updatedAt = UpdateDT
                };
                entities.goal_todoitem.Add(goal_todoitem_New);
            }

            /* Map TodoItem to Routines */
            foreach (var routineID in TodoItem.RoutineIDs)
            {
                var routine_todoitem_New = new routine_todoitem
                {
                    idRoutine = routineID,
                    idToDoItem = TodoItem.ID,
                    updatedAt = UpdateDT
                };
                entities.routine_todoitem.Add(routine_todoitem_New);
            }
            
            /* Map TodoItem to Parent TodoItems */
            foreach (var parentID in TodoItem.ParentIDs)
            {
                var todoitem_todoitem_New = new todoitem_todoitem
                {
                    idParentTodoItem = parentID,
                    idTodoItem = TodoItem.ID,
                    updatedAt = UpdateDT
                };
                entities.todoitem_todoitem.Add(todoitem_todoitem_New);
            }
            /* Map TodoItem to Child TodoItems */
            foreach (var childID in TodoItem.ChildIDs)
            {
                var todoitem_todoitem_New = new todoitem_todoitem
                {
                    idParentTodoItem = TodoItem.ID,
                    idTodoItem = childID,
                    updatedAt = UpdateDT
                };
                entities.todoitem_todoitem.Add(todoitem_todoitem_New);
            }
            
            return -1;
        }

        public void SaveGoal(string goalJSON)
        {
            var goalNew = JsonConvert.DeserializeObject<_Data._JSONModel.Goal>(goalJSON);

            var jsonEntities = new _Data._JSONModel.jsonEntities();
            var goal = jsonEntities.Goals.SingleOrDefault(x => x.idGoal == goalNew.idGoal);

            goal.title = goalNew.title;
            goal.repetition = goalNew.repetition;
            goal.type = goalNew.type;
            goal.timeframe = goalNew.timeframe;
            goal.deadline = goalNew.deadline;
            goal.explaination = goalNew.explaination;
            goal.idTodoItems = goalNew.idTodoItems;
            goal.idParents = goalNew.idParents;
            goal.active = goalNew.active;

            jsonEntities.SaveChanges();
        }
    }
}
