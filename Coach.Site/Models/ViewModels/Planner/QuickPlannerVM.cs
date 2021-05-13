using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coach.Site.Models.ViewModels.Planner2
{
    public class QuickPlannerVM
    {
        public void CreateEvents()
        {

        }

        public Day CurrentDay { get; set; }
        public List<Day> Days { get; set; }
    }

    public class Day
    {
        public DateTime Date { get; set; }
        public string DateString => Date.ToShortDateString();
        public string Title { get; set; }
        public string SuccessPercent { get; set; }
        public bool State { get; set; }
        public List<Goal> Goals { get; set; }
    }

    public class Event
    {

    }
    
    public class Goal
    {
        // Metrics/Info
        public List<Todo> Todos { get; set; }
    }

    public class Todo
    {

    }

    public interface ISuccessState
    {
        int SuccessPercent { get; set; }
        bool State { get; set; }
    }

    public class SuccessState 
    {

    }
}