using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Coach.ServiceOG._Data._CoachModel;
using CoachModel._Inventory;
using CoachModel._Planner;
using CoachModel._App._Time;
using CoachModel._App._Helper;
using CoachModel._App._Task;
using System.Data;
using CoachModel._ViewModel._Planner;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;
using CoachModel._ViewModel._Items;
using CoachModel._Inventory._Physical._Nutrition;
using EntityState = System.Data.Entity.EntityState;
using CoachModel._Inventory._Log;

namespace Coach.ServiceOG._Planner
{
    public class PlannerService
    {
        coachogEntities entities;
        IMapper CoachMapper;

        public PlannerService()
        {
            this.entities = new _Data._CoachModel.coachogEntities();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<char, int>().ConvertUsing(c => Convert.ToInt32(c));
                cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
                cfg.CreateMap<string, double>().ConvertUsing(s => Convert.ToDouble(s));
                #region Inventory

                cfg.CreateMap<inventoryitem_view, InventoryItem>()
                    .ForMember(g => g.GoalIDs_Concat, m => m.MapFrom(g_v => g_v.goalIDs))
                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))
                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<inventoryitemspotlight, SpotlightMapping>();

                cfg.CreateMap<goal_view, Goal>()
                    .ForMember(g => g.Types_Concat, m => m.MapFrom(g_v => g_v.types))
                    .ForMember(g => g.Timeframes_Concat, m => m.MapFrom(g_v => g_v.timeframes))
                    .ForMember(g => g.RepeatIDs_Concat, m => m.MapFrom(g_v => g_v.repeatIDs))

                    .ForMember(g => g.ParentIDs_Concat, m => m.MapFrom(g_v => g_v.parentIDs))
                    .ForMember(g => g.ChildIDs_Concat, m => m.MapFrom(g_v => g_v.childIDs))

