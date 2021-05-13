var MealChart;
var RecentFoodItems;

$(document).ready(function () {
    initializeSearchBar();
    getRecentFoodItems();
    getMealCharts();

    $('#mealSearch').on('shown.bs.collapse', function () {
        $(".meal #mealSearch .searchbar input").focus();
    });
    $('#mealSearch').on('hide.bs.collapse', function () {
        deselectResultGrid();
    });

    var tabStrip = $(".search-tab-strip").kendoTabStrip({
        animation: {
            open: {
                effects: "fadeIn"
            }
        },
        select: function (e) {
            mealSearch.selectedTab = $(e.item).data("tab");
            onSingleSearchTabChanged();
        },
    }).data("kendoTabStrip");

    //tabStrip.select(3);

    $('.meal-event .single .searchbar input').on("keyup", function (e) {
        if (e.keyCode == 13) {
            onSearchSingleItems();
        }
    });
    $('.meal .search .multiple textarea').on("keyup", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
            onSearchMultipleItems();
        }
    });
    $('.meal .search .multiple textarea').on("keypress", function (e) {
        if (e.keyCode == 13) {
            e.preventDefault();
        }
    });

    $("input[name='water-amount']").on("check", function (e) {
        $(this).find("label").css("background-color", "lightblue");
        $(".tally-amount label:not(:checked)").css("background-color", "transparent");
    });

    $("input[name='water-amount']:checked").closest("label").css("background-color", "lightblue");
    $(".tally-amount label.water-amount").on("click", function (e) {
        $(this).css("background-color", "lightblue");
        $("input[name='water-amount']:not(:checked)").closest("label").css("background-color", "transparent");;
        //$(".tally-amount label:not(:checked)").css("background-color", "transparent");
    });

    //$(":not(.meal-grid *)").click(function () {
    //    if (mealSearch.resultGrid != null) mealSearch.resultGrid.clearSelection();
    //    if (mealSearch.plannedGrid != null) mealSearch.plannedGrid.clearSelection();
    //});
});

function initializeSearchBar() {
    $(".searchbar input").kendoAutoComplete({
        height: 580,
        placeholder: "Search food item",
        template: kendo.template($("#food-item-search-result-template").html()),
        fixedGroupTemplate: kendo.template("<h4 style='text-align:left;'>#:data#</h4>"),
        highlightFirst: false,
        dataTextField: "food_name",
        dataSource: {
            serverFiltering: true,
            type: "json",
            group: {
                field: "itemType"
            },
            transport: {
                read: {
                    type: "post",
                    dataType: "json",
                    url: "/Physical/NutritionixAutoComplete",
                    data: function () {
                        return { query: $(".searchbar input").val() };
                    }
                }
            },
            schema: {
                type: "json",
                parse: function (response) {
                    if (response.success)
                        return response.FoodItems;
                    else
                        return response;
                }
            },
            errors: function (error) {
                return response.error;
            }
        },
        select: function (e) {
            if (e.dataItem.itemType === "Common Foods") {
                Framework.invokeJsonWebService("/Physical/NutritionixCommonItemSearch", { query: e.dataItem.food_name }, function (result) {
                    if (result.success) {
                        mealSearch.selectedResultItems = result.Model.MealItems;
                        onAddMealResultsClicked();
                    } else {
                        alert(reult.error);
                    }
                });
            } else if (e.dataItem.itemType === "Branded Foods") {
                Framework.invokeJsonWebService("/Physical/NutritionixUPCLookup", { query: e.dataItem.food_name }, function (result) {
                    if (result.success) {
                        mealSearch.selectedResultItems = result.Model.MealItems;
                        onAddMealResultsClicked();
                    } else {
                        alert(reult.error);
                    }
                });
            }
        }
    });
}

function getCommonFoodItem(foodName) {
    Framework.invokeJsonWebService("/Physical/NutritionixCommonItemSearch", { query: foodName }, function (result) {
        if (result.success) {
            return result.Model.FoodItems;
        } else {
            alertModal(result.error);
        }
    });
}

function getRecentFoodItems(callback, daysBack = 30) {
    //var data = {
    //    startDTString: Date.today().add(-1 * daysBack).day().toLocaleString(),
    //    endDTString: new Date().toLocaleString()
    //};
    

    Framework.invokeJsonWebService("/Physical/GetRecentFoodItems", function (result) {
        if (result.success) {
            RecentFoodItems = result.Model.MealItems;

            if (callback) callback();
        } else {
            alertModal(result.error);
        }
    });
}

