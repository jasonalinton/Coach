var spreadsheet;

$(document).ready(function () {
    $("#clipboard").hide();
    spreadsheet = $("#spreadsheet").kendoSpreadsheet().data("kendoSpreadsheet");
});

function onSubmit() {
    var json = spreadsheet.toJSON();
    var columns = json.sheets[0].rows[0].cells;
    var sheetName = json.sheets[0].name;

    var jsonRecords = {};
    jsonRecords[sheetName] = [];

    for (var i = 1; i < json.sheets[0].rows.length; i++) {
        var record = {};
        for (var j = 0; j < columns.length - 1; j++) {
            var columnName = columns[j].value;
            var value = json.sheets[0].rows[i].cells[j].value;

            //var columnName = (j === columns.length - 1) ? columns[j].value.replace("\r", "") : columns[j].value;
            //var value;
            //if (j === columns.length - 1) {
            //    if (typeof value === 'string' || value instanceof String) {
            //        value = json.sheets[0].rows[i].cells[j].value.replace("\r", "");
            //    } else {
            //        value = json.sheets[0].rows[i].cells[j].value;
            //    }
            //} else {
            //    value = json.sheets[0].rows[i].cells[j].value;
            //}

            if ((typeof value === 'string' || value instanceof String) && value.toLowerCase() === "null")
                continue;
            else if (columnName.search("IDs") > -1) {
                if (typeof value === 'string' || value instanceof String) {
                    record[columnName] = [];
                    var stringArray = value.split(",");
                    $.each(stringArray, function (index, number) {
                        record[columnName].push(parseInt(number));
                    });
                }
                else if (typeof value === "number")
                    record[columnName] = [value];
            } else {
                record[columnName] = value;
            }
            var hi = 1;
        }

        jsonRecords[sheetName].push(record);
    }

    var jsonString = JSON.stringify(jsonRecords, null, 2);
    $("#json").text(jsonString);

    var clipboard = $("#clipboard");
    clipboard.val(jsonString);
    clipboard.show();
    clipboard.select();

    document.execCommand("copy");
    clipboard.hide();
}