                    .ForMember(g => g.InventoryItemIDs_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs))
                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))

                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                #endregion

                #region Planner

                cfg.CreateMap<todoitem_view, TodoItem>()
                    .ForMember(g => g.Types_Concat, m => m.MapFrom(g_v => g_v.types))
                    .ForMember(g => g.Timeframes_Concat, m => m.MapFrom(g_v => g_v.timeframes))
                    .ForMember(g => g.RepeatIDs_Concat, m => m.MapFrom(g_v => g_v.repeatIDs))

                    .ForMember(g => g.ParentIDs_Concat, m => m.MapFrom(g_v => g_v.parentIDs))
                    .ForMember(g => g.ChildIDs_Concat, m => m.MapFrom(g_v => g_v.childIDs))

                    .ForMember(g => g.InventoryItemIDs_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs))
                    .ForMember(g => g.GoalIDs_Concat, m => m.MapFrom(g_v => g_v.goalIDs))
                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))

                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<routine_view, Routine>()
                    .ForMember(g => g.Types_Concat, m => m.MapFrom(g_v => g_v.types))
                    .ForMember(g => g.RepeatIDs_Concat, m => m.MapFrom(g_v => g_v.repeatIDs))

                    .ForMember(g => g.InventoryItemIDs_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs))
                    .ForMember(g => g.GoalIDs_Concat, m => m.MapFrom(g_v => g_v.goalIDs))
                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))

                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))

                    .ForMember(g => g.InventoryItemIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_TodoItem))
                    .ForMember(g => g.GoalIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.goalIDs_TodoItem))

                    .ForMember(g => g.TaskIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.taskIDs_TodoItem))
                    .ForMember(g => g.EventIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.eventIDs_TodoItem))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<task_view, Task>()
                    .ForMember(g => g.EventIDs_TodoItemRoutine_Concat, m => m.MapFrom(g_v => g_v.eventIDs_TodoItemRoutine))
                    .ForMember(g => g.InventoryItemIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_TodoItem))
                    .ForMember(g => g.InventoryItemIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_Routine))
                    .ForMember(g => g.InventoryItemIDs_RoutineTodoItem_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_RoutineTodoItem))

                    .ForMember(g => g.GoalIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.goalIDs_TodoItem))
                    .ForMember(g => g.GoalIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.goalIDs_Routine))
                    .ForMember(g => g.GoalIDs_RoutineTodoItem_Concat, m => m.MapFrom(g_v => g_v.goalIDs_RoutineTodoItem))

                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                    .ForMember(g => g.TodoItemIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs_Routine))
                    .ForMember(g => g.TodoItemIDs_TodoItemRoutine_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs_TodoItemRoutine))

                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))
                    .ForMember(g => g.RoutineIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.routineIDs_TodoItem))

                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<event_view, Event>()
                    .ForMember(g => g.InventoryItemIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_TodoItem))
                    .ForMember(g => g.InventoryItemIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_Routine))
                    .ForMember(g => g.InventoryItemIDs_RoutineTodoItem_Concat, m => m.MapFrom(g_v => g_v.inventoryItemIDs_RoutineTodoItem))

                    .ForMember(g => g.GoalIDs_TodoItem_Concat, m => m.MapFrom(g_v => g_v.goalIDs_TodoItem))
                    .ForMember(g => g.GoalIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.goalIDs_Routine))
                    .ForMember(g => g.GoalIDs_RoutineTodoItem_Concat, m => m.MapFrom(g_v => g_v.goalIDs_RoutineTodoItem))

                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                    .ForMember(g => g.TodoItemIDs_Routine_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs_Routine))

                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))

                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.MealIDs_Concat, m => m.MapFrom(g_v => g_v.mealIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<repeat_view, Repeat>()
                    .ForMember(g => g.Repeat_DayOfWeekIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_DayOfWeekIDs))
                    .ForMember(g => g.Repeat_DayOfMonthIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_DayOfMonthIDs))
                    .ForMember(g => g.Repeat_MonthIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_MonthIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<repeat_dayofmonth, Repeat_DayOfMonth>();
                cfg.CreateMap<repeat_dayofweek, Repeat_DayOfWeek>();



                cfg.CreateMap<@event, Event>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<time, Time>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                #endregion


                cfg.CreateMap<type, CoachModel._App._Universal.Type>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<medium, CoachModel._App._Universal.Medium>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<deadline, CoachModel._App._Universal.Deadline>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<timeframe, Timeframe>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();


                #region Food Tracker 

                cfg.CreateMap<task_mealfooditemnutrient_view, MealItem>()
                    .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                /* Meal Chart */
                cfg.CreateMap<task_mealfooditemnutrient_view, FoodItem>()
                    .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.foodName))
                    .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                    .ForMember(g => g.DateTime, m => m.MapFrom(g_v => g_v.startDateTime))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<fooditem, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.name))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<MealItem, fooditem>()
                    .ForMember(g => g.name, m => m.MapFrom(g_v => g_v.FoodName))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<waterlog_mealtask_view, WaterLog>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<waterlog, WaterLog>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<Food, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.food_name))
                    .ForMember(g => g.Quantity, m => m.MapFrom(g_v => g_v.serving_qty))
                    .ForMember(g => g.Unit, m => m.MapFrom(g_v => g_v.serving_unit))
                    .ForMember(g => g.Calaries, m => m.MapFrom(g_v => g_v.nf_calories))
                    .ForMember(g => g.Carbohydrates, m => m.MapFrom(g_v => g_v.nf_total_carbohydrate))
                    .ForMember(g => g.Protein, m => m.MapFrom(g_v => g_v.nf_protein))
                    .ForMember(g => g.Fat, m => m.MapFrom(g_v => g_v.nf_total_fat))
                    .ForMember(g => g.Cholesterol, m => m.MapFrom(g_v => g_v.nf_cholesterol))
                    .ForMember(g => g.SaturatedFat, m => m.MapFrom(g_v => g_v.nf_saturated_fat))
                    .ForMember(g => g.Fiber, m => m.MapFrom(g_v => g_v.nf_dietary_fiber))
                    .ForMember(g => g.Potassium, m => m.MapFrom(g_v => g_v.nf_potassium))
                    .ForMember(g => g.Sodium, m => m.MapFrom(g_v => g_v.nf_sodium))
                    .ForMember(g => g.Sugars, m => m.MapFrom(g_v => g_v.nf_sugars))
                    .ForMember(g => g.Phosphorus, m => m.MapFrom(g_v => g_v.nf_p))
                    .ForMember(g => g.ThumbURL, m => m.MapFrom(g_v => g_v.photo.thumb))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<FoodBranded, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.food_name))
                    .ForMember(g => g.Quantity, m => m.MapFrom(g_v => g_v.serving_qty))
                    .ForMember(g => g.Unit, m => m.MapFrom(g_v => g_v.serving_unit))
                    .ForMember(g => g.Calaries, m => m.MapFrom(g_v => g_v.nf_calories))
                    .ForMember(g => g.Carbohydrates, m => m.MapFrom(g_v => g_v.nf_total_carbohydrate))
                    .ForMember(g => g.Protein, m => m.MapFrom(g_v => g_v.nf_protein))
                    .ForMember(g => g.Fat, m => m.MapFrom(g_v => g_v.nf_total_fat))
                    .ForMember(g => g.Cholesterol, m => m.MapFrom(g_v => g_v.nf_cholesterol))
                    .ForMember(g => g.SaturatedFat, m => m.MapFrom(g_v => g_v.nf_saturated_fat))
                    .ForMember(g => g.Fiber, m => m.MapFrom(g_v => g_v.nf_dietary_fiber))
                    .ForMember(g => g.Potassium, m => m.MapFrom(g_v => g_v.nf_potassium))
                    .ForMember(g => g.Sodium, m => m.MapFrom(g_v => g_v.nf_sodium))
                    .ForMember(g => g.Sugars, m => m.MapFrom(g_v => g_v.nf_sugars))
                    .ForMember(g => g.Phosphorus, m => m.MapFrom(g_v => g_v.nf_p))
                    .ForMember(g => g.ThumbURL, m => m.MapFrom(g_v => g_v.photo.thumb))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();


                #endregion

                #region Inventory ItemTracker

                /* Log Item */
                cfg.CreateMap<logitemfield_view, LogItem>()
                    .ForMember(g => g.InventoryItemName, m => m.MapFrom(g_v => g_v.inventoryItem))
                    .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.logItem))
                    .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_LogItem))
                    .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_LogItem))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                /* Log Item Field */
                cfg.CreateMap<logitemfield_view, ItemField>()
                    .ForMember(g => g.idType, m => m.MapFrom(g_v => g_v.idType_Field))
                    .ForMember(g => g.idType_Data, m => m.MapFrom(g_v => g_v.idType_Data))
                    .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.field))
                    .ForMember(g => g.DateType, m => m.MapFrom(g_v => g_v.dataType))
                    .ForMember(g => g.Type, m => m.MapFrom(g_v => g_v.fieldType))
                    .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_Field))
                    .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_Field))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                /* Log Item Field Value */
                cfg.CreateMap<logentry_logitemfield, FieldValue>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                #endregion
            });

            CoachMapper = config.CreateMapper();
        }

        public PlannerVM GetPlannerViewModel()
        {
            PlannerVM PlannerVM = new PlannerVM
            {
                //InventoryItems = GetInventoryItems(),
                SpotlightMappings = GetSpotlightMappings(),
                //Goals = GetGoals(),
                //TodoItems = GetTodoItems(),
                //Routines = GetRoutines(),
                //Tasks = GetTasks(),
                Timeframes = GetTimeframes()
                //Events = Events,
            };

            return PlannerVM;
        }

        public ItemsVM GetItemsVM()
        {
            ItemsVM ItemsVM = new ItemsVM
            {
                InventoryItems = GetInventoryItems(),
                Goals = GetGoals(),
                TodoItems = GetTodoItems(),
                Routines = GetRoutines(),
            };

            return ItemsVM;
        }

        public List<InventoryItem> GetInventoryItems()
        {
            var InventoryItems = new List<InventoryItem>();

            foreach (var inventoryitem_view in entities.inventoryitem_view)
            {
                var InventoryItem = new InventoryItem();
                InventoryItems.Add(InventoryItem);

                CoachMapper.Map(inventoryitem_view, InventoryItem);
            }

            return InventoryItems;
        }

        public List<SpotlightMapping> GetSpotlightMappings()
        {
            var SpotlightMappings = new List<SpotlightMapping>();

            foreach (var inventoryitemspotlight in entities.inventoryitemspotlights.ToList())
            {
                var SpotlightMapping = new SpotlightMapping();
                SpotlightMappings.Add(SpotlightMapping);

                /* Map database model to app model */
                //CoachMapper.CreateMap<inventoryitemspotlight, SpotlightMapping>();
                CoachMapper.Map(inventoryitemspotlight, SpotlightMapping);

                SpotlightMapping.TimeframeClass.idTimeframe = inventoryitemspotlight.idTimeframe; // Setting now allows you to find the Timeframe in the database
            }

            foreach (var spotlightMapping in SpotlightMappings)
            {
                spotlightMapping.InventoryItemString = InventoryItems.GetInventoryItem(spotlightMapping.idInventoryItem);
                //spotlightMapping.TimeframeString = Timeframes.GetTimeframe(spotlightMapping.Timeframe.idTimeframe);

                /* Create new Timeframe based on Repeat's Timeframe and map to Repeats TimeClass */
                var timeframe = entities.timeframes.SingleOrDefault(x => x.idTimeframe == spotlightMapping.TimeframeClass.idTimeframe);
                //Mapper.CreateMap<timeframe, Timeframe>();
                CoachMapper.Map(timeframe, spotlightMapping.TimeframeClass);
            }

            return SpotlightMappings;
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*--------------------------------------------- Get Items --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public List<Goal> GetGoals(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            var Goals = new List<Goal>();
            
            foreach (var goal_view in entities.goal_view)
            {
                var Goal = new Goal();
                Goals.Add(Goal);

                /* Map database model to app model */
                CoachMapper.Map(goal_view, Goal);
            }

            foreach (var goal in Goals)
            {
                foreach (var repeat_view in entities.repeat_view.Where(x => goal.RepeatIDs.Contains(x.idRepeat)).ToList())
                {
                    var Repeat = new Repeat();
                    goal.Repeats.Add(Repeat);

                    CoachMapper.Map(repeat_view, Repeat);

                    /* Create new Timeframe based on Repeat's Timeframe and map to Repeats TimeClass */
                    var timeframe = entities.timeframes.SingleOrDefault(x => x.idTimeframe == Repeat.idTimeframe);
                    //CoachMapper.CreateMap<timeframe, Timeframe>();
                    CoachMapper.Map(timeframe, Repeat.TimeframeClass);

                    if (Repeat.TimeframeClass.Repetition == "Yearly")
                    {

                    }
                    else if (Repeat.TimeframeClass.Repetition == "Monthly")
                    {
                        var repeat_dayofmonths = entities.repeat_dayofmonth.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();

                        foreach (var repeat_dayofmonth in repeat_dayofmonths)
                        {
                            var Repeat_DayOfMonth = new Repeat_DayOfMonth();
                            Repeat.Repeat_DayOfMonths.Add(Repeat_DayOfMonth);
                            Repeat.Repeat_DayOfMonthIDs.Add(repeat_dayofmonth.idRepeat_DayOfMonth);

                            //CoachMapper.CreateMap<repeat_dayofmonth, Repeat_DayOfMonth>();
                            CoachMapper.Map(repeat_dayofmonth, Repeat_DayOfMonth);
                        }
                    }
                    else if (Repeat.TimeframeClass.Repetition == "Weekly")
                    {
                        var repeat_dayofweeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();
                        foreach (var repeat_dayofweek in repeat_dayofweeks)
                        {
                            var Repeat_DayOfWeek = new Repeat_DayOfWeek();
                            Repeat.Repeat_DayOfWeeks.Add(Repeat_DayOfWeek);
                            Repeat.Repeat_DayOfWeekIDs.Add(repeat_dayofweek.idRepeat_DayOfWeek);

                            //CoachMapper.CreateMap<repeat_dayofweek, Repeat_DayOfWeek>();
                            CoachMapper.Map(repeat_dayofweek, Repeat_DayOfWeek);
                        }
                    }
                }
            }
            return Goals;
        }

        public void AddGoals()
        {

        }

        public List<TodoItem> GetTodoItems(bool isActive = true)
        {
            var todoitem_views = entities.todoitem_view.Where(x => x.idTodoItem > 1 && x.isActive == isActive).ToList();
            var TodoItems = this.CreateTodoItemModels(todoitem_views);
            return TodoItems;
        }

        public List<TodoItem> CreateTodoItemModels(List<todoitem_view> todoitem_views)
        {
            var TodoItems = new List<TodoItem>();

            /* Create new TodoItems based on database records */
            foreach (var todoitem_view in todoitem_views)
            {
                var TodoItem = new TodoItem();
                TodoItems.Add(TodoItem);

                CoachMapper.Map(todoitem_view, TodoItem);
            }

            foreach (var todoItem in TodoItems)
            {
                /* Time */
                foreach (var time in entities.times.Where(x => x.idTime == todoItem.idTime).ToList())
                {
                    todoItem.idTime = time.idTime;
                    
                    CoachMapper.Map(time, todoItem.TimeClass);
                }

                foreach (var repeat_view in entities.repeat_view.Where(x => todoItem.RepeatIDs.Contains(x.idRepeat)).ToList())
                {
                    var Repeat = new Repeat();
                    todoItem.Repeats.Add(Repeat);

                    CoachMapper.Map(repeat_view, Repeat);

                    /* Create new Timeframe based on Repeat's Timeframe and map to Repeats TimeClass */
                    var timeframe = entities.timeframes.SingleOrDefault(x => x.idTimeframe == Repeat.idTimeframe);
                    //CoachMapper.CreateMap<timeframe, Timeframe>();
                    CoachMapper.Map(timeframe, Repeat.TimeframeClass);

                    if (Repeat.TimeframeClass.Repetition == "Yearly")
                    {

                    }
                    else if (Repeat.TimeframeClass.Repetition == "Monthly")
                    {
                        var repeat_dayofmonths = entities.repeat_dayofmonth.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();

                        foreach (var repeat_dayofmonth in repeat_dayofmonths)
                        {
                            var Repeat_DayOfMonth = new Repeat_DayOfMonth();
                            Repeat.Repeat_DayOfMonths.Add(Repeat_DayOfMonth);
                            Repeat.Repeat_DayOfMonthIDs.Add(repeat_dayofmonth.idRepeat_DayOfMonth);

                            //CoachMapper.CreateMap<repeat_dayofmonth, Repeat_DayOfMonth>();
                            CoachMapper.Map(repeat_dayofmonth, Repeat_DayOfMonth);
                        }
                    }
                    else if (Repeat.TimeframeClass.Repetition == "Weekly")
                    {
                        var repeat_dayofweeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();
                        foreach (var repeat_dayofweek in repeat_dayofweeks)
                        {
                            var Repeat_DayOfWeek = new Repeat_DayOfWeek();
                            Repeat.Repeat_DayOfWeeks.Add(Repeat_DayOfWeek);
                            Repeat.Repeat_DayOfWeekIDs.Add(repeat_dayofweek.idRepeat_DayOfWeek);

                            //CoachMapper.CreateMap<repeat_dayofweek, Repeat_DayOfWeek>();
                            CoachMapper.Map(repeat_dayofweek, Repeat_DayOfWeek);
                        }
                    }
                }
            }

            return TodoItems;
        }

        public List<Routine> GetRoutines()
        {
            var Routines = new List<Routine>();

            /* Create new Routines based on database records */
            foreach (var routine_view in entities.routine_view.Where(x => x.idRoutine > 1 && x.isActive == true))
            {
                var Routine = new Routine();
                Routines.Add(Routine);

                CoachMapper.Map(routine_view, Routine);
            }

            foreach (var routine in Routines)
            {
                /* Time */
                foreach (var time in entities.times.Where(x => x.idTime == routine.idTime).ToList())
                {
                    routine.idTime = time.idTime;
                    
                    CoachMapper.Map(time, routine.TimeClass);
                }

                foreach (var repeat_view in entities.repeat_view.Where(x => routine.RepeatIDs.Contains(x.idRepeat)).ToList())
                {
                    var Repeat = new Repeat();
                    routine.Repeats.Add(Repeat);

                    CoachMapper.Map(repeat_view, Repeat);

                    /* Create new Timeframe based on Repeat's Timeframe and map to Repeats TimeClass */
                    var timeframe = entities.timeframes.SingleOrDefault(x => x.idTimeframe == Repeat.idTimeframe);
                    //CoachMapper.CreateMap<timeframe, Timeframe>();
                    CoachMapper.Map(timeframe, Repeat.TimeframeClass);

                    if (Repeat.TimeframeClass.Repetition == "Yearly")
                    {

                    }
                    else if (Repeat.TimeframeClass.Repetition == "Monthly")
                    {
                        var repeat_dayofmonths = entities.repeat_dayofmonth.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();

                        foreach (var repeat_dayofmonth in repeat_dayofmonths)
                        {
                            var Repeat_DayOfMonth = new Repeat_DayOfMonth();
                            Repeat.Repeat_DayOfMonths.Add(Repeat_DayOfMonth);
                            Repeat.Repeat_DayOfMonthIDs.Add(repeat_dayofmonth.idRepeat_DayOfMonth);

                            //CoachMapper.CreateMap<repeat_dayofmonth, Repeat_DayOfMonth>();
                            CoachMapper.Map(repeat_dayofmonth, Repeat_DayOfMonth);
                        }
                    }
                    else if (Repeat.TimeframeClass.Repetition == "Weekly")
                    {
                        var repeat_dayofweeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat_view.idRepeat).ToList();
                        foreach (var repeat_dayofweek in repeat_dayofweeks)
                        {
                            var Repeat_DayOfWeek = new Repeat_DayOfWeek();
                            Repeat.Repeat_DayOfWeeks.Add(Repeat_DayOfWeek);
                            Repeat.Repeat_DayOfWeekIDs.Add(repeat_dayofweek.idRepeat_DayOfWeek);

                            //CoachMapper.CreateMap<repeat_dayofweek, Repeat_DayOfWeek>();
                            CoachMapper.Map(repeat_dayofweek, Repeat_DayOfWeek);
                        }
                    }
                }
            }

            return Routines;
        }

        /*----------------------------------------------------------------------------------------------------*/
        /*--------------------------------------------- Get Misc ---------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        //public List<Meal> GetMeals(DateTime? StartDT = null, DateTime? EndDT = null)
        //{
        //    List<task_meal_fooditem_view> task_meal_fooditem_views;

        //    /* If there are start and end date-times, get views between start and end date-times */
        //    if (StartDT != null && EndDT != null)
        //    {
        //        task_meal_fooditem_views = entities.task_meal_fooditem_view.Where(x => x.startDateTime >= StartDT && x.endDateTime <= EndDT).ToList();
        //    }
        //    /* If there is a start date-time, get views after start date-time */
        //    else if (StartDT != null)
        //    {
        //        task_meal_fooditem_views = entities.task_meal_fooditem_view.Where(x => x.startDateTime >= StartDT).ToList();
        //    }
        //    /* If there is an end date-time, get views before end date-time */
        //    else if (EndDT != null)
        //    {
        //        task_meal_fooditem_views = entities.task_meal_fooditem_view.Where(x => x.endDateTime <= EndDT).ToList();
        //    }
        //    /* If there isn't a start or end date-time, get all event views */
        //    else
        //    {
        //        task_meal_fooditem_views = entities.task_meal_fooditem_view.ToList();
        //    }
            
        //    var Meals = this.CreateMealModels(task_meal_fooditem_views);
        //    return Meals;
        //}

        //public List<Meal> CreateMealModels(List<task_meal_fooditem_view> task_meal_fooditem_views)
        //{
        //    var Meals = new List<Meal>();

        //    foreach (var taskMealFoodItem in task_meal_fooditem_views)
        //    {
        //        Meal Meal;
        //        if (Meals.Select(x => x.idMeal).Contains(taskMealFoodItem.idMeal)) // If meal was already created 
        //        {
        //            Meal = Meals.SingleOrDefault(x => x.idMeal == taskMealFoodItem.idMeal); // Get meal
        //        }
        //        else
        //        {
        //            Meal = new Meal(taskMealFoodItem.idMeal, taskMealFoodItem.mealType, taskMealFoodItem.mealName, ); // Create meal
        //            Meals.Add(Meal); // Add new meal to collection
        //        }

        //        if (!Meal.TaskIDs.Contains(taskMealFoodItem.idTask))
        //        {
        //            Meal.TaskIDs.Add(taskMealFoodItem.idTask);
        //        }

        //        /* Creat new MealItem */
        //        var MealItem = new MealItem();
        //        Meal.MealItems.Add(MealItem);
        //        CoachMapper.Map(taskMealFoodItem, MealItem);
        //    }

        //    return Meals;
        //}

        /*----------------------------------------------------------------------------------------------------*/
        /*----------------------------------------------- Tasks ----------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public List<Task> GetTasks(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            List<task_view> task_views;

            /* If there are start and end date-times, get task views between start and end date-times */
            if (StartDT != null && EndDT != null) 
            {
                task_views = entities.task_view.Where(x => x.startDateTime >= StartDT && x.startDateTime <= EndDT).ToList();
            }
            /* If there is a start date-time, get task views after start date-time */
            else if (StartDT != null) 
            {
                task_views = entities.task_view.Where(x => x.startDateTime >= StartDT).ToList();
            }
            /* If there is a end date-time, get task views before end date-time */
            else if (EndDT != null) 
            {
                task_views = entities.task_view.Where(x => x.startDateTime <= EndDT).ToList();
            }
            /* If there isn't a start or end date-time, get all task views */
            else
            {
                task_views = entities.task_view.ToList();
            }
            
            var Tasks = this.GetTaskModels(task_views);
            return Tasks;
        }

        public List<Task> GetTaskModels(List<task_view> task_views)
        {
            var Tasks = new List<Task>();

            foreach (var task_view in task_views)
            {
                var Task = new Task();
                Tasks.Add(Task);

                CoachMapper.Map(task_view, Task);
            }

            return Tasks;
        }

        /* Refresh Tasks to relect any new updates to Goals, Todo Items, etc */
        public void RefreshTasks(DateTime StartDate, DateTime EndDate, bool shouldSave = false)
        {
            var TodoItems = this.GetTodoItems();
            var Routines = this.GetRoutines();

            this.RefreshTasks_Repetitive(TodoItems, StartDate, 3, true);
            this.RefreshTasks_Spotlight(TodoItems, StartDate, true);
            this.RefreshTasks(TodoItems, StartDate, true);
            this.RefreshTasks_Repetitive(Routines, StartDate, 3, true);
            this.RefreshTasks(Routines, StartDate, true);

            var Tasks = GetTasks();
            this.RefreshEvents(Tasks, true); // Refresh Events to relect any new updates to Tasks, etc

            var eventIDs = this.GetEvents().Select(x => x.idEvent).ToList();
            this.UpdateEventPercentComplete(eventIDs, true); // Refresh PercentagComplete for Events

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        /* Make sure there is a task for every TodoItem for the next __ */
        public void RefreshTasks_Repetitive(List<TodoItem> TodoItems, DateTime SeedDate, int months, bool shouldSave = false)
        {
            /* Exclude TodoItems that are in a Routine, their Task's will be refreshed with the Routine */
            var todoItemIDs_Excluded = new List<int>();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(entities.routine_todoitem.Select(x => x.idToDoItem)).ToList();

            /* Loop through TodoItems that are not marked as group, not active, and are in the exclude list, and create tasks */
            foreach (var todoItem in TodoItems.Where(x => !todoItemIDs_Excluded.Contains(x.idTodoItem) && x.IsGroup != true && x.IsActive == true))
            {
                var taskIDs_TodoItem = entities.task_todoitem.Where(y => y.idToDoItem == todoItem.idTodoItem).Select(x => x.idTask); // Task IDs for todoItem
                var tasks = entities.tasks.Where(x => taskIDs_TodoItem.Contains(x.idTask)).ToList();

                /* Get the times for the todoItem */
                Time TodoItemTime = new Time();
                if (todoItem.idTime != null)
                {
                    CoachMapper.Map(entities.times.Where(x => x.idTime == todoItem.idTime).Single(), TodoItemTime);
                }
                var EndDateTime_TodoItem = (todoItem.idTime != null && TodoItemTime.EndDate > new DateTime()) ? TodoItemTime.EndDate : new DateTime(3000, 12, 31); // Date when repetition for TodoItem ends. If no end date, set variable to date you will never see (this prevents you from having to deal with null dates)

                DateTime StartDateTime = new DateTime();
                DateTime EndDateTime = new DateTime();

                DateTime DateIterated; // Date that will get changed when looping through repetitinos

                // YOU HAVE TO SEE IF TASKS CHANGE DATES AFTER THE INTERVAL OR IF I SHOULD JUST CREATE A NEW ONE

                foreach (var repeat in todoItem.Repeats)
                {
                    // Get start and end dates based for repetition
                    // DONT FORGET TO CONSIDER FREQUENCY AND INTERVAL

                    var taskType = "TodoItem_Repetitive";
                    if (todoItem.Types.Contains(Types._errand))
                        taskType = "TodoItem_Errand_Repetitive";
                    else if (todoItem.Types.Contains(Types._tally))
                        taskType = "TodoItem_Tally_Repetitive";
                    else if (todoItem.Types.Contains(Types._inverse))
                        taskType = "TodoItem_Inverse_Repetitive";

                    if (repeat.TimeframeClass.Repetition == "Monthly")
                    {
                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day); // Set date iterated to original seed date

                        for (int i = 0; i < months; i++) // Loop through the next __ months including current (create Tasks for the next year)
                        {
                            /* If Todo Item's end date is before date iterated, break loop  */
                            if (EndDateTime_TodoItem < DateIterated)
                                break;

                            /* Get first and last day of the month */
                            var FirstDayOfMonth = new DateTime(DateIterated.Year, DateIterated.Month, 1);
                            var LastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

                            /* Determin Spotlight Inventory Item for DateIterated */
                            var idMonthTimeframe = Repetitions.RepetitionList.IndexOf("Monthly");
                            var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idMonthTimeframe && x.datetime == FirstDayOfMonth).Count() > 0) ?
                                entities.inventoryitemspotlights.Where(x => x.idTimeframe == idMonthTimeframe && x.datetime == FirstDayOfMonth).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == todoItem.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            /* If the type is Spotlight but is not in its spotlight month */
                            if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            {
                                continue;
                            }
                            else if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() > 0)
                            {
                                taskType = "TodoItem_Spotlight_Repetitive";
                            }

                            // Check Repeat_DayOfMonth for idRepeat
                            var repeat_DayOfMonths = entities.repeat_dayofmonth.Where(x => x.idRepeat == repeat.idRepeat).Select(y => y.idRepeat_DayOfMonth).ToList();
                            foreach (var repeat_DayOfMonth in repeat_DayOfMonths)
                            {
                                /* Get 'StartDate' based off of 'DateIterated' */
                                /* The start and end days will be the same since there is a specific date */
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, repeat_DayOfMonth); // Day is the only difference
                                EndDateTime = StartDateTime; // Start and end DateTime are the same since there is a set DayOfMonth

                                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                    EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                                this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, "Month"); // Create task if one doesn't already exist
                            }

                            // Check Repeat_DayOfWeek for idRepeat
                            var repeat_DayOfWeeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat.idRepeat).ToList();
                            foreach (var repeat_DayOfWeek in repeat_DayOfWeeks)
                            {
                                // Since Repetition = Month the DayOfWeek must also have a corresponding position
                                if (repeat_DayOfWeek.position != null && repeat_DayOfWeek.position != "")
                                {
                                    /* Get date */
                                    var dayOfMonth = this.GetDayOfMonth(DateIterated, repeat_DayOfWeek.position, repeat_DayOfWeek.dayOfWeek);

                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                    EndDateTime = StartDateTime; // Start and end DateTime are the same since there is a set day of week

                                    /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                    if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                        EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                                    this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, "Month"); // Create task if one doesn't already exist
                                }
                            }

                            /* Check start day, date, and week */
                            if (TodoItemTime.StartDate != null && TodoItemTime.StartDate > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, TodoItemTime.StartDate.Day);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.StartDayOfMonth != null)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, (int)TodoItemTime.StartDayOfMonth);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartDate != null && TodoItemTime.RecommendedStartDate > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)TodoItemTime.RecommendedStartDate).Day);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartDayOfMonth != null)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, (int)TodoItemTime.RecommendedStartDayOfMonth);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.StartWeek != null && TodoItemTime.StartWeek > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)TodoItemTime.StartWeek).Day);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartWeek != null && TodoItemTime.RecommendedStartWeek > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)TodoItemTime.RecommendedStartWeek).Day);
                                EndDateTime = StartDateTime;
                            }
                            else
                            {
                                StartDateTime = FirstDayOfMonth;
                                EndDateTime = LastDayOfMonth;
                            }

                            /* Check start time and recommended start time */
                            if (TodoItemTime.StartTime != null && TodoItemTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.StartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartTime != null && TodoItemTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.RecommendedStartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }

                            /* If TodoItem has an EstimatedTime add that to the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                            /* Check end time and recommended end time */
                            if (TodoItemTime.EndTime != null && TodoItemTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.EndTime.TotalSeconds);
                            }
                            else if (TodoItemTime.RecommendedEndTime != null && TodoItemTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.RecommendedEndTime.TotalSeconds);
                            }

                            this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, "Month"); // Create task if one doesn't already exist

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddMonths(1);
                        }
                    }

                    if (repeat.TimeframeClass.Repetition == "Weekly")
                    {

                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day); // Set date iterated to original seed date

                        for (int i = 0; i < (months * 5); i++) // Loop through the next __ weeks including current (create Tasks for the next year)
                        {
                            /* If Todo Item's end date is before date iterated, break loop  */
                            if (EndDateTime_TodoItem < DateIterated)
                            {
                                break;
                            }

                            /* Get first and last day of the week */
                            var FirstDayOfWeek = DateIterated.AddDays(-(int)DateIterated.DayOfWeek);
                            var LastDayOfWeek = FirstDayOfWeek.AddDays(6);

                            /* Determin Spotlight Inventory Item for DateIterated */
                            var idWeekTimeframe = Repetitions.RepetitionList.IndexOf("Weekly");
                            var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idWeekTimeframe && x.datetime == FirstDayOfWeek).Count() > 0) ?
                                entities.inventoryitemspotlights.Where(x => x.idTimeframe == idWeekTimeframe && x.datetime == FirstDayOfWeek).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == todoItem.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            /* If the type is Spotlight but is not in its spotlight week */
                            if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            {
                                continue;
                            }
                            else if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() > 0)
                            {
                                taskType = "TodoItem_Spotlight_Repetitive";
                            }

                            // Check Repeat_DayOfWeek for idRepeat
                            var repeat_DayOfWeeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat.idRepeat).ToList();
                            foreach (var repeat_DayOfWeek in repeat_DayOfWeeks)
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, repeat_DayOfWeek.dayOfWeek);

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = StartDateTime; // Start and end DateTime are the same since there is a set day of week

                                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                    EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                                this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, "Week"); // Create task if one doesn't already exist
                            }

                            /* Check start day and date */
                            if (TodoItemTime.StartDate != null && TodoItemTime.StartDate > new DateTime())
                            {
                                /* If todoItem.startDate is before current iterated date, set StartDateTime for the task to the same day of week in the current week */
                                if (TodoItemTime.StartDate < DateIterated)
                                {
                                    var startDayOfWeek = ((DateTime)TodoItemTime.StartDate).DayOfWeek;

                                    StartDateTime = FirstDayOfWeek.AddDays((int)startDayOfWeek);
                                    EndDateTime = StartDateTime;
                                }
                                /* If todoItem.startDate is in current iterated week, set StartDateTime for the task to the same date */
                                else if (TodoItemTime.StartDate > DateIterated && TodoItemTime.StartDate < LastDayOfWeek)
                                {
                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                    EndDateTime = StartDateTime;
                                }
                            }

                            if (TodoItemTime.StartDayOfWeek != null && TodoItemTime.StartDayOfWeek != "")
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, TodoItemTime.StartDayOfWeek);

                                if (dayOfMonth == -1) continue;

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = StartDateTime; // Start and end DateTime are the same since there is a set day of week
                            }
                            else if (TodoItemTime.RecommendedStartDate != null && TodoItemTime.RecommendedStartDate > new DateTime())
                            {
                                /* If todoItem.startDate is before current iterated date, set StartDateTime for the task to the same day of week in the current week */
                                if (TodoItemTime.RecommendedStartDate < DateIterated)
                                {
                                    var startDayOfWeek = ((DateTime)TodoItemTime.RecommendedStartDate).DayOfWeek;

                                    StartDateTime = FirstDayOfWeek.AddDays((int)startDayOfWeek);
                                    EndDateTime = StartDateTime;
                                }
                                /* If todoItem.startDate is in current iterated week, set StartDateTime for the task to the same date */
                                else if (TodoItemTime.StartDate > DateIterated && TodoItemTime.StartDate < LastDayOfWeek)
                                {
                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                    EndDateTime = StartDateTime;
                                }
                            }
                            else if (TodoItemTime.RecommendedStartDayOfWeek != null && TodoItemTime.RecommendedStartDayOfWeek != "")
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, TodoItemTime.RecommendedStartDayOfWeek);

                                if (dayOfMonth == -1) continue;

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = StartDateTime; // Start and end DateTime are the same since there is a set day of week
                            }
                            else
                            {
                                StartDateTime = FirstDayOfWeek;
                                EndDateTime = LastDayOfWeek;
                            }

                            /* Check start time and recommended start time */
                            if (TodoItemTime.StartTime != null && TodoItemTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.StartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartTime != null && TodoItemTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.RecommendedStartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }

                            /* If TodoItem has an EstimatedTime add that the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                            /* Check end time and recommended end time */
                            if (TodoItemTime.EndTime != null && TodoItemTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.EndTime.TotalSeconds);
                            }
                            else if (TodoItemTime.RecommendedEndTime != null && TodoItemTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.RecommendedEndTime.TotalSeconds);
                            }

                            this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, "Week"); // Create task if one doesn't already exist

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddDays(7);
                        }
                    }

                    if (repeat.TimeframeClass.Repetition == "Daily" || repeat.TimeframeClass.Repetition == "Weekdaily" || repeat.TimeframeClass.Repetition == "Workdaily" || repeat.TimeframeClass.Repetition == "Weekendaily")
                    {
                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day);

                        for (int i = 0; i < (months * 31); i++) // Loop through the next ___ days including current (create Tasks for the next year)
                        {
                            if (repeat.TimeframeClass.Repetition == "Weekdaily" || repeat.TimeframeClass.Repetition == "Workdaily")
                            {
                                if (DateIterated.DayOfWeek == DayOfWeek.Saturday || DateIterated.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    DateIterated = DateIterated.AddDays(1);
                                    continue;
                                }
                            }
                            else if (repeat.TimeframeClass.Repetition == "Weekendaily")
                            {
                                if (DateIterated.DayOfWeek != DayOfWeek.Saturday && DateIterated.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    DateIterated = DateIterated.AddDays(1);
                                    continue;
                                }
                            }

                            /* If Todo Item's end date is before date iterated, break loop  */
                            if (EndDateTime_TodoItem < DateIterated)
                            {
                                break;
                            }
                            /* Determin Spotlight Inventory Item for DateIterated */
                            var idDayTimeframe = Repetitions.RepetitionList.IndexOf("Daily");
                            var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idDayTimeframe && x.datetime == DateIterated).Count() > 0) ?
                                entities.inventoryitemspotlights.Where(x => x.idTimeframe == idDayTimeframe && x.datetime == DateIterated).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == todoItem.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            /* If the type is Spotlight but is not in its spotlight week */
                            if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            {
                                continue;
                            }
                            else if (todoItem.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() > 0)
                            {
                                taskType = "TodoItem_Spotlight_Repetitive";
                            }

                            /* Check start date */
                            if (TodoItemTime.StartDate != null && TodoItemTime.StartDate > new DateTime() && TodoItemTime.StartDate < DateIterated)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartDate != null && TodoItemTime.RecommendedStartDate > new DateTime() && TodoItemTime.RecommendedStartDate < DateIterated)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                EndDateTime = StartDateTime;
                            }
                            else
                            {
                                StartDateTime = DateIterated;
                                EndDateTime = StartDateTime;
                            }

                            /* Check start time and recommended start time */
                            if (TodoItemTime.StartTime != null && TodoItemTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.StartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }
                            else if (TodoItemTime.RecommendedStartTime != null && TodoItemTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(TodoItemTime.RecommendedStartTime.TotalSeconds);
                                EndDateTime = StartDateTime;
                            }

                            /* If TodoItem has an EstimatedTime add that the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                            /* Check end time and recommended end time */
                            if (TodoItemTime.EndTime != null && TodoItemTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.EndTime.TotalSeconds);
                            }
                            else if (TodoItemTime.RecommendedEndTime != null && TodoItemTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(TodoItemTime.RecommendedEndTime.TotalSeconds);
                            }

                            this.CreateTask(todoItem, tasks, StartDateTime, EndDateTime, taskType, repeat.TimeframeClass.Timeframe1); // Create task if one doesn't already exist

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddDays(1);
                        }
                    }
                }
            }

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        public void RefreshTasks_Spotlight(List<TodoItem> TodoItems, DateTime SeedDate, bool shouldSave = false)
        {
            /*Don't refresh Task if one already exists for Todo Item. There should only be 1 task per errand (multiple events are allowed) */
            List<int> todoItemIDs_Task = entities.task_todoitem.Where(x => x.idTask > 0).Select(x => x.idToDoItem).ToList();

            /* Don't refresh Task if Todo Item is assigned to a Routine. It will be refreshed with the Routines */
            var todoItemIDs_Routine = entities.routine_todoitem.Select(x => x.idToDoItem).ToList();

            /*Don't create task if TodoItem has a repetition. Repetitive TodoItems get created seperately */
            var todoItemIDs = TodoItems.Select(x => x.idTodoItem).ToList();
            var todoItemIDs_Repeat = entities.todoitem_repeat.Where(x => todoItemIDs.Contains(x.idToDoItem)).Select(y => y.idToDoItem).ToList();

            /* Create a single list with all the excluded Todo Items */
            var todoItemIDs_Excluded = new List<int>();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Task).ToList();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Routine).ToList();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Repeat).ToList();

            /* Get all TodoItems that have Type Spotlight */
            var spotlightTypeID = Types.GetID(Types._spotlight);
            var todoItemIDs_Spotlight = entities.todoitem_type.Where(x => x.idType == spotlightTypeID).Select(x => x.idToDoItem).ToList();

            /* Loop through Todo Items that have Type of Spotlight were not excluded */
            foreach (var todoItem in TodoItems.Where(x => todoItemIDs_Spotlight.Contains(x.idTodoItem) && !todoItemIDs_Excluded.Contains(x.idTodoItem) && x.IsGroup != true && x.IsActive == true))
            {
                /* Get the times for the todoItem */
                Time todoItemTime = new Time();
                if (todoItem.idTime != null)
                    CoachMapper.Map(entities.times.Where(x => x.idTime == todoItem.idTime).Single(), todoItemTime);

                /* Check if End Date passed */
                var EndDateTime_TodoItem = (todoItem.idTime != null && todoItemTime.EndDate > new DateTime()) ? todoItemTime.EndDate : new DateTime(3000, 12, 31); // If no end date, set variable to date you will never see (this prevents you from having to deal with null dates)
                if (EndDateTime_TodoItem < SeedDate) continue; // If Todo Item's end date is before seed date, don't create new task

                DateTime StartDateTime = new DateTime();
                DateTime EndDateTime = new DateTime();

                /* Check start day, date, and week */
                if (todoItemTime.StartDate != null && todoItemTime.StartDate > new DateTime() && todoItemTime.StartDate > SeedDate)
                {
                    StartDateTime = todoItemTime.StartDate;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.RecommendedStartDate != null && todoItemTime.RecommendedStartDate > new DateTime() && todoItemTime.RecommendedStartDate > SeedDate)
                {
                    StartDateTime = ((DateTime)todoItemTime.RecommendedStartDate);
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.StartWeek != null && todoItemTime.StartWeek > new DateTime() && todoItemTime.StartWeek > SeedDate)
                {
                    StartDateTime = todoItemTime.StartWeek;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.RecommendedStartWeek != null && todoItemTime.RecommendedStartWeek > new DateTime() && todoItemTime.RecommendedStartWeek > SeedDate)
                {
                    StartDateTime = todoItemTime.RecommendedStartWeek;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else
                {
                    StartDateTime = SeedDate;
                    EndDateTime = EndDateTime_TodoItem;
                }

                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                    EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                /* Check start time and recommended start time */
                if (todoItemTime.StartTime != null && todoItemTime.StartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(todoItemTime.StartTime.TotalSeconds);
                }
                else if (todoItemTime.RecommendedStartTime != null && todoItemTime.RecommendedStartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(todoItemTime.RecommendedStartTime.TotalSeconds);
                }
                /* Check end time and recommended end time */
                if (todoItemTime.EndTime != null && todoItemTime.EndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(todoItemTime.EndTime.TotalSeconds);
                }
                else if (todoItemTime.RecommendedEndTime != null && todoItemTime.RecommendedEndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(todoItemTime.RecommendedEndTime.TotalSeconds);
                }

                this.CreateTask(todoItem, new List<task>(), StartDateTime, EndDateTime, "TodoItem_Spotlight", "Day"); // Create task if one doesn't already exist. There should only be one ask for these todoitems because there are not repetitive
            }

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        public void RefreshTasks(List<TodoItem> TodoItems, DateTime SeedDate, bool shouldSave = false)
        {
            /*Don't refresh Task if one already exists for Todo Item. There should only be 1 task per TodoItem (multiple events are allowed) */
            List<int> todoItemIDs_Task = entities.task_todoitem.Select(x => x.idToDoItem).ToList();

            /* Don't refresh Task if TodoItem type is Spotlight. It will be refreshed with the Spotlight Todo Items */
            var spotlightTypeID = Types.GetID(Types._spotlight);
            var todoItemIDs_Spotlight = entities.todoitem_type.Where(x => x.idType == spotlightTypeID).Select(x => x.idToDoItem).ToList();

            /* Don't refresh Task if Todo Item is assigned to a Routine. It will be refreshed with the Routines */
            var todoItemIDs_Routine = entities.routine_todoitem.Select(x => x.idToDoItem).ToList();

            /*Don't create task if TodoItem has a repetition. Repetitive TodoItems get created seperately */
            var todoItemIDs = TodoItems.Select(x => x.idTodoItem).ToList();
            var todoItemIDs_Repeat = entities.todoitem_repeat.Where(x => todoItemIDs.Contains(x.idToDoItem)).Select(y => y.idToDoItem).ToList();

            /* Create a single list with all the excluded Todo Items */
            var todoItemIDs_Excluded = new List<int>();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Task).ToList();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Spotlight).ToList();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Routine).ToList();
            todoItemIDs_Excluded = todoItemIDs_Excluded.Union(todoItemIDs_Repeat).ToList();

            /* Loop through Todo Items that were not excluded */
            foreach (var todoItem in TodoItems.Where(x => !todoItemIDs_Excluded.Contains(x.idTodoItem) && x.IsGroup != true && x.IsActive == true))
            {
                /* Get the times for the todoItem */
                Time todoItemTime = new Time();
                if (todoItem.idTime != null)
                    CoachMapper.Map(entities.times.Where(x => x.idTime == todoItem.idTime).Single(), todoItemTime);

                /* Check if End Date passed */
                var EndDateTime_TodoItem = (todoItem.idTime != null && todoItemTime.EndDate > new DateTime()) ? todoItemTime.EndDate : new DateTime(3000, 12, 31); // If no end date, set variable to date you will never see (this prevents you from having to deal with null dates)
                if (EndDateTime_TodoItem < SeedDate) continue; // If Todo Item's end date is before seed date, don't create new task

                DateTime StartDateTime = new DateTime();
                DateTime EndDateTime = new DateTime();

                /* Check start day, date, and week */
                if (todoItemTime.StartDate != null && todoItemTime.StartDate > new DateTime() && todoItemTime.StartDate > SeedDate)
                {
                    StartDateTime = todoItemTime.StartDate;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.RecommendedStartDate != null && todoItemTime.RecommendedStartDate > new DateTime() && todoItemTime.RecommendedStartDate > SeedDate)
                {
                    StartDateTime = ((DateTime)todoItemTime.RecommendedStartDate);
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.StartWeek != null && todoItemTime.StartWeek > new DateTime() && todoItemTime.StartWeek > SeedDate)
                {
                    StartDateTime = todoItemTime.StartWeek;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else if (todoItemTime.RecommendedStartWeek != null && todoItemTime.RecommendedStartWeek > new DateTime() && todoItemTime.RecommendedStartWeek > SeedDate)
                {
                    StartDateTime = todoItemTime.RecommendedStartWeek;
                    EndDateTime = EndDateTime_TodoItem;
                }
                else
                {
                    StartDateTime = SeedDate;
                    EndDateTime = EndDateTime_TodoItem;
                }

                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                if (todoItem.EstimatedTime != null && todoItem.EstimatedTime > 0)
                    EndDateTime.AddSeconds((int)todoItem.EstimatedTime);

                /* Check start time and recommended start time */
                if (todoItemTime.StartTime != null && todoItemTime.StartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(todoItemTime.StartTime.TotalSeconds);
                }
                else if (todoItemTime.RecommendedStartTime != null && todoItemTime.RecommendedStartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(todoItemTime.RecommendedStartTime.TotalSeconds);
                }
                /* Check end time and recommended end time */
                if (todoItemTime.EndTime != null && todoItemTime.EndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(todoItemTime.EndTime.TotalSeconds);
                }
                else if (todoItemTime.RecommendedEndTime != null && todoItemTime.RecommendedEndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(todoItemTime.RecommendedEndTime.TotalSeconds);
                }


                string taskType;

                if (todoItem.Types.Contains(Types._errand))
                    taskType = "TodoItem_Errand";
                else if (todoItem.Types.Contains(Types._tally))
                    taskType = "TodoItem_Tally";
                else if (todoItem.Types.Contains(Types._inverse))
                    taskType = "TodoItem_Inverse";
                else
                    taskType = "TodoItem_Single";

                this.CreateTask(todoItem, new List<task>(), StartDateTime, EndDateTime, taskType, "Day"); // Create task if one doesn't already exist
            }

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        /* Make sure there is a task for every Routine for the next __ */
        public List<dynamic> RefreshTasks_Repetitive(List<Routine> Routines, DateTime SeedDate, int months, bool shouldSave = false)
        {
            List<dynamic> QueuedTasks = new List<dynamic>();

            foreach (var routine in Routines.Where(x => x.IsActive == true))
            {
                var taskType = "Routine_Repetitive";
                var taskIDs_Routine = entities.task_routine.Where(x => x.idRoutine == routine.idRoutine).Select(y => y.idTask); // Task IDs for routine
                var tasks = entities.tasks.Where(x => taskIDs_Routine.Contains(x.idTask)).ToList();

                /* Get the times for the todoItem */
                Time routineTime = new Time();
                if (routine.idTime != null)
                {
                    CoachMapper.Map(entities.times.Where(x => x.idTime == routine.idTime).Single(), routineTime);
                }
                var EndDateTime_Routine = (routine.idTime != null && routineTime.EndDate > new DateTime()) ? routineTime.EndDate : new DateTime(3000, 12, 31); // Date when Routine's repetition stops. If no end date, set variable to date you will never see (this prevents you from having to deal with null dates)

                DateTime StartDateTime = new DateTime();
                DateTime EndDateTime = new DateTime();

                DateTime DateIterated; // Date that will get changed when looping through repetitinos


                // YOU HAVE TO SEE IF TASKS CHANGE DATES AFTER THE INTERVAL OR IF I SHOULD JUST CREATE A NEW ONE

                foreach (var repeat in routine.Repeats)
                {
                    // Get start and end dates based for repetition
                    // DONT FORGET TO CONSIDER FREQUENCY AND INTERVAL

                    if (repeat.TimeframeClass.Repetition == "Monthly")
                    {
                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day); // Set date iterated to original seed date

                        for (int i = 0; i < months; i++) // Loop through the next __ months including current (create Tasks for the next year)
                        {
                            /* If Routines's end date is before date iterated, break loop  */
                            if (EndDateTime_Routine < DateIterated)
                            {
                                break;
                            }

                            /* Get first and last day of the month */
                            var FirstDayOfMonth = new DateTime(DateIterated.Year, DateIterated.Month, 1);
                            var LastDayOfMonth = FirstDayOfMonth.AddMonths(1).AddDays(-1);

                            ///* Determin Spotlight Inventory Item for DateIterated */
                            //var idMonthTimeframe = Repetitions.RepetitionList.IndexOf("Monthly");
                            //var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idMonthTimeframe && x.datetime == FirstDayOfMonth).Count() > 0) ?
                            //    entities.inventoryitemspotlights.Where(x => x.idTimeframe == idMonthTimeframe && x.datetime == FirstDayOfMonth).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            //var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == routine.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            ///* If the type is Spotlight but is not in its spotlight month */
                            //if (routine.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            //{
                            //    continue;
                            //}

                            // Check Repeat_DayOfMonth for idRepeat
                            var repeat_DayOfMonths = entities.repeat_dayofmonth.Where(x => x.idRepeat == repeat.idRepeat).Select(y => y.idRepeat_DayOfMonth).ToList();
                            foreach (var repeat_DayOfMonth in repeat_DayOfMonths)
                            {
                                /* Get 'StartDate' based off of 'DateIterated' */
                                /* The start and end days will be the same since there is a specific date */
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, repeat_DayOfMonth); // Day is the only difference
                                EndDateTime = LastDayOfMonth; // Start and end DateTime are the same since there is a set DayOfMonth

                                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                    EndDateTime.AddSeconds((int)routine.EstimatedTime);

                                QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                                //this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist
                            }

                            // Check Repeat_DayOfWeek for idRepeat
                            var repeat_DayOfWeeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat.idRepeat).ToList();
                            foreach (var repeat_DayOfWeek in repeat_DayOfWeeks)
                            {
                                // Since Repetition = Month the DayOfWeek must also have a corresponding position
                                if (repeat_DayOfWeek.position != null && repeat_DayOfWeek.position != "")
                                {
                                    /* Get date */
                                    var dayOfMonth = this.GetDayOfMonth(DateIterated, repeat_DayOfWeek.position, repeat_DayOfWeek.dayOfWeek);

                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                    EndDateTime = LastDayOfMonth; // Start and end DateTime are the same since there is a set day of week

                                    /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                    if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                        EndDateTime.AddSeconds((int)routine.EstimatedTime);

                                    QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                                    this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist
                                }
                            }

                            /* Check start day, date, and week */
                            if (routineTime.StartDate != null && routineTime.StartDate > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, routineTime.StartDate.Day);
                                EndDateTime = LastDayOfMonth;
                            }
                            else if (routineTime.StartDayOfMonth != null)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, (int)routineTime.StartDayOfMonth);
                                EndDateTime = LastDayOfMonth;
                            }
                            else if (routineTime.RecommendedStartDate != null && routineTime.RecommendedStartDate > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)routineTime.RecommendedStartDate).Day);
                                EndDateTime = LastDayOfMonth;
                            }
                            else if (routineTime.RecommendedStartDayOfMonth != null)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, (int)routineTime.RecommendedStartDayOfMonth);
                                EndDateTime = LastDayOfMonth;
                            }
                            else if (routineTime.StartWeek != null && routineTime.StartWeek > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)routineTime.StartWeek).Day);
                                EndDateTime = LastDayOfMonth;
                            }
                            else if (routineTime.RecommendedStartWeek != null && routineTime.RecommendedStartWeek > new DateTime())
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, ((DateTime)routineTime.RecommendedStartWeek).Day);
                                EndDateTime = LastDayOfMonth;
                            }
                            else
                            {
                                StartDateTime = FirstDayOfMonth;
                                EndDateTime = LastDayOfMonth;
                            }

                            /* If TodoItem has an EstimatedTime add that the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)routine.EstimatedTime);

                            /* Check start time and recommended start time */
                            if (routineTime.StartTime != null && routineTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.StartTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedStartTime != null && routineTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.RecommendedStartTime.TotalSeconds);
                            }
                            /* Check end time and recommended end time */
                            if (routineTime.EndTime != null && routineTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.EndTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedEndTime != null && routineTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.RecommendedEndTime.TotalSeconds);
                            }

                            QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                            //this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist

                            /* Create Task for each TodoItem in Routine */
                            // Get TodoItems for Routine
                            var todoitem_views = entities.todoitem_view.Where(x => routine.TodoItemIDs.Contains(x.idTodoItem)).ToList();
                            var TodoItems = this.CreateTodoItemModels(todoitem_views);
                            // Create Task foreach TodoItem
                            foreach (var todoItem in TodoItems.Where(x => x.IsGroup != true && x.IsActive == true).ToList())
                            {
                                var tasks_TodoItem = entities.tasks.Where(x => todoItem.TaskIDs.Contains(x.idTask)).ToList();
                                QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType = "Routine_Item", timeframe = "Day", shouldSave = false });
                                //this.CreateTask(todoItem, tasks_TodoItem, StartDateTime, EndDateTime, "Routine_Item", "Day");
                            }

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddMonths(1);
                        }
                    }

                    if (repeat.TimeframeClass.Repetition == "Weekly")
                    {
                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day); // Set date iterated to original seed date

                        for (int i = 0; i < (months * 5); i++) // Loop through the next 52 weeks including current (create Tasks for the next year)
                        {
                            /* If Todo Item's end date is before date iterated, break loop  */
                            if (EndDateTime_Routine < DateIterated)
                            {
                                break;
                            }

                            /* Get first and last day of the week */
                            var FirstDayOfWeek = DateIterated.AddDays(-(int)DateIterated.DayOfWeek);
                            var LastDayOfWeek = FirstDayOfWeek.AddDays(6);

                            ///* Determin Spotlight Inventory Item for DateIterated */
                            //var idWeekTimeframe = Repetitions.RepetitionList.IndexOf("Weekly");
                            //var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idWeekTimeframe && x.datetime == FirstDayOfWeek).Count() > 0) ?
                            //    entities.inventoryitemspotlights.Where(x => x.idTimeframe == idWeekTimeframe && x.datetime == FirstDayOfWeek).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            //var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == routine.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            ///* If the type is Spotlight but is not in its spotlight week */
                            //if (routine.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            //{
                            //    continue;
                            //}

                            // Check Repeat_DayOfWeek for idRepeat
                            var repeat_DayOfWeeks = entities.repeat_dayofweek.Where(x => x.idRepeat == repeat.idRepeat).ToList();
                            foreach (var repeat_DayOfWeek in repeat_DayOfWeeks)
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, repeat_DayOfWeek.dayOfWeek);

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = LastDayOfWeek; // Start and end DateTime are the same since there is a set day of week

                                /* If TodoItem has an EstimatedTime add that the EndDateTime */
                                if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                    EndDateTime.AddSeconds((int)routine.EstimatedTime);

                                QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                                //this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist
                            }

                            /* Check start day and date */
                            if (routineTime.StartDate != null && routineTime.StartDate > new DateTime())
                            {
                                /* If todoItem.startDate is before current iterated date, set StartDateTime for the task to the same day of week in the current week */
                                if (routineTime.StartDate < DateIterated)
                                {
                                    var startDayOfWeek = ((DateTime)routineTime.StartDate).DayOfWeek;

                                    StartDateTime = FirstDayOfWeek.AddDays((int)startDayOfWeek);
                                    EndDateTime = LastDayOfWeek;
                                }
                                /* If todoItem.startDate is in current iterated week, set StartDateTime for the task to the same date */
                                else if (routineTime.StartDate > DateIterated && routineTime.StartDate < LastDayOfWeek)
                                {
                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                    EndDateTime = LastDayOfWeek;
                                }
                            }
                            else if (routineTime.StartDayOfWeek != null && routineTime.StartDayOfWeek != "")
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, routineTime.StartDayOfWeek);

                                if (dayOfMonth == -1) continue;

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = LastDayOfWeek; // Start and end DateTime are the same since there is a set day of week
                            }
                            else if (routineTime.RecommendedStartDate != null && routineTime.RecommendedStartDate > new DateTime())
                            {
                                /* If todoItem.startDate is before current iterated date, set StartDateTime for the task to the same day of week in the current week */
                                if (routineTime.RecommendedStartDate < DateIterated)
                                {
                                    var startDayOfWeek = ((DateTime)routineTime.RecommendedStartDate).DayOfWeek;

                                    StartDateTime = FirstDayOfWeek.AddDays((int)startDayOfWeek);
                                    EndDateTime = LastDayOfWeek;
                                }
                                /* If todoItem.startDate is in current iterated week, set StartDateTime for the task to the same date */
                                else if (routineTime.StartDate > DateIterated && routineTime.StartDate < LastDayOfWeek)
                                {
                                    StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                    EndDateTime = LastDayOfWeek;
                                }
                            }
                            else if (routineTime.RecommendedStartDayOfWeek != null && routineTime.RecommendedStartDayOfWeek != "")
                            {
                                /* Get date */
                                var dayOfMonth = this.GetDayOfMonth(DateIterated, routineTime.RecommendedStartDayOfWeek);

                                if (dayOfMonth == -1) continue;

                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, dayOfMonth); // Day is the only difference, dayOfMonth
                                EndDateTime = LastDayOfWeek; // Start and end DateTime are the same since there is a set day of week
                            }
                            else
                            {
                                StartDateTime = FirstDayOfWeek;
                                EndDateTime = LastDayOfWeek;
                            }

                            /* If TodoItem has an EstimatedTime add that the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)routine.EstimatedTime);

                            /* Check start time and recommended start time */
                            if (routineTime.StartTime != null && routineTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.StartTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedStartTime != null && routineTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.RecommendedStartTime.TotalSeconds);
                            }
                            /* Check end time and recommended end time */
                            if (routineTime.EndTime != null && routineTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.EndTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedEndTime != null && routineTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.RecommendedEndTime.TotalSeconds);
                            }

                            QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                            //this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist

                            /* Create Task for each TodoItem in Routine */
                            // Get TodoItems for Routine
                            //var todoItemIDs_Routine = entities.routine_todoitem.Where(x => x.idRoutine == routine.idRoutine).Select(y => y.idToDoItem).ToList();
                            var todoitem_views = entities.todoitem_view.Where(x => routine.TodoItemIDs.Contains(x.idTodoItem)).ToList();
                            var TodoItems = this.CreateTodoItemModels(todoitem_views);
                            // Create Task foreach TodoItem
                            foreach (var todoItem in TodoItems.Where(x => x.IsGroup != true && x.IsActive == true).ToList())
                            {
                                //var taskIDs_TodoItem = entities.task_todoitem.Where(x => x.idToDoItem == todoItem.idTodoItem).Select(y => y.idTask).ToList();
                                var tasks_TodoItem = entities.tasks.Where(x => todoItem.TaskIDs.Contains(x.idTask)).ToList();
                                QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType = "Routine_Item", timeframe = "Day", shouldSave = false });
                                //this.CreateTask(todoItem, tasks_TodoItem, StartDateTime, EndDateTime, "Routine_Item", "Day");
                            }

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddDays(7);
                        }
                    }

                    if (repeat.TimeframeClass.Repetition == "Daily" || repeat.TimeframeClass.Repetition == "Weekdaily" || repeat.TimeframeClass.Repetition == "Workdaily" || repeat.TimeframeClass.Repetition == "Weekendaily")
                    {
                        DateIterated = new DateTime(SeedDate.Year, SeedDate.Month, SeedDate.Day);

                        for (int i = 0; i < (months * 31); i++) // Loop through the next 365 days including current (create Tasks for the next year)
                        {
                            if (repeat.TimeframeClass.Repetition == "Weekdaily" || repeat.TimeframeClass.Repetition == "Workdaily")
                            {
                                if (DateIterated.DayOfWeek == DayOfWeek.Saturday || DateIterated.DayOfWeek == DayOfWeek.Sunday)
                                {
                                    DateIterated = DateIterated.AddDays(1);
                                    continue;
                                }
                            }
                            else if (repeat.TimeframeClass.Repetition == "Weekendaily")
                            {
                                if (DateIterated.DayOfWeek != DayOfWeek.Saturday && DateIterated.DayOfWeek != DayOfWeek.Sunday)
                                {
                                    DateIterated = DateIterated.AddDays(1);
                                    continue;
                                }
                            }

                            /* If Todo Item's end date is before date iterated, break loop  */
                            if (EndDateTime_Routine < DateIterated)
                            {
                                break;
                            }

                            ///* Determin Spotlight Inventory Item for DateIterated */
                            //var idDayTimeframe = Repetitions.RepetitionList.IndexOf("Daily");
                            //var idInventoryItem = (entities.inventoryitemspotlights.Where(x => x.idTimeframe == idDayTimeframe && x.datetime == DateIterated).Count() > 0) ?
                            //    entities.inventoryitemspotlights.Where(x => x.idTimeframe == idDayTimeframe && x.datetime == DateIterated).ToList().Last().idInventoryItem : -1; // Get ID of the Inventory Item that is being spotlit in the current iterated month
                            //var inventoryitem_todoitems = entities.inventoryitem_todoitem.Where(x => x.idInventoryItem == idInventoryItem && x.idToDoItem == routine.idTodoItem).ToList(); // Is this Todo Item connected to the Spotlight Inventory Item?

                            ///* If the type is Spotlight but is not in its spotlight week */
                            //if (routine.Types.IndexOf("Spotlight") > -1 && inventoryitem_todoitems.Count() == 0)
                            //{
                            //    continue;
                            //}

                            /* Check start date */
                            if (routineTime.StartDate != null && routineTime.StartDate > new DateTime() && routineTime.StartDate < DateIterated)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                EndDateTime = StartDateTime;
                            }
                            else if (routineTime.RecommendedStartDate != null && routineTime.RecommendedStartDate > new DateTime() && routineTime.RecommendedStartDate < DateIterated)
                            {
                                StartDateTime = new DateTime(DateIterated.Year, DateIterated.Month, DateIterated.Day);
                                EndDateTime = StartDateTime;
                            }
                            else
                            {
                                StartDateTime = DateIterated;
                                EndDateTime = StartDateTime;
                            }

                            /* If TodoItem has an EstimatedTime add that the EndDateTime */
                            // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                            if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                                EndDateTime.AddSeconds((int)routine.EstimatedTime);

                            /* Check start time and recommended start time */
                            if (routineTime.StartTime != null && routineTime.StartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.StartTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedStartTime != null && routineTime.RecommendedStartTime > new TimeSpan())
                            {
                                StartDateTime = StartDateTime.AddSeconds(routineTime.RecommendedStartTime.TotalSeconds);
                            }
                            /* Check end time and recommended end time */
                            if (routineTime.EndTime != null && routineTime.EndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.EndTime.TotalSeconds);
                            }
                            else if (routineTime.RecommendedEndTime != null && routineTime.RecommendedEndTime > new TimeSpan())
                            {
                                EndDateTime = EndDateTime.AddSeconds(routineTime.RecommendedEndTime.TotalSeconds);
                            }

                            QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType, timeframe = "", shouldSave = false });
                            //this.CreateTask_Routine(routine, tasks, StartDateTime, EndDateTime, "Routine_Repetitive"); // Create task if one doesn't already exist

                            /* Create Task for each TodoItem in Routine */
                            // Get TodoItems for Routine
                            //var todoItemIDs_Routine = entities.routine_todoitem.Where(x => x.idRoutine == routine.idRoutine).Select(y => y.idToDoItem).ToList();
                            var todoitem_views = entities.todoitem_view.Where(x => routine.TodoItemIDs.Contains(x.idTodoItem)).ToList();
                            var TodoItems = this.CreateTodoItemModels(todoitem_views);
                            // Create Task foreach TodoItem
                            foreach (var todoItem in TodoItems.Where(x => x.IsGroup != true && x.IsActive == true).ToList())
                            {
                                //var taskIDs_TodoItem = entities.task_todoitem.Where(x => x.idToDoItem == todoItem.idTodoItem).Select(y => y.idTask).ToList();
                                var tasks_TodoItem = entities.tasks.Where(x => todoItem.TaskIDs.Contains(x.idTask)).ToList();
                                QueuedTasks.Add(new { routine, tasks, StartDateTime, EndDateTime, taskType = "Routine_Item", timeframe = "Day", shouldSave = false });
                                //this.CreateTask(todoItem, tasks_TodoItem, StartDateTime, EndDateTime, "Routine_Item", "Day");
                            }

                            // CHECK FOR INTERVAL AND FREQUENCY

                            DateIterated = DateIterated.AddDays(1);
                        }
                    }
                }
            }

            foreach (var annony in QueuedTasks)
            {
                this.CreateTask_Routine(annony.routine, annony.tasks, annony.StartDateTime, annony.EndDateTime, annony.taskType, annony.shouldSave);
            }

            return QueuedTasks;
        }

        public void RefreshTasks(List<Routine> Routines, DateTime SeedDate, bool shouldSave = false)
        {
            /*Don't refresh Task if one already exists for Routine. There should only be 1 task per Routine (multiple events are allowed) */
            List<int> routineIDs_Task = entities.task_routine.Select(x => x.idRoutine).ToList();

            ///* Don't refresh Task if Routine type is Spotlight. It will be refreshed with the Spotlight Todo Items */
            //var spotlightTypeID = Types.GetID(Types._spotlight);
            //var todoItemIDs_Spotlight = entities.todoitem_type.Where(x => x.idType == spotlightTypeID).Select(x => x.idToDoItem).ToList();

            /*Don't create task if Routine has a repetition. Repetitive Routines get created seperately */
            var routineIDs = Routines.Select(x => x.idRoutine).ToList();
            var routineIDs_Repeat = entities.routine_repeat.Where(x => routineIDs.Contains(x.idRoutine)).Select(y => y.idRoutine).ToList();

            /* Create a single list with all the excluded Routines */
            var routineIDs_Excluded = new List<int>();
            routineIDs_Excluded = routineIDs_Excluded.Union(routineIDs_Task).ToList();
            routineIDs_Excluded = routineIDs_Excluded.Union(routineIDs_Repeat).ToList();

            /* Loop through Routines that were not excluded */
            foreach (var routine in Routines.Where(x => !routineIDs_Excluded.Contains(x.idRoutine) && x.IsActive == true))
            {
                /* Get the times for the Routine */
                Time routineTime = new Time();
                if (routine.idTime != null)
                    CoachMapper.Map(entities.times.Where(x => x.idTime == routine.idTime).Single(), routineTime);

                /* Check if End Date passed */
                var EndDateTime_Routine = (routine.idTime != null && routineTime.EndDate > new DateTime()) ? routineTime.EndDate : new DateTime(3000, 12, 31); // If no end date, set variable to date you will never see (this prevents you from having to deal with null dates)
                if (EndDateTime_Routine < SeedDate) continue; // If Routine's end date is before seed date, don't create new task

                DateTime StartDateTime = new DateTime();
                DateTime EndDateTime = new DateTime();

                /* Check start day, date, and week */
                if (routineTime.StartDate != null && routineTime.StartDate > new DateTime() && routineTime.StartDate > SeedDate)
                {
                    StartDateTime = routineTime.StartDate;
                    EndDateTime = EndDateTime_Routine;
                }
                else if (routineTime.RecommendedStartDate != null && routineTime.RecommendedStartDate > new DateTime() && routineTime.RecommendedStartDate > SeedDate)
                {
                    StartDateTime = ((DateTime)routineTime.RecommendedStartDate);
                    EndDateTime = EndDateTime_Routine;
                }
                else if (routineTime.StartWeek != null && routineTime.StartWeek > new DateTime() && routineTime.StartWeek > SeedDate)
                {
                    StartDateTime = routineTime.StartWeek;
                    EndDateTime = EndDateTime_Routine;
                }
                else if (routineTime.RecommendedStartWeek != null && routineTime.RecommendedStartWeek > new DateTime() && routineTime.RecommendedStartWeek > SeedDate)
                {
                    StartDateTime = routineTime.RecommendedStartWeek;
                    EndDateTime = EndDateTime_Routine;
                }
                else
                {
                    StartDateTime = SeedDate;
                    EndDateTime = EndDateTime_Routine;
                }

                /* If Routine has an EstimatedTime add that to the EndDateTime */
                // EndDateTime determined by Estimated time will be overwrtten if a specific end time is set
                if (routine.EstimatedTime != null && routine.EstimatedTime > 0)
                    EndDateTime.AddSeconds((int)routine.EstimatedTime);

                /* Check start time and recommended start time */
                if (routineTime.StartTime != null && routineTime.StartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(routineTime.StartTime.TotalSeconds);
                }
                else if (routineTime.RecommendedStartTime != null && routineTime.RecommendedStartTime > new TimeSpan())
                {
                    StartDateTime = StartDateTime.AddSeconds(routineTime.RecommendedStartTime.TotalSeconds);
                }
                /* Check end time and recommended end time */
                if (routineTime.EndTime != null && routineTime.EndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(routineTime.EndTime.TotalSeconds);
                }
                else if (routineTime.RecommendedEndTime != null && routineTime.RecommendedEndTime > new TimeSpan())
                {
                    EndDateTime = EndDateTime.AddSeconds(routineTime.RecommendedEndTime.TotalSeconds);
                }

                this.CreateTask_Routine(routine, new List<task>(), StartDateTime, EndDateTime, "Routine_Single"); // Create task if one doesn't already exist

                /* Create Task for each TodoItem in Routine */
                // Get TodoItems for Routine
                var todoItemIDs_Routine = entities.routine_todoitem.Where(x => x.idRoutine == routine.idRoutine).Select(y => y.idToDoItem).ToList();
                var todoitem_views = entities.todoitem_view.Where(x => routine.TodoItemIDs.Contains(x.idTodoItem)).ToList();
                var TodoItems = this.CreateTodoItemModels(todoitem_views);
                /* Create Task foreach TodoItem */
                foreach (var todoItem in TodoItems.Where(x => x.IsGroup != true && x.IsActive == true).ToList())
                {
                    var tasks_TodoItem = entities.tasks.Where(x => todoItem.TaskIDs.Contains(x.idTask)).ToList();
                    this.CreateTask(todoItem, tasks_TodoItem, StartDateTime, EndDateTime, "Routine_Item", "Day");
                }
            }

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        public void CreateTasks(List<Object> QueuedTasks)
        {
            var idTask = (entities.tasks.Count() > 0) ? entities.tasks.ToList().Last().idTask : 0;
            var idTask_TodoItem = (entities.task_todoitem.Count() > 0) ? entities.task_todoitem.ToList().Last().idTask_TodoItem : 0;

            foreach (var annony in QueuedTasks)
            {

            }
        }

        public int CreateTask(TodoItem TodoItem, List<task> tasks, DateTime StartDateTime, DateTime EndDateTime, string type, string timeframe = "", bool saveChanges = false)
        {
            /* Check is a Task already excists for the TodoItem with the same start and end DateTimes. Don't create new Task if one already excists */
            foreach (var task in tasks)
            {
                if (task.startDateTime == StartDateTime && task.endDateTime == EndDateTime) // If a Task excists with same start and end DateTime
                    return -1; // Return -1 to indicate no task was created
            }

            /* Create a new Task */
            var task_New = new task
            {
                title = TodoItem.Title,
                type = type,
                startDateTime = StartDateTime,
                endDateTime = EndDateTime,
                timeframe = (timeframe != "") ? timeframe : null,
                isComplete = false,
                isVisible = true,
                isActive = true
            };
            entities.tasks.Add(task_New);
            entities.Entry(task_New).State = EntityState.Added;

            /* Map Task to TodoItem */
            var task_todoitem_New = new task_todoitem
            {
                idToDoItem = TodoItem.ID,
                task = task_New
            };
            entities.task_todoitem.Add(task_todoitem_New);
            entities.Entry(task_todoitem_New).State = EntityState.Added;

            if (saveChanges == true) entities.SaveChanges();

            Debug.WriteLine($"Task: {task_New.idTask} - {task_New.title}");
            return task_New.idTask;
        }

        //public int CreateTask(TodoItem TodoItem, List<task> tasks, DateTime StartDateTime, DateTime EndDateTime, string type, string timeframe = "", bool saveChanges = false)
        //{
        //    /* Check is a Task already excists for the TodoItem with the same start and end DateTimes. Don't create new Task if one already excists */
        //    foreach (var task in tasks)
        //    {
        //        if (task.startDateTime == StartDateTime && task.endDateTime == EndDateTime) // If a Task excists with same start and end DateTime
        //            return -1; // Return -1 to indicate no task was created
        //    }

        //    /* Create a new Task */
        //    int idTask_New = 1;
        //    if (entities.tasks.Local.Count() > 0)
        //    {
        //        idTask_New = entities.tasks.Local.Last().idTask + 1;
        //    }
        //    else if (entities.tasks.Count() > 0)
        //    {
        //        idTask_New = entities.tasks.ToList().Last().idTask + 1;
        //    }
        //    var task_New = new task
        //    {
        //        idTask = idTask_New,
        //        title = TodoItem.Title,
        //        type = type,
        //        startDateTime = StartDateTime,
        //        endDateTime = EndDateTime,
        //        timeframe = (timeframe != "") ? timeframe : null,
        //        isComplete = false,
        //        isVisible = true,
        //        isActive = true
        //    };

        //    entities.tasks.Add(task_New);
        //    entities.Entry(task_New).State = EntityState.Added;

        //    /* Map Task to TodoItem */
        //    int idTask_TodoItem_New = 1;
        //    if (entities.task_todoitem.Local.Count() > 0)
        //    {
        //        idTask_TodoItem_New = entities.task_todoitem.Local.Last().idTask_TodoItem + 1;
        //    }
        //    else if (entities.task_todoitem.Count() > 0)
        //    {
        //        idTask_TodoItem_New = entities.task_todoitem.ToList().Last().idTask_TodoItem + 1;
        //    }
        //    var task_todoitem_New = new task_todoitem
        //    {
        //        idTask_TodoItem = idTask_TodoItem_New,
        //        idTask = idTask_New,
        //        idToDoItem = TodoItem.ID
        //    };

        //    entities.task_todoitem.Add(task_todoitem_New);
        //    entities.Entry(task_todoitem_New).State = EntityState.Added;

        //    if (saveChanges == true) entities.SaveChanges();

        //    Debug.WriteLine($"Task: {idTask_New} - {task_New.title}");
        //    return idTask_New;
        //}

        public int CreateTask_Routine(Routine Routine, List<task> tasks, DateTime StartDateTime, DateTime EndDateTime, string type, bool shouldSave = false)
        {
            /* Check is a Task already excists for the Routine with the same start and end DateTimes. Don't create new Task if one already excists */
            foreach (var task in tasks)
            {
                if (task.startDateTime == StartDateTime && task.endDateTime == EndDateTime) // If Task excists with same start and end DateTime
                    return -1; // Return -1 to indicate no task was created
            }

            /* Create a new Task */
            var task_New = new task
            {
                title = Routine.Title,
                type = type,
                startDateTime = StartDateTime,
                endDateTime = EndDateTime,
                isComplete = false,
                isVisible = true,
                isActive = true
            };

            entities.tasks.Add(task_New);
            entities.Entry(task_New).State = EntityState.Added;

            /* Map Task to Routine */
            var task_routine_New = new task_routine
            {
                idRoutine = Routine.ID,
                task = task_New
            };

            entities.task_routine.Add(task_routine_New);
            entities.Entry(task_routine_New).State = EntityState.Added;

            if (shouldSave) entities.SaveChanges();

            Debug.WriteLine($"Task: {task_New.idTask} - {task_New.title}");
            return task_New.idTask;
        }

        public void DeleteTasks(List<int> TaskIDs)
        {
            var eventIDs = new List<int>();

            //var tasks = entities.tasks.Where(x => TaskIDs.Contains(x.idTask)).ToList();
            var task_views = entities.task_view.Where(x => TaskIDs.Contains(x.idTask)).ToList();
            foreach (var task in task_views)
            {
                /* Remove Task_Routines */
                foreach (var taskRoutine in entities.task_routine.Where(x => x.idTask == task.idTask))
                    entities.task_routine.Remove(taskRoutine);

                /* Remove Task_TodoItems */
                foreach (var taskTodoItem in entities.task_todoitem.Where(x => x.idTask == task.idTask))
                    entities.task_todoitem.Remove(taskTodoItem);

                /* Remove Task_Events */
                foreach (var taskEvent in entities.task_event.Where(x => x.idTask == task.idTask))
                    entities.task_event.Remove(taskEvent);

                /* Remove Task */
                entities.tasks.Remove(entities.tasks.SingleOrDefault(x => x.idTask == task.idTask));

                /* Add Event IDs so the Events percentage complete can be updated later */
                if (task.eventIDs != null && task.eventIDs != "")
                {
                    eventIDs = eventIDs.Union(task.eventIDs.Split(',').ToList().ConvertAll(int.Parse)).ToList();
                }
                else  // If Task is from Routine
                {
                    var eventIDs_TodoItemRoutine = task.eventIDs_TodoItemRoutine.Split(',').ToList().ConvertAll(int.Parse); // List of all Event IDs for the Routine that this Task's Todo Item is mapped to
                    var eventIDs_Temp = entities.events.Where(x => eventIDs_TodoItemRoutine.Contains(x.idEvent) 
                        && task.startDateTime >= x.startDateTime && task.startDateTime <= x.endDateTime).Select(x => x.idEvent).ToList(); // Event IDs for this Task's specific Event
                    eventIDs = eventIDs.Union(eventIDs_Temp).ToList();
                }

            }

            entities.SaveChanges();

            /* Update Event percentage complete */
            this.UpdateEventPercentComplete(eventIDs, true);

        }

        /// <summary>
        /// Mark Task's completion status
        /// </summary>
        /// <param name="Task"></param>
        public List<Event> MarkTaskCompletion(Task Task)
        {
            var task = entities.tasks.SingleOrDefault(x => x.idTask == Task.idTask);
            task.isComplete = Task.IsComplete;
            task.dateTimeCompleted = (Task.IsComplete) ? Task.DateTimeCompleted : (DateTime?)null;

            List<Event> Events_Task = UpdateEventPercentComplete(Task, false); // Update percentComplete for all the Events in Task and return their models

            //entities.Entry(task).State = EntityState.Modified;
            entities.SaveChanges();

            return Events_Task;
        }



        /*----------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------ Events --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public List<Event> GetEvents(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            List<event_view> event_views;

            /* If there are start and end date-times, get event views between start and end date-times */
            if (StartDT != null && EndDT != null)
            {
                event_views = entities.event_view.Where(x => x.startDateTime >= StartDT && x.startDateTime <= EndDT).ToList();
            }
            /* If there is a start date-time, get event views after start date-time */
            else if (StartDT != null)
            {
                event_views = entities.event_view.Where(x => x.startDateTime >= StartDT).ToList();
            }
            /* If there is an end date-time, get event views before end date-time */
            else if (EndDT != null)
            {
                event_views = entities.event_view.Where(x => x.startDateTime <= EndDT).ToList();
            }
            /* If there isn't a start or end date-time, get all event views */
            else
            {
                event_views = entities.event_view.ToList();
            }

            var Events = this.GetEventModels(event_views);
            return Events;
        }

        public List<Event> GetEventModels(List<event_view> event_views)
        {
            var Events = new List<Event>();

            foreach (var event_view in event_views)
            {
                var Event = new Event();
                Events.Add(Event);

                CoachMapper.Map(event_view, Event);
            }

            foreach (var @event in Events)
            {
                var tasks = entities.tasks.Where(x => @event.TaskIDs.Contains(x.idTask) && x.isActive == true).Include("task_meal").ToList();
                foreach (var task in tasks)
                {
                    if (task.timeframe != null && task.timeframe != "")
                        @event.Timeframes.Add(task.timeframe);

                    if (task.task_meal != null && task.task_meal.Count > 0)
                        @event.MealID = task.task_meal.First().idMeal;

                }

                //foreach (var type in entities.types.Where(x => @event.TaskIDs.Contains(x.idTask) && x.isActive == true))
                //{
                //    if (task.timeframe != null && task.timeframe != "")
                //        @event.Timeframes.Add(task.timeframe);
                //}
            }

            return Events;
        }

        public void RefreshEvents(List<Task> Tasks, bool shouldSave = false)
        {
            foreach (var task in Tasks)
            {
                if (task.Type == "Routine_Item") continue; // Do not create event for "Routine Item" Tasks. The Routine's Event will be used for both

                var eventIDs = entities.task_event.Where(x => x.idTask == task.idTask).Select(y => y.idEvent).ToList();

                if (eventIDs.Count() == 0) // If no events have been create for the task
                {
                    /* If Task start and end date-time is less than 24 hours apart. It is ready for an event */
                    if ((task.EndDateTime.Date - task.StartDateTime) < new TimeSpan(24, 0, 0))
                    {
                        this.CreateEvent(task); // Create Event
                    }
                }
            }

            if (shouldSave)
            {
                this.entities.SaveChanges();
                this.entities = new coachogEntities();
            }
        }

        public List<Event> RefreshEvents_PercentageComplete(List<Event> Events, DateTime CurrentDate, bool shouldSave = false)
        {
            foreach (var @event in Events)
            {
                var taskTally = 0;
                var completeTally = 0;

                if (@event.Type == "Routine_Single" || @event.Type == "Routine_Repetitive")
                {
                    var taskIDs_RoutineTodoItems = entities.task_todoitem.Where(x => @event.TodoItemIDs_Routine.Contains(x.idToDoItem)).Select(y => y.idTask).ToList();

                    foreach (var task in entities.tasks.Where(x => taskIDs_RoutineTodoItems.Contains(x.idTask)).ToList())
                    {
                        if (task.startDateTime >= @event.StartDateTime && task.startDateTime <= @event.EndDateTime)
                        {
                            taskTally++;

                            if (task.isComplete == true)
                                completeTally++;
                        }
                    }

                    decimal @decimal = (taskTally > 0) ? ((decimal)completeTally / taskTally) * 100 : 0; // Numbers can't be divided my 0
                    @event.PercentComplete = (int)Math.Round(@decimal, MidpointRounding.AwayFromZero);


                    if (@event.EndDateTime < CurrentDate && @event.PercentComplete == 100) // If Event already passed and is 100% complete
                    {
                        @event.Type = "Routine_Complete";
                    }
                    else if (@event.EndDateTime < CurrentDate && @event.PercentComplete < 100) // If Event already passed and is not 100% complete
                    {
                        @event.Type = "Routine_Incomplete";
                    }

                    //var evvent = entities.events.Single(x => x.idEvent == @event.idEvent);
                    //evvent.percentComplete = @event.PercentComplete;
                    //evvent.type = @event.Type;
                }
                else if (@event.Type == "TodoItem_Spotlight" || @event.Type == "TodoItem_Errand" ||
                    @event.Type == "TodoItem_Tally" || @event.Type == "TodoItem_Inverse" || @event.Type == "TodoItem_Repetitive")
                {
                    //var taskIDs_Event = entities.task_event.Where(x => x.idEvent == @event.idEvent).Select(y => y.idTask).ToList();
                    //foreach (var taskID_Event in taskIDs_Event)
                    foreach (var taskID_Event in @event.TaskIDs)
                    {
                        var task = entities.tasks.SingleOrDefault(x => x.idTask == taskID_Event);

                        //var time_Task = entities.times.SingleOrDefault(x => x.idTime == task.idTime);

                        //var DateTime = new DateTime(time_Task.startDate.Value.Year, time_Task.startDate.Value.Month, time_Task.startDate.Value.Day);
                        //if (time_Task.startTime != null && time_Task.startTime != new TimeSpan())
                        //    DateTime = DateTime.AddHours(time_Task.startTime.Value.TotalHours);

                        if (task.startDateTime >= @event.StartDateTime && task.startDateTime <= @event.EndDateTime)
                        //if (DateTime >= @event.StartDateTime && DateTime <= @event.EndDateTime)
                        {
                            taskTally++;

                            if (task.isComplete == true)
                                completeTally++;
                        }
                    }

                    decimal @decimal = (taskTally > 0) ? ((decimal)completeTally / taskTally) * 100 : 0; // Numbers can't be divided my 0
                    @event.PercentComplete = (int)Math.Round(@decimal, MidpointRounding.AwayFromZero);

                    if (@event.EndDateTime < CurrentDate && @event.PercentComplete == 100) // If Event already passed and is 100% complete
                    {
                        @event.Type = "Task_Complete";
                    }
                    else if (@event.StartDateTime < CurrentDate && @event.PercentComplete < 100) // If Event already passed and is not 100% complete
                    {
                        @event.Type = "Task_Incomplete";
                    }

                    //var evvent = entities.events.Single(x => x.idEvent == @event.idEvent);
                    //evvent.percentComplete = @event.PercentComplete;
                    //evvent.type = @event.Type;
                }
            }

            if (shouldSave) entities.SaveChanges();

            return Events;
        }

        public int CreateEvent(Task Task, bool shouldSave = false)
        {
            /* Dont create Events for Task if Task is connected to TodoItem that is assigned to a Routine */
            if (Task.Type != null && Task.Type == "Routine_Item") return -1;

            /* If Task's have different start and end dates, dont create Event */
            // THIS IS ONLY TEMPORARY until I figure out a better was to prevent some Tasks from creating an Event
            // Ideas: isEventReady, LeftDate and RightDate, EariestDate and LatestDate
            if ((Task.EndDateTime.Date - Task.StartDateTime) < new TimeSpan(24, 0, 0)) // If Task start and end DateTime is less than 24 hours apart
            {
                /* Create new Event */
                //int idEvent_New = 1;
                //if (entities.events.Local.Count() > 0)
                //{
                //    idEvent_New = entities.events.Local.Last().idEvent + 1;
                //}
                //else if (entities.events.Count() > 0)
                //{
                //    idEvent_New = entities.events.ToList().Last().idEvent + 1;
                //}
                var event_New = new @event
                {
                    //idEvent = idEvent_New,
                    title = Task.Title,
                    type = Task.Type,
                    isAllDay = (Task.Type == "TodoItem_Errand") ? true : false,
                    timezone = "Etc/UTC",
                    startDateTime = Task.StartDateTime,
                    endDateTime = (Task.StartDateTime < Task.EndDateTime) ? Task.EndDateTime : Task.StartDateTime.AddMinutes(15),
                    isActive = true,
                    isVisible = true,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now
                };

                entities.events.Add(event_New);
                entities.Entry(event_New).State = EntityState.Added;

                /* Map Event to Task */
                //int idTask_Event_New = 1;
                //if (entities.task_event.Local.Count() > 0)
                //{
                //    idTask_Event_New = entities.task_event.Local.Last().idTask_Event + 1;
                //}
                //else if (entities.task_event.Count() > 0)
                //{
                //    idTask_Event_New = entities.task_event.ToList().Last().idTask_Event + 1;
                //}
                var task_event_New = new task_event
                {
                    //idTask_Event = idTask_Event_New,
                    idTask = Task.idTask,
                    //idEvent = idEvent_New
                    @event = event_New
                };

                entities.task_event.Add(task_event_New);
                entities.Entry(task_event_New).State = EntityState.Added;

                if (shouldSave) entities.SaveChanges();

                Debug.WriteLine($"Event: {event_New.idEvent} - {event_New.title}");
                return event_New.idEvent;
            }

            return -1; // Return -1 to indicate no event was created
        }

        public void CreateEvents_Errands(List<Event> Events, bool shouldSave = false)
        {
            foreach (var @event in Events)
            {
                /* Create new Time for Todo Item */
                int idTime_New = 1;
                if (entities.times.Local.Count() > 0)
                {
                    idTime_New = entities.times.Local.Last().idTime + 1;
                }
                else if (entities.times.Count() > 0)
                {
                    idTime_New = entities.times.ToList().Last().idTime + 1;
                }

                var time_New = new time
                {
                    idTime = idTime_New,
                    startTime = @event.StartDateTime.Value.TimeOfDay,
                    startDate = @event.StartDateTime,
                    endTime = @event.EndDateTime.Value.TimeOfDay,
                    endDate = @event.EndDateTime,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                };
                entities.times.Add(time_New);
                entities.Entry(time_New).State = EntityState.Added;

                /* Create new Todo Item */
                int idTodoItem_New = 1;
                if (entities.todoitems.Local.Count() > 0)
                {
                    idTodoItem_New = entities.todoitems.Local.Last().idToDoItem + 1;
                }
                else if (entities.todoitems.Count() > 0)
                {
                    idTodoItem_New = entities.todoitems.ToList().Last().idToDoItem + 1;
                }
                var todoitem_New = new todoitem
                {
                    idToDoItem = idTodoItem_New,
                    isGroup = false,
                    title = @event.Title,
                    percentComplete = 0,
                    idTime = idTime_New,
                    isComplete = false,
                    isVisible = true,
                    isActive = true,
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                };
                entities.todoitems.Add(todoitem_New);
                entities.Entry(todoitem_New).State = EntityState.Added;


                /* Map TodoItem to Type */
                int idTodoItem_Type_New = 1;
                if (entities.todoitem_type.Local.Count() > 0)
                {
                    idTodoItem_Type_New = entities.todoitem_type.Local.Last().idTodoItem_Type + 1;
                }
                else if (entities.todoitem_type.Count() > 0)
                {
                    idTodoItem_Type_New = entities.todoitem_type.ToList().Last().idTodoItem_Type + 1;
                }
                var todoitem_type_New = new todoitem_type
                {
                    idTodoItem_Type = idTodoItem_Type_New,
                    idToDoItem = idTodoItem_New,
                    idType = Types.GetID(Types._errand),
                    createdAt = DateTime.Now,
                    updatedAt = DateTime.Now,
                };
                entities.todoitem_type.Add(todoitem_type_New);
                entities.Entry(todoitem_type_New).State = EntityState.Added;

                if (shouldSave) entities.SaveChanges();

                /* Create a TodoItem model from newly created todoitem */
                var todoitem_views = entities.todoitem_view.Where(x => x.idTodoItem == todoitem_New.idToDoItem).ToList();
                var TodoItem = this.CreateTodoItemModels(todoitem_views)[0];

                /* Create new task in database then create new Task model based on new task, to create Event */
                var idTask_New = this.CreateTask(TodoItem, new List<task>(), @event.StartDateTime.Value, @event.EndDateTime.Value, "TodoItem_Errand", "Day", true);
                var task_views = entities.task_view.Where(x => x.idTask == idTask_New).ToList();
                var Task = this.GetTaskModels(task_views)[0];

                @event.idEvent = this.CreateEvent(Task, shouldSave);
            }
        }

        public void UpdateEvents(List<Event> Events)
        {
            foreach (var @event in Events)
            {
                var eventModel = entities.events.SingleOrDefault(x => x.idEvent == @event.idEvent);

                eventModel.title = @event.Title;
                eventModel.type = @event.Type;
                eventModel.startDateTime = @event.StartDateTime;
                eventModel.endDateTime = @event.EndDateTime;
                eventModel.isAllDay = @event.IsAllDay;
                eventModel.timezone = @event.Timezone;
                eventModel.repeat = @event.Repeat;
                eventModel.description = @event.Description;
                eventModel.isVisible = @event.IsVisible;
                eventModel.isActive = (bool)@event.IsActive;
                //eventModel.updatedAt = DateTime.Now;

                entities.Entry(eventModel).State = EntityState.Modified;
            }
            entities.SaveChanges();
        }

        public void DeleteEvents(List<Event> Events)
        {
            foreach (var @event in Events)
            {
                var taskIDs_Event = entities.task_event.Where(x => x.idEvent == @event.idEvent).Select(y => y.idTask); // Get TaskIDs for Event

                /* Remove Task_Routines */
                    foreach (var taskRoutine in entities.task_routine.Where(x => taskIDs_Event.Contains(x.idTask)))
                    entities.task_routine.Remove(taskRoutine);

                /* Remove Task_TodoItems */
                foreach (var taskTodoItem in entities.task_todoitem.Where(x => taskIDs_Event.Contains(x.idTask)))
                    entities.task_todoitem.Remove(taskTodoItem);

                /* Remove Task_Events */
                foreach (var taskEvent in entities.task_event.Where(x => x.idEvent == @event.idEvent))
                    entities.task_event.Remove(taskEvent);

                /* Remove Task */
                foreach (var task in entities.tasks.Where(x => taskIDs_Event.Contains(x.idTask)))
                    entities.tasks.Remove(task);

                /* Remove Event*/
                entities.events.Remove(entities.events.SingleOrDefault(x => x.idEvent == @event.idEvent));
            }

            entities.SaveChanges();
        }

        public List<Event> UpdateEventPercentComplete(bool shouldSave = false)
        {
            var eventIDs = this.GetEvents().Select(x => x.idEvent).ToList();
            return this.UpdateEventPercentComplete(eventIDs);
        }

        public List<Event> UpdateEventPercentComplete(Task Task, bool shouldSave = false)
        {
            var eventIDs = (Task.EventIDs.Count() > 0) ?
                //entities.events.Where(x => Task.EventIDs.Contains(x.idEvent) && Task.StartDateTime >= x.startDateTime && Task.StartDateTime <= x.endDateTime).Select(x => x.idEvent).ToList() : // If Task is from TodoItem
                entities.events.Where(x => Task.EventIDs.Contains(x.idEvent)).Select(x => x.idEvent).ToList() : // If Task is from TodoItem
                entities.events.Where(x => Task.EventIDs_TodoItemRoutine.Contains(x.idEvent) && Task.StartDateTime >= x.startDateTime && Task.StartDateTime <= x.endDateTime).Select(x => x.idEvent).ToList(); // If Task is from Routine

            return this.UpdateEventPercentComplete(eventIDs);
        }

        public List<Event> UpdateEventPercentComplete(List<int> eventIDs, bool shouldSave = false)
        {
            var Events = new List<Event>();
            //var events = entities.events.Where(x => eventIDs.Contains(x.idEvent)).ToList(); // If Task is from Routine

            ////var events = (Task.EventIDs.Count() > 0) ?
            ////    entities.events.Where(x => Task.EventIDs.Contains(x.idEvent) && Task.StartDateTime >= x.startDateTime && Task.StartDateTime <= x.endDateTime).ToList() : // If Task is from TodoItem
            ////    entities.events.Where(x => Task.EventIDs_TodoItemRoutine.Contains(x.idEvent) && Task.StartDateTime >= x.startDateTime && Task.StartDateTime <= x.endDateTime).ToList(); // If Task is from Routine

            //foreach (var @event in events)
            //{
            //    var taskTally = 0;
            //    var completeTally = 0;
            //    var CurrentDate = DateTime.Now;

            //    if (@event.type == "Routine_Single" || @event.type == "Routine_Repetitive" || @event.type == "Routine_Complete" || @event.type == "Routine_Incomplete")
            //    {
            //        var taskIDs_Event = entities.task_event.Where(x => x.idEvent == @event.idEvent).Select(y => y.idTask).ToList();
            //        foreach (var taskID_Event in taskIDs_Event)
            //        {
            //            var routineID = entities.task_routine.SingleOrDefault(x => x.idTask == taskID_Event).idRoutine;
            //            var todoItemIDs_Routine = entities.routine_todoitem.Where(x => x.idRoutine == routineID).Select(y => y.idToDoItem).ToList();
            //            var taskIDs_TodoItem = entities.task_todoitem.Where(x => todoItemIDs_Routine.Contains(x.idToDoItem)).Select(y => y.idTask).ToList();

            //            foreach (var task in entities.tasks.Where(x => taskIDs_TodoItem.Contains(x.idTask) &&
            //                x.startDateTime >= @event.startDateTime && x.startDateTime <= @event.endDateTime).ToList())
            //            {
            //                taskTally++;

            //                if (task.isComplete == true)
            //                    completeTally++;
            //            }
            //        }

            //        decimal @decimal = (taskTally > 0) ? ((decimal)completeTally / taskTally) * 100 : 0; // Numbers can't be divided my 0
            //        @event.percentComplete = (int)Math.Round(@decimal, MidpointRounding.AwayFromZero);

            //        if (@event.percentComplete == 100) // If Event already passed and is 100% complete
            //        {
            //            @event.type = "Routine_Complete";
            //        }
            //        else if (@event.endDateTime < CurrentDate && @event.percentComplete < 100) // If Event already passed and is not 100% complete
            //        {
            //            @event.type = "Routine_Incomplete";
            //        }

            //        entities.Entry(@event).State = EntityState.Modified;

            //        var Event = new Event();
            //        Events.Add(Event);

            //        CoachMapper.Map(@event, Event);
            //    }
            //    else if (@event.type == "TodoItem_Spotlight" || @event.type == "TodoItem_Errand" || @event.type == "TodoItem_Tally" ||
            //        @event.type == "TodoItem_Inverse" || @event.type == "TodoItem_Repetitive" || @event.type == "Task_Complete" || @event.type == "Task_Incomplete")
            //    {
            //        var taskIDs_Event = entities.task_event.Where(x => x.idEvent == @event.idEvent).Select(y => y.idTask).ToList();
            //        foreach (var taskID_Event in taskIDs_Event)
            //        {
            //            var task = entities.tasks.SingleOrDefault(x => x.idTask == taskID_Event && x.startDateTime >= @event.startDateTime && x.startDateTime <= @event.endDateTime);

            //            taskTally++;

            //            if (task.isComplete == true)
            //                completeTally++;
            //        }

            //        decimal @decimal = (taskTally > 0) ? ((decimal)completeTally / taskTally) * 100 : 0; // Numbers can't be divided my 0
            //        @event.percentComplete = (int)Math.Round(@decimal, MidpointRounding.AwayFromZero);

            //        if (@event.percentComplete == 100) // If Event is 100% complete
            //        {
            //            @event.type = "Task_Complete";
            //        }
            //        else if (@event.endDateTime < CurrentDate && @event.percentComplete < 100) // If Event already passed and is not 100% complete
            //        {
            //            @event.type = "Task_Incomplete";
            //        }

            //        entities.Entry(@event).State = EntityState.Modified;

            //        var Event = new Event();
            //        Events.Add(Event);

            //        CoachMapper.Map(@event, Event);
            //    }

            //    Debug.WriteLine($"Updated percent complete for Event: {@event.idEvent} - {@event.title}");
            //}

            //if (shouldSave == true) entities.SaveChanges();

            return Events;
        }



        /*----------------------------------------------------------------------------------------------------*/
        /*-------------------------------------------- Extentions --------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        /* Get day of the month for date based off of dayOfWeek */
        public int GetDayOfMonth(DateTime DateTime_In, string dayOfWeek)
        {
            var daysOfWeek = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            //var dayCountInMonth = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            var dayOfWeekInt = daysOfWeek.IndexOf(dayOfWeek);

            var FirstDayOfWeek = DateTime_In.AddDays(-(int)DateTime_In.DayOfWeek);
            var DateTime_Out = FirstDayOfWeek.AddDays(dayOfWeekInt);

            if (DateTime_Out.Month == DateTime_In.Month)
            {
                return DateTime_Out.Day;
            }
            else
            {
                return -1;
            }

        }

        public int GetDayOfMonth(DateTime DateTime, string position, string dayOfWeek)
        {
            var positions = new List<string> { "First", "Second", "Third", "Fourth", "Fifth", "Last" };
            var daysOfWeek = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

            var positionInt = positions.IndexOf(position);
            DayOfWeek DayOfWeek;

            if (dayOfWeek == "Sunday")
            {
                DayOfWeek = DayOfWeek.Sunday;
            }
            else if (dayOfWeek == "Monday")
            {
                DayOfWeek = DayOfWeek.Monday;
            }
            else if (dayOfWeek == "Tuesday")
            {
                DayOfWeek = DayOfWeek.Tuesday;
            }
            else if (dayOfWeek == "Wednesday")
            {
                DayOfWeek = DayOfWeek.Wednesday;
            }
            else if (dayOfWeek == "Thursday")
            {
                DayOfWeek = DayOfWeek.Thursday;
            }
            else if (dayOfWeek == "Friday")
            {
                DayOfWeek = DayOfWeek.Friday;
            }
            else
            {
                DayOfWeek = DayOfWeek.Saturday;
            }

            /* Get date */
            int dayOfWeekCount = 0;

            var LastDayOfMonth = DateTime.AddMonths(1).AddDays(-1);

            var SeedDate = new DateTime(DateTime.Year, DateTime.Month, 1); // Variable to find weekday, start on first day of month
            while (SeedDate.Day != LastDayOfMonth.Day)
            {
                if (SeedDate.DayOfWeek == DayOfWeek)
                {
                    if (dayOfWeekCount == positionInt)
                    {
                        return SeedDate.Day;
                    }

                    dayOfWeekCount++;
                }
                SeedDate = SeedDate.AddDays(1);
            }

            return -1; // If date wasn't found, return -1
        }

        public List<Timeframe> GetTimeframes()
        {
            var Timeframes = new List<Timeframe>();

            foreach (var timeframe in entities.timeframes.ToList())
            {
                var Timeframe = new Timeframe();
                Timeframes.Add(Timeframe);

                /* Map database model to app model */
                //CoachMapper.CreateMap<timeframe, Timeframe>();
                CoachMapper.Map(timeframe, Timeframe);
            }

            return Timeframes;
        }
    }

    public static class Temp
    {
        public static void SaveExceptionToDatabase(Exception ex)
        {
            var entities = new coachogEntities();

            var note_New = new note
            {
                note1 = ex.GetFullStackTraceString()
            };

            entities.notes.Add(note_New);
            entities.SaveChanges();
        }
    }

    public static class ExtensionMethods
    {
        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem, Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(this TSource source, Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }
    }

    public static class ExceptionExtensions
    {
        public static string GetFullMessageString(this Exception exception)
        {
            var fullMessageString = "";
            var exceptions = exception.FromHierarchy(ex => ex.InnerException).ToList();

            foreach (var exceptionTemp in exceptions)
            {
                fullMessageString += exceptionTemp.GetType().FullName + ": " + exceptionTemp.Message;
                fullMessageString += (exceptionTemp != exceptions.Last()) ? " --->\r\n" : "";
            }

            return fullMessageString;
        }

        public static string GetFullStackTraceString(this Exception exception)
        {
            var stackTrace = "";
            var exceptions = exception.FromHierarchy(ex => ex.InnerException).ToList();

            foreach (var exceptionTemp in exceptions)
            {
                stackTrace += exceptionTemp.GetType().FullName + ": " + exceptionTemp.Message;
                stackTrace += "\r\n";
                stackTrace += exceptionTemp.StackTrace;
                stackTrace += (exceptionTemp != exceptions.Last()) ?
                    $"\r\n--- End of {exceptionTemp.GetType().FullName} stack trace ---\r\n\r\n" : $"\r\n--- End of {exceptionTemp.GetType().FullName} stack trace ---";
            }

            return stackTrace;
        }

        //public static IEnumerable<string> GetFullStackTrace(this Exception exception)
        //{
        //    var exceptions = exception.FromHierarchy(ex => ex.InnerException);

        //    return exceptions.Select(x => x.StackTrace);
        //}

        //public static string GetFullStackTraceString(this Exception exception)
        //{
        //    var stackTraci = exception.GetFullStackTrace().Reverse().ToList();
        //    var stackTraciString = String.Join("\r\n", stackTraci);

        //    return stackTraciString;
        //}


        public static List<string> GetAllMessages(this Exception exception)
        {
            var exceptions = exception.FromHierarchy(ex => ex.InnerException);

            return exceptions.Select(x => x.Message).ToList();
        }

        public static List<string> GetAllMessagesWithClassName(this Exception exception)
        {
            var exceptions = exception.FromHierarchy(ex => ex.InnerException);

            return exceptions.Select(x => x.GetType().Name + ": " + x.Message).ToList();
        }

        //public static string GetFullMessageString1(this Exception exception)
        //{
        //    var messages = exception.GetAllMessages().Reverse().ToList();
        //    var messagesString = String.Join("\r\n", messages);

        //    return messagesString;
        //}
    }

}