function getMealCharts(callback) {
    var data = {
        startDTString: Date.today().addDays(-30).toLocaleString(),
        endDTString: Date.today().addDays(1).toLocaleString()
    };

    Framework.invokeJsonWebService("/Physical/GetMealCharts", data, function (result) {
        if (result.success) {
            MealChart = result.MealChart;

            if (callback) callback();
        } else {
            alertModal(result.error);
        }
    });
}

/* ------------------------ Meal Event ------------------------ */

var macros = {
    carbs: {
        attemeted: 0,
        actual: 0
    },
    fat: {
        attemeted: 0,
        actual: 0
    },
    protein: {
        attemeted: 0,
        actual: 0
    },
}

function createChart(eventID, macros) {
    if (!macros) return;

    $(".macro-chart").kendoChart({
        //title: {
        //    position: "top",
        //    text: "Macronutrients"
        //},
        legend: {
            visible: false,
        },
        chartArea: {
            background: "",
            width: 200,
            height: 200
        },
        //legend: {
        //    orientation: "horizontal"
        //},
        seriesDefaults: {
            labels: {
                visible: true,
                background: "transparent",
                template: "#= category #: \n #= value#%"
            }
        },
        series: [{
            type: "pie",
            //size: 100,
            autoFit: true,
            labels: {
                visible: true,
                position: "bottom"
            },
            //padding: 0,
            //startAngle: 150,
            data: [
                {
                    category: "Carbs",
                    value: macros.Carbs,
                    color: "#1CA4FC"
                },
                {
                    category: "Fat",
                    value: macros.Fat,
                    color: "#65D643"
                },
                {
                    category: "Protein",
                    value: macros.Protein,
                    color: "#FC6554"
                }]
        }],
        tooltip: {
            visible: true,
            format: "{0}%"
        },
        seriesHover: function (e) {
            var hello = e;
        }
    });
}

//$(document).ready(createChart);
//$(document).bind("kendo:skinChange", createChart);

function createMealChart(meal) {

    mealChart = $(".meal-chart").kendoChart({
        title: {
            text: "Nutrients Consumed"
        },
        chartArea: {
            background: "",
            height: 200
        },
        legend: {
            position: "bottom"
        },
        series: [{
            name: "Calaries (Meal)",
            type: "line",
            data: MealChart[meal + "Meals"],
            field: "CalariesConsumed",
            categoryAxis: "Date",
            categoryField: "DateTime",
            aggregate: "sum",
            axis: "Calaries"
        },
        {
            name: "Calaries (Day)",
            type: "area",
            data: MealChart.Meals,
            field: "CalariesConsumed",
            categoryAxis: "Date",
            categoryField: "DateTime",
            aggregate: "sum",
            color: "#f1a571",
            axis: "Calaries"
        },
        {
            name: "Waterlog (Meal)",
            type: "line",
            data: MealChart[meal + "WaterLogs"],
            field: "Amount",
            categoryAxis: "Date",
            categoryField: "DateTime",
            aggregate: "sum",
            color: "#007eff",
            axis: "Water"
        },
        {
            name: "Waterlog (Day)",
            type: "area",
            data: MealChart.WaterLogs,
            field: "Amount",
            categoryAxis: "Date",
            categoryField: "DateTime",
            aggregate: "sum",
            color: "#007eff",
            axis: "Water"
        }],
        categoryAxis: [{
            name: "Date",
            type: "date",
            baseUnit: "days",
            baseUnitStep: 1,
            maxDateGroups: 30,
            axisCrossingValue: [-1000, 1000],
            //autoBaseUnitSteps: {
            //    days: [1]
            //},
            labels: {
                dateFormats: {
                    days: "ddd M/d"
                }
            }
        }],
        valueAxes: [{
            name: "Calaries",
            color: "#ff6d09",
            min: 0,
            max: 2500
        },
        {
            name: "Water",
            color: "#007eff",
            min: 0,
            max: 200
        }]
    }).data("kendoChart");
}

function addMealWater() {
    var amount = parseInt($("input[name='water-amount']:checked").val());
    mealSearch.waterAmount += amount;
}

function subtractMealWater() {
    var amount = parseInt($("input[name='water-amount']:checked").val());
    mealSearch.waterAmount -= amount;
}

function logMealWater() {
    if (mealSearch.waterAmount > 0) {

        var controllerMethod = "LogWater";
        var data = { amount: mealSearch.waterAmount };

        var meal = getMeal($(".meal-event").data("eventID"));
        if (meal) {
            controllerMethod = "LogMealWater"
            data.mealID = meal.idMeal; // Map water log to meal if necessary
        }

        Framework.invokeJsonWebService("/Physical/" + controllerMethod, data, function (result) {
            if (result.success) {
                createMealChart(meal);
            } else {
                alertModal(result.error);
            }
        });
    }
}



