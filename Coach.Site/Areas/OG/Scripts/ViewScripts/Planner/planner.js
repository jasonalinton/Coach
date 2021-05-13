var initialized;

var date, // Current date
    plannerDate, // Current selected dated on planner
    plannerStartDate, // First date on the planner;
    plannerEndDate; // Last date on the planner;

var plannerTimeframe = "day";

var inventoryItem;
var SpotlightMappings;

var InventoryItems = [],
    Goals = [],
    TodoItems = [],
    Routines = [],
    Tasks = [],
    Events = [];

var Meals = [];

var Timeframes = [];

var spotlights = {
    month: {
        idInventoryItem: null,
        inventoryItemString: null
    },
    week: {
        idInventoryItem: null,
        inventoryItemString: null
    },
    day: {
        idInventoryItem: null,
        inventoryItemString: null
    },
};

var months = ["January", "February", "March", "April", "May", "June",
    "July", "August", "September", "October", "November", "December"];

/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */
/* ---------------------------------------------------------- Initialization ---------------------------------------------------------- */
/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */

$(document).ready(function () {
    //getNutritionixSearch();
    initialize();

});

//function fitWidget() {
//    var widget = $("#scheduler").data("kendoScheduler");
//    var height = $(window).outerHeight();

//    // Size the widget to take the whole view.
//    widget.element.height(height);
//    widget.resize(true);
//}


function initialize() {
    initDates();
    initPlanner();
    initPlannerData();
    getPlannerItems();
}

/* Initialize planner start and end dates. 
   I HAVE TO FIGURE OUT IF THIS THE DATES VISIBLE ON THE PLANNER OR THE DATES OF THE EVENTS AND TASKS THAT ARE QUERIED */
function initDates() {
    date = new Date();

    if (plannerTimeframe == "day") {
        plannerStartDate = date; // Planner start date
        plannerEndDate = new Date(date.getFullYear(), date.getMonth(), date.getDate()); // Planner end date
        /* The start date should be set to the beginning of the day not the current time */
        plannerStartDate.setSeconds(0);
        plannerStartDate.setMinutes(0);
        plannerStartDate.setHours(0);

    }
    else if (plannerTimeframe == "workWeek") {
        plannerStartDate = Date.monday(); // Planner start date
        plannerEndDate = Date.friday(); // Planner end date
    }
    else if (plannerTimeframe == "week") {
        plannerStartDate = Date.sunday(); // Planner start date
        plannerEndDate = Date.saturday(); // Planner end date
    }
    else if (plannerTimeframe == "month") {
        plannerStartDate = Date.today().moveToFirstDayOfMonth(); // Planner start date
        plannerEndDate = Date.today().moveToLastDayOfMonth(); // Planner end date
    }

    /* The end date should be set to the end of the day */
    plannerEndDate.setSeconds(59);
    plannerEndDate.setMinutes(59);
    plannerEndDate.setHours(23);
}

function initPlanner() {
    var views = [
        { type: "day" },
        { type: "workWeek" },
        { type: "week" },
        { type: "month" },
        { type: "agenda" },
        { type: "timeline", eventHeight: 50 }
    ];

    $(views).each(function () {
        if (this.type == plannerTimeframe) {
            this.selected = true;
        }
    });

    // Create scheduler
    planner = $("#planner").kendoScheduler({
        date: date,
        startTime: new Date(date.getFullYear(), date.getMonth(), date.getDate(), 6, 0, 0),
        //endTime: new Date(date.getFullYear(), date.getMonth(), date.getDate(), 6, 0, 0),
        //showWorkHours: false,
        //selectable: true,
        //height: $(window).height() - 40,
        //height: $(window).height() - $(".navbar").height(),
        allDayEventTemplate: $("#event-template").html(),
        eventTemplate: $("#event-template").html(),
        footer: false,
        views: views,
        //mobile: true,
        timezone: "Etc/UTC",
        //showFooter: false,
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
            SpotlightMappings = result.model.SpotlightMappings;
            Timeframes = result.model.Timeframes;

            refreshSpotlights(date);

            setPlannerDateSource();

            //initHeader();
            refreshItemGrid();
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
                    //startDTString: plannerStartDate.toJSON(),
                    //endDTString: plannerEndDate.toJSON()
                    startDTString: plannerStartDate.toLocaleString(),
                    endDTString: plannerEndDate.toLocaleString()
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
                /* Get Events that have a Timeframe of "Month" */
                /* If view was changed to Month, only show events with the timeframe of Month */
                if (plannerTimeframe == "month") {
                    events = $.grep(events, function (event, index) {
                        if (event.Timeframes.indexOf("Month") > -1) { // Check if Event contains Timeframe of "Month"
                            return event;
                        }
                    });
                }
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
}

function getPlannerItems() {
    getInventoryItems();
    getGoals();
    getTodoItems();
    getRoutines();
    getTasks();
    getMeals();
}

function getInventoryItems() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetInventoryItems", data, function (result) {
        if (result.success) {
            InventoryItems = result.InventoryItems;
        } else {
            alertModal(result.error);
        }
    });
}

