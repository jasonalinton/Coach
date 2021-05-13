var defaultCurrentSleepEvent = {
    SessionDuration: "00:00",
    PercentReuiredSleep: "000",
    TimeInBed: "00:00",
    TimeAwake: "00:00",
    SessionDurationSuccessClass: "no-data",
    InBedSuccessClass: "fas fa-circle no-data",
    OutBedSuccessClass: "fas fa-circle no-data"
};
var sleepSessions = [];

$(document).ready(function () {
    InitializeKendoUpload();
    InitSleepEvents();

    dateChangedEventHandlers.push({
        function: onSelectedDateChanged,
        data: {}
    });
});

function InitializeKendoUpload() {
    $("#sleep-upload").kendoUpload({
        async: {
            saveUrl: "/Sleep/UploadAutoSleepData",
            autoUpload: true,
            validation: {
                allowedExtensions: ["csv"]
            }
        },
        success: function () {
            RefreshSleepEvent(selectedDate, DrawSleepEvent);
        }
    }).data("kendoUpload");
}

function InitSleepEvents() {
    RefreshSleepEvent(currentDate, DrawSleepEvent);
}

function RefreshSleepEvent(date, callback) {
    $.ajax({
        url: "/Sleep/GetSleepVM",
        type: "POST",
        data: { date: date.toLocaleString() },
        success: function (response) {
            if (response.success) {
                sleepSessions = response.sleepVM.SleepSessions;
                if (response.sleepVM.CurrentSleepEvent)
                    currentSleepEvent = response.sleepVM.CurrentSleepEvent;
                else
                    currentSleepEvent = defaultCurrentSleepEvent;

                if (callback) callback();
            }
        }
    });
}

function DrawSleepEvent() {
    var template = kendo.template($("#sleep-event-template").html());
    var html = template(currentSleepEvent);
    $("#sleep-event-container").html(html);
}

function onSleepLabelClick() {
    $("#sleep-upload").click();
}

function onSelectedDateChanged() {
    RefreshSleepEvent(selectedDate, DrawSleepEvent);
}