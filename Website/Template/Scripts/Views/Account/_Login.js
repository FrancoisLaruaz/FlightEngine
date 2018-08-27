$(document).ready(function () {
    SetLoginForm();
    SetLoginFormLinks();
});

function SetLoginFormLinks() {
    $("#PasswordForgotLink").unbind("click");
    $("#PasswordForgotLink").on("click", function (e) {
        e.preventDefault();
        ShowPasswordForgotForm();
    });


    $("#loginOrSignInModalBody .SignUpLink").unbind("click");
    $("#loginOrSignInModalBody .SignUpLink").on("click", function (e) {
        e.preventDefault();
        ShowSignUpForm();
    });
}


function LoginFailure() {
    SetLoginSubmitForm();
    ErrorActions();
}

function handleLoginBegin() {
    $('#SubmitButtonLogin').val("[[[Logging In ...]]]");
    $("#SubmitButtonLogin").toggleClass("disabled", true);
}

function LoginSuccess(Data) {


    $('#ErrorLoginForm').html(Data.Error);

    if (typeof Data === "undefined" || Data.Result || Data.IsUserAlreadyLoggedIn || typeof Data.IsUserAlreadyLoggedIn === "undefined") {
        if ($('#loginOrSignInModal').length > 0) {
            $('#loginOrSignInModal').click();
        }
        var language = Data.LangTag;
        var CentralGoToUrl = $('#CentralGoToUrl').val();

        var toGo = '';
        if (Data.URLRedirect != null && Data.URLRedirect != "") {
            toGo = Data.URLRedirect;
        }
        else if (CentralGoToUrl != null && CentralGoToUrl != "") {
            toGo = CentralGoToUrl;
        }

        if (toGo.length > 0 && toGo.trim() != "/") {


            if (toGo.indexOf("http") == -1 && toGo.indexOf("www.") == -1) {

                if (language != null && language != "") {
                    toGo = "/" + language + toGo;
                }
                var Base = GetHomePageUrl();

                toGo = Base + toGo;
            }

            window.location.href = toGo;
        } else {
            location.reload();
        }

    }
    else {
        SetLoginSubmitForm();
    }

}



function SetLoginForm() {
    if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
        // SetEnterKey('SubmitButtonLogin');
        SetLoginSubmitForm();
    }

}

function SetLoginSubmitForm() {

    $('#SubmitButtonLogin').show();
    $('#SubmitButtonLogin').val("[[[Log In]]]");
    $('#SubmitButtonLogin').removeAttr('disabled');
    $("#SubmitButtonLogin").toggleClass("disabled", false);
    HideSpinner();
}