function getGoals() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetGoals", data, function (result) {
        if (result.success) {
            Goals = result.Goals;
        } else {
            alertModal(result.error);
        }
    });
}

function getTodoItems() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetTodoItems", data, function (result) {
        if (result.success) {
            TodoItems = result.TodoItems;
        } else {
            alertModal(result.error);
        }
    });
}

function getRoutines() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetRoutines", data, function (result) {
        if (result.success) {
            Routines = result.Routines;
        } else {
            alertModal(result.error);
        }
    });
}

function getTasks() {
    var data = {
        startDTString: plannerStartDate.toLocaleString(),
        endDTString: plannerEndDate.toLocaleString()
    };

    Framework.invokeJsonWebService("/Planner/GetTasks", data, function (result) {
        if (result.success) {
            Tasks = result.Tasks;
        } else {
            alertModal(result.error);
        }
    });
}

function getMeals(callback) {
    var data = {
        startDTString: plannerStartDate.toLocaleString(),
        endDTString: plannerEndDate.toLocaleString()
    };

    Framework.invokeJsonWebService("/Physical/GetMeals", data, function (result) {
        if (result.success) {
            Meals = result.Meals;
            if (callback) callback();
        } else {
            alertModal(result.error);
        }
    });
}

//function getNutritionixSearch() {
//    var data = { query: "chobani" };

//    Framework.invokeJsonWebService("/Physical/SearchNutrionixAPIAsync", data, function (result) {
//        if (result.success) {
//            InventoryItems = result.InventoryItems;
//        } else {
//            alertModal(result.error);
//        }
//    });
//}



/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */
/* ----------------------------------------------------------  Select Event  ---------------------------------------------------------- */
/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */

function Event(eventID) {
    this.eventID = eventID;
    this.event = null;
    this.todoItems = null;
    this.getIsTodoItem = function () {
        if (this.getEvent().Type.includes("TodoItem") || this.getEvent().Type.includes("Task")) {
            return true;
        } else {
            return false;
        }
    };
    this.getIsRoutine = function () {
        if (this.getEvent().Type.includes("Routine")) {
            return true;
        } else {
            return false;
        }
    };
    this.getEvent = function () {
        if (this.event) {
            return this.event;
        } else {
            return Events.filter(function (event) {
                return event.idEvent == eventID;
            })[0];
        }
    };
    /* Get Routines */
    this.getRoutineIDs = function () {
        return this.getEvent().RoutineIDs;
    };
    this.getRoutines = function () {
        var routinesIDs = this.getRoutineIDs();
        return Routines.filter(x => routinesIDs.indexOf(x.idRoutine) > -1);
    };
    /* Get Todo Items */
    this.getTodoItemIDs = function () {
        if (this.getIsTodoItem()) {
            return this.getEvent().TodoItemIDs;
        } else if (this.getIsRoutine()) {
            return this.getEvent().TodoItemIDs_Routine;
        }
    };
    this.getTodoItems = function () {
        var todoItemIDs = this.getTodoItemIDs();
        return TodoItems.filter(x => todoItemIDs.indexOf(x.idTodoItem) > -1);
    };
    /* Get Tasks */
    this.getTaskIDs = function () {
        return this.getTaskIDs_TodoItem().concat(this.getTaskIDs_Routine());
    };
    this.getTaskIDs_TodoItem = function () {
        return this.getEvent().TaskIDs;
    };
    this.getTaskIDs_Routine = function () {
        var taskIDs = [];

        if (this.getIsRoutine()) {
            var thisEvent = this;
            $(thisEvent.getTodoItems()).each(function (index, todoItem) {
                var task = Tasks.filter(x =>
                    todoItem.TaskIDs.indexOf(x.idTask) > -1 && // ID in todoItem's TaskIDs
                    new Date(x.StartDateTime_String) >= thisEvent.getEvent().start && // StartTime after Routine's Start
                    new Date(x.EndDateTime_String) <= thisEvent.getEvent().end // EndTime before Routine's End
                )[0];

                if (task)
                    taskIDs.push(task.idTask);
            });
        }

        return taskIDs;
    };
    this.getTasks = function () {
        var taskIDs = this.getTaskIDs()
        return Tasks.filter(x => taskIDs.indexOf(x.idTask) > -1);
    };
    this.getTasks_TodoItem = function () {
        var taskIDs_TodoItem = this.getTaskIDs_TodoItem();
        return Tasks.filter(x => taskIDs_TodoItem.indexOf(x.idTask) > -1);
    };
    this.getTasks_Routine = function () {
        var taskIDs_Routine = this.getTaskIDs_Routine();
        return Tasks.filter(x => taskIDs_Routine.indexOf(x.idTask) > -1);
    };
}

