﻿using Coach.Data.EFExtention.Model.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coach.Data.Model
{
    public partial class todo_time : ICoachDevModel, IMappingTable
    {
        public string text { get; set; }
        public string TableName => "todo_time";
        public string DisplayName => "Todo-Time";
        public string LeftTableName => "todo";
        public string RightTableName => "time";
        public ICoachDevModel LeftTable => this.todo;
        public ICoachDevModel RightTable => this.time;

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