var selectedDate;


$(document).ready(function () {
    initialize();
});

function initialize() {
    initDates();
    initPlanner(selectedDate);
    initPlannerData();
}

function initDates() {
    selectedDate = new Date();

    selectedDate.setSeconds(0);
    selectedDate.setMinutes(0);
    selectedDate.setHours(0);
}

function initPlanner(date) {
    /* Create scheduler */
    planner = $("#planner").kendoScheduler({
        date: date,
        startTime: new Date(date.getFullYear(), date.getMonth(), date.getDate(), 6, 0, 0),
        allDayEventTemplate: $("#event-template").html(),
        eventTemplate: $("#event-template").html(),
        footer: false,
        views: [
            {
                type: "day",
                selected: true
            }
        ],
        timezone: "Etc/UTC",
        resources: [
            {
                field: "ownerId",
                title: "Type",
                dataSource: [
                    { text: "Routine_Complete", value: 1, color: "#50FF82" },
                    { text: "Routine_Incomplete", value: 2, color: "#FF6A6A" },
                    { text: "Task_Complete", value: 3, color: "#14B242" },
                    { text: "Task_Incomplete", value: 4, color: "#B22726" },

                    { text: "Routine_Single", value: 5, color: "#60FF3C" },
                    { text: "Routine_Repetitive", value: 6, color: "#3E9CE8" },

                    { text: "TodoItem_Spotlight", value: 7, color: "#3E9CE8" },
                    { text: "TodoItem_Errand", value: 8, color: "#38FFF2" },
                    //{ text: "TodoItem_Errand_Scheduled", value: 8, color: "#38FFF2" }, // It an Errand was set to a specific time
                    { text: "TodoItem_Tally", value: 9, color: "#3E9CE8" },
                    { text: "TodoItem_Inverse", value: 10, color: "#3E9CE8" },
                    { text: "TodoItem_Repetitive", value: 11, color: "#444DFF" },

                    { text: "TodoItem_Spotlight_Repetitive", value: 12, color: "#51a0ed" },
                    { text: "TodoItem_Errand_Repetitive", value: 13, color: "#51a0ed" },
                    { text: "TodoItem_Tally_Repetitive", value: 14, color: "#51a0ed" },
                    { text: "TodoItem_Inverse_Repetitive", value: 15, color: "#56ca85" },
                ]
            }
        ],
        navigate: plannerView_Change
    }).data("kendoScheduler");
}

function initPlannerData() {
    Framework.invokeJsonWebService("/Planner/GetPlannerViewModel", null, function (result) {
        if (result.success) {
            setPlannerDateSource();
        } else {
            alertModal(result.error);
        }
    });
}

function setPlannerDateSource(events) {
    var dataSource = new kendo.data.SchedulerDataSource({
        batch: true,
        transport: {
            read: function (options) {
                /* Date variable to pass in date */
                var data = {
                    startDTString: selectedDate.toLocaleString()
                };
                transportEvents(data, options, "GetEvents");
            },
            update: function (options) {
                var editable = $("#allow-event-edit").prop("checked");

                if (editable) {
                    var dataSource = planner.dataSource.data();

                    $(options.data.models).each(function (index, event) {
                        event.StartDateTime = event.StartDateTime.toLocaleString();
                        event.EndDateTime = event.EndDateTime.toLocaleString();
                    });

                    var data = { Events: options.data.models }; // Date variable to pass in Events
                    transportEvents(data, options, "UpdateEvents", "Planner update successful!");
                } else {
                    return options.error();
                }

            },
            create: function (options) {
                $(options.data.models).each(function (index, event) {
                    event.StartDateTime = event.StartDateTime.toLocaleString();
                    event.EndDateTime = event.EndDateTime.toLocaleString();
                })

                var data = { Events: options.data.models }; // Date variable to pass in Events
                transportEvents(data, options, "CreateEvents", ((data.Events.length == 1) ? "Event" : "Events") + " created successfully!");

            },
            destroy: function (options) {
                $(options.data.models).each(function (index, event) {
                    event.StartDateTime = event.StartDateTime.toLocaleString();
                    event.EndDateTime = event.EndDateTime.toLocaleString();
                })

                var data = { Events: options.data.models }; // Date variable to pass in Events
                transportEvents(data, options, "DeleteEvents", ((data.Events.length == 1) ? "Event" : "Events") + " deleted successfully!");
            },
            parameterMap: function (options, operation) {
                if (operation !== "read" && options.models) {
                    return { models: kendo.stringify(options.models) };
                }
            }
        },
        schema: {
            model: {
                id: "idEvent",
                fields: {
                    idEvent: { from: "idEvent", type: "number" },
                    title: { from: "Title", defaultValue: "No title" },
                    start: { type: "date", from: "StartDateTime" },
                    end: { type: "date", from: "EndDateTime" },
                    description: { from: "Description" },
                    isAllDay: { type: "boolean", from: "IsAllDay" },
                    ownerId: { from: "OwnerID", defaultValue: 8 },
                    Type: { from: "Type" } // Type is not a native property for a Kendo event so it needs to be added explicitly 
                }
            }
        },
    });

    $("#planner").data("kendoScheduler").setDataSource(dataSource);
}

function transportEvents(data, options, controllerMethod, successMessage) {
    Framework.invokeJsonWebService("/Planner/" + controllerMethod, data, function (result) {
        if (result.success) {
            var events = result.Events;

            /* If Reading data */
            if (controllerMethod == "GetEvents") {
                Events = result.Events;

                // I CANT REMEMBER WHY THIS IS HERE. IT CAN PROBABLY BE DELETED
                $(events).each(function (index, event) {
                    event.StartDateTime = new Date(event.StartDateTime);
                    event.EndDateTime = new Date(event.EndDateTime);
                });
                options.success(events); // Refresh scheduler events
            } else {
                options.success();
            }

            if (successMessage) notifySuccess(successMessage); // Notify successful
        } else {
            alertModal(result.error);
        }
    });
} function plannerView_Change(e) {
    var view = planner.view();
    plannerTimeframe = e.view;

    if (e.action == "today" || e.action == "changeView" || e.action == "changeDate") {
        if (e.view == "day") {
            selectedDate = (e.date >= view.startDate() || e.date <= view.endDate())
                ? e.date : new Date(view.startDate().getTime());
            //plannerStartDate = new Date(e.date.getTime());
            //plannerEndDate = new Date(e.date.getTime());
        }
    } else if (e.action == "next" || e.action == "previous") {
        if (e.view == "day") {
            selectedDate = new Date(e.date.getTime());
            //plannerStartDate = new Date(plannerDate.getTime());
            //plannerEndDate = new Date(plannerDate.getTime());
        } 
    }

    /* The end date should be set to the end of the day */
    //plannerEndDate.setSeconds(59);
    //plannerEndDate.setMinutes(59);
    //plannerEndDate.setHours(23);

    //refreshSpotlights(plannerDate);

    planner.dataSource.read();
    planner.refresh();
    //getTasks();
    //getMeals();

    //refreshItemGrid();
}

function eventDiv_Clicked(eventDiv) {
    var eventID = $(eventDiv).attr("data-eventid"),
        eventType = $(eventDiv).attr("data-eventType");

    var eventContainer = $("#event-container")
        .empty()
        .append('<div id="event-info"></div>');

    if (eventType === "meal-event") {
        var mealID = $(eventDiv).attr("data-mealid");
        DrawMealEvent(mealID);
    }
}