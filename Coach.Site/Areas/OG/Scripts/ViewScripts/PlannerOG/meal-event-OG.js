var Meal;

$(document).ready(function () {
    
});

function InitializeSearchBar() {
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
                DrawResultGrid({ query: e.dataItem.food_name })
            //if (e.dataItem.itemType === "Common Foods") {
            //    Framework.invokeJsonWebService("/Physical/NutritionixCommonItemSearch", { query: e.dataItem.food_name }, function (result) {
            //        if (result.success) {
            //            mealSearch.selectedResultItems = result.Model.MealItems;
            //            onAddMealResultsClicked();
            //        } else {
            //            alert(reult.error);
            //        }
            //    });
            //} else if (e.dataItem.itemType === "Branded Foods") {
            //    Framework.invokeJsonWebService("/Physical/NutritionixUPCLookup", { query: e.dataItem.food_name }, function (result) {
            //        if (result.success) {
            //            mealSearch.selectedResultItems = result.Model.MealItems;
            //            onAddMealResultsClicked();
            //        } else {
            //            alert(reult.error);
            //        }
            //    });
            //}
        }
    });
}

//function createFoodItemGrid(gridSelector, controllerMethod, query) {
function DrawResultGrid(data) {
    var controllerMethod;

    var searchType = $(".search-type label.active input").data("search-type");
    if (searchType === "recent") {
        var daysBack = 30;
        controllerMethod = "GetRecentFoodItems";
    } else if (searchType === "upc")
        controllerMethod = "NutritionixUPCLookup";
    else if (searchType === "branded") 
        controllerMethod = "NutritionixBrandedItemSearch";
    else if (searchType === "common") 
        controllerMethod = "NutritionixCommonItemSearch";

    resultGrid = $("#result-grid").kendoGrid({
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

function RefreshMealInfo(mealID, callback) {
    $.ajax({
        url: "/Physical/GetMealFromID",
        type: "POST",
        data: { mealID: mealID },
        success: function (response) {
            if (response.success) {
                if (response.Meal)
                    Meal = response.Meal;

                if (callback) callback();
            }
        }
    });
}

function DrawMealEvent(mealID) {
    RefreshMealInfo(mealID, RefreshMealEvent);
}

function RefreshMealEvent() {
    var template = kendo.template($("#meal-event-template").html());
    $("#event-info").empty().append(template);

    InitializeSearchBar();
}