function selectEvent(eventID) {
    $("#event-info-container").empty();

    showEventInfo(eventID);
    showEventTasks(eventID); // Show tasks list for event
}

function showEventInfo(eventID) {
    showMealEventInfo(eventID);
}

// EITHER SHOW RECOMMENDED NUTRIENTS OR RECOMMENDED MEALS IF EXISTS
// ALSO SHOW ACTUAL MEALS
/* Show meal info if event represents a meal */
function showMealEventInfo(eventID) {
    var meal = getMeal(eventID)


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

    if (isMeal) {
        var meals = []
        $.each(Meals, function (index, meal) {
            var isMatched = meal.TaskIDs.some(function (taskID) {
                return taskIDs.indexOf(taskID) > -1;
            });
            if (isMatched) meals.push(meal);
        });

        var data = {
            event: eventObject.getEvent(),
            meal: meals[0], // Select the first meal because there currently isn't a situation where there would be more that one meals for a single event
        };

        var mealEvent_Template = kendo.template($("#meal-event-template").html());
        var mealEvent_HTML = mealEvent_Template(data);

        $("#event-info-container").append(mealEvent_HTML);
        if (meals.length > 0) {
            createChart(eventID, data.meal.Macros);
        }

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

function showEventTasks(eventID) {
    var eventObject = new Event(eventID);
    var ul = $(document.createElement("ul")).addClass("task-checklist");

    if (eventObject.getIsTodoItem()) {
        var tasks = eventObject.getTasks_TodoItem();
        $(tasks).each(function (index, task) {
            /* Create list item with text, switch, and delete button */
            var taskLI_Template = kendo.template($("#task-li-template").html());
            var taskLI_HTML = taskLI_Template(task);

            ul.append(taskLI_HTML); // Add list item to unordered list
            ul.addClass("todo-item"); // Add "routine" class to unordered list
        });
    } else if (eventObject.getIsRoutine()) {
        var tasks = eventObject.getTasks_Routine();
        $(tasks).each(function (index, task) {
            /* Create list item with text, switch, and delete button */
            var taskLI_Template = kendo.template($("#task-li-template").html());
            var taskLI_HTML = taskLI_Template(task);

            ul.append(taskLI_HTML); // Add list item to unordered list
            ul.addClass("routine"); // Add "routine" class to unordered list
        });
    }

    $("#task-checklist-container").empty().append(ul);

    $(".task:checkbox").on("change", function () {
        var isComplete = this.checked;
        var taskID = $(this).data("taskid");

        markTaskCompletion(taskID, isComplete)
    });
}

function markTaskCompletion(taskID, isComplete) {
    var task = Tasks.filter(x => x.idTask == taskID)[0]; // Get Task from TaskID
    task.IsComplete = isComplete; // Set Task's completion status
    task.DateTimeCompleted = new Date().toLocaleString();

    var data = { Task: task };

    Framework.invokeJsonWebService("/Planner/MarkTaskCompletion", data, function (result) {
        if (result.success) {
            var events_Task = result.Events_Task;
            updatePercentComplete(events_Task);

            notifySuccess("Task marked complete");
        } else {
            $("input[type='checkbox'][data-taskid='" + task.idTask + "']").prop("checked", !task.IsComplete);

            alertModal(result.error);
        }
    });
}

function updatePercentComplete(events) {
    $(events).each(function (index, event) {
        /* Get Planner Events */
        var scheduler = $("#planner").data("kendoScheduler");
        var events_Planner = scheduler.data();

        var event_Old = events_Planner.filter(x => x.idEvent == event.idEvent)[0]; // Get the original Event model 
        event_Old.PercentComplete = event.PercentComplete; // Update PercentComplete
        event_Old.Type = event.Type; // Update Type

        var eventDIV = $(".event-div[data-eventid='" + event.idEvent + "'"); // Get the div for the Event from the Planner
        eventDIV.attr("data-type", event.Type); // Update div's Type
    });
}

function refreshTasks() {
    var data = { startDateString: new Date().toLocaleString(), endDateString: new Date().toLocaleString() };

    Framework.invokeJsonWebService("/Planner/RefreshTasks", data, function (result) {
        if (!result.success) {
            alertModal(result.error);
        }
    });
}

function initHeader() {
    $("#inventory-item-label").val(inventoryItem);
    $("#month-label").val(months[plannerDate.getMonth()]);
}

//Spotlight - Json object with current spotlight items (current month, current week, current day)
function refreshItemGrid() {
    chengeItemGrid(plannerTimeframe, spotlights);
}

function chengeItemGrid(plannerTimeframe, spotlights) {
    for (var i = 0; i < Timeframes.length; i++) {
        var timeframe = Timeframes[i];

        /* Goals */
        /* Add Spotlight Goals to grid based on Repetition */
        $(Goals).each(function (index, goal) {
            if (goal.Types.indexOf("Spotlight") > -1 && goal.Timeframes.indexOf(timeframe.timeframe) > -1) {
                if (plannerTimeframe == "month" && timeframe.TimeframeAltText == "month") { // If the Planner View is Month and the current iterated Timeframe is Month
                    if (goal.InventoryItemIDs.indexOf(spotlights.month.idInventoryItem) > -1) { // If goal is in the Spotlight
                        var li = createGoalLI(goal);
                        $(".goal ." + timeframe.TimeframeAltText + ".item-list").append(li);
                    }
                } else if (plannerTimeframe == "week" && timeframe.TimeframeAltText == "week") { // If the Planner View is Week and the current iterated Timeframe is week
                    if (goal.InventoryItemIDs.indexOf(spotlights.week.idInventoryItem) > -1) { // If goal is in the Spotlight
                        var li = createGoalLI(goal);
                        $(".goal ." + timeframe.TimeframeAltText + ".item-list").append(li);
                    }
                } else if (plannerTimeframe == "workWeek" && timeframe.TimeframeAltText == "workWeek") { // If the Planner View is WorkWeek and the current iterated Timeframe is WorkWeek
                    if (goal.InventoryItemIDs.indexOf(spotlights.workWeek.idInventoryItem) > -1) { // If goal is in the Spotlight
                        var li = createGoalLI(goal);
                        $(".goal ." + timeframe.TimeframeAltText + ".item-list").append(li);
                    }
                } else if (plannerTimeframe == "day" && timeframe.TimeframeAltText == "day") { // If the Planner View is Day and the current iterated Timeframe is Day
                    if (goal.InventoryItemIDs.indexOf(spotlights.day.idInventoryItem) > -1) { // If goal is in the Spotlight
                        var li = createGoalLI(goal);
                        $(".goal ." + timeframe.TimeframeAltText + ".item-list").append(li);
                    }
                }
            }
        });

        /* Add Primary Goals to grid based on Repetition */
        $(Goals).each(function (index, goal) {
            if (goal.Types.indexOf("Primary") > -1 && goal.Timeframes.indexOf(timeframe.timeframe) > -1) {
                var li = createGoalLI(goal);
                $(".goal ." + timeframe.TimeframeAltText + ".item-list").append(li);
            }
        });



        /* Todo Items */
        /* Add Inventory Todo Items to the grid based on Repetition (All active Todo Items except Errands) */
        $(TodoItems).each(function (index, todoItem) {
            if (todoItem.Types.indexOf("Errand") == -1 && todoItem.Repetitions.indexOf(timeframe.Repetition) > -1) { // If not an Errand
                var li = createTodoItemLI(todoItem);
                $(".todo-item ." + timeframe.RepetitionAltText + ".item-list").append(li);
            }
        });

        /* Add Errand Todo Items to the grid based on Repetition */
        $(TodoItems).each(function (index, todoItem) {
            if (todoItem.Types.indexOf("Errand") > -1 && todoItem.Repetitions.indexOf(timeframe.Repetition) > -1) { // If Errand
                var li = createTodoItemLI(todoItem);
                $(".todo-item ." + timeframe.RepetitionAltText + ".item-list").append(li);
            }
        });



        /* Routines */
        /* Add Spotlight Routines to grid based on Repetition */
        $(Routines).each(function (index, routine) {
            if (routine.Types.indexOf("Spotlight") > -1 && routine.Repetitions.indexOf(timeframe.Repetition) > -1) {
                var li = createRoutineLI(routine);
                $(".routine ." + timeframe.RepetitionAltText + ".item-list").append(li);
            }
        });

        /* Add Primary Routines to the grid based on Repetition */
        $(Routines).each(function (index, routine) {
            if (routine.Types.indexOf("Primary") > -1 && routine.Repetitions.indexOf(timeframe.Repetition) > -1) {
                var li = createRoutineLI(routine);
                $(".routine ." + timeframe.RepetitionAltText + ".item-list").append(li);
            }
        });
    }
}

/* Get mappings for Spotlight Timeframes */
// Ex. The month of January 2018 has Physical Spotlight
function getSpotlightMappings() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetSpotlightMappings", data, function (result) {
        if (result.success) {
            spotlightMappings = result.model.SpotlightMappings;
        } else {
            alertModal(result.error);
        }
    });
}

