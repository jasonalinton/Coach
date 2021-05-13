using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoachModel._Inventory._Physical._Nutrition
{
    public class NutritionixCommonItemSearch
    {
        public List<Common> common { get; set; }
    }

    public class Common
    {
        public string food_name { get; set; }
        public string serving_unit { get; set; }
        public int? serving_qty { get; set; }
        public PhotoCommon photo { get; set; }
        public string tag_id { get; set; }
        public string locale { get; set; }
    }

    public class PhotoCommon
    {
        public string thumb { get; set; }
    }
}
