$(document).ready(function () {

    $("#StartNowHomePage").unbind("click");
    $("#StartNowHomePage").on("click", function (e) {
        e.preventDefault();
        ShowSignUpFormNow(true);
    });

    if ($("#PromptLogin").prop("checked")) {
        recordGoToUrl($("#hidden_RedirectTo").val());
        ShowLogInForm(false);
    }
    else if ($("#SignUp").prop("checked"))
    {
        ShowSignUpFormNow(false);
    }

    HideSpinner();


});