function getEvents() {
    var data = {};

    Framework.invokeJsonWebService("/Planner/GetEvents", data, function (result) {
        if (result.success) {
            //Events = result.events;
        } else {
            alertModal(result.error);
        }
    });
}

function createGoalLI(goal) {
    var parentIDArray = [];
    var todoItemIDArray = [];
    var routineIDArray = [];

    /* Add classes for Parent Goals */
    $(goal.ParentIDs).each(function (index2, parentID) {
        parentIDArray.push("idParent-" + parentID);
    });

    /* Add classes for Todo Items */
    $(goal.TodoItemIDs).each(function (index2, todoItemID) {
        todoItemIDArray.push("idTodoItem-" + todoItemID);
    });

    /* Add classes for Routines */
    $(goal.RoutineIDs).each(function (index2, routineID) {
        routineIDArray.push("idRoutine-" + routineID);
    });

    var parentIDsString = parentIDArray.join(" ");
    var todoItemIDsString = todoItemIDArray.join(" ");
    var routineIDsString = routineIDArray.join(" ");

    var classString = "";

    classString += parentIDsString
    classString += todoItemIDsString
    classString += routineIDsString

    var li = "<li class='item-list-item " + classString + "'>" + goal.Title + "</li>";

    return li;
}

function createTodoItemLI(todoItem) {
    var parentIDArray = [];
    var goalIDArray = [];
    var routineIDArray = [];

    /* Add classes for Parent TodoItems */
    $(todoItem.ParentIDs).each(function (index2, parentID) {
        parentIDArray.push("idParent-" + parentID);
    });

    /* Add classes for Goals */
    $(todoItem.GoalIDs).each(function (index2, goalID) {
        goalIDArray.push("idGoal-" + goalID);
    });

    /* Add classes for Routines */
    $(todoItem.RoutineIDs).each(function (index2, routineID) {
        routineIDArray.push("idRoutine-" + routineID);
    });

    var parentIDsString = parentIDArray.join(" ");
    var goalIDsString = goalIDArray.join(" ");
    var routineIDsString = routineIDArray.join(" ");

    var classString = "";

    classString += parentIDsString
    classString += goalIDsString
    classString += routineIDsString

    var li = "<li class='item-list-item " + classString + "'>" + todoItem.Title + "</li>";

    return li;
}

