$(document).ready(function () {


    HideSpinner();
});

function ChangePasswordFailure() {
    ErrorActions();
}

function ChangePasswordSuccess(data) {
    $("#ErrorChangePasswordForm").html('');

    if (data != null && data.Result) {

        window.location.href = GetHomePageUrl() + '/' + data.Langtag + '/PasswordChanged';
    }
    else if(data.Error!=null && data.Error.trim()!='')
    {
        $("#ErrorChangePasswordForm").html(data.Error);
    }
    else {
        ChangePasswordFailure();
    }
}

function handleChangePasswordBegin() {

}