//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.Service._Date._CoachModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class task
    {
        public task()
        {
            this.task_event = new HashSet<task_event>();
            this.task_routine = new HashSet<task_routine>();
            this.task_todoitem = new HashSet<task_todoitem>();
        }
    
        public int idTask { get; set; }
        public string label { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string type { get; set; }
        public string timeframe { get; set; }
        public Nullable<System.DateTime> startDateTime { get; set; }
        public Nullable<System.DateTime> endDateTime { get; set; }
        public Nullable<int> idTime { get; set; }
        public Nullable<bool> isComplete { get; set; }
        public Nullable<bool> isVisible { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> dateTimeCompleted { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual ICollection<task_event> task_event { get; set; }
        public virtual ICollection<task_routine> task_routine { get; set; }
        public virtual time time { get; set; }
        public virtual ICollection<task_todoitem> task_todoitem { get; set; }
    }
}