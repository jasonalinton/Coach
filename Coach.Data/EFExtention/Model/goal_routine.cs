﻿using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Coach.Data.Model
{
    public partial class goal_routine : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "goal_routine";
        public string DisplayName => "Goal-Routine";
        public string LeftTableName => "goal";
        public string RightTableName => "routine";
        public ICoachDevModel LeftTable => this.goal;
        public ICoachDevModel RightTable => this.routine;

        public override string ToString()
        {
            if (this.LeftTable != null && this.RightTable != null)
                return $"{this.DisplayName}: {this.LeftTable.text} :: {this.RightTable.text}";
            else if (this.LeftTable != null)
                return $"{this.DisplayName}: {this.LeftTable.text} :: {this.RightTableName}";
            else if (this.RightTable != null)
                return $"{this.DisplayName}: {this.LeftTableName} :: {this.RightTable.text}";

            return $"{this.DisplayName}";
        }
    }
}