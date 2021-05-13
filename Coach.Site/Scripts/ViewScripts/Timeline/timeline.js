var currentDate = new Date(),
    selectedDate = currentDate;
var padding = 4;
var timelineVM = {
    CurrentDate: null,
    Weeks: [
        {
            Days: [
                {
                    Month: "Jun",
                    DayOfMonth: "28",
                    DayOfWeek: "Sun"
                },
                {
                    Month: "Jun",
                    DayOfMonth: "29",
                    DayOfWeek: "Mon"
                },
                {
                    Month: "Jun",
                    DayOfMonth: "30",
                    DayOfWeek: "Tue"
                },
                {
                    Month: "Jul",
                    DayOfMonth: "1",
                    DayOfWeek: "Wed"
                },
                {
                    Month: "Jul",
                    DayOfMonth: "2",
                    DayOfWeek: "Thu"
                },
                {
                    Month: "Jul",
                    DayOfMonth: "3",
                    DayOfWeek: "Fri"
                },
                {
                    Month: "Jul",
                    DayOfMonth: "4",
                    DayOfWeek: "Sat"
                }
            ]
        }
    ]
};
var dateChangedEventHandlers = [
];

$(document).ready(function () {
    Init();
});

function Init() {
    $.ajax({
        url: "/Timeline/GetTimelineVM",
        type: "POST",
        data: {
            date: currentDate.toLocaleString(),
            padding: padding
        },
        success: function (response) {
            if (response.success) {
                timelineVM = response.timelineVM;
                DrawTimeline();
            }
        }
    });
}

function DrawTimeline() {
    /* Create week elements and append them to timeline-week-container */
    $(".timeline-week-container").empty();
    $(timelineVM.Weeks).each(function (index, week) {
        var template = kendo.template($("#timeline-week-template").html());
        var html = template(week);
        $(".timeline-week-container").append(html);
    });

    $(".timeline-week-container").css("margin-left", -padding * $(".week").width() + "px");

    $(".week").kendoTouch({
        enableSwipe: true,
        swipe: function (e) {
            if (e.direction === "left")
                var newWeekIndex = $(this.element[0]).data("index") + 1;
            else if (e.direction === "right")
                newWeekIndex = $(this.element[0]).data("index") - 1;

            if (newWeekIndex >= 0 && newWeekIndex <= padding * 2) {
                var offset = -newWeekIndex * $(".week").width();
                $(".timeline-week-container").animate({ 'marginLeft': offset + "px" }, 400);
            }
        }
    });

    $(".view.date").on("click", onDateViewClicked);
    $(".view.percent").on("click", onPercentViewClicked);
}

function onDateViewClicked() {
    var day = $(this).closest(".day");
    selectedDate = new Date(day.data("date"));

    if (day.hasClass("selected")) {
        $(".view.date").addClass("d-none");
        $(".view.percent").removeClass("d-none");
    } else {
        $(".day").removeClass("selected");
        day.addClass("selected");

        $(dateChangedEventHandlers).each(function (index, handler) {
            //if (handler.function)
                handler.function(handler.data);
        })
    }
}

function onPercentViewClicked() {
    var day = $(this).closest(".day");
    selectedDate = new Date(day.data("date"));

    if (day.hasClass("selected")) {
        $(".view.percent").addClass("d-none");
        $(".view.date").removeClass("d-none");
    } else {
        $(".day").removeClass("selected");
        day.addClass("selected");
    }
}