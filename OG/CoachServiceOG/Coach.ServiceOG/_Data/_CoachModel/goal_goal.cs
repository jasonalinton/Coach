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
    
    public partial class goal_goal
    {
        public int idGoal_Goal { get; set; }
        public int idGoal { get; set; }
        public int idParentGoal { get; set; }
        public int level { get; set; }
        public System.DateTime createdAt { get; set; }
        public System.DateTime updatedAt { get; set; }
    
        public virtual goal goal { get; set; }
        public virtual goal goal1 { get; set; }
    }
}
