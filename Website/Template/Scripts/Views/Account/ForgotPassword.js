$(document).ready(function () {

    $("#GoBackToLogInBtn").unbind("click");
    $("#GoBackToLogInBtn").on("click", function (e) {
        e.preventDefault();
        $("#ForgotPassword").fadeOut(500, function () {
            $("#LoginForm").fadeIn(500);
        });
    });

    HideSpinner();
});

function ForgotPasswordFailure() {
    $("#ErrorForgotPasswordForm").html('');
    ErrorActions();
}

function ForgotPasswordSuccess(data) {
    $("#ErrorForgotPasswordForm").html('');

    if (data != null && data.Result) {
        $("#divMailSentoTo").html('[[[An email has been set to ]]]' + data.UserMail+'.');
        $("#DivFormForgotPassword").fadeOut(500, function () {
            $("#DivConfirmationForgotPassword").fadeIn(500);
        });
    }
    else if(data.Error!=null && data.Error.trim()!='')
    {
        $("#ErrorForgotPasswordForm").html(data.Error);
    }
    else {
        ForgotPasswordFailure();
    }
}

function handleForgotPasswordBegin() {

}