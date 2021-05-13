
function shuffleArray(array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        var temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}

function htmlDecode(value) {
    return value.replace(/&lt;/g, "<").replace(/&gt;/g, ">");
}


/** Convert string to pascal */
String.prototype.toPascal = function () {
    return this
        .replace(new RegExp(/[-_]+/, 'g'), ' ')
        .replace(new RegExp(/[^\w\s]/, 'g'), '')
        .replace(
            new RegExp(/\s+(.)(\w+)/, 'g'),
            ($1, $2, $3) => `${$2.toUpperCase() + $3.toLowerCase()}`
        )
        .replace(new RegExp(/\s/, 'g'), '')
        .replace(new RegExp(/\w/), s => s.toUpperCase());
};

/** Convert string to camel */
String.prototype.toCamel = function () {
    return this
        .replace(/\s(.)/g, function ($1) { return $1.toUpperCase(); })
        .replace(/\s/g, '')
        .replace(/^(.)/, function ($1) { return $1.toLowerCase(); });
}

/** Convert string array to pascal */
Array.prototype.toPascal = function () {
    var newArray = [];

    $(this).each(function (index, item) {
        newArray.push(item.toPascal());
    });

    return newArray;
}

/** Concert string array to camel case */
Array.prototype.toCamel = function () {
    var newArray = [];

    $(this).each(function (index, item) {
        newArray.push(item.toPascal());
    });

    return newArray;
}



if (typeof String.prototype.beginsWithSpace === "undefined") {
    String.prototype.beginsWithSpace = function () {
        var patternBeginsWith = /^\s/g;
        return (patternBeginsWith.test(this));
    };
}

if (typeof String.prototype.endsWithSpace === "undefined") {
    String.prototype.endsWithSpace = function () {
        var patternEndsWith = /\s$/g;
        return (patternEndsWith.test(this));
    };
}

if (typeof String.prototype.beginsOrEndsWithSpace === "undefined") {
    String.prototype.beginsOrEndsWithSpace = function () {
        return (this.beginsWithSpace() || this.endsWithSpace());
    };
}


/* This will get new expiration time for ajax calls */
$(document).ajaxSend(function (event, request, settings) {
    //Get AntiForgeryToken to be added to all POST requests. This is akready available on all pages through a hidden form.
    var antiForgeryToken = $("input[name=__RequestVerificationToken]").val();

    //add featureUrl to all requests and AntiForgeryToken to all POST requests. 
    if (ObjectHasPropertyOfType(settings, "data", "object") && (settings.data instanceof FormData)) {
        settings.data.append("featureurl", $("#feature-path").val());
    }
    else if (settings.data === undefined) {
        settings.data = "featureurl=" + $("#feature-path").val();
    } else {
        if (settings.data === null || settings.data === "") {
            settings.data = "featureurl=" + $("#feature-path").val();
        }        
        else {
            settings.data = settings.data + "&featureurl=" + $("#feature-path").val();
        } 
    }

    //Add antiforgeryToken to requests
    if (settings.hasContent && antiForgeryToken) {   // handle all verbs with content
        var tokenParam = "__RequestVerificationToken=" + encodeURIComponent(antiForgeryToken);
        if (settings.type === "POST") {
            if (ObjectHasPropertyOfType(settings, "data", "object") && (settings.data instanceof FormData)) {
                settings.data.append("__RequestVerificationToken", encodeURIComponent(antiForgeryToken));
            }
            else {
                settings.data = settings.data ? [settings.data, tokenParam].join("&") : tokenParam;
            }
            // ensure Content-Type header is present!
            if (settings.contentType !== false || event.contentType) {
                request.setRequestHeader("Content-Type", settings.contentType);
            }
        }
    }
});

function ObjectHasPropertyOfType(testObject, testPropertyName, testPropertyType) {
    var returnValue = false;

    coreBlock: {
        if ((testObject === null) || (typeof (testObject) !== "object")) break coreBlock;
        if (typeof (testPropertyName) !== "string") break coreBlock;
        if (testPropertyName === "") break coreBlock;
        if (testPropertyName.beginsOrEndsWithSpace()) break coreBlock;
        if (testPropertyType === undefined) testPropertyType = "undefined";
        if (testPropertyType === null) testPropertyType = "null";
        if (typeof (testPropertyType) !== "string") break coreBlock;
        if (!Object.prototype.hasOwnProperty.call(testObject, testPropertyName)) break coreBlock;

        var compareValue = testObject[testPropertyName];
        var compareType;
        if (compareValue === null) {
            compareType = "null";
        }
        else {
            compareType = typeof (compareValue);
        }
        returnValue = (testPropertyType === compareType);
    }

    return returnValue;
}