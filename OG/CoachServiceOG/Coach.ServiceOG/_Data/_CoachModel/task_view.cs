//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.ServiceOG._Data._CoachModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class task_view
    {
        public int idTask { get; set; }
        public string label { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string timeframe { get; set; }
        public Nullable<System.DateTime> startDateTime { get; set; }
        public Nullable<System.DateTime> endDateTime { get; set; }
        public string todoItems { get; set; }
        public string goals_TodoItem { get; set; }
        public string routines { get; set; }
        public string todoItems_Routine { get; set; }
        public string goals_Routine { get; set; }
        public string goals_RoutineTodoItem { get; set; }
        public string inventoryItems_TodoItem { get; set; }
        public string inventoryItems_Routine { get; set; }
        public string inventoryItems_RoutineTodoItem { get; set; }
        public Nullable<int> idTime { get; set; }
        public Nullable<bool> isComplete { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> dateTimeCompleted { get; set; }
        public string eventIDs { get; set; }
        public string eventIDs_TodoItemRoutine { get; set; }
        public string todoItemIDs { get; set; }
        public string goalIDs_TodoItem { get; set; }
        public string routineIDs { get; set; }
        public string routineIDs_TodoItem { get; set; }
        public string todoItemIDs_Routine { get; set; }
        public string todoItemIDs_TodoItemRoutine { get; set; }
        public string goalIDs_Routine { get; set; }
        public string goalIDs_RoutineTodoItem { get; set; }
        public string inventoryItemIDs_TodoItem { get; set; }
        public string inventoryItemIDs_Routine { get; set; }
        public string inventoryItemIDs_RoutineTodoItem { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    }
}
