$(document).ready(function () {


    HideSpinner();
});

function ResetPasswordFailure() {
    ErrorActions();
}

function ResetPasswordSuccess(data) {
    $("#ErrorResetPasswordForm").html('');

    if (data != null && data.Result) {

        window.location.href = GetHomePageUrl() + '/' + data.Langtag + '/PasswordChanged';
    }
    else if(data.Error!=null && data.Error.trim()!='')
    {
        $("#ErrorResetPasswordForm").html(data.Error);
    }
    else {
        ResetPasswordFailure();
    }
}

function handleResetPasswordBegin() {

}