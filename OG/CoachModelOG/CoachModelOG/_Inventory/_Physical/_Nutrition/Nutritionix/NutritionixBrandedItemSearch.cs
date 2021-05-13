using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Inventory._Physical._Nutrition
{

    public class NutritionixBrandedItemSearch
    {
        public NutritionixBrandedItemSearch()
        {
            this.foods = new List<FoodBranded>();
        }

        public List<FoodBranded> foods { get; set; }
    }

    public class FoodBranded
    {
        public string food_name { get; set; }
        public string brand_name { get; set; }
        public double? serving_qty { get; set; }
        public string serving_unit { get; set; }
        public object serving_weight_grams { get; set; }
        public double? nf_calories { get; set; }
        public object nf_total_fat { get; set; }
        public object nf_saturated_fat { get; set; }
        public double? nf_cholesterol { get; set; }
        public double? nf_sodium { get; set; }
        public double? nf_total_carbohydrate { get; set; }
        public object nf_dietary_fiber { get; set; }
        public double? nf_sugars { get; set; }
        public double? nf_protein { get; set; }
        public object nf_potassium { get; set; }
        public object nf_p { get; set; }
        public List<FullNutrientBranded> full_nutrients { get; set; }
        public string nix_brand_name { get; set; }
        public string nix_brand_id { get; set; }
        public string nix_item_name { get; set; }
        public string nix_item_id { get; set; }
        public MetadataBranded metadata { get; set; }
        public int? source { get; set; }
        public object ndb_no { get; set; }
        public object tags { get; set; }
        public object alt_measures { get; set; }
        public PhotoBranded photo { get; set; }
        public DateTime updated_at { get; set; }
        public object nf_ingredient_statement { get; set; }
    }

    public class FullNutrientBranded
    {
        public int? attr_id { get; set; }
        public double? value { get; set; }
    }

    public class MetadataBranded
    {
    }

    public class PhotoBranded
    {
        public string thumb { get; set; }
        public object highres { get; set; }
        public bool? is_user_uploaded { get; set; }
    }
}
