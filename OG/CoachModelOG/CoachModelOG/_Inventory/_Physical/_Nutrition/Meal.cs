using CoachModel._App._Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CoachModel._Inventory._Physical._Nutrition
{
    public class Meal
    {
        //#region Variable
        //public double? calaries;
        //public double? calcium;
        //public double carbohydrates;
        //public double? cholesterol;
        //public double? monounsaturatedFat;
        //public double? polyunsaturatedFat;
        //public double? saturatedFat;
        //public double? transFat;
        //public double fat;
        //public double? iron;
        //public double? fiber;
        //public double? folate;
        //public double? motassium;
        //public double? magnesium;
        //public double? sodium;
        //public double? niacinB3;
        //public double? phosphorus;
        //public double protein;
        //public double? riboflavinB2;
        //public double? sugars;
        //public double? thiaminB1;
        //public double? vitaminE;
        //public double? vitaminA;
        //public double? vitaminB12;
        //public double? vitaminB6;
        //public double? vitaminC;
        //public double? vitaminD;
        //public double? vitaminK;
        //#endregion

        public Meal()
        {
            this.Macros = new Macros(this);
            this.TaskIDs = new List<int>();
            this.MealItems = new List<MealItem>();
        }

        public Meal(int idMeal, string name, string type, bool wasConsumed)
            : this()
        {
            this.idMeal = idMeal;
            this.Name = name;
            this.Type = type;
            this.WasConsumed = wasConsumed;

        }

        public Meal(List<MealItem> mealItems)
            : this()
        {
            this.MealItems = mealItems;
        }

        public int idMeal { get; set; }
        public int? idType { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool WasConsumed { get; set; }
        public DateTime DateTime { get; set; }

        #region Nutients
        public double? Calaries
        {
            get
            {
                if (this.MealItems.Where(x => x.Calaries != null).Count() > 0)
                {
                    return this.MealItems.Where(x => x.Calaries != null).Sum(x => x.Calaries);
                }
                else
                {
                    return null;
                }
            }
        }
        public double? Calcium { get { return this.MealItems.Sum(x => x.Calcium); } }
        public double Carbohydrates { get { return this.MealItems.Sum(x => x.Carbohydrates); } }
        public double? Cholesterol { get { return this.MealItems.Sum(x => x.Cholesterol); } }
        public double? MonounsaturatedFat { get { return this.MealItems.Sum(x => x.MonounsaturatedFat); } }
        public double? PolyunsaturatedFat { get { return this.MealItems.Sum(x => x.PolyunsaturatedFat); } }
        public double? SaturatedFat { get { return this.MealItems.Sum(x => x.SaturatedFat); } }
        public double Fat { get { return this.MealItems.Sum(x => x.Fat); } }
        public double? TransFat { get { return this.MealItems.Sum(x => x.TransFat); } }
        public double? Iron { get { return this.MealItems.Sum(x => x.Iron); } }
        public double? Fiber { get { return this.MealItems.Sum(x => x.Fiber); } }
        public double? Folate { get { return this.MealItems.Sum(x => x.Folate); } }
        public double? Potassium { get { return this.MealItems.Sum(x => x.Potassium); } }
        public double? Magnesium { get { return this.MealItems.Sum(x => x.Magnesium); } }
        public double? Sodium { get { return this.MealItems.Sum(x => x.Sodium); } }
        public double? NiacinB3
        {
            get
            {
                if (this.MealItems.Where(x => x.NiacinB3 != null).Count() > 0)
                {
                    return this.MealItems.Where(x => x.NiacinB3 != null).Sum(x => x.NiacinB3);
                }
                else
                {
                    return null;
                }
            }
            set { }
        }
        //public double? NiacinB3 { get { return this.MealItems.Where(x => x.NiacinB3 != null).Sum(x => x.NiacinB3); } }
        public double? Phosphorus { get { return this.MealItems.Sum(x => x.Phosphorus); } }
        public double Protein { get { return this.MealItems.Sum(x => x.Protein); } }
        public double? RiboflavinB2 { get { return this.MealItems.Sum(x => x.RiboflavinB2); } }
        public double? Sugars { get { return this.MealItems.Sum(x => x.Sugars); } }
        public double? ThiaminB1
        {
            get
            {
                if (this.MealItems.Where(x => x.ThiaminB1 != null).Count() > 0)
                {
                    return this.MealItems.Where(x => x.ThiaminB1 != null).Sum(x => x.ThiaminB1);
                }
                else
                {
                    return null;
                }
            }
        }
        public double? VitaminE { get { return this.MealItems.Sum(x => x.VitaminE); } }
        public double? VitaminA { get { return this.MealItems.Sum(x => x.VitaminA); } }
        public double? VitaminB12 { get { return this.MealItems.Sum(x => x.VitaminB12); } }
        public double? VitaminB6 { get { return this.MealItems.Sum(x => x.VitaminB6); } }
        public double? VitaminC { get { return this.MealItems.Sum(x => x.VitaminC); } }
        public double? VitaminD { get { return this.MealItems.Sum(x => x.VitaminD); } }
        public double? VitaminK { get { return this.MealItems.Sum(x => x.VitaminK); } }
        #endregion

        public Macros Macros { get; set; }
        public List<int> TaskIDs { get; set; }
        public List<MealItem> MealItems { get; set; }
    }

    public class MealItem
    {
        public int idFoodItem { get; set; }
        public string FoodName { get; set; }
        public double? Quantity { get; set; }
        public string Unit { get; set; }
        public string ThumbURL { get; set; }
        public bool WasConsumed { get; set; }

        #region Nutients
        public double? Calaries { get; set; }
        public double Carbohydrates { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double? Calcium { get; set; }
        public double? Cholesterol { get; set; }
        public double? MonounsaturatedFat { get; set; }
        public double? PolyunsaturatedFat { get; set; }
        public double? SaturatedFat { get; set; }
        public double? TransFat { get; set; }
        public double? Iron { get; set; }
        public double? Fiber { get; set; }
        public double? Folate { get; set; }
        public double? Potassium { get; set; }
        public double? Magnesium { get; set; }
        public double? Sodium { get; set; }
        public double? NiacinB3 { get; set; }
        public double? Phosphorus { get; set; }
        public double? RiboflavinB2 { get; set; }
        public double? Sugars { get; set; }
        public double? ThiaminB1 { get; set; }
        public double? VitaminE { get; set; }
        public double? VitaminA { get; set; }
        public double? VitaminB12 { get; set; }
        public double? VitaminB6 { get; set; }
        public double? VitaminC { get; set; }
        public double? VitaminD { get; set; }
        public double? VitaminK { get; set; }
        #endregion
    }

    public class Macros
    {
        public Macros(Meal meal)
        {
            this.Meal = meal;
        }

        [ScriptIgnore]
        public Meal Meal { get; set; }

        public int Carbs
        {
            get
            {
                double carbs = this.Meal.Carbohydrates;
                double protein = this.Meal.Protein;
                double fat = this.Meal.Fat;

                try 
                {
                    return Convert.ToInt32(carbs / (carbs + protein + fat) * 100);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public int Protein
        {
            get
            {
                double carbs = this.Meal.Carbohydrates;
                double protein = this.Meal.Protein;
                double fat = this.Meal.Fat;

                try
                {
                    return Convert.ToInt32(protein / (carbs + protein + fat) * 100);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public int Fat
        {
            get
            {
                double carbs = this.Meal.Carbohydrates;
                double protein = this.Meal.Protein;
                double fat = this.Meal.Fat;

                try
                {
                    return Convert.ToInt32(fat / (carbs + protein + fat) * 100);
                }
                catch
                {
                    return 0;
                }
            }
        }
    }
}