
window.onerror = function (_errorMsg, _url, _lineNumber, _col, _error) {
    LogJsError(_errorMsg, _url, _lineNumber, _col, _error, false);
};



function LogJsError(_errorMsg, _url, _lineNumber, _col, _error, _custom) {
    try {

        var strError = "";
        var _browser = GetFullBrowserName();
        if (typeof _error == "undefined" || _error == null) {
            strError = "";
        }
        else {
            strError = _error.toString();
        }

        if (typeof _custom == "undefined" || _custom == null) {
            _custom = true;
        }

        if (typeof _url == "undefined" || _url == null || _url == '') {
            _url = window.location.href;
        }

        $.ajax({
            url: '/Error/LogJavascriptError',
            type: 'POST',
            data: { errorMsg: _errorMsg, url: _url, lineNumber: _lineNumber, col: _col, error: strError, browser: _browser, custom: _custom },
            success: function (data) {
                HideSpinner(); 
                if (IsTestEnvironment()) {

                    if (typeof notificationKOErrorDev !== "undefined") {
                        notificationKOErrorDev("[ERROR] You have just generated a js error : <br><br> " + _errorMsg + "<br><br> Please check the full <a style='text-decoration: underline;' onclick='OpenLogPage()'>LOG </a> ");
                    }
                    else {
                        alert("[ERROR] You have just generated a js error : <br><br> " + _errorMsg + "<br><br> Please check the full <a style='text-decoration: underline;' onclick='OpenLogPage()'>LOG </a> ");
                    }
                }
            },
            error: function (xhr, status, error) {
                HideSpinner(); 
            }
        });
    }
    catch (err) {
        HideSpinner(); 
        JSCaughtException(err);
    }
}

function IsTestEnvironment()
{
    var result = false;
    if (location.hostname == '127.0.0.1' || location.hostname == 'localhost') {
        result = true;
    }
    return result;
}

function JSCaughtException(err)
{
    
    if (IsTestEnvironment())
    {
        var message = "";
        if (err != null)
        {
            message = err.toString();
        }

        notificationKO(message);
    }
}

function OpenLogPage()
{
    var baseUrl = GetHomePageUrl();

    var url = baseUrl + '/Admin/Logs';
    var win = window.open(url, '_blank');
    win.focus();
}

