var selectedDate;
var routineViewModel;

$("document").ready(function () {
    //$("#calendar").kendoCalendar({
    //    change: function () {
    //        selectedDate = this.value();
    //        refreshRoutine();
    //    }
    //});

    
    dateChangedEventHandlers.push({
        function: refreshRoutine,
        data: {}
    });

    selectedDate = new Date();
    initializeRoutine();
});

function initializeRoutine() {
    refreshRoutine();
}

function refreshRoutine() {
    $.ajax({
        url: "/Routine/GetRoutineForDate",
        type: "POST",
        data: {
            routineID: 2,
            date: selectedDate.toLocaleString()
        },
        success: function (result) {
            if (result.success) {
                routineViewModel = result.RoutineViewModel;
                displayRoutineEvent(result.RoutineViewModel.Routine);
            }
        }
    });
}

function displayRoutineEvent(routine) {
    var isShown = $(".routine.event .body").hasClass("show");

    var template = kendo.template($("#routine-template").html());
    var html = template(routine);
    $("#routine-container").empty().append(html);

    if (isShown)
        $(".routine.event .body").addClass("show");
}

function onEditRoutineClick(routineID) {
    var template = kendo.template($("#routine-edit-template").html());
    var html = template(routineViewModel.Routine);
    $("#routine-container").empty().append(html);

    $("#routine-" + routineID + " ul").kendoSortable({
        hint: function (element) {
            return element.clone().addClass("hint");
        },
        placeholder: function (element) {
            return element.clone().addClass("placeholder").text("drop here");
        },
        cursorOffset: {
            top: -10,
            left: -230
        },
        change: function (e) {
            routineID = $(e.item).data("routine-id");
            var todoID = $(e.item).data("todo-id");
            $.ajax({
                url: "/Routine/ReorderTodo",
                type: "POST",
                data: { routineID: routineID, todoID: todoID, newPosition: ++e.newIndex },
                success: function (result) {
                    if (!result.success)
                        $(".todo-switch[data-eventtask-id=" + idEventTask + "]").data("kendoSwitch").toggle();
                }
            });
        }
    });
}

function onDoneEditRoutineClick(routineID) {
    refreshRoutine();
}

function onTodoSwitchToggled(e) {
    var isComplete = e.target.checked;
    var idEventTask = $(e.target).data("eventtask-id");

    $.ajax({
        url: "/Todo/SetTodoCompletion",
        type: "POST",
        data: { idEventTask: idEventTask, isComplete: isComplete },
        success: function (result) {
            if (result.success)
                refreshRoutine();
            else
                $(".todo-switch[data-eventtask-id=" + idEventTask + "]").data("kendoSwitch").toggle();
        }
    });
}