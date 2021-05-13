using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Coach.ServiceOG._Inventory;
using Coach.Site.Areas.OG.Services;
using CoachModel._App._Helper;
using CoachModel._Inventory._Physical._Nutrition;
using Microsoft.VisualBasic.FileIO;

namespace Coach.Site.Areas.OG.Controllers
{
    public class PhysicalController : Controller
    {
        // GET: Physical
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetMeal(int taskID)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Meal = PhysicalService.GetMeal(taskID);

                return new CustomJsonResult()
                {
                    Data = new { Meal, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetMealFromID(int mealID)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Meal = PhysicalService.GetMealFromID(mealID);

                return new CustomJsonResult()
                {
                    Data = new { Meal, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetMeals(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                {
                    StartDT = DateTime.Parse(startDTString);
                }
                if (endDTString != null)
                {
                    EndDT = DateTime.Parse(endDTString);
                }

                var PhysicalService = new PhysicalService();
                var Meals = PhysicalService.GetMeals(StartDT, EndDT);

                return new CustomJsonResult()
                {
                    Data = new { Meals, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetRecentFoodItems(string startDTString = null, string endDTString = null)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                {
                    StartDT = DateTime.Parse(startDTString);
                }
                if (endDTString != null)
                {
                    EndDT = DateTime.Parse(endDTString);
                }

                var PhysicalService = new PhysicalService();
                var MealItems = PhysicalService.GetMealItems(StartDT, EndDT);
                var Model = new { MealItems };

                return new CustomJsonResult()
                {
                    Data = new { Model, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult GetMealCharts(string startDTString, string endDTString)
        {
            try
            {
                DateTime? StartDT = null;
                DateTime? EndDT = null;

                if (startDTString != null)
                {
                    StartDT = DateTime.Parse(startDTString);
                }
                if (endDTString != null)
                {
                    EndDT = DateTime.Parse(endDTString);
                }

                var PhysicalService = new PhysicalService();
                var MealChart = PhysicalService.GetMealCharts(StartDT, EndDT);

                return new CustomJsonResult()
                {
                    Data = new { MealChart, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        [HttpPost]
        public JsonResult SaveMeal(Meal meal)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var mealID = PhysicalService.SaveMeal(meal);

                return new CustomJsonResult()
                {
                    Data = new { success = true, mealID },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult ToggleMealConsumed(int mealID, bool wasConsumed)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                PhysicalService.ToggleMealConsumed(mealID, wasConsumed);

                return new CustomJsonResult()
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult ToggleFoodItemConsumed(int mealID, int foodItemID, bool wasConsumed)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                PhysicalService.ToggleFoodItemConsumed(mealID, foodItemID, wasConsumed);

                return new CustomJsonResult()
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult LogWater(int amount)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                PhysicalService.LogWater(amount);

                return new CustomJsonResult()
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public JsonResult LogMealWater(int mealID, int amount)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                PhysicalService.LogWater(mealID, amount);

                return new CustomJsonResult()
                {
                    Data = new { success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        [HttpPost]
        public ActionResult SaveFoodLog(IEnumerable<HttpPostedFileBase> foodLogs, string mealType)
        {
            try
            {
                var origininalFormat = new List<string> { "Date", "Meal", "Time", "Calories", "Fat (g)", "Saturated Fat", "Polyunsaturated Fat",
                                                     "Monounsaturated Fat", "Trans Fat", "Cholesterol", "Sodium (mg)", "Potassium",
                                                      "Carbohydrates (g)", "Fiber", "Sugar", "Protein (g)", "Vitamin A", "Vitamin C", "Calcium", "Iron" };

                // The Name of the Upload component is "files"
                if (foodLogs != null)
                {
                    var Meals = new List<Meal>();

                    foreach (var file in foodLogs)
                    {
                        using (TextFieldParser csvParser = new TextFieldParser(file.InputStream))
                        {
                            csvParser.SetDelimiters(new string[] { "," });

                            var i = 0;
                            while (!csvParser.EndOfData)
                            {
                                /* Read current line fields, pointer moves to the next line. */
                                string[] fields = csvParser.ReadFields();

                                if (i++ == 0)
                                {
                                    if (!fields.SequenceEqual(origininalFormat))
                                    {
                                        return Json(new { success = false, error = "Could not save log. CSV file is not in the correct format" },
                                            "application/json", JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    /* Create Meal */
                                    var Meal = new Meal();
                                    Meal.Name = fields[1];
                                    Meal.DateTime = DateTime.Parse(fields[0] + " " + fields[2]);
                                    Meal.idType = Types.GetID(mealType);

                                    /* Create MealItem for the Meal that represents the whole meal */
                                    var MealItem = new MealItem();
                                    MealItem.FoodName = "Whole Meal";
                                    MealItem.Calaries = Double.Parse(fields[3]);
                                    MealItem.Fat = Double.Parse(fields[4]);
                                    MealItem.SaturatedFat = Double.Parse(fields[5]);
                                    MealItem.PolyunsaturatedFat = Double.Parse(fields[6]);
                                    MealItem.MonounsaturatedFat = Double.Parse(fields[7]);
                                    MealItem.TransFat = Double.Parse(fields[8]);
                                    MealItem.Cholesterol = Double.Parse(fields[9]);
                                    MealItem.Sodium = Double.Parse(fields[10]);
                                    MealItem.Potassium = Double.Parse(fields[11]);
                                    MealItem.Carbohydrates = Double.Parse(fields[12]);
                                    MealItem.Fiber = Double.Parse(fields[13]);
                                    MealItem.Sugars = Double.Parse(fields[14]);
                                    MealItem.Protein = Double.Parse(fields[15]);
                                    MealItem.VitaminA = Double.Parse(fields[16]);
                                    MealItem.VitaminC = Double.Parse(fields[17]);
                                    MealItem.Calcium = Double.Parse(fields[18]);
                                    MealItem.Iron = Double.Parse(fields[19]);

                                    Meals.Add(Meal); // Add Meal to collection
                                    Meal.MealItems.Add(MealItem); // Add meal item to meal
                                }
                            }
                        }
                    }

                    var PhysicalService = new PhysicalService();
                    PhysicalService.SaveMeals(Meals);
                }

                return Json(new { success = true }, "application/json", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public async Task<ActionResult> NutritionixAutoComplete(string query)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Model = await PhysicalService.NutritionixAutoComplete(query);

                /* Only show the first 5 items of each category */
                var displayCount = 5;
                if (Model.common.Count() > displayCount)
                    Model.common.RemoveRange(displayCount - 1, Model.common.Count() - displayCount);
                if (Model.branded.Count() > displayCount)
                    Model.branded.RemoveRange(displayCount - 1, Model.branded.Count() - displayCount);

                return new CustomJsonResult()
                {
                    Data = new { Model.FoodItems, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public async Task<ActionResult> NutritionixCommonItemSearch(string query)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Model = await PhysicalService.NutritionixMultiItemSearch(query);

                return new CustomJsonResult()
                {
                    Data = new { Model, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public async Task<ActionResult> NutritionixBrandedItemSearch(string query, int brandedItemCount = 3)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Model = await PhysicalService.NutritionixBrandedtemSearch(query, brandedItemCount);

                return new CustomJsonResult()
                {
                    Data = new { Model, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public async Task<ActionResult> NutritionixUPCLookup(string query)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Model = await PhysicalService.NutritionixUPCLookup(query);

                return new CustomJsonResult()
                {
                    Data = new { Model, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }

        public async Task<ActionResult> NutritionixMultiItemSearch(string query)
        {
            try
            {
                var PhysicalService = new PhysicalService();
                var Model = await PhysicalService.NutritionixMultiItemSearch(query);

                return new CustomJsonResult()
                {
                    Data = new { Model, success = true },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    ContentType = "application/json"
                };
            }
            catch (Exception ex)
            {
                return CatchControllerError(ex);
            }
        }



        public JsonResult CatchControllerError(Exception ex)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/ErrorLog.txt";
            var errorString = System.IO.File.ReadAllText(path);

            errorString += "\r\n";
            errorString += ex.Message;
            if (ex.InnerException != null)
                errorString += ex.InnerException.Message;

            System.IO.File.WriteAllText(path, errorString);

            return Json(new { success = false, error = ex.Message }, "application/json", JsonRequestBehavior.AllowGet);
        }


    }
}