/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */
/* ----------------------------------------------------------      Meal      ---------------------------------------------------------- */
/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */

var selectedSingleSearchTab;

var mealSearch = {
    selectedTab: "",
    singleQuery: "",
    multipleQuery: "",
    lastSingleQuery: "",
    lastMultipleQuery: "",
    isRecentInit: false,
    isUPCInit: false,
    isBrandedInit: false,
    isCommonInit: false,
    isMealsInit: false,
    plannedGridSelector: ".meal .planned-items .meal-grid",
    plannedGrid: null,
    selectedPlannedItems: [],
    resultGrid: null,
    selectedResultItems: [],
    shouldSavePlannedGrid: true,
    waterAmount: 0
}

function getMeal(eventID) {
    if (!eventID) return;

    var eventObject = new Event(eventID);
    var event = eventObject.getEvent();
    var taskIDs = eventObject.getTaskIDs();

    /* Check if event represents a meal */
    var mealTodoItems = [3, 4, 5, 6, 7]; // Hardcoded ID's for meal todo items
    var todoItemIDs = event.TodoItemIDs.concat(event.TodoItemIDs_Routine); // Get list of all the potential todo item IDs
    /* Check if the TodoItem ID's for the event are from a meal */
    var isMeal = todoItemIDs.some(function (ID) {
        return mealTodoItems.indexOf(ID) > -1;
    });

    var meal = null;

    if (isMeal) {
        var meals = []
        $.each(Meals, function (index, meal) {
            var isMatched = meal.TaskIDs.some(function (taskID) {
                return taskIDs.indexOf(taskID) > -1;
            });
            if (isMatched) meals.push(meal);
        });

        meal = meals[0]; // Select the first meal because there currently isn't a situation where there would be more that one meals for a single event
    }

    return meal;
}

/* Show meal info if event represents a meal */
function showMealEventInfo(eventID) {
    $(".meal-event").data("eventID", eventID);
    var eventObject = new Event(eventID);
    var event = eventObject.getEvent();

    /* Check if event represents a meal */
    var mealTodoItems = [3, 4, 5, 6, 7]; // Hardcoded ID's for meal todo items
    var todoItemIDs = event.TodoItemIDs.concat(event.TodoItemIDs_Routine); // Get list of all the potential todo item IDs
    /* Check if the TodoItem ID's for the event are from a meal */
    var isMeal = todoItemIDs.some(function (ID) {
        return mealTodoItems.indexOf(ID) > -1;
    });

    if (isMeal) {

        var meal = getMeal(eventID);

        if (!meal) {
            $("#mealSearch").collapse("show");
            meal = { MealItems: [] };
            $(".meal-title").empty();
        }

        $(".meal-title").text(meal.Name);

        var data = {
            event: eventObject.getEvent(),
            meal: meal, // Select the first meal because there currently isn't a situation where there would be more that one meals for a single event
        };

        //var mealEvent_Template = kendo.template($("#meal-event-template").html());
        //var mealEvent_HTML = mealEvent_Template(data);
        //$("#event-info-container").append(mealEvent_HTML);

        initPlannedMeal(mealSearch, meal);

        createChart(eventID, data.meal.Macros);
        createMealChart(data.meal.Name);

        $("#add-food-log").kendoUpload({
            async: {
                saveUrl: "/Physical/SaveFoodLog?mealType=Consumed",
                //removeUrl: "remove",
                autoUpload: true
            },
            validation: {
                allowedExtensions: [".csv"]
            },
            success: function (result) {
                if (result.response.sucess) {

                } else {

                }
            },
            showFileList: false,
            dropZone: ".meal-event"
        });
    }
}