function createRoutineLI(routine) {
    var parentIDArray = [];
    var todoItemIDArray = [];
    var goalIDArray = [];

    /* Add classes for Parent Routines */
    $(routine.ParentIDs).each(function (index2, parentID) {
        parentIDArray.push("idParent-" + parentID);
    });

    /* Add classes for Todo Items */
    $(routine.TodoItemIDs).each(function (index2, todoItemID) {
        todoItemIDArray.push("idTodoItem-" + todoItemID);
    });

    /* Add classes for Goals */
    $(routine.GoalIDs).each(function (index2, goalID) {
        goalIDArray.push("idGoal-" + goalID);
    });

    var parentIDsString = parentIDArray.join(" ");
    var todoItemIDsString = todoItemIDArray.join(" ");
    var goalIDsString = goalIDArray.join(" ");

    var classString = "";

    classString += parentIDsString
    classString += todoItemIDsString
    classString += goalIDsString

    var li = "<li class='item-list-item " + classString + "'>" + routine.Title + "</li>";

    return li;

}

function refreshItemQueue() {
    var plannerTimeframe = planner.viewName();
    var seedDate = planner.Date().clearTime(); // Planner date at 12:00 AM

    /* Find taks that don't have an event and need to be queued up at their timeframe to get on event */
    $(Tasks).each(function (index, task) {
        if (task.EventIDs.lenth == 0) {
            var taskTimeframe = task.Timeframe.toLowerCase();

            if (plannerTimeframe == "Month") {
                if (taskTimeframe == "Month") {
                    if (task.Time.StartDayOfWeek || task.Time.StartDayOfMonth || task.StartWeek) { // Check for start day of week, start bay of month, start week, 
                        return; // Don't add to month queue if task has a set day or week
                    }
                    // Add to queue
                }
            } else if (plannerTimeframe == "Week") {
                if (taskTimeframe == "Week") {
                    if (task.StartDayOfWeek) { // Check for start day
                        return; // Don't add to week queue if task has a set day
                    }
                    // Add to queue
                } else if (taskTimeframe == "Month") {
                    if (task.StartDayOfWeek) { // Check for start day
                        return; // Don't add to week queue if task has a set day
                        /* THIS MIGHT BE CHANGED CAUSE TECHNICALLY YOU CAN SET THE TIME OF DAY IN THE WEEK VIEW */

                    } else if (task.StartWeek && task.StartWeek == seedDate.moveToDayOfWeek(0, -1)) { // Check if start week is the same as current week
                        // Add to queue
                    } else if (task.RecommendedStartWeek && task.RecommendedStartWeek == seedDate.moveToDayOfWeek(0, -1)) { // Check if recommened start week is the same as current week
                        // Add to queue
                    } else {
                        return;
                    }
                }
            } else if (plannerTimeframe == "Day") {
                if (taskTimeframe == "Week" || taskTimeframe == "Month") {
                    if (task.StartDay && task.StartDay.getDayNumberFromName() == seedDate.getDay()) { // Check if start date it the same as current planner date
                        // Add to queue
                    }
                } else if (taskTimeframe == "Day") {
                    // Add to queue
                }
            }
        }
    });
}

