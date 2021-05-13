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
    
    public partial class task_mealfooditemnutrient_view
    {
        public int idTask { get; set; }
        public Nullable<int> idMeal { get; set; }
        public int idFoodItem { get; set; }
        public Nullable<int> idType_Meal { get; set; }
        public string waterLogIDs { get; set; }
        public Nullable<System.DateTime> startDateTime { get; set; }
        public Nullable<System.DateTime> endDateTime { get; set; }
        public string taskTitle { get; set; }
        public string mealType { get; set; }
        public Nullable<bool> wasMealConsumed { get; set; }
        public bool wasFoodItemConsumed { get; set; }
        public Nullable<decimal> waterLogAmount { get; set; }
        public string mealName { get; set; }
        public string foodName { get; set; }
        public Nullable<double> quantity { get; set; }
        public string unit { get; set; }
        public Nullable<double> calaries { get; set; }
        public Nullable<double> carbohydrates { get; set; }
        public Nullable<double> protein { get; set; }
        public Nullable<double> fat { get; set; }
        public Nullable<double> calcium { get; set; }
        public Nullable<double> cholesterol { get; set; }
        public Nullable<double> monounsaturated { get; set; }
        public Nullable<double> polyunsaturated { get; set; }
        public Nullable<double> saturated { get; set; }
        public Nullable<double> trans { get; set; }
        public Nullable<double> iron { get; set; }
        public Nullable<double> fiber { get; set; }
        public Nullable<double> folateEquivalent { get; set; }
        public Nullable<double> potassium { get; set; }
        public Nullable<double> Magnesium { get; set; }
        public Nullable<double> Sodium { get; set; }
        public Nullable<double> NiacinB3 { get; set; }
        public Nullable<double> Phosphorus { get; set; }
        public Nullable<double> RiboflavinB2 { get; set; }
        public Nullable<double> Sugars { get; set; }
        public Nullable<double> ThiaminB1 { get; set; }
        public Nullable<double> VitaminE { get; set; }
        public Nullable<double> VitaminA { get; set; }
        public Nullable<double> VitaminB12 { get; set; }
        public Nullable<double> VitaminB6 { get; set; }
        public Nullable<double> VitaminC { get; set; }
        public Nullable<double> VitaminD { get; set; }
        public Nullable<double> VitaminK { get; set; }
        public string thumbURL { get; set; }
    }
}