function initPlannedMeal(mealSearch, meal) {
    resetMealGrid(".meal-event .planned-items .meal-grid-wrapper", "planned");

    mealSearch.plannedGrid = $(mealSearch.plannedGridSelector).kendoGrid({
        selectable: "multiple",
        //editable: "incell",
        resizable: true,
        noRecords: {
            template: "Meal hasnt been planned yet"
        },
        dataSource: {
            data: meal,
            schema: {
                type: "json",
                data: function (response) {
                    return response.MealItems;
                }
            },
            errors: function (error) {
                return response.error;
            },
            aggregate: [
                { field: "Calaries", aggregate: "sum" },
                { field: "Carbohydrates", aggregate: "sum" },
                { field: "Protein", aggregate: "sum" },
                { field: "Fat", aggregate: "sum" }
            ],
        },
        error: function (e) {
            if (e.errors) {
                var errorwindow = CreateModalWindow("errorwindow", "Error", true, false, false, 300);
                errorwindow.content(e.errors);
                $("#errorwindow").append(kendo.template($("#errorwindowclose").html()));
                errorwindow.setOptions({
                    open: function () {
                        this.wrapper.css({ top: 20 });
                    }
                });
                errorwindow.center().open();
            }
        },
        columns: [
            {
                field: "WasConsumed",
                title: "Consumed",
                width: 40,
                headerAttributes: { style: "text-align: center;" },
                headerTemplate: "<div align=center><input type='checkbox' class='was-meal-consumed checkbox'/></div>",
                template: "<div align=center><input type='checkbox' #= WasConsumed ? checked='checked' : '' # class='was-item-consumed checkbox'/></div>",
                footerTemplate: kendo.template($("#planned-grid-footer-template").html())
            },
            {
                field: "ThumbURL",
                title: "Pic",
                headerAttributes: { style: "text-align: center;" },
                template: function (dataItem) {
                    return "<img class='food-item img-responsive img-rounded' src='" + dataItem.ThumbURL + "'  height=25/>";
                },
                width: 60,
                editable: false
            },
            {
                field: "Quantity",
                title: "Qty",
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                width: 50
            },
            {
                field: "Unit",
                title: "Unit",
                attributes: { style: "text-transform:capitalize;" },
                editable: function () {
                    return false;
                },
                width: 125
            },
            {
                field: "FoodName",
                title: "Food",
                attributes: { style: "text-transform:capitalize;" },
                editable: false,
                width: 125
            },
            {
                field: "Calaries",
                title: "Calaries",
                width: 75,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Calaries) #",
                footerTemplate: "<div class='total wrapper'><div class='calaries aggregate'>#: parseInt(sum) #</div><div class='calaries potential-total'></div></div>",
                editor: false
            },
            {
                field: "Carbohydrates",
                title: "Carbs",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Carbohydrates) #",
                footerTemplate: "<div class='total wrapper'><div class='carbs aggregate'>#: parseInt(sum) #</div><div class='carbs potential-total'></div></div>",
                editable: false
            },
            {
                field: "Protein",
                title: "Prot",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Protein) #",
                footerTemplate: "<div class='total wrapper'><div class='protein aggregate'>#: parseInt(sum) #</div><div class='protein potential-total'></div></div>",
                editable: false
            },
            {
                field: "Fat",
                title: "Fat",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Fat) #",
                footerTemplate: "<div class='total wrapper'><div class='fat aggregate'>#: parseInt(sum) #</div><div class='fat potential-total'></div></div>",
                editable: false
            }
        ],
        //cellClose: function (e) {
        //    CloseCell(e.model);
        //},
        change: onPlannedItemSelected,
        dataBound: function () {
            refreshMealConsumedCheckbox(mealSearch.plannedGridSelector);
        }
    }).data("kendoGrid");

    $(mealSearch.plannedGridSelector + " .k-grid-content").on("change", "input.checkbox", function (e) {

        refreshMealConsumedCheckbox(mealSearch.plannedGridSelector);

        /* Toggle WasConsumed property of dataitem */
        dataItem = mealSearch.plannedGrid.dataItem($(e.target).closest("tr"));
        dataItem.set("WasConsumed", this.checked);

        var meal = getMeal($(".meal-event").data("eventID"));

        /* Toggle meal item was consumed, on the server */
        if (meal && dataItem.idFoodItem)
            ToggleFoodItemConsumed(meal.idMeal, dataItem.idFoodItem, dataItem.WasConsumed);
    });

    $(mealSearch.plannedGridSelector).on("change", "input.was-meal-consumed", function (e) {
        var wasConsumed = this.checked;

        /* If meal was consumed, check consumed checkboxes for all food items and set their data items to consumed */
        $(mealSearch.plannedGridSelector + " .was-item-consumed").attr("checked", wasConsumed); // Get all item consumed checkboxes to match the meal consumed checkbox

        /* Get wasConsumed for the data items for all the food items to match the meal  */
        $(mealSearch.plannedGrid.dataSource.data()).each(function (index, dataItem) {
            dataItem.set("WasConsumed", wasConsumed);

        });

        /* Toggle meal item was consumed, on the server */
        var meal = getMeal($(".meal-event").data("eventID"));
        if (meal) ToggleMealConsumed(meal.idMeal, wasConsumed);
    });
}

