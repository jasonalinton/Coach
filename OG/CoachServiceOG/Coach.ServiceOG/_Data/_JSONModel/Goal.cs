//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coach.Service._Data._JSONModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Goal
    {
        public int idGoal { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string repetition { get; set; }
        public string timeframe { get; set; }
        public string deadline { get; set; }
        public string idInventoryItems { get; set; }
        public string idParents { get; set; }
        public string idChildren { get; set; }
        public string idRoutines { get; set; }
        public string explaination { get; set; }
        public Nullable<bool> active { get; set; }
        public string idTodoItems { get; set; }
    }
}