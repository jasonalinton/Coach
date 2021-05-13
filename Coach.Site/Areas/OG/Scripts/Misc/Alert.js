function notifySuccess(message) {
    $.notify({
        message: message
    }, { // settings
        type: "success",
        allow_dismiss: true,
        delay: 3000
    }
    );
}

function notifyFailure(message) {
    $.notify({
        message: message
    }, { // settings
        type: "danger",
        allow_dismiss: true,
        delay: 3000
    }
    );
}

function alertModal(message) {
    var alertModal = createModal("#alert-modal", "Alert!", true, false, false, 300);
    var alertTemplate = kendo.template($("#alert-modal-template").html());
    alertTemplate = alertTemplate({ message: message });

    //$("#alert-message").val("<span>" + message + "</span>");

    alertModal.content(alertTemplate);
    alertModal.setOptions({
        open: function () {
            this.wrapper.css({ top: 20 });
        },
        close: function () {
            // Handle window close
        }
    });
    alertModal.center().open();
}

function confirmModal(message, onYes, yesParams = null, onNo = null, noParams = null, onClose = null, closeParams = null) {
    var confirmModal = createModal("#confirm-modal", "Are You Sure!", true, false, false, 300);
    var confirmTemplate = kendo.template($("#confirm-modal-template").html());
    confirmTemplate = confirmTemplate({ message: message });

    confirmModal.content(confirmTemplate);
    confirmModal.setOptions({
        open: function () {
            this.wrapper.css({ top: 20 });
        },
        close: function () {
            if (onClose) {
                onClose(closeParams);
            }
        }
    });
    confirmModal.center().open();

    $("#confirm-modal button.yes").click({ params: yesParams, modal: confirmModal }, onYes);
    if (onNo) {
        $("#confirm-modal button.no").on("click", onNo(noParams));
    } else {
        $("#confirm-modal button.no").on("click", function () {
            confirmModal.close();
        });
    }
}

function yesButton_Click() {
    kendoModalClose();
}


function kendoModalClose(e) {
    var name = $(e).data("windowname");
    var window = $("#" + name).data("kendoWindow");
    window.close();
}

function createModal(windowSelector, title, modal, visible, resizable, width, launch, content) {
    if (launch === undefined || null) {
        launch = false;
    }
    var window = $(windowSelector).kendoWindow({
        modal: modal,
        visible: visible,
        resizable: resizable,
        width: width,
        title: title,
        close: function (e) {
            $(this.element).empty();
        }
    }).data("kendoWindow");

    window.setOptions({
        open: function () {
            this.wrapper.css({ top: 20 });
        }
    });
    //converts title to html to include icons etc
    //Note if any changes are made to the window after this line is called, any html encoding on the title will be lost. 
    //This line will have to be called again to apply the encoding.
    window.element.prev().find(".k-window-title").html(title);
    if (launch) {
        window.content(content);
        window.center().open();
        return window;
    }
    else {
        return window;
    }
}

function validateAutoCompleteInput(value, data) {
    var isValid = false;
    //var data = $(input).data("kendoAutoComplete").dataSource.data();
    $.each(data, function (i, dataItem) {
        if (dataItem.toLowerCase() === value.toLowerCase()) {
            isValid = true;
        }
    });
    return isValid;
}