function CloseCell(dataItem) {
    var factor = dataItem.Quantity / dataItem.OldQuantity;
    dataItem.Calaries = factor * dataItem.Calaries;
    dataItem.Carbohydrates = factor * dataItem.Carbohydrates;
    dataItem.Protein = factor * dataItem.Protein;
    dataItem.Fat = factor * dataItem.Fat;

    mealSearch.resultGrid.refresh();
}

function ToggleMealConsumed(mealID, wasConsumed) {
    Framework.invokeJsonWebService("/Physical/ToggleMealConsumed", { mealID, wasConsumed }, function (result) {
        if (result.success) {
            getMeals(function () {
                var meal = getMeal($(".meal-event").data("eventID"));
                initPlannedMeal(mealSearch, meal);
            });
        } else {
            alertModal(result.error);
        }
    });
}

function ToggleFoodItemConsumed(mealID, foodItemID, wasConsumed) {
    var data = {
        mealID: mealID,
        foodItemID: foodItemID,
        wasConsumed: wasConsumed
    };
    Framework.invokeJsonWebService("/Physical/ToggleFoodItemConsumed", data, function (result) {
        if (result.success) {
            getMeals(function () {
                var meal = getMeal($(".meal-event").data("eventID"));
                initPlannedMeal(mealSearch, meal);
            });

            getMealCharts(function () {
                mealChart.refresh();
                mealChart.redraw();
            });
        } else {
            alertModal(result.error);
        }
    });
}

function resetPlannedTotals() {
    var totals = getPlannedTotals();
    $(mealSearch.plannedGridSelector + " .calaries.aggregate").empty().append(totals.calaries);
    $(mealSearch.plannedGridSelector + " .carbs.aggregate").empty().append(totals.carbs);
    $(mealSearch.plannedGridSelector + " .protein.aggregate").empty().append(totals.protein);
    $(mealSearch.plannedGridSelector + " .fat.aggregate").empty().append(totals.fat);
}

function resetPotentialTotals() {
    $(mealSearch.plannedGridSelector + " .calaries.potential-total").empty();
    $(mealSearch.plannedGridSelector + " .carbs.potential-total").empty();
    $(mealSearch.plannedGridSelector + " .protein.potential-total").empty();
    $(mealSearch.plannedGridSelector + " .fat.potential-total").empty();
}

function getPlannedTotals() {
    var dataItems = mealSearch.plannedGrid.dataSource.data();
    var totals = {
        calaries: 0,
        carbs: 0,
        protein: 0,
        fat: 0
    };

    $(dataItems).each(function (index, dataItem) {
        totals.calaries += dataItem.Calaries;
        totals.protein += dataItem.Protein;
        totals.carbs += dataItem.Carbohydrates;
        totals.fat += dataItem.Fat;
    });

    return totals;
}

function displayPotentialTotals() {
    var totals = getPlannedTotals();

    $(mealSearch.selectedResultItems).each(function (index, dataItem) {
        totals.calaries += dataItem.Calaries;
        totals.protein += dataItem.Protein;
        totals.carbs += dataItem.Carbohydrates;
        totals.fat += dataItem.Fat;
    });

    $(mealSearch.plannedGridSelector + " .calaries.potential-total").empty().append(parseInt(totals.calaries));
    $(mealSearch.plannedGridSelector + " .carbs.potential-total").empty().append(parseInt(totals.carbs));
    $(mealSearch.plannedGridSelector + " .protein.potential-total").empty().append(parseInt(totals.protein));
    $(mealSearch.plannedGridSelector + " .fat.potential-total").empty().append(parseInt(totals.fat));
}

function displaySingleSearch() {
    deselectResultGrid();
    hideMultipleSearch();

    $(".meal .search .single").show();
    $("#mealSearch").collapse("show"); // Show search display
}

function displayMultipleSearch() {
    deselectResultGrid();
    hideSingleSearch();

    $(".meal .search .multiple").show();
    $("#mealSearch").collapse("show"); // Show search display
}

function hideSingleSearch() {
    //$("#mealSearch").collapse("hide"); // Collapse search display
    $(".meal .search .single .searchbar input").val("");

    $(".meal .search .single").hide();
    $(".meal .search .single .tab-strip-wrapper").hide();
}

function hideMultipleSearch() {
    //$("#mealSearch").collapse("hide"); // Collapse search display
    $(".meal .search .multiple .searchbar textarea").val("");

    $(".meal .search .multiple").hide();
    $(".meal .search .multiple .meal-grid").hide();
}

