var todoPlannerVM;
var currentDate = new Date(),
    selectedDate = currentDate;
var weekPadding = 1;

var minDayWidth = 250;
var defaultGoogleTaskList = "Todo";


$(document).ready(function () {
    Init();
});

$(window).resize(function () {
    if (todoPlannerVM) DrawDays();
});

function Init() {
    $(".left-panel .calendar").kendoCalendar({
        change: function () {
            selectedDate = this.value();
            RefreshTodoPlannerVM(selectedDate, PopulateGoalsAndTodos);
        }
    });

    InitTodoPlannerVM();
}

function InitTodoPlannerVM() {
    RefreshTodoPlannerVM(currentDate, PopulateGoalsAndTodos);
}

function RefreshTodoPlannerVM(date = selectedDate, callback) {
    $.ajax({ 
        url: "/Planner1/GetTodoPlannerVM",
        type: "POST",
        data: {
            date: date.toLocaleString(),
            weekPadding: weekPadding
        },
        success: function (response) {
            if (response.success) {
                todoPlannerVM = response.todoPlannerVM;
                DrawDays(todoPlannerVM.Days);
                PopulateGoalsAndTodos();
            }
            //if (callback) callback();
        }
    });
}

function ScheduleTodo(idTodo, text, scheduledDateTime, callback = null, googleTaskList = defaultGoogleTaskList) {
    $.ajax({
        url: "/Planner1/ScheduleTodo",
        type: "POST",
        data: {
            idTodo: idTodo,
            text: text,
            scheduledDateTime: scheduledDateTime.toLocaleString(),
            googleTaskList: googleTaskList
        },
        success: function (response) {
            if (response.success) {
                if (callback) callback.function();
            }
            RefreshTodoPlannerVM();
        }
    });
}

function SetTaskCompletionStatus(idTodo, text, scheduledDateTime, isComplete, googleTaskList = defaultGoogleTaskList, callback = null) {
    $.ajax({
        url: "/Planner1/SetTaskCompletionStatus",
        type: "POST",
        data: {
            idTodo: idTodo,
            text: text,
            scheduledDateTime: scheduledDateTime.toLocaleString(),
            isComplete: isComplete,
            googleTaskList: googleTaskList
        },
        success: function (response) {
            if (response.success) {
                if (callback) callback.function();
            }
            RefreshTodoPlannerVM();
        }
    });
}

function SetTaskAttemptedStatus(idTodo, scheduledDateTime, isAttempted, googleTaskList = defaultGoogleTaskList, callback = null) {
    $.ajax({
        url: "/Planner1/MarkTaskAttempted",
        type: "POST",
        data: {
            idTodo: idTodo,
            //text: text,
            scheduledDateTime: scheduledDateTime.toLocaleString(),
            isAttempted: isAttempted,
            googleTaskList: googleTaskList
        },
        success: function (response) {
            if (response.success) {
                if (callback) callback.function();
            }
            RefreshTodoPlannerVM();
        }
    });
}

function DeleteTask(idTodo, scheduledDateTime, googleTaskList = defaultGoogleTaskList, callback = null) {
    $.ajax({
        url: "/Planner1/DeleteGoogleTaskEventTask",
        type: "POST",
        data: {
            idTodo: idTodo,
            scheduledDateTime: scheduledDateTime.toLocaleString(),
            googleTaskList: googleTaskList
        },
        success: function (response) {
            if (response.success) {
                if (callback) callback.function();
            }

            RefreshTodoPlannerVM();
        }
    });
}

function DrawDays(days = todoPlannerVM.Days, date = selectedDate) {
    $(".center-panel .wrapper").empty();
    
    var indexCurrentDay;;
    $(days).each(function (index, day) {
        if ((new Date(day.DateString)).toDateString() == date.toDateString())
            indexCurrentDay = index;
    });

    var width = $(".center-panel").width();
    var daysDisplayedCount = Math.floor(width / minDayWidth);
    for (var i = indexCurrentDay; i < (indexCurrentDay + daysDisplayedCount); i++) {
        var day = days[i];
        var template = kendo.template($("#day-template").html());
        var html = template(day);
        $(".center-panel .wrapper").append(html);

        $(day.Tasks).each(function (index, task) {
            AppendNewTask(task, day.DateString);
        });
    }

    $(".task-list").droppable({
        accept: ".todo-list li, .task-list li",
        drop: function (event, ui) {
            var li = $(ui.draggable[0]);
            var datetimeString = $(event.target).closest(".day-col").data("date");
            var scheduledDateTime = new Date(datetimeString);
            var todo = li.data("todo");

            //var callback = {
            //    todo: todo,
            //    function: function () {
            //        AppendNewTask(this.todo, datetimeString);
            //    }
            //};
            ScheduleTodo(todo.ID, todo.Text, scheduledDateTime, /*callback*/);
            RefreshTaskContextMenu();
            //li.remove();
        }
    });

    $(".task-list li").draggable({
        revert: "invalid"
    });

    RefreshTaskContextMenu();
}

