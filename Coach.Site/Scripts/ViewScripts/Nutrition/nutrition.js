var nutritionVM = {
    Days: [
        {
            Date: "2020-07-15 00:00:00",
            Calorie: {
                Recommended: 2000,
                Consumed: 457,
                Remaining: 1543,
                SuccessState: "Pending"
            },
            Water: {
                Recommended: 140,
                Consumed: 28,
                Remaining: 112,
                SuccessState: "Pending"
            },
            Carbs: {
                Recommended: null,
                Consumed: null,
                Remaining: null,
                SuccessState: "No-Data"
            },
            Protein: {
                Recommended: null,
                Consumed: null,
                Remaining: null,
                SuccessState: "No-Data"
            },
            Fat: {
                Recommended: null,
                Consumed: null,
                Remaining: null,
                SuccessState: "No-Data"
            }
        }
    ]
};
var defaultCurrentNutritionInfo = nutritionVM.Days[0];
var currentNutritionInfo;

$(document).ready(function () {
    RefreshNutritionInfo(selectedDate, DrawNutrition);

    dateChangedEventHandlers.push({
        function: OnSelectedDateChangedNutitionInfo,
        data: {}
    });
});

function RefreshNutritionInfo(date, callback) {
    $.ajax({
        url: "/Nutrition/GetNutritionVM",
        type: "POST",
        data: { startDate: date.toLocaleString() },
        success: function (response) {
            if (response.success) {
                nutritionVM = response.NutritionVM;
                if (response.NutritionVM.CurrentNutritionInfo)
                    currentNutritionInfo = response.NutritionVM.CurrentNutritionInfo;
                else
                    currentNutritionInfo = defaultCurrentNutritionInfo;

                if (callback) callback();
            }
        }
    });
}

function DrawNutrition() {
    DrawNutritionInfo();
    DrawMealEvent();
}

function DrawNutritionInfo() {
    var template = kendo.template($("#nutrition-info-template").html());
    var html = template(currentNutritionInfo);
    $("#nutrition-info-container").html(html);
}

function DrawMealEvent() {
    $(".meal-event-container").empty();
    $(currentNutritionInfo.Meals).each(function (index, meal) {
        var template = kendo.template($("#meal-event-template").html());
        var html = template(meal);
        $(".meal-event-container").append(html);
    });
}

function OnSelectedDateChangedNutitionInfo() {
    RefreshNutritionInfo(selectedDate, DrawNutrition);
}