function searchSingleItems(query) {
    if (query) { // Display tab strip if query is not empty
        initSingleFoodGrid(mealSearch);
        $(".meal .search .single .tab-strip-wrapper").show();
    }
}

function searchMultipleItems(query) {
    if (query) { // Display tab strip if query is not empty
        initFoodGrid(mealSearch.multipleQuery, ".search .multiple .meal-grid-wrapper", ".meal .search .multiple .meal-grid", "NutritionixMultiItemSearch");
        $(".meal .search .multiple .meal-grid").show();
    }
}

function initSingleFoodGrid(mealSearch) {
    var endpoint = "";
    var data = (mealSearch.singleQuery) ? { query: mealSearch.singleQuery } : null;

    if (mealSearch.singleQuery !== mealSearch.lastSingleQuery // If query changed or
        || mealSearch.singleQuery === mealSearch.lastSingleQuery && !mealSearch.isRecentInit) { // If query is the same and grid is not initialized
        if (mealSearch.selectedTab === "recent") {
            var daysBack = 30;
            endpoint = "GetRecentFoodItems";
            data = {
                //startDTString: Date.today().add(-1 * daysBack).day().toLocaleString(),
                //endDTString: new Date().toLocaleString()
            };
        } else if (mealSearch.selectedTab === "upc") {
            endpoint = "NutritionixUPCLookup";
        } else if (mealSearch.selectedTab === "branded") {
            endpoint = "NutritionixBrandedItemSearch";
        } else if (mealSearch.selectedTab === "common") {
            endpoint = "NutritionixCommonItemSearch";
        } //else if (mealSearch.selectedTab === "meals") {
        //    endpoint = "MyMeals";
        //}
        initFoodGrid(data, ".search .single .meal-grid-wrapper", ".meal .search .single ." + mealSearch.selectedTab + "-tab .meal-grid", endpoint);
    }
}

function initFoodGrid(data, gridContainerSelector, gridSelector, endpoint) {
    deselectResultGrid();
    resetMealGrid(gridContainerSelector, mealSearch.selectedTab);

    //if (query) createFoodItemGrid(gridSelector, endpoint, query);
    if (data) createFoodItemGrid(gridSelector, endpoint, data);
    //.setDataSource(createFoodItemDataSource(endpoint, query));
}

function resetMealGrid(selector, wrapperClass) {
    $(".meal-info-wrapper").remove();
    wrapperClass = (wrapperClass) ? wrapperClass : "";
    var mealGridWrapperTemplate = kendo.template($("#meal-grid-wrapper-template").html());
    var mealGridWrapper = mealGridWrapperTemplate({ wrapperClass: wrapperClass });
    $(selector).replaceWith(mealGridWrapper);

}


