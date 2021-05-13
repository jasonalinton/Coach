function Framework() { };

Framework.invokeJsonWebService = function (url, data, callbackFunction, isAsync, dataType) {

    if (isAsync == null) {
        isAsync = (callbackFunction == null ? false : true);
    }

    return Framework.ajax(url, data, isAsync, callbackFunction, dataType, false);
};

Framework.invokeTraditionalJsonWebService = function (url, data, callbackFunction, isAsync, dataType) {

    if (isAsync == null) {
        isAsync = (callbackFunction == null ? false : true);
    }

    return Framework.ajax(url, data, isAsync, callbackFunction, dataType, true);
};

Framework.ajax = function (url, data, isAsync, callbackFunction, dataType, isTraditional) {
    var returnObject;

    var converterFunction = Framework.jQueryAjaxJsondDataConverter;

    jQuery.ajax({
        url: url,
        type: 'POST',
        async: isAsync,
        dataType: dataType,
        converters: converterFunction,
        data: data,
        traditional: isTraditional,
        success: function (result) {
            returnObject = result;
            if (callbackFunction != null)
                callbackFunction(returnObject);

            if (!isAsync)
                return returnObject;

        },
        error: function (xhr) {
            ajaxErrorHandler(url, xhr);
        }
    });

    return returnObject;
}

Framework.convertJQueryAjaxJsondData = function (responseBody) {
    return (responseBody.hasOwnProperty("d") ? responseBody.d : responseBody);
};

Framework.jQueryAjaxJsondDataConverter = { "json jsond": Framework.convertJQueryAjaxJsondData };


function ajaxErrorHandler(url, xhr) {
    //JL("jsLogger").fatal('AJAX Error on call to ' + url + ': ' + xhr.status + " " + xhr.statusText);
    //alert('Something went wrong. Details of this error have automatically been sent to the IM360 Administrator on your behalf.');
}