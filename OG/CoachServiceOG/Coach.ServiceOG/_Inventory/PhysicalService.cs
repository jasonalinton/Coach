using AutoMapper;
using Coach.ServiceOG._Data._CoachModel;
using CoachModel._App._Helper;
using CoachModel._Inventory;
using CoachModel._Inventory._Physical._Nutrition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EntityState = System.Data.Entity.EntityState;

namespace Coach.ServiceOG._Inventory
{
    public class PhysicalService
    {
        coachogEntities entities;
        HttpClient nutritionixClient;
        string nutritionixServiceURL = "https://trackapi.nutritionix.com";
        IMapper CoachMapper;

        public PhysicalService()
        {
            this.entities = new coachogEntities();

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<char, int>().ConvertUsing(c => Convert.ToInt32(c));
                cfg.CreateMap<string, int>().ConvertUsing(s => Convert.ToInt32(s));
                cfg.CreateMap<string, double>().ConvertUsing(s => Convert.ToDouble(s));
                #region Inventory

                cfg.CreateMap<inventoryitem_view, InventoryItem>()
                    .ForMember(g => g.GoalIDs_Concat, m => m.MapFrom(g_v => g_v.goalIDs))
                    .ForMember(g => g.TodoItemIDs_Concat, m => m.MapFrom(g_v => g_v.todoItemIDs))
                    .ForMember(g => g.RoutineIDs_Concat, m => m.MapFrom(g_v => g_v.routineIDs))
                    .ForMember(g => g.TaskIDs_Concat, m => m.MapFrom(g_v => g_v.taskIDs))
                    .ForMember(g => g.EventIDs_Concat, m => m.MapFrom(g_v => g_v.eventIDs))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<inventoryitemspotlight, SpotlightMapping>();

                #endregion


                cfg.CreateMap<type, CoachModel._App._Universal.Type>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<medium, CoachModel._App._Universal.Medium>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<deadline, CoachModel._App._Universal.Deadline>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();


                #region Food Tracker 

                cfg.CreateMap<task_mealfooditemnutrient_view, MealItem>()
                    .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                /* Meal Chart */
                cfg.CreateMap<task_mealfooditemnutrient_view, FoodItem>()
                    .ForMember(g => g.Name, m => m.MapFrom(g_v => g_v.foodName))
                    .ForMember(g => g.Carbs, m => m.MapFrom(g_v => g_v.carbohydrates))
                    .ForMember(g => g.WasConsumed, m => m.MapFrom(g_v => g_v.wasFoodItemConsumed))
                    .ForMember(g => g.DateTime, m => m.MapFrom(g_v => g_v.startDateTime))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<fooditem, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.name))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<MealItem, fooditem>()
                    .ForMember(g => g.name, m => m.MapFrom(g_v => g_v.FoodName))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<waterlog_mealtask_view, WaterLog>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();
                cfg.CreateMap<waterlog, WaterLog>()
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<Food, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.food_name))
                    .ForMember(g => g.Quantity, m => m.MapFrom(g_v => g_v.serving_qty))
                    .ForMember(g => g.Unit, m => m.MapFrom(g_v => g_v.serving_unit))
                    .ForMember(g => g.Calaries, m => m.MapFrom(g_v => g_v.nf_calories))
                    .ForMember(g => g.Carbohydrates, m => m.MapFrom(g_v => g_v.nf_total_carbohydrate))
                    .ForMember(g => g.Protein, m => m.MapFrom(g_v => g_v.nf_protein))
                    .ForMember(g => g.Fat, m => m.MapFrom(g_v => g_v.nf_total_fat))
                    .ForMember(g => g.Cholesterol, m => m.MapFrom(g_v => g_v.nf_cholesterol))
                    .ForMember(g => g.SaturatedFat, m => m.MapFrom(g_v => g_v.nf_saturated_fat))
                    .ForMember(g => g.Fiber, m => m.MapFrom(g_v => g_v.nf_dietary_fiber))
                    .ForMember(g => g.Potassium, m => m.MapFrom(g_v => g_v.nf_potassium))
                    .ForMember(g => g.Sodium, m => m.MapFrom(g_v => g_v.nf_sodium))
                    .ForMember(g => g.Sugars, m => m.MapFrom(g_v => g_v.nf_sugars))
                    .ForMember(g => g.Phosphorus, m => m.MapFrom(g_v => g_v.nf_p))
                    .ForMember(g => g.ThumbURL, m => m.MapFrom(g_v => g_v.photo.thumb))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();

                cfg.CreateMap<FoodBranded, MealItem>()
                    .ForMember(g => g.FoodName, m => m.MapFrom(g_v => g_v.food_name))
                    .ForMember(g => g.Quantity, m => m.MapFrom(g_v => g_v.serving_qty))
                    .ForMember(g => g.Unit, m => m.MapFrom(g_v => g_v.serving_unit))
                    .ForMember(g => g.Calaries, m => m.MapFrom(g_v => g_v.nf_calories))
                    .ForMember(g => g.Carbohydrates, m => m.MapFrom(g_v => g_v.nf_total_carbohydrate))
                    .ForMember(g => g.Protein, m => m.MapFrom(g_v => g_v.nf_protein))
                    .ForMember(g => g.Fat, m => m.MapFrom(g_v => g_v.nf_total_fat))
                    .ForMember(g => g.Cholesterol, m => m.MapFrom(g_v => g_v.nf_cholesterol))
                    .ForMember(g => g.SaturatedFat, m => m.MapFrom(g_v => g_v.nf_saturated_fat))
                    .ForMember(g => g.Fiber, m => m.MapFrom(g_v => g_v.nf_dietary_fiber))
                    .ForMember(g => g.Potassium, m => m.MapFrom(g_v => g_v.nf_potassium))
                    .ForMember(g => g.Sodium, m => m.MapFrom(g_v => g_v.nf_sodium))
                    .ForMember(g => g.Sugars, m => m.MapFrom(g_v => g_v.nf_sugars))
                    .ForMember(g => g.Phosphorus, m => m.MapFrom(g_v => g_v.nf_p))
                    .ForMember(g => g.ThumbURL, m => m.MapFrom(g_v => g_v.photo.thumb))
                    .IgnoreAllPropertiesWithAnInaccessibleSetter();


                #endregion

            });

            CoachMapper = config.CreateMapper();
            this.InitializeNutritionixClient();
        }

        public void InitializeNutritionixClient()
        {
            nutritionixClient = new HttpClient();
            nutritionixClient.DefaultRequestHeaders.Add("x-app-id", "9e3290a0");
            nutritionixClient.DefaultRequestHeaders.Add("x-app-key", "3d97e583f2f69dd815913b6224807b1f");
            nutritionixClient.DefaultRequestHeaders.Add("x-remote-user-id", "0");
        }

        public async Task<NutritionixItemSearch> NutritionixAutoComplete(string query)
        {
            Debug.WriteLine(query);

            NutritionixItemSearch results = new NutritionixItemSearch();
            string restUrl = this.nutritionixServiceURL + "/v2/search/instant";
            var uri = new Uri(restUrl);

            try
            {
                var content = new StringContent("{\"query\": \"" + query + "\"}", UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await nutritionixClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string json_Out = await response.Content.ReadAsStringAsync();
                    results = JsonConvert.DeserializeObject<NutritionixItemSearch>(json_Out);

                    Debug.WriteLine(@"  ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return results;
        }

        public async Task<Meal> NutritionixCommonItemSearch(string query)
        {
            return await this.NutritionixMultiItemSearch(query);
        }

        public async Task<Meal> NutritionixBrandedtemSearch(string query, int brandedItemCount = 3)
        {
            NutritionixItemSearch idResults = new NutritionixItemSearch();
            string restUrl = this.nutritionixServiceURL + "/v2/search/instant";
            var uri = new Uri(restUrl);

            try
            {
                var content = new StringContent("{\"query\": \"" + query + "\"}", UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await nutritionixClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string json_Out = await response.Content.ReadAsStringAsync();
                    idResults = JsonConvert.DeserializeObject<NutritionixItemSearch>(json_Out);

                    Debug.WriteLine(@"  ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            NutritionixBrandedItems itemResults = new NutritionixBrandedItems();

            var i = 0;
            foreach (var item in idResults.branded)
            {
                if (i++ >= brandedItemCount)
                    continue;

                NutritionixBrandedItems resultsTemp = null;
                restUrl = this.nutritionixServiceURL + "/v2/search/item?nix_item_id=" + item.nix_item_id;
                uri = new Uri(restUrl);

                try
                {
                    HttpResponseMessage response = await nutritionixClient.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        string json_Out = await response.Content.ReadAsStringAsync();
                        resultsTemp = JsonConvert.DeserializeObject<NutritionixBrandedItems>(json_Out);
                        itemResults.foods = itemResults.foods.Union(resultsTemp.foods).ToList();

                        Debug.WriteLine(@"  ");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"				ERROR {0}", ex.Message);
                }
            }

            var foodItems = new List<MealItem>();
            CoachMapper.Map(itemResults.foods, foodItems);
            var meal = new Meal(foodItems);

            return meal;
        }

        public async Task<Meal> NutritionixMultiItemSearch(string query)
        {
            Meal Meal = null;
            string restUrl = this.nutritionixServiceURL + "/v2/natural/nutrients";
            var uri = new Uri(restUrl);

            try
            {
                var content = new StringContent("{\"query\": \"" + query + "\"}", UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage response = await nutritionixClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    string json_Out = await response.Content.ReadAsStringAsync();
                    var results = JsonConvert.DeserializeObject<NutritionixCommonItems>(json_Out);

                    var foodItems = new List<MealItem>();
                    CoachMapper.Map(results.foods, foodItems);
                    Meal = new Meal(foodItems);

                    Debug.WriteLine(@"  ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            return Meal;
        }

        public async Task<Meal> NutritionixUPCLookup(string upc)
        {
            NutritionixBrandedItems itemResults = new NutritionixBrandedItems();
            var restUrl = this.nutritionixServiceURL + "/v2/search/item?upc=" + upc;
            var uri = new Uri(restUrl);

            try
            {
                HttpResponseMessage response = await nutritionixClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string json_Out = await response.Content.ReadAsStringAsync();
                    itemResults = JsonConvert.DeserializeObject<NutritionixBrandedItems>(json_Out);
                    //itemResults.foods = itemResults.foods.Union(resultsTemp.foods).ToList();

                    Debug.WriteLine(@"  ");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"				ERROR {0}", ex.Message);
            }

            var foodItems = new List<MealItem>();
            CoachMapper.Map(itemResults.foods, foodItems);
            var meal = new Meal(foodItems);

            return meal;
        }

        //public async Task<MealItem> GetNutritionixCommonItem(string foodName)
        //{

        //}

        //public async Task<NutritionixBrandedItemSearch> NutritionixBrandedItemSearch2(string query)
        //{
        //    NutritionixBrandedItemSearch results = null;
        //    string restUrl = this.nutritionixServiceURL + "/v2/search/item?nix_item_id=" + query;
        //    var uri = new Uri(restUrl);

        //    try
        //    {
        //        HttpResponseMessage response = await nutritionixClient.GetAsync(uri);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            string json_Out = await response.Content.ReadAsStringAsync();
        //            results = JsonConvert.DeserializeObject<NutritionixBrandedItemSearch>(json_Out);

        //            Debug.WriteLine(@"  ");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(@"				ERROR {0}", ex.Message);
        //    }

        //    return results;
        //}

        /*----------------------------------------------------------------------------------------------------*/
        /*------------------------------------------------- Meal ---------------------------------------------*/
        /*----------------------------------------------------------------------------------------------------*/

        public Meal GetMeal(int taskID)
        {
            var task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.idTask == taskID).ToList();
            var Meals = this.CreateMealModels(task_meal_fooditem_views);

            return Meals[0];
        }

        public Meal GetMealFromID(int mealID)
        {
            var task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.idMeal == mealID).ToList();
            var Meals = this.CreateMealModels(task_meal_fooditem_views);

            return Meals[0];
        }

        public List<Meal> GetMeals(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            List<task_mealfooditemnutrient_view> task_meal_fooditem_views;

            /* If there are start and end date-times, get views between start and end date-times */
            if (StartDT != null && EndDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT && x.endDateTime <= EndDT).ToList();
            }
            /* If there is a start date-time, get views after start date-time */
            else if (StartDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT).ToList();
            }
            /* If there is an end date-time, get views before end date-time */
            else if (EndDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.endDateTime <= EndDT).ToList();
            }
            /* If there isn't a start or end date-time, get all event views */
            else
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.ToList();
            }

            var Meals = this.CreateMealModels(task_meal_fooditem_views);
            return Meals;
        }

        public List<Meal> CreateMealModels(List<task_mealfooditemnutrient_view> task_meal_fooditem_views)
        {
            var Meals = new List<Meal>();

            foreach (var taskMealFoodItem in task_meal_fooditem_views)
            {
                Meal Meal;
                if (Meals.Select(x => x.idMeal).Contains((int)taskMealFoodItem.idMeal)) // If meal was already created 
                {
                    Meal = Meals.SingleOrDefault(x => x.idMeal == taskMealFoodItem.idMeal); // Get meal
                }
                else
                {
                    Meal = new Meal((int)taskMealFoodItem.idMeal, taskMealFoodItem.mealName, taskMealFoodItem.mealType, (bool)taskMealFoodItem.wasMealConsumed); // Create meal
                    Meals.Add(Meal); // Add new meal to collection
                }

                if (!Meal.TaskIDs.Contains(taskMealFoodItem.idTask))
                {
                    Meal.TaskIDs.Add(taskMealFoodItem.idTask);
                }

                /* Creat new MealItem */
                var MealItem = new MealItem();
                Meal.MealItems.Add(MealItem);
                CoachMapper.Map(taskMealFoodItem, MealItem);
            }

            return Meals;
        }

        public List<MealItem> GetMealItems(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            List<task_mealfooditemnutrient_view> task_mealfooditemnutrient_views;

            /* If there are start and end date-times, get views between start and end date-times */
            if (StartDT != null && EndDT != null)
            {
                task_mealfooditemnutrient_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT && x.endDateTime <= EndDT).OrderByDescending(x => x.startDateTime).ToList();
            }
            /* If there is a start date-time, get views after start date-time */
            else if (StartDT != null)
            {
                task_mealfooditemnutrient_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT).OrderByDescending(x => x.startDateTime).ToList();
            }
            /* If there is an end date-time, get views before end date-time */
            else if (EndDT != null)
            {
                task_mealfooditemnutrient_views = entities.task_mealfooditemnutrient_view.Where(x => x.endDateTime <= EndDT).OrderByDescending(x => x.startDateTime).ToList();
            }
            /* If there isn't a start or end date-time, get all event views */
            else
            {
                task_mealfooditemnutrient_views = entities.task_mealfooditemnutrient_view.OrderByDescending(x => x.startDateTime).ToList();
            }

            var MealItems = this.CreateMealItemModels(task_mealfooditemnutrient_views);
            return MealItems;
        }

        public List<MealItem> CreateMealItemModels(List<task_mealfooditemnutrient_view> task_mealfooditemnutrient_views)
        {
            var MealItems = new List<MealItem>();

            foreach (var taskMealFoodItem in task_mealfooditemnutrient_views)
            {
                /* Creat new MealItem */
                var MealItem = new MealItem();
                MealItems.Add(MealItem); // Add meal new meal item to meal items
                CoachMapper.Map(taskMealFoodItem, MealItem); // Map view record to meal item
            }

            return MealItems;
        }

        public MealChart GetMealCharts(DateTime? StartDT = null, DateTime? EndDT = null)
        {
            List<task_mealfooditemnutrient_view> task_meal_fooditem_views;
            List<waterlog> waterlogs;

            /* If there are start and end date-times, get views between start and end date-times */
            if (StartDT != null && EndDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT && x.endDateTime <= EndDT).ToList();
                waterlogs = entities.waterlogs.Where(x => x.dateTime >= StartDT && x.dateTime <= EndDT).ToList();
            }
            /* If there is a start date-time, get views after start date-time */
            else if (StartDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.startDateTime >= StartDT).ToList();
                waterlogs = entities.waterlogs.Where(x => x.dateTime >= StartDT).ToList();
            }
            /* If there is an end date-time, get views before end date-time */
            else if (EndDT != null)
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.Where(x => x.endDateTime <= EndDT).ToList();
                waterlogs = entities.waterlogs.Where(x => x.dateTime <= EndDT).ToList();
            }
            /* If there isn't a start or end date-time, get all event views */
            else
            {
                task_meal_fooditem_views = entities.task_mealfooditemnutrient_view.ToList();
                waterlogs = entities.waterlogs.ToList();
            }

            task_meal_fooditem_views = task_meal_fooditem_views.OrderBy(x => x.startDateTime).ToList();
            waterlogs = waterlogs.OrderBy(x => x.dateTime).ToList();

            var MealChartMeals = this.CreateMealChartModels(task_meal_fooditem_views);

            var MealChart = new MealChart(MealChartMeals);
            CoachMapper.Map(waterlogs, MealChart.WaterLogs);

            return MealChart;
        }

        public List<MealChartMeal> CreateMealChartModels(List<task_mealfooditemnutrient_view> task_meal_fooditem_views)
        {
            var Meals = new List<MealChartMeal>();

            foreach (var taskMealFoodItem in task_meal_fooditem_views)
            {
                MealChartMeal Meal;
                if (Meals.Select(x => x.idMeal).Contains((int)taskMealFoodItem.idMeal)) // If meal was already created 
                {
                    Meal = Meals.SingleOrDefault(x => x.idMeal == taskMealFoodItem.idMeal); // Get meal
                }
                else
                {
                    Meal = new MealChartMeal((int)taskMealFoodItem.idMeal, taskMealFoodItem.mealName, (DateTime)taskMealFoodItem.startDateTime); // Create meal
                    Meals.Add(Meal); // Add new meal to collection

                    var waterlogs = entities.waterlog_mealtask_view.Where(x => x.idMeal == taskMealFoodItem.idMeal).ToList();
                    CoachMapper.Map(waterlogs, Meal.WaterLogs);
                }

                /* Creat new MealItem */
                var FoodItem = new FoodItem();
                Meal.FoodItems.Add(FoodItem);
                CoachMapper.Map(taskMealFoodItem, FoodItem);
            }

            return Meals;
        }

        public int SaveMeal(Meal meal)
        {
            return this.SaveMeals(new List<Meal>() { meal })[0];
        }

        public List<int> SaveMeals(List<Meal> Meals)
        {
            var meals_New = new List<meal>();

            foreach (var meal in Meals)
            {
                meal mealModel = null;

                /* If meal already exists, delete food items, nutrients, and mappings (if food item is only mapped to this meal. New food items will be mapped in later step  */
                if (meal.idMeal != 0)
                {
                    mealModel = entities.meals.SingleOrDefault(x => x.idMeal == meal.idMeal);
                    foreach (var meal_fooditem in mealModel.meal_fooditem.ToList())
                    {
                        /* Delete food item if it is only mapped to this meal */
                        if (entities.meal_fooditem.Where(x => x.idFoodItem == meal_fooditem.idFoodItem).Count() == 1)
                        {
                            /* Un-map food item from nutrient */
                            foreach (var fooditem_nutrient in meal_fooditem.fooditem.fooditem_nutrient.ToList())
                            {
                                entities.Entry(fooditem_nutrient).State = EntityState.Deleted;
                                entities.fooditem_nutrient.Remove(fooditem_nutrient);
                                //entities.nutrients.Remove(fooditem_nutrient.nutrient);
                            }
                            var fooditem = meal_fooditem.fooditem;
                            entities.Entry(fooditem).State = EntityState.Deleted;
                            entities.fooditems.Remove(fooditem); // Delete food item
                        }
                        entities.Entry(meal_fooditem).State = EntityState.Deleted;
                        entities.meal_fooditem.Remove(meal_fooditem); // Un-map food item from meal
                    }
                    meals_New.Add(mealModel);
                }
                else
                {
                    /* Create and add meal */
                    mealModel = new meal
                    {
                        name = meal.Name,
                        wasConsumed = meal.WasConsumed,
                    };
                    entities.meals.Add(mealModel);
                    entities.Entry(mealModel).State = EntityState.Added;
                    meals_New.Add(mealModel);

                    /* Map meal to task */
                    List<task> tasks = null;
                    if (meal.TaskIDs != null && meal.TaskIDs.Count() > 0)
                    {
                        tasks = entities.tasks.Where(x => meal.TaskIDs.Contains(x.idTask)).ToList();
                    }
                    else
                    {
                        var StartOfDay = meal.DateTime.Date;
                        var EndOfDay = meal.DateTime.Date.AddDays(1).AddTicks(-1);
                        tasks = entities.tasks.Where(x => x.startDateTime >= StartOfDay && x.startDateTime <= EndOfDay && x.title == meal.Name).ToList();
                    }

                    foreach (var task in tasks)
                    {
                        var task_meal = new task_meal
                        {
                            meal = mealModel,
                            task = task,
                        };
                        entities.task_meal.Add(task_meal);
                        entities.Entry(task_meal).State = EntityState.Added;
                    }
                }
 
                foreach (var mealItem in meal.MealItems)
                {
                    /* Create and add food item */
                    var fooditem = new fooditem();
                    CoachMapper.Map(mealItem, fooditem);
                    entities.fooditems.Add(fooditem);
                    entities.Entry(fooditem).State = EntityState.Added;

                    /* Map food item to meal */
                    var meal_fooditem = new meal_fooditem
                    {
                        quantity = mealItem.Quantity,
                        unit = mealItem.Unit,
                        wasConsumed = mealItem.WasConsumed,
                        meal = mealModel,
                        fooditem = fooditem
                    };
                    entities.meal_fooditem.Add(meal_fooditem);
                    entities.Entry(meal_fooditem).State = EntityState.Added;

                    #region Add fooditem_nutrient
                    if (mealItem.Calcium != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._calcium),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Calcium
                        });
                    }

                    if (mealItem.Carbohydrates != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._carbs),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Carbohydrates
                        });
                    }

                    if (mealItem.Cholesterol != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._cholesterol),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Cholesterol
                        });
                    }

                    if (mealItem.MonounsaturatedFat != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._monounsaturated),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.MonounsaturatedFat
                        });
                    }

                    if (mealItem.PolyunsaturatedFat != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._polyunsaturated),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.PolyunsaturatedFat
                        });
                    }

                    if (mealItem.SaturatedFat != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._saturated),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.SaturatedFat
                        });
                    }

                    if (mealItem.Fat != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._fat),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Fat
                        });
                    }

                    if (mealItem.TransFat != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._trans),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.TransFat
                        });
                    }

                    if (mealItem.Iron != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._iron),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Iron
                        });
                    }

                    if (mealItem.Fiber != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._fiber),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Fiber
                        });
                    }

                    if (mealItem.Folate != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._folateEquivalent),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Folate
                        });
                    }

                    if (mealItem.Potassium != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._potassium),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Potassium
                        });
                    }

                    if (mealItem.Magnesium != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._magnesium),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Magnesium
                        });
                    }

                    if (mealItem.Sodium != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._sodium),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Sodium
                        });
                    }

                    if (mealItem.Calaries != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._energy),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Calaries
                        });
                    }

                    if (mealItem.NiacinB3 != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._niacinB3),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.NiacinB3
                        });
                    }

                    if (mealItem.Phosphorus != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._phosphorus),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Phosphorus
                        });
                    }

                    if (mealItem.Protein != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._protein),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Protein
                        });
                    }

                    if (mealItem.RiboflavinB2 != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._riboflavinB2),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.RiboflavinB2
                        });
                    }

                    if (mealItem.Sugars != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._sugars),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.Sugars
                        });
                    }

                    if (mealItem.ThiaminB1 != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._thiaminB1),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.ThiaminB1
                        });
                    }

                    if (mealItem.VitaminE != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminE),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminE
                        });
                    }

                    if (mealItem.VitaminA != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminA),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminA
                        });
                    }

                    if (mealItem.VitaminB12 != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminB12),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminB12
                        });
                    }

                    if (mealItem.VitaminB6 != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminB6),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminB6
                        });
                    }

                    if (mealItem.VitaminC != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminC),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminC
                        });
                    }

                    if (mealItem.VitaminD != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminD),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminD
                        });
                    }

                    if (mealItem.VitaminK != null)
                    {
                        entities.fooditem_nutrient.Add(new fooditem_nutrient
                        {
                            idNutrient = CoachModel._App._Helper.Nutrients.GetID(CoachModel._App._Helper.Nutrients._vitaminK),
                            fooditem = fooditem,
                            nutrientQuantity = mealItem.VitaminK
                        });
                    }
                    #endregion
                }

                var meal_fooditems = mealModel.meal_fooditem.ToList();
                /* If all of the food items were consumed, set meal as consumed */
                if (!meal_fooditems.Any(x => x.wasConsumed == false))
                    mealModel.wasConsumed = true;
                /* If any of the food items were not consumed, set meal as not consumed */
                if (meal_fooditems.Any(x => x.wasConsumed == false))
                    mealModel.wasConsumed = false;

            }
            entities.SaveChanges();

            return meals_New.Select(x => x.idMeal).ToList();
        }

        public void ToggleMealConsumed(int mealID, bool wasConsumed)
        {
            entities.meal_fooditem.Where(x => x.idMeal == mealID).ToList().ForEach(x => x.wasConsumed = wasConsumed); // Set all food items' wasConsumed to match the meal's wasConsumed
            entities.meals.SingleOrDefault(x => x.idMeal == mealID).wasConsumed = wasConsumed; // Set meal wasConsumed
            entities.SaveChanges();
        }

        public void ToggleFoodItemConsumed(int mealID, int foodItemID, bool wasConsumed)
        {
            var meal_fooditems = entities.meal_fooditem.Where(x => x.idMeal == mealID).ToList();

            var meal_fooditem = meal_fooditems.SingleOrDefault(x => x.idFoodItem == foodItemID);
            if (meal_fooditem != null)
                meal_fooditem.wasConsumed = wasConsumed;

            /* If all of the food items were consumed, set meal as consumed */
            if (!meal_fooditems.Any(x => x.wasConsumed == false))
                entities.meals.SingleOrDefault(x => x.idMeal == mealID).wasConsumed = true;
            /* If any of the food items were not consumed, set meal as not consumed */
            if (meal_fooditems.Any(x => x.wasConsumed == false))
                entities.meals.SingleOrDefault(x => x.idMeal == mealID).wasConsumed = false;

            entities.SaveChanges();
        }

        public void LogWater(int amount)
        {
            var waterlog_New = new waterlog
            {
                amount = amount
            };
            entities.waterlogs.Add(waterlog_New);
            entities.SaveChanges();
        }

        public void LogWater(int mealID, int amount)
        {
            // IT IS PROBABLY NOT THE BEST IDEA TO ASSUME THAT THE FIRST RECORD WILL HAVE THE CORRECT DATETIME, 
                // BUT RIGHT NOW THERE SHOULDN'T BE A SITUALTION WHERE MULTIPLE TASKS HAVE WILL BE MAPPED TO THE SOME MEAL 
            var logTime = entities.task_mealfooditemnutrient_view.Where(x => x.idMeal == mealID).First().startDateTime;

            var waterlog_New = new waterlog
            {
                amount = amount,
                dateTime = logTime
            };
            entities.waterlogs.Add(waterlog_New);


            var meal_waterlog = new meal_waterlog
            {
                idMeal = mealID,
                waterlog = waterlog_New
            };
            entities.meal_waterlog.Add(meal_waterlog);

            entities.SaveChanges();
        }
    }
}
