var selectedDate;
var briefingViewModel;

$("document").ready(function () {
    $("#calendar").kendoCalendar({
        change: function () {
            selectedDate = this.value();
            refreshBriefing(selectedDate);
            $("#selected-date").append(selectedDate.toDateString());
        }
    });

    selectedDate = new Date();
    initializeBriefing();
});

function initializeBriefing() {
    refreshBriefing(selectedDate);
}

function refreshBriefing(date) {
    $.ajax({
        url: "/Briefing/GetBriefingsForDate",
        type: "POST",
        data: { date: date.toLocaleString() },
        success: function (result) {
            if (result.success) {
                briefingViewModel = result.BriefingViewModel;
                displayBriefing(result.BriefingViewModel);
            }
        }
    });
}

function displayBriefing(briefingViewModel) {
    var templatebrief = kendo.template($("#briefing-template").html());
    var htmlBrief = templatebrief(briefingViewModel);
    $("#briefing-container").empty().append(htmlBrief);

    var templatedebrief = kendo.template($("#debriefing-template").html());
    var htmlDebrief = templatedebrief(briefingViewModel);
    $("#debriefing-container").empty().append(htmlDebrief);

    $(".briefing-container textarea.briefing").kendoEditor({
        resizable: {
            content: true,
            toolbar: true
        },
        tools: [
            "bold",
            "italic",
            "underline",
            "strikethrough",
            "justifyLeft",
            "justifyCenter",
            "justifyRight",
            "justifyFull",
            "createLink",
            "unlink",
            "insertImage",
            "createTable",
            "addColumnLeft",
            "addColumnRight",
            "addRowAbove",
            "addRowBelow",
            "deleteRow",
            "deleteColumn",
            "foreColor",
            "backColor"
        ]
    });
}

function saveBriefing(briefingType) {
    var textEditor = (briefingType === "Briefing") ?
        $("#briefing-textarea").data("kendoEditor") : $("#debriefing-textarea").data("kendoEditor");
    //$($("#briefing-textarea")[0]).data("kendoEditor") : $($("#debriefing-textarea")[0]).data("kendoEditor");

    var briefing = {
        InventoryItemID: (briefingType === "Briefing") ? $("#briefing-inventory-item").val() : $("#debriefing-inventory-item").val(),
        TypeID: (briefingType === "Briefing") ? 101 : 102,
        //Text: (briefingType === "Briefing") ? $("#briefing-textarea").val() : $("#debriefing-textarea").val(),
        Text: textEditor.encodedValue(),
        BriefingDate: selectedDate
    };

    $.ajax({
        url: "/Briefing/AddBriefing",
        type: "POST",
        data: { briefing: briefing },
        success: function (result) {
            refreshBriefing(selectedDate);
        }
    });
}