function RefreshTaskContextMenu() {
    $("#task-context-menu").remove();
    var template = kendo.template($("#context-menu-template").html());
    var templateHTML = template({});
    $("body").append(templateHTML);

    $("#task-context-menu").kendoContextMenu({
        target: ".task-list",
        filter: ".task",
        select: function (e) {
            var idTodo = $(e.target).data("idtodo");
            var datetimeString = $(e.target).closest(".day-col").data("date");
            var scheduledDateTime = new Date(datetimeString);
            var todo = $(".task[data-idtodo='" + idTodo + "'").closest("li").data("todo");

            var callback = {
                function: function () {
                    $(".task[data-idtodo='" + idTodo + "'").remove();
                }
            };

            var item = $(e.item);
            var event = item.data("event");
            if (item.hasClass("complete"))
                event(idTodo, todo.Text, scheduledDateTime, true, defaultGoogleTaskList, callback);
            else if (item.hasClass("incomplete"))
                event(idTodo, todo.Text, scheduledDateTime, false, defaultGoogleTaskList, callback);
            else if (item.hasClass("attempted"))
                event(idTodo, scheduledDateTime, true, defaultGoogleTaskList, callback);
            else if (item.hasClass("delete")) 
                event(idTodo, scheduledDateTime, null, defaultGoogleTaskList, callback);
            
        }
    });

    $("#task-context-menu .complete, #task-context-menu .incomplete").data("event", SetTaskCompletionStatus);
    $("#task-context-menu .incomplete").data("event", SetTaskCompletionStatus);
    $("#task-context-menu .attempted").data("event", SetTaskAttemptedStatus);
    $("#task-context-menu .delete").data("event", DeleteTask);
}

function PopulateGoalsAndTodos() {
    PopulateGoals(todoPlannerVM.Goals);
    PopulateUnplannedTodos(todoPlannerVM.UnplannerTodos);
    PopulateHeirarchyTodoList(todoPlannerVM.TodoHierarchy);

    $(".todo-list li").draggable({
        revert: "invalid"
    });

    $(".todo-list").droppable({
        accept: ".task-list li",
        drop: function (event, ui) {
            var li = $(ui.draggable[0]);
            var todo = li.data("todo");
            AppendTaskToTodo(this.task);
            
            li.remove();
        }
    });
}

function PopulateGoals(goals) {
    $("ul.goal-list").empty();

    $(goals).each(function (index, goal) {
        var li = $("<li>" + goal.Text + "</li>");
        li.data("goal", goal);
        $("ul.goal-list").append(li);
    });
}

function PopulateUnplannedTodos(todos) {
    $("ul.todo-list.unplanned").empty();

    $(todos).each(function (index, todo) {
        var li = $("<li>" + todo.Text + "</li>");
        li.data("todo", todo);
        $("ul.todo-list.unplanned").append(li);
    });
}

function PopulateHeirarchyTodoList(todos) {
    $("ul.todo-list.hierarchy").empty();

    $(todos).each(function (index, todo) {
        var template = kendo.template($("#hierarchical-todo-template").html());
        var li = template(todo);
        $("ul.todo-list.hierarchy[data-parent-id='0']").append(li);

        $("li[data-todo-id='" + todo.ID + "']").data("todo", todo);

        PopulateHeirarchyTodoChildren(todo);

        //$(todo.Children).each(function (index, childTodo) {
        //    PopulateHeirarchyTodo(childTodo.Children, ID)
        //});
    });
}

function PopulateHeirarchyTodoChildren(todoParent) {

    $(todoParent.Children).each(function (index, childTodo) {
        //var li = $("<li>" + todo.Text + "</li>");
        //li.data("todo", todo);
        var template = kendo.template($("#hierarchical-todo-template").html());
        var li = template(childTodo);
        //$(li).data("todo", childTodo);
        $("ul.todo-list.hierarchy[data-parent-id='" + todoParent.ID + "']").append(li);

        $("li[data-todo-id='" + childTodo.ID + "']").data("todo", childTodo);

        PopulateHeirarchyTodoChildren(childTodo);
    });

}

//function PopulateHeirarchyTodo(todos, parentID) {

//    $(todos).each(function (index, todo) {
//        //var li = $("<li>" + todo.Text + "</li>");
//        //li.data("todo", todo);
//        var template = kendo.template($("#hierarchical-todo-template").html());
//        var li = template(todo);
//        $("ul.todo-list.hierarchy[data-parent-id='" + parentID + "']").append(li);
//    });

//}

function AppendNewTask(todo, datetimeString) {

    var li = $("<li></li>");
    li.draggable({ revert: "invalid" });
    li.data("todo", todo);

    var template = kendo.template($("#task-template").html());
    var html = template(todo);

    li.append(html);
    $("ul.task-list[data-date='" + datetimeString + "']").append(li);
}

function AppendTaskToTodo(todo) {
    var li = $("<li>" + todo.Text + "</li>");
    li.draggable({ revert: "invalid" });
    li.data("todo", todo);
    $("ul.todo-list.unplanned").append(li);
}


/* UNIVERSAL
 * 
 * Keep a global view model
 * ** Every data action will check with the global view model first
 *  ** ... and untimately use it's data ...
 */