//function createFoodItemGrid(gridSelector, controllerMethod, query) {
function createFoodItemGrid(gridSelector, controllerMethod, data) {
    mealSearch.resultGrid = $(gridSelector).kendoGrid({
        selectable: "multiple",
        resizable: true,
        editable: "incell",
        scrollable: true,
        noRecords: true,
        dataSource: {
            type: "json",
            transport: {
                read: {
                    type: "post",
                    dataType: "json",
                    url: "/Physical/" + controllerMethod,
                    data: data
                }
            },
            schema: {
                type: "json",
                parse: function (response) {
                    if (response.success)
                        return response.Model.MealItems;
                    else
                        return response;
                }
            },
            aggregate: [
                { field: "Calaries", aggregate: "sum" },
                { field: "Carbohydrates", aggregate: "sum" },
                { field: "Protein", aggregate: "sum" },
                { field: "Fat", aggregate: "sum" }
            ],
            errors: function (error) {
                return response.error;
            }
        },
        pageable: {
            pageSize: 3,
            pageSizes: [5, 10, 20, 50, "all"],
            buttonCount: 3
        },
        columns: [
            {
                width: 40
            },
            {
                field: "ThumbURL",
                title: "Pic",
                headerAttributes: { style: "text-align: center;" },
                template: function (dataItem) {
                    return "<img class='food-item img-responsive img-rounded' src='" + dataItem.ThumbURL + "'  height=50/>";
                },
                width: 60,
                footerTemplate: kendo.template($("#result-grid-footer-template").html()),
                editable: function () {
                    return false;
                },
            },
            {
                field: "Quantity",
                title: "Qty",
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                width: 50,
                editor: function (container, options) {
                    //create input element and add the validation attribute
                    var input = $('<input name="' + options.field + '" type="number" required="required" style="width:35px" />');
                    //input.kendoNumericTextBox();
                    input.appendTo(container);
                    //append the editor
                    //input.appendTo(container);
                    ////enhance the input into NumericTextBox

                    ////create tooltipElement element, NOTE: data-for attribute should match editor's name attribute
                    //var tooltipElement = $('<span class="k-invalid-msg" data-for="' + options.field + '"></span>');
                    ////append the tooltip element
                    //tooltipElement.appendTo(container);
                },
                editable: function (dataItem) {
                    dataItem.OldQuantity = dataItem.Quantity
                    return true;
                }
            },
            {
                field: "Unit",
                title: "Unit",
                attributes: { style: "text-transform:capitalize;" },
                width: 125,
                editable: function () {
                    return false;
                },
            },
            {
                field: "FoodName",
                title: "Food",
                attributes: { style: "text-transform:capitalize;" },
                width: 125,
                editable: function () {
                    return false;
                },
            },
            {
                field: "Calaries",
                title: "Calaries",
                width: 75,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Calaries) #",
                //footerTemplate: "#: parseInt(sum) #"
                footerTemplate: "<div class='total wrapper'><div class='calaries aggregate'>#: parseInt(sum) #</div></div>",
                editable: function () {
                    return false;
                },
            },
            {
                field: "Carbohydrates",
                title: "Carbs",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Carbohydrates) #",
                //footerTemplate: "#: parseInt(sum) #"
                footerTemplate: "<div class='total wrapper'><div class='carbs aggregate'>#: parseInt(sum) #</div></div>",
                editable: function () {
                    return false;
                },
            },
            {
                field: "Protein",
                title: "Prot",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Protein) #",
                //footerTemplate: "#: parseInt(sum) #"
                footerTemplate: "<div class='total wrapper'><div class='protein aggregate'>#: parseInt(sum) #</div></div>",
                editable: function () {
                    return false;
                },
            },
            {
                field: "Fat",
                title: "Fat",
                width: 60,
                headerAttributes: { style: "text-align: center;" },
                attributes: { style: "text-align: center;" },
                template: "#: parseInt(Fat) #",
                //footerTemplate: "#: parseInt(sum) #"
                footerTemplate: "<div class='total wrapper'><div class='fat aggregate'>#: parseInt(sum) #</div></div>",
                editable: function () {
                    return false;
                },
            }
        ],
        cellClose: function (e) {
            CloseCell(e.model);
        },
        change: onResultSelected,
        error: function (e) {
            if (e.errors) {
                var errorwindow = CreateModalWindow("errorwindow", "Error", true, false, false, 300);
                errorwindow.content(e.errors);
                $("#errorwindow").append(kendo.template($("#errorwindowclose").html()));
                errorwindow.setOptions({
                    open: function () {
                        this.wrapper.css({ top: 20 });
                    }
                });
                errorwindow.center().open();
            }
        },
        dataBound: function () {
            if (mealSearch.selectedTab == "recent") {
                mealSearch.isRecentInit = true;
            } else if (mealSearch.selectedTab == "upc") {
                mealSearch.isUPCInit = true;
            } else if (mealSearch.selectedTab == "branded") {
                mealSearch.isBrandedInit = true;
            } else if (mealSearch.selectedTab == "common") {
                mealSearch.isCommonInit = true;
            }
            //else if (mealSearch.selectedTab == "meals") {
            //    mealSearch.isMealsInit = true;
            //}
        }
    }).data("kendoGrid");

    //$(gridSelector).on("click", "tr", function (e) {
    //    onResultSelected(e);
    //});

    return mealSearch.resultGrid;
}

function createFoodItemDataSource(controllerMethod, query) {
    return new kendo.data.DataSource({
        type: "json",
        transport: {
            read: {
                type: "post",
                dataType: "json",
                url: "/Physical/" + controllerMethod,
                data: { query }
            }
        },
        schema: {
            type: "json",
            parse: function (response) {
                if (response.success)
                    return response.Model.MealItems;
                else
                    return response;
            }
        },
        aggregate: [
            { field: "Calaries", aggregate: "sum" },
            { field: "Carbohydrates", aggregate: "sum" },
            { field: "Protein", aggregate: "sum" },
            { field: "Fat", aggregate: "sum" }
        ],
        errors: function (error) {
            return response.error;
        }
    });
}

function deselectResultGrid() {
    resetPotentialTotals();
}

function refreshMealConsumedCheckbox(gridSelector) {
    /* If any of the food items were not consumed, set meal as not consumed */
    if ($(gridSelector + " input:checkbox.was-item-consumed:not(:checked)").length > 0) {
        $(gridSelector + " .was-meal-consumed").prop("checked", false); // Un-check meal consumed checkbox
    }

    /* If all of the food items were consumed, set meal as consumed */
    if ($(gridSelector + " input:checkbox.was-item-consumed:not(:checked)").length == 0) {
        $(gridSelector + " .was-meal-consumed").prop("checked", true); // Un-check meal consumed checkbox
    }
}

