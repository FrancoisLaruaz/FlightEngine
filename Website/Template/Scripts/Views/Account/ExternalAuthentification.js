function externalAuthentificationCallback(success, returnUrl, error, media, isSignUp, imageSrc, language, Redirection, firstName, lastName, isAlreadyLoggedIn) {
    ResetErrors();


    if (!HasValue(Redirection))
    {
        Redirection = "";
    }

    if (typeof IsAlreadyLoggedIn != "undefined" && IsAlreadyLoggedIn!=null && IsAlreadyLoggedIn == 'true') {
        window.location.href = window.location.href;
    }
    else if (Redirection == Constants.ExternalAuthentificationRedirection.RedirectToEmailSignUp) {

        $("#SignUpChoice").fadeOut(500, function () {
            $("#SignUpFormDiv").fadeIn(500);
        });

        $('#RedirectToEmailSignUpError').html(error);
        setTimeout(function () { $("#FirstName").val(CorrectCharacters(firstName)); }, 500);
        setTimeout(function () { $("#LastName").val(CorrectCharacters(lastName)); }, 500);
        setTimeout(function () { SetSexyCSS(); }, 510);

    }
    else if (Redirection == Constants.ExternalAuthentificationRedirection.RedirectToExternalSignUp ) {
        $('#GeneralExternalSignUpError').html(error);
        $("#SignUpFormDiv").hide();
        $("#SignUpChoice").show();
        $("#SignUpLink").click();
    }
    else if (Redirection == Constants.ExternalAuthentificationRedirection.RedirectToLogin) {
        $('#GeneralExternalLogInError').html(error);
        $("#LoginNowLink").click();
    }
    else {
   
        if (isSignUp == 'true') {

            
            if (success == 'true') {
                hideAndShowGuidePg('loginOrSignInModalBody', 'SignUpWelcomePage');
            } else {
                if (error == null || error.trim() == "") {
                    error = "[[[An unexpected error occured. Please try again.]]]";
                }
                SetExternalSignUpForm(media);
                document.getElementById("SignUpV2Result_" + media).innerHTML = error;
                HideSpinner();
            }
        }
        else {
            ShowSpinner();

         
            if (success == 'true') {
                var URLRedirect = $('#URLRedirect').val();
          
                var CentralGoToUrl = $('#CentralGoToUrl').val();

                var toGo = '';
                if (URLRedirect != null && URLRedirect != "") {
                    toGo = URLRedirect;
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
            } else {

                SetExternalLogInForm(media);
                if (error == null || error.trim() == "") {
                    error = "[[[An unexpected error occured. Please try again.]]]";
                }
                $('#GeneralExternalLogInError').html(error);
                HideSpinner();
            }
        }
    }


}

function invokeExternalAuthentification(IdPopUp) {
   
    var chrome = 100;
    var width = 550;
    var height = 550;
    var left = (screen.width - width) / 2;
    var top = (screen.height - height - chrome) / 2;
    var options = "status=0,toolbar=0,location=1,resizable=1,scrollbars=1,left=" + left + ",top=" + top + ",width=" + width + ",height=" + height;
    var NewWindow = window.open("about:blank", IdPopUp, options);
    var timer = setInterval(function () {
        if (NewWindow.closed) {
            clearInterval(timer);
            externalAuthentificationWindowClose();
        }
    }, 500);

}


function externalAuthentificationWindowClose() {
    SetExternalSignUpBtns();
    SetExternalLogInBtns();
    HideSpinner();
}

function ExternalSignUpFormOnBegin(Media) {
    ShowSpinner();
}

function SetExternalSignUpForm(Media) {
    
}

function SetExternalSignUpBtns() {


}

function SetExternalLogInBtns() {



}

function ExternalLogInFormOnBegin(Media) {
    ShowSpinner();
}

function SetExternalLogInForm(Media) {

}

function ResetErrors() {
    $('.resultExternalAuthentification').html('');
    $('#ErrorLoginForm').html('');
    $('#ErrorSignUpForm').html('');
    $('#RedirectToEmailSignUpError').html('');
    $('#GeneralExternalSignUpError').html('');
    $('#GeneralExternalLogInError').html('');
}