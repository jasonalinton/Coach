using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Coach.ServiceOG._Data._CoachModel;
using CoachModel._App._Task;
using CoachModel._App._Time;
using CoachModel._Inventory;
using CoachModel._Inventory._Log;
using CoachModel._Inventory._Physical._Nutrition;
using CoachModel._Planner;

namespace Coach.ServiceOG._Mappings
{
    public class MyMapper
    {
        public static void CreateMaps()
        {
            #region Inventory

            Mapper.CreateMap<inventoryitem_view, InventoryItem>()
                .ForMember(g => g.GoalIDs_Concat, m => m.MapFrom(g_v => g_v.goalIDs))
                .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))
                .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<goal_view, Goal>()
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

            Mapper.CreateMap<todoitem_view, TodoItem>()
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

            Mapper.CreateMap<routine_view, Routine>()
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

            Mapper.CreateMap<task_view, Task>()
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

            Mapper.CreateMap<event_view, Event>()
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
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<repeat_view, Repeat>()
                .ForMember(g => g.Repeat_DayOfWeekIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_DayOfWeekIDs))
                .ForMember(g => g.Repeat_DayOfMonthIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_DayOfMonthIDs))
                .ForMember(g => g.Repeat_MonthIDs_Concat, m => m.MapFrom(g_v => g_v.repeat_MonthIDs))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();



            Mapper.CreateMap<@event, Event>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<time, Time>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion


            Mapper.CreateMap<type, CoachModel._App._Universal.Type>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<medium, CoachModel._App._Universal.Medium>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<deadline, CoachModel._App._Universal.Deadline>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<timeframe, Timeframe>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();


            #region Food Tracker 

            Mapper.CreateMap<task_mealfooditemnutrient_view, MealItem>()
                .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            /* Meal Chart */
            Mapper.CreateMap<task_mealfooditemnutrient_view, FoodItem>()
                .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.foodName))
                .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                .ForMember(g => g.DateTime, m => m.MapFrom(g_v => g_v.startDateTime))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<fooditem, MealItem>()
                .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.name))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<MealItem, fooditem>()
                .ForMember(g => g.name, m => m.MapFrom(g_v => g_v.FoodName))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<waterlog_mealtask_view, WaterLog>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
            Mapper.CreateMap<waterlog, WaterLog>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            Mapper.CreateMap<Food, MealItem>()
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

            Mapper.CreateMap<FoodBranded, MealItem>()
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
            Mapper.CreateMap<logitemfield_view, LogItem>()
                .ForMember(g => g.InventoryItemName, m => m.MapFrom(g_v => g_v.inventoryItem))
                .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.logItem))
                .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_LogItem))
                .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_LogItem))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            /* Log Item Field */
            Mapper.CreateMap<logitemfield_view, ItemField>()
                .ForMember(g => g.idType, m => m.MapFrom(g_v => g_v.idType_Field))
                .ForMember(g => g.idType_Data, m => m.MapFrom(g_v => g_v.idType_Data))
                .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.field))
                .ForMember(g => g.DateType, m => m.MapFrom(g_v => g_v.dataType))
                .ForMember(g => g.Type, m => m.MapFrom(g_v => g_v.fieldType))
                .ForMember(g => g.Position, m => m.MapFrom(g_v => g_v.position_Field))
                .ForMember(g => g.IsActive, m => m.MapFrom(g_v => g_v.isActive_Field))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            /* Log Item Field Value */
            Mapper.CreateMap<logentry_logitemfield, FieldValue>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            #endregion
        }
    }
}