function onSearchSingleItems() {
    mealSearch.lastSingleQuery = mealSearch.singleQuery;
    mealSearch.singleQuery = $(".meal .search .single .searchbar input").val();

    searchSingleItems(mealSearch.singleQuery);
}

function onSearchMultipleItems() {
    mealSearch.lastMultipleQuery = mealSearch.multipleQuery;
    mealSearch.multipleQuery = $(".meal .search .multiple textarea").val();

    searchMultipleItems(mealSearch.multipleQuery);
}

function onSingleSearchTabChanged() {
    deselectResultGrid();
    initSingleFoodGrid(mealSearch);
}

function onResultSelected() {
    var grid = this,
        rows = grid.select();

    mealSearch.selectedResultItems = [];
    rows.each(function (index, row) {
        mealSearch.selectedResultItems.push(grid.dataItem(row));
    });

    displayPotentialTotals();

    if (mealSearch.selectedResultItems.length > 0)
        $(".meal .k-grid button.select-meal-results").show();
    else
        $(".meal .k-grid button.select-meal-results").hide();
}

function onPlannedItemSelected() {
    var grid = this,
        rows = grid.select();

    mealSearch.selectedPlannedItems = [];
    rows.each(function (index, row) {
        mealSearch.selectedPlannedItems.push(grid.dataItem(row));
    });

    if (mealSearch.selectedPlannedItems.length > 0)
        $(".meal .k-grid button.remove-meal-item").show();
    else
        $(".meal .k-grid button.remove-meal-item").hide();
}

function onAddMealResultsClicked(button) {
    $(mealSearch.selectedResultItems).each(function (index, item) {
        mealSearch.plannedGrid.dataSource.add(item);
        //mealSearch.resultGrid.dataSource.remove(item);
    });

    refreshMealConsumedCheckbox(mealSearch.plannedGridSelector);

    $(".save-meal").show(); // Display save meal button. It will be hidden after the meal is saved


    /* Hide search items after adding all results */
    //var dataItems = mealSearch.resultGrid.dataSource.data();
    //if (dataItems.length == 0) {
    //    $("#mealSearch").collapse("hide"); // Collapse search display
    //    //hideSingleSearch();
    //    //$(".meal .search .single .tab-strip-wrapper").hide();
    //    //$(".meal .search .multiple .meal-grid").hide();
    //}
}

function onRemoveMealItemClicked(button) {
    $(mealSearch.selectedPlannedItems).each(function (index, item) {
        mealSearch.plannedGrid.dataSource.remove(item);
    });

    refreshMealConsumedCheckbox(mealSearch.plannedGridSelector);

    $(".save-meal").show(); // Display save meal button. It will be hidden after the meal is saved
}

function onSaveMealClicked(button) {
    var eventID = $(".meal-event").data("eventID");
    var eventObject = new Event(eventID);
    var event = eventObject.getEvent();

    var meal = getMeal(eventID);
    if (!meal) {
        meal = {
            TaskIDs: eventObject.getTaskIDs()
        };
    }
    meal.Name = event.title;
    meal.Type = "Recommended";
    meal.MealItems = mealSearch.plannedGrid.dataSource.data().toJSON();
    delete meal.Macros;



    var data = { meal: meal };
    Framework.invokeJsonWebService("/Physical/SaveMeal", data, function (result) {
        if (result.success) {
            getMeals(function () {
                var meal = getMeal(eventID);
                initPlannedMeal(mealSearch, meal);

                $(".save-meal").hide(); // Hide save meal button. It is shown when a food item is added or removed
            });
        } else {
            alertModal(result.error);
        }
    });
}

function onSelectableClicked(checkbox) {
    //var value = $(checkbox).val();

    if (!$(checkbox).prop("checked")) {
        if (mealSearch.plannedGrid) {
            mealSearch.plannedGrid.setOptions({
                selectable: "multiple"
            });
        }
        if (mealSearch.resultGrid) {
            mealSearch.resultGrid.setOptions({
                selectable: "multiple"
            });
        }
    } else {
        if (mealSearch.plannedGrid) {
            mealSearch.plannedGrid.setOptions({
                selectable: false
            });
        }
        if (mealSearch.resultGrid) {
            mealSearch.resultGrid.setOptions({
                selectable: false
            });
        }
    }
}