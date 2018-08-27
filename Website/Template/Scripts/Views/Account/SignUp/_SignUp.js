$(document).ready(function () {
    SetSignUpForm();
    SetSignUpFormLinks();
});



function SignUpFailure() {
    SetSignUpSubmitForm();
    ErrorActions();
}

function handleSignUpBegin() {
    $('#SubmitButtonSignUp').val("[[[Creation ...]]]");
    $("#SubmitButtonSignUp").toggleClass("disabled", true);
}

function SignUpSuccess(Data) {
    $('#ErrorSignUpForm').html(Data.Error);
    if (Data.Result) {
        hideAndShowGuidePg('loginOrSignInModalBody', 'SignUpWelcomePage');
    }
    else if (typeof Data === "undefined" || Data.IsUserAlreadyLoggedIn || typeof Data.IsUserAlreadyLoggedIn === "undefined") {
        window.location.href = window.location.href;
    }
    else {
        $("#SubmitButtonSignUp").toggleClass("disabled", true);
        grecaptcha.reset();
        SetSignUpSubmitForm();
    }
}

function SetSignUpForm() {



    $("#SignUpWithEmail").unbind("click");
    $("#SignUpWithEmail").on("click", function (e) {
        e.preventDefault();
        $("#SignUpChoice").fadeOut(500, function () {
            $("#SignUpFormDiv").fadeIn(500);
        });
    });


    $("#GoBackToSignUpChoiceBtn").unbind("click");
    $("#GoBackToSignUpChoiceBtn").on("click", function (e) {
        e.preventDefault();
        $("#SignUpFormDiv").fadeOut(500, function () {
            $("#SignUpChoice").fadeIn(500);
        });
    });

    SetSignUpSubmitForm();

}

function SetSignUpSubmitForm() {
    SetPassword();
    $('#SubmitButtonSignUp').show();
    $('#SubmitButtonSignUp').val("[[[Create]]]");
    $('#SubmitButtonSignUp').removeAttr('disabled');
    $("#SubmitButtonSignUp").toggleClass("disabled", false);

}


function SetSignUpFormLinks() {
    $(".LoginNowLink").unbind("click");
    $(".LoginNowLink").on("click", function (e) {
        e.preventDefault();
        ShowLogInForm(true);
    });
}

var correctCaptcha = function (response) {
    if (HasValue(response)) {
        $("#SubmitButtonSignUp").toggleClass("disabled", false);
        $("#SubmitButtonSignUp").removeAttr('disabled');
        $("#HiddenCaptchaSetSignUpForm").val(true);
    }
    SetCreateAccountBtn();
};

function SetCreateAccountBtn() {

    var disabledSubmitButton = true;
    if ($("#HiddenCaptchaSetSignUpForm").val().toLowerCase() == "true" && $("#HiddenPasswordSetSignUpForm").val().toLowerCase() == "true") {
        disabledSubmitButton = false;
    }


    $("#SubmitButtonSignUp").toggleClass("disabled", disabledSubmitButton);
    if (!disabledSubmitButton) {
        $("#SubmitButtonSignUp").removeAttr('disabled');
    }
}