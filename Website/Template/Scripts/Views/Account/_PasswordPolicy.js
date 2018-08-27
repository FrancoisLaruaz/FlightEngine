$(function () {
    SetPassword();
});

function SetPassword() {
    setTimeout(function () {
        $('.PasswordToCheck').each(function (index, value) {
            SetPasswordForm(this);
        });
    }, 1000);

    $('.PasswordToCheck').each(function (index, value) {
        SetPasswordForm(this);
        $(this).unbind("propertychange change keyup paste input keypress change keydown");
        $(this).on("propertychange change keyup paste input keypress change keydown", function (e) {
            //   e.preventDefault();
            SetPasswordForm(this);
        });
    });


}




function SetPasswordForm(passwordElement) {


    var disabledSubmitButton = false;
    var form = $(passwordElement).closest("form");
    //alert('SetPasswordForm : :'+$(form).attr('id'));
    var passWordButton = $(form).find(".PasswordButton");
    // alert('passWordButton : :' + $(passWordButton).attr('id'));
    $(passWordButton).show();

    if ($(passwordElement).length > 0 && $(form).length > 0) {
        var FormId = $(form).attr('id');

        var Password = $(passwordElement).val();
        var upperCase = new RegExp('[A-Z]');
        var lowerCase = new RegExp('[a-z]');
        var numbers = new RegExp('[0-9]');

        var PasswordStrengthScore = 0;

        if (Password == null || typeof Password == "undefined" || Password.trim().length < 8) {
            disabledSubmitButton = true;
            SetIconKO("EightCharacters", FormId);
        }
        else {
            SetIconOK("EightCharacters", FormId);
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(lowerCase)) {
            disabledSubmitButton = true;
            SetIconKO("LowerCaseLetter", FormId);
        }
        else {
            SetIconOK("LowerCaseLetter", FormId);
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(upperCase)) {
            disabledSubmitButton = true;
            SetIconKO("UpperCaseLetter", FormId);
        }
        else {
            SetIconOK("UpperCaseLetter", FormId);
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(numbers)) {
            disabledSubmitButton = true;
            SetIconKO("OneNumber", FormId);
        }
        else {
            SetIconOK("OneNumber", FormId);
        }


        if (disabledSubmitButton) {
            PasswordStrengthScore = 5;
        }
        else if (Password.trim().length >= 12 || ContainSpecialCharacter(Password)) {
            PasswordStrengthScore = 13;
        }
        else {
            PasswordStrengthScore = 11;
        }
        

        SetPasswordStrengthScore(PasswordStrengthScore, form);
        if (FormId !='SignUpForm')
        {
            $(passWordButton).toggleClass("disabled", disabledSubmitButton);
            if (!disabledSubmitButton) {
                $(passWordButton).removeAttr('disabled');
            }
        }
        else {
            var PasswordSetSignUpForm = !disabledSubmitButton;
            $("#HiddenPasswordSetSignUpForm").val(PasswordSetSignUpForm);
            SetCreateAccountBtn();
        }
    }

}

function SetPasswordStrengthScore(Score, form) {
    var color = "red";
    var text = "Very weak";
    if (Score >= 13) {
        var color = "green";
        var text = "Very strong";
    }
    else if (Score >= 11) {
        var color = "green";
        var text = "Strong";
    }
    else if (Score >= 9) {
        var color = "blue";
        var text = "Good";
    }
    else if (Score >= 6) {
        var color = "yellow";
        var text = "Intermediate";
    }
    else if (Score >= 3) {
        var color = "red";
        var text = "Weak";
    }

    $(form).find(".ScorePasswordStrength").html(text);
    $(form).find(".textPasswordStrength").removeClass("green").removeClass("blue").removeClass("yellow").removeClass("red");
    $(form).find(".textPasswordStrength").addClass(color);
}

function SetIconOK(Element, FormId) {

    var IconElement = $("#" + FormId + " .Icon" + Element);
    if (IconElement) {
        $(IconElement).removeClass("red").removeClass("glyphicon-remove").addClass("glyphicon-ok").addClass("green");
    }
    var TextElement = $("#" + FormId + " .text" + Element);
    if (TextElement) {
        $(TextElement).removeClass("red").addClass("green");
    }
}

function SetIconKO(Element, FormId) {
    var IconElement = $("#" + FormId + " .Icon" + Element);
    if (IconElement) {
        $(IconElement).removeClass("glyphicon-ok").removeClass("green").addClass("red").addClass("glyphicon-remove");
    }
    var TextElement = $("#" + FormId + " .text" + Element);
    if (TextElement) {
        $(TextElement).removeClass("green").addClass("red");
    }
}