function FillTree(todoItems, maxLevel) {
    var children = [];

    for (var i = 1; i < maxLevel + 1; i++) {
        for (var j = 0; j < todoItems.length; j++) {
            var todoItem = todoItems[j];

            /* Create a child unordered list if TodoItem has children */
            var childrenUL;
            if (todoItem.Level == i) {
                if (todoItem.Children.length > 0) {
                    childrenUL = $(document.createElement("ul"));

                    $(todoItem.Children).each(function (index, child) {
                        childrenUL.append("<li class='item-list-item' data-id='" + child.idTodoItem + "'>" + child.Item + "</li>");

                        children.push({ idParent: todoItem.idTodoItem, idTodoItem: child.idTodoItem });
                    });
                }


                if (i == 1) {
                    $(".todo.item-list").append("<li class='item-list-item' data-id='" + todoItem.idTodoItem + "'>" + todoItem.Item + "</li>");
                    if (childrenUL) $(".todo.item-list").append(childrenUL);
                } else {
                    $(children).each(function (index, child) {
                        if (child.idTodoItem == todoItem.idTodoItem) {
                            if (childrenUL) $(".item-list-item[data-id='" + child.idParent + "'").append(childrenUL);
                        }
                    });
                }
            }
        }
    }

    //$(".todo.item-list").kendoTreeView();

    $(".item-list-item").draggable({
        revert: true
    });

    $("#setup-planner td[role='gridcell']").droppable();
}

function initSpotlightMappings() {
    spotlights = {
        month: {
            idInventoryItem: null,
            inventoryItemString: null
        },
        week: {
            idInventoryItem: null,
            inventoryItemString: null
        },
        day: {
            idInventoryItem: null,
            inventoryItemString: null
        },
    };
}

function refreshSpotlights(plannerDate) {
    initSpotlightMappings();

    $(SpotlightMappings).each(function (index, spotlightMapping) {
        var firstDayOfMonth = plannerDate.clone().clearTime().moveToFirstDayOfMonth();
        var lastDayOfMonth = plannerDate.clone().clearTime().moveToLastDayOfMonth();

        mappingDate = new Date(spotlightMapping.DateTimeString);

        if (mappingDate.between(firstDayOfMonth, lastDayOfMonth)) {
            if (spotlightMapping.TimeframeClass.timeframe == "Month") {
                spotlights.month.idInventoryItem = spotlightMapping.idInventoryItem;
                spotlights.month.inventoryItemString = spotlightMapping.InventoryItemString;
            } else if (spotlightMapping.TimeframeClass.timeframe == "Week") {
                var weekStartDate = new Date(spotlightMapping.DateTimeString);
                var weekEndDate = weekStartDate.clone().addDays(6);

                if (mappingDate.between(weekStartDate, weekEndDate)) {
                    spotlights.week.idInventoryItem = spotlightMapping.idInventoryItem;
                    spotlights.week.inventoryItemString = spotlightMapping.InventoryItemString;
                }
            } else if (spotlightMapping.TimeframeClass.timeframe == "Day") {
                if (mappingDate.clearTime() = plannerDate.clearTime()) {
                    spotlights.day.idInventoryItem = spotlightMapping.idInventoryItem;
                    spotlights.day.inventoryItemString = spotlightMapping.InventoryItemString;
                }
            }
        }
    });
}


/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */
/* ----------------------------------------------------------     Events     ---------------------------------------------------------- */
/* ---------------------------------------------------------- -------------- ---------------------------------------------------------- */

function eventDiv_Clicked(eventDiv) {
    var eventID = $(eventDiv).attr("data-eventid");
    selectEvent(parseInt(eventID));
}

function deleteTaskButton_Click(svg) {
    $(svg).hide(); // Hide delete button

    var taskID = $(svg).siblings("label").children("input").data("taskid");
    var data = { TaskIDs: [parseInt(taskID)] };

    var showAlart = $("#show-delete-task-alart").prop("checked");
    if (showAlart) {
        var deleteFunction = function (e) {
            Framework.invokeJsonWebService("/Planner/DeleteTasks", e.data.params, function (result) {
                if (result.success) {
                    $(svg).closest("li").remove(); // Remove list item from list
                    Tasks = Tasks.filter(x => x.idTask != taskID); // Remove Task from tasks
                    // Remove TaskID from and connected Items
                    // Edit percentages complete
                    if (showAlart) var modal = e.data.modal.close();
                    notifySuccess("Task deleted");
                } else {
                    alertModal(result.error);
                }
                $(svg).show(); // Show delete
            });
        }

        var cancelFunction = function (e) {
            e.data.modal.close();
            $(svg).show(); // Show delete
        }

        confirmModal("Are you sure you want to delete this Task?", deleteFunction, data);
    } else {
        Framework.invokeJsonWebService("/Planner/DeleteTasks", data, function (result) {
            if (result.success) {
                $(svg).closest("li").remove(); // Remove list item from list
                Tasks = Tasks.filter(x => x.idTask != taskID); // Remove Task from tasks
                // Remove TaskID from and connected Items
                // Edit percentages complete

                notifySuccess("Task deleted");
            } else {
                alertModal(result.error);
            }
            $(svg).show();
        });
    }
}

function plannerView_Change(e) {
    var view = planner.view();
    plannerTimeframe = e.view;

    if (e.action == "today" || e.action == "changeView" || e.action == "changeDate") {
        if (e.view == "day") {
            plannerDate = (e.date >= view.startDate() || e.date <= view.endDate())
                ? e.date : new Date(view.startDate().getTime());
            plannerStartDate = new Date(e.date.getTime());
            plannerEndDate = new Date(e.date.getTime());
        } else if (e.view == "week") {
            plannerDate = (e.date >= view.startDate() || e.date <= view.endDate())
                ? e.date : new Date(view.startDate().getTime());

            if (plannerDate.is().sunday())
                plannerStartDate = new Date(plannerDate.getTime()); // Sunday date
            else if (plannerDate.is().monday())
                plannerStartDate = new Date(plannerDate.getTime()).addDays(-1);// Sunday date
            else
                plannerStartDate = getMonday(new Date(plannerDate.getTime())).addDays(-1); // Sunday date

            plannerEndDate = new Date(plannerStartDate.getTime()).addDays(6); // Saturday date
        } else if (e.view == "workWeek") {
            plannerDate = (e.date >= view.startDate() || e.date <= view.endDate())
                ? e.date : new Date(view.startDate().getTime());

            if (plannerDate.is().sunday())
                plannerStartDate = new Date(plannerDate.getTime()).addDays(1); // Monday date
            else if (plannerDate.is().monday())
                plannerStartDate = new Date(plannerDate.getTime()); // Monday date
            else
                plannerStartDate = getMonday(new Date(plannerDate.getTime())); // Monday date

            plannerEndDate = new Date(plannerStartDate.getTime()).addDays(4); // Friday date
        } else if (e.view == "month") {
            plannerDate = (e.date >= view.startDate() || e.date <= view.endDate()) ? e.date : new Date(view.startDate().getTime());
            plannerStartDate = new Date(plannerDate.getTime()).moveToFirstDayOfMonth(); // First day of month
            plannerEndDate = new Date(plannerDate.getTime()).moveToLastDayOfMonth(); // Last day of month
        }
    } else if (e.action == "next" || e.action == "previous") {
        if (e.view == "day") {
            plannerDate = new Date(e.date.getTime());
            plannerStartDate = new Date(plannerDate.getTime());
            plannerEndDate = new Date(plannerDate.getTime());
        } else if (e.view == "week") {
            plannerDate = new Date(e.date.getTime());
            plannerStartDate = (e.action == "next") ? new Date(plannerDate.getTime()) : new Date(plannerDate.getTime()).addDays(-6); // Sunday date
            plannerEndDate = new Date(plannerStartDate.getTime()).addDays(6); // Saturday date
        } else if (e.view == "workWeek") {
            plannerDate = new Date(e.date.getTime());
            plannerStartDate = (e.action == "next") ? new Date(plannerDate.getTime()) : new Date(plannerDate.getTime()).addDays(-4); // Monday date
            plannerEndDate = new Date(plannerStartDate.getTime()).addDays(4); // Friday date
        } else if (e.view == "month") {
            plannerDate = new Date(e.date.getTime());
            plannerStartDate = (e.action == "next") ? new Date(plannerDate.getTime()) : new Date(plannerDate.getTime()).moveToFirstDayOfMonth(); // First day of month
            plannerEndDate = new Date(plannerDate.getTime()).moveToLastDayOfMonth(); // Last day of month
        }
    }

    /* The end date should be set to the end of the day */
    plannerEndDate.setSeconds(59);
    plannerEndDate.setMinutes(59);
    plannerEndDate.setHours(23);

    refreshSpotlights(plannerDate);

    planner.dataSource.read();
    planner.refresh();
    getTasks();
    getMeals();

    refreshItemGrid();
}

function getMonday(date) {
    var day = date.getDay() || 7;
    if (day !== 1)
        date.setHours(-24 * (day - 1));
    return date;
}





