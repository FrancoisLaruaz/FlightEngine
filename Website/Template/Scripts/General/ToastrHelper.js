
// Message à l'utilisateur lorsqu'une action est correctement réalisée
function notificationOK(message) {

    try {

        if (message === undefined || message == null || message.trim() == "") {
            message = Constants.SuccessMessages.Success;
        }

        SetUpToastrOK();

        toastr.success(message);
    }
    catch (err) {

    }
}

// Message à l'utilisateur lorsqu'une action n'est pas correctement réalisée
function notificationKO(message) {
    try {
        if (message === undefined || message == null || message.trim() == "") {
            message = Constants.ErrorMessages.UnknownError;
        }
        SetUpToastr();

        toastr.error(message);
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function notificationKOErrorDev(message) {
    try {
        SetUpToastrErrorDev();

        toastr.error(message);
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function notificationWarning(message) {
    try {
        SetUpToastr();

        toastr.warning(message);
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function notificationInfo(message) {
    try {
        SetUpToastr();

        toastr.info(message);
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function SetUpToastr() {
    try {
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "8000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function SetUpToastrErrorDev() {
    try {
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "30000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function SetUpToastrOK() {
    try {
        toastr.options = {
            "closeButton": false,
            "debug": false,
            "newestOnTop": false,
            "progressBar": false,
            "positionClass": "toast-top-right",
            "preventDuplicates": false,
            "onclick": null,
            "showDuration": "300",
            "hideDuration": "1000",
            "timeOut": "6000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
    }
    catch (err) {
        JSCaughtException(err);
    }
}



function CleanURL() {
    try {
        var oldUrl = window.location.href;
        var newURL = oldUrl.replace("TypeToastrMessage=" + getParameterByName("TypeToastrMessage"), "");
        var newURL = newURL.replace("&ToastrMessageText=" + getParameterByName("ToastrMessageText"), "");
        var tabUrl = oldUrl.split('?');
        if (tabUrl.length == 1 || tabUrl.length[1] == "" || tabUrl.length[1] == null) {
            newURL = newURL.replace("?", "");
        }
        tabUrl = oldUrl.split('&');
        if (tabUrl.length == 1 || tabUrl.length[tabUrl.length - 1] == "" || tabUrl.length[tabUrl.length - 1] == null) {
            newURL = newURL.replace("&", "");
        }
        history.pushState(null, null, newURL);
    }
    catch (err) {
        JSCaughtException(err);
    }
}

function ShowToastr(initToastrType, initToastrText) {
    try {
        if (initToastrType == "Success") {
            notificationOK(initToastrText);
        }
        else if (initToastrType == "Error") {
            notificationKO(initToastrText);
        }
        else if (initToastrType == "Warning") {
            notificationWarning(initToastrText);
        }
        else if (initToastrType == "Info") {
            notificationInfo(initToastrText);
        }
    }
    catch (err) {
        JSCaughtException(err);
    }

}

function getParameterByName(name, url) {
    try {
        if (!url) url = window.location.href;
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        var result = results[2];
    }
    catch (err) {
        JSCaughtException(err);
        return "";
    }
    return result;
}