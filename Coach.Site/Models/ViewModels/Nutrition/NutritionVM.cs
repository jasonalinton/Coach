using Coach.Data.Extension;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Coach.Site.Models.ViewModels.Nutrition
{
    public class NutritionVM
    {
        public NutritionVM(List<CoachModel._Inventory._Physical._Nutrition.MealChartMeal> mealChartMeals, DateTime? startDate = null, DateTime? endDate = null, DateTime? currentDate = null, int caloriesRecommendedDay = 2000, int waterRecommendedDay = 140)
        {
            Days = new List<NutritionDay>();

            startDate = startDate?.Date;
            endDate = endDate?.Date ?? startDate;
            currentDate = currentDate?.Date;

            if (mealChartMeals.Count > 0)
            {
                mealChartMeals = mealChartMeals.OrderBy(x => x.DateTime).ToList();
                if (startDate == null)
                    startDate = mealChartMeals.First().DateTime.Date;
                if (endDate == null)
                    endDate = mealChartMeals.Last().DateTime.Date;

                for (var date = startDate?.Date; date <= endDate; date = date?.AddDays(1))
                {
                    var mealChartMealsForDate = mealChartMeals.Where(x => x.DateTime.Date == date).ToList();
                    Days.Add(new NutritionDay((DateTime)date, mealChartMealsForDate, caloriesRecommendedDay, waterRecommendedDay));
                }
            }

            CurrentNutritionInfo = Days.SingleOrDefault(x => x.Date == (currentDate?.Date.ToShortDateString() ?? DateTime.Today.Date.ToShortDateString()));
        }

        public List<NutritionDay> Days { get; set; }
        public NutritionDay CurrentNutritionInfo { get; set; }
    }

    public class NutritionDay
    {
        public NutritionDay(DateTime date, List<CoachModel._Inventory._Physical._Nutrition.MealChartMeal> mealChartMeals, int caloriesRecommended, int waterRecommended)
        {
            Date = date.ToShortDateString();

            CalorieSuccessStates = new SuccessStates(new SuccessState(90), new SuccessState(50, 89), new SuccessState(0, 49));
            MacroSuccessStates = new SuccessStates(new SuccessState(80, 100, true), new SuccessState(60, 79, true), new SuccessState(null, 59, true));
            WaterSuccessStates = new SuccessStates(new SuccessState(90), new SuccessState(50, 89), new SuccessState(0, 49));

            Calorie = new Nutrient(CalorieSuccessStates, caloriesRecommended, (int)mealChartMeals.Sum(x => x.Calaries));
            Water = new Nutrient(WaterSuccessStates, waterRecommended, mealChartMeals.Sum(x => x.Water));

            var carbsConsumed = (int)mealChartMeals.Sum(x => x.Carbs);
            var proteinConsumed = (int)mealChartMeals.Sum(x => x.Protein);
            var fatConsumed = (int)mealChartMeals.Sum(x => x.Fat);
            var macrosConsumed = carbsConsumed + proteinConsumed + fatConsumed;

            Carbs = CreateMacroNutrient(50, macrosConsumed, carbsConsumed);
            Protein = CreateMacroNutrient(25, macrosConsumed, proteinConsumed);
            Fat = CreateMacroNutrient(25, macrosConsumed, fatConsumed);

            Meals = CreateMeals(date, mealChartMeals, caloriesRecommended, waterRecommended);
        }

        public string Date { get; set; }
        public Nutrient Calorie { get; set; }
        public Nutrient Water { get; set; }
        public Nutrient Carbs { get; set; }
        public Nutrient Protein { get; set; }
        public Nutrient Fat { get; set; }
        public List<Meal> Meals { get; set; }
        public SuccessStates CalorieSuccessStates {get;set;}
        public SuccessStates MacroSuccessStates {get;set;}
        public SuccessStates WaterSuccessStates {get;set;}
        public enum RecommendedMeals
        {
            Breakfast,
            Brunch,
            Lunch,
            Linner,
            Dinner
        }

        public List<Meal> CreateMeals(DateTime date, List<CoachModel._Inventory._Physical._Nutrition.MealChartMeal> mealChartMeals, int caloriesRecommended, int waterRecommended)
        {
            var meals = new List<Meal>();

            var recommendedMeals = Enum.GetValues(typeof(RecommendedMeals)).Cast<RecommendedMeals>().Select(x => x.ToString()).ToList();
            var caloriesRecommendedMeal = caloriesRecommended / recommendedMeals.Count;
            var waterRecommendedMeal = waterRecommended / recommendedMeals.Count;


            foreach (var recommendedMeal in recommendedMeals)
            {
                /* If iterated date is before current date && and the meal doesn't exist for that date, set consumed -1 to flag it as having no data */
                int caloriesConsumed = (date < DateTime.Today.Date) ? -1 : 0;
                int waterConsumed = (date < DateTime.Today.Date) ? -1 : 0;

                int carbsConsumed = (date < DateTime.Today.Date) ? -1 : 0;
                int proteinConsumed = (date < DateTime.Today.Date) ? -1 : 0;
                int fatConsumed = (date < DateTime.Today.Date) ? -1 : 0;
                int macrosConsumed = (date < DateTime.Today.Date) ? -1 : 0;

                bool wasConsumed = false;

                var mealChartMeal = mealChartMeals.FirstOrDefault(x => x.Name == recommendedMeal);
                if (mealChartMeal != null)
                {
                    caloriesConsumed = (int)mealChartMeal.Calaries;
                    waterConsumed = mealChartMeal.Water;
                    
                    carbsConsumed = (int)mealChartMeal.Carbs;
                    proteinConsumed = (int)mealChartMeal.Protein;
                    fatConsumed = (int)mealChartMeal.Fat;
                    macrosConsumed = carbsConsumed + proteinConsumed + fatConsumed;

                    wasConsumed = mealChartMeal.WasConsumed;

                    mealChartMeals.Remove(mealChartMeal);
                }

                var calorie = new Nutrient(CalorieSuccessStates, caloriesRecommendedMeal, caloriesConsumed);
                var water = new Nutrient(WaterSuccessStates, waterRecommendedMeal, waterConsumed);

                var carbs = CreateMacroNutrient(50, macrosConsumed, carbsConsumed);
                var protein = CreateMacroNutrient(25, macrosConsumed, proteinConsumed);
                var fat = CreateMacroNutrient(25, macrosConsumed, fatConsumed);


                meals.Add(new Meal(recommendedMeal, wasConsumed, calorie, water, carbs, protein, fat));
            }

            return meals;
        }

        // TODO: Come up with a much better way to handle nutrients and macro nutrients
        public Nutrient CreateMacroNutrient(int percentOfMacroCalories, int macroNutrientCaloriesConsumed, int macroConsumed)
        {
            if (macroConsumed == 12)
            {
                var hi = 1;
            }
            /* The Macro Nutrients recommended is based on the total calories from macro nutrients consumed */
            var macroRecommendedDouble = ((double)percentOfMacroCalories / 100) * macroNutrientCaloriesConsumed;
            var macroRecommended = (int)Math.Round(macroRecommendedDouble, MidpointRounding.AwayFromZero);
            return new Nutrient(MacroSuccessStates, macroRecommended, macroConsumed);
        }
    }

    public class Meal
    {
        public Meal(string name, bool wasConsumed, Nutrient calorie, Nutrient water, Nutrient carbs, Nutrient protein, Nutrient fat)
        {
            Name = name;

            Calorie = calorie;
            Water = water;
            Carbs = carbs;
            Protein = protein;
            Fat = fat;

            WasConsumed = wasConsumed;
        }

        public string Name { get; set; }
        public bool WasConsumed { get; set; }
        public Nutrient Calorie { get; set; }
        public Nutrient Water { get; set; }
        public Nutrient Carbs { get; set; }
        public Nutrient Protein { get; set; }
        public Nutrient Fat { get; set; }
    }

    public class Nutrient
    {
        public Nutrient(SuccessStates successStates, int recommended, int consumed = 0)
        {
            Recommended = recommended;
            Consumed = consumed;
            SuccessStates = successStates;
        }

        public int Recommended { get; set; }
        public int Consumed { get; set; }
        public int Remaining => Recommended - Consumed;
        public int Percent
        {
            get
            {
                var consumed = (double)Consumed / (double)Recommended * 100;
                return (Recommended == 0) ? 0 : (int)Math.Round(consumed, MidpointRounding.AwayFromZero);
            }
        }
        [ScriptIgnore]
        public SuccessStates SuccessStates { get; set; }
        public string SuccessState => SuccessStates.GetState(Consumed, Recommended);
    }

    public class SuccessStates
    {
        public SuccessStates(SuccessState success, SuccessState close, SuccessState failure)
        {
            Success = success;
            Close = close;
            Failure = failure;
        }

        public SuccessState Success { get; set; }
        public SuccessState Close { get; set; }
        public SuccessState Failure { get; set; }
        public string GetState(int consumed, int recommended)
        {
            if (consumed == -1)
                return "No-Data";
            else if (Success.IsState(consumed, recommended))
                return "Success";
            else if (Close.IsState(consumed, recommended))
                return "Close";
            else if (Failure.IsState(consumed, recommended))
                return "Failure";
            else
                return "Pending";
        }
    }

    public class SuccessState
    {
        public SuccessState(int? minimumPercent = 0, int? maximumPercent = null, bool isAbsolute = false)
        {
            MinimumPercent = minimumPercent;
            MaximumPercent = maximumPercent;
            IsAbsolute = isAbsolute;
        }

        public int? MinimumPercent { get; set; }
        public int? MaximumPercent { get; set; }
        public bool IsAbsolute { get; set; }

        public bool IsState(int consumed, int recommended)
        {

            if (IsAbsolute && consumed > recommended)
                consumed = recommended + (recommended - consumed);

            var minimumConsumed = (MinimumPercent != null) ? (double?)(((double)MinimumPercent / 100) * recommended) : null;
            var maximumConsumed = (MaximumPercent != null) ? (double?)((((double)MaximumPercent + 1) / 100) * recommended) : null;

            if ((maximumConsumed != null && (consumed >= minimumConsumed && consumed < maximumConsumed)) ||
                (maximumConsumed == null && (consumed >= minimumConsumed)))
                return true;
            else if (minimumConsumed == null && consumed < maximumConsumed)
                return true;
            else
                return false;
        }
    }
}