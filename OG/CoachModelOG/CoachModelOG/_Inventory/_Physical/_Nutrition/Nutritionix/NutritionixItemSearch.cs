using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Inventory._Physical._Nutrition
{
    public class NutritionixAutoComplete
    {
        public NutritionixAutoComplete()
        {

        }
        public List<IFoodItem> FoodItems { get; set; }
    }

    public class NutritionixItemSearch
    {
        public NutritionixItemSearch()
        {
            this.common = new List<CommonItem>();
            this.branded = new List<BrandedItem>();
        }
        public List<CommonItem> common { get; set; }
        public List<BrandedItem> branded { get; set; }

        public List<IFoodItem> FoodItems
        {
            get
            {
                return this.common.Cast<IFoodItem>().Concat(this.branded).ToList();
            }
        }
    }

    public interface IFoodItem
    {
        string itemType { get; }
        string brand_name { get; set; }
        string food_name { get; set; }
        double? serving_qty { get; set; }
        string serving_unit { get; set; }
        double? nf_calories { get; set; }
        string photoThumb { get; }
    }

    public class CommonItem : IFoodItem
    {
        public string itemType { get { return "Common Foods"; } }
        public string brand_name { get; set; }
        public string food_name { get; set; }
        public double? serving_qty { get; set; }
        public string serving_unit { get; set; }
        public double? nf_calories { get; set; } // This isn't actually a property returned from the API. It's just needed for the AutoComplete template
        public string photoThumb {
            get
            {
                if (this.photo.thumb != null)
                    return this.photo.thumb;
                else return "https://d2eawub7utcl6.cloudfront.net/images/nix-apple-grey.png";
            }
        }
        public Photo photo { get; set; }
        public string tag_id { get; set; }
        public string locale { get; set; }
    }

    public class BrandedItem: IFoodItem
    {
        public string itemType { get { return "Branded Foods"; } }
        public string brand_name { get; set; }
        public string food_name { get; set; }
        public double? serving_qty { get; set; }
        public string serving_unit { get; set; }
        public double? nf_calories { get; set; }
        public string photoThumb
        {
            get
            {
                if (this.photo.thumb != null)
                    return this.photo.thumb;
                else return "https://d2eawub7utcl6.cloudfront.net/images/nix-apple-grey.png";
            }
        }
        public string nix_brand_id { get; set; }
        public string brand_name_item_name { get; set; }
        public Photo photo { get; set; }
        public int? region { get; set; }
        public int? brand_type { get; set; }
        public string nix_item_id { get; set; }
        public string locale { get; set; }
    }

    public class Photo
    {
        public string thumb { get; set; }
    }
}
