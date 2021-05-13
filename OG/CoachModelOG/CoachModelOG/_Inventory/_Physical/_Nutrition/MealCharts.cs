using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Inventory._Physical._Nutrition
{
    public class MealCharts
    {
        public MealCharts()
        {
            this.Meals = new List<MealChartMeal>();
        }

        public List<MealChartMeal> Meals { get; set; }
    }

    public class MealChart
    {
        public MealChart()
        {
            this.Meals = new List<MealChartMeal>();
        }

        public MealChart(List<MealChartMeal> meals)
        {
            this.Meals = meals;
            this.WaterLogs = new List<WaterLog>();
        }


        public List<double> BreakfastCalaries { get { return this.Meals.Where(x => x.Name == "Breakfast").Select(x => x.Calaries).ToList(); } }
        //public double BrunchCalaries { get { return this.FoodItems.Where(x => x.MealName == "Brunch").Sum(x => x.Calaries); } }
        //public double LunchCalaries { get { return this.FoodItems.Where(x => x.MealName == "Lunch").Sum(x => x.Calaries); } }
        //public double LinnerCalaries { get { return this.FoodItems.Where(x => x.MealName == "Linner").Sum(x => x.Calaries); } }
        //public double DinnerCalaries { get { return this.FoodItems.Where(x => x.MealName == "Dinner").Sum(x => x.Calaries); } }
        //public double PreWorkoutCalaries { get { return this.FoodItems.Where(x => x.MealName == "PreWorkout").Sum(x => x.Calaries); } }



        public List<MealChartMeal> Meals { get; set; }
        public List<WaterLog> WaterLogs { get; set; }

        public List<MealChartMeal> BreakfastMeals { get { return this.Meals.Where(x => x.Name == "Breakfast").ToList(); } }
        public List<MealChartMeal> BrunchMeals { get { return this.Meals.Where(x => x.Name == "Brunch").ToList(); } }
        public List<MealChartMeal> LunchMeals { get { return this.Meals.Where(x => x.Name == "Lunch").ToList(); } }
        public List<MealChartMeal> LinnerMeals { get { return this.Meals.Where(x => x.Name == "Linner").ToList(); } }
        public List<MealChartMeal> DinnerMeals { get { return this.Meals.Where(x => x.Name == "Dinner").ToList(); } }
        public List<MealChartMeal> PreWorkoutMeals { get { return this.Meals.Where(x => x.Name == "PreWorkout").ToList(); } }

        public List<WaterLog> BreakfastWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }
        public List<WaterLog> BrunchWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }
        public List<WaterLog> LunchWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }
        public List<WaterLog> LinnerWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }
        public List<WaterLog> DinnerWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }
        public List<WaterLog> PreWorkoutWaterLogs { get { return this.BreakfastMeals.SelectMany(x => x.WaterLogs).ToList(); } }



    }

    public class MealChartMeal
    {
        public MealChartMeal(int idMeal, string name, DateTime date)
        {
            this.idMeal = idMeal;
            this.Name = name;
            this.DateTime = date;

            this.TaskIDs = new List<int>();
            this.FoodItems = new List<FoodItem>();
            this.WaterLogs = new List<WaterLog>();
        }
        public int idMeal { get; set; }
        public string Name { get; set; }
        public double Calaries { get { return this.FoodItems.Sum(x => x.Calaries); } }
        public double Carbs { get { return this.FoodItems.Sum(x => x.Carbs); } }
        public double Protein { get { return this.FoodItems.Sum(x => x.Protein); } }
        public double Fat { get { return this.FoodItems.Sum(x => x.Fat); } }
        public double CalariesConsumed { get { return this.FoodItems.Where(x => x.WasConsumed == true).Sum(x => x.Calaries); } }
        public double CarbsConsumed { get { return this.FoodItems.Where(x => x.WasConsumed == true).Sum(x => x.Carbs); } }
        public double ProteinConsumed { get { return this.FoodItems.Where(x => x.WasConsumed == true).Sum(x => x.Protein); } }
        public double FatConsumed { get { return this.FoodItems.Where(x => x.WasConsumed == true).Sum(x => x.Fat); } }
        public int Water { get { return this.WaterLogs.Sum(x => x.Amount); } }
        public bool WasConsumed { get; set; }
        public DateTime DateTime { get; set; }
        public List<int> TaskIDs { get; set; }
        public List<FoodItem> FoodItems { get; set; }
        public List<WaterLog> WaterLogs { get; set; }
    }

    public class FoodItem
    {
        public int idFoodItem { get; set; }
        public string Name { get; set; }
        public string MealName { get; set; }
        public double Calaries { get; set; }
        public double Carbs { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public bool WasConsumed { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class WaterLog
    {
        public int idWaterLog { get; set; }
        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
    }
}
