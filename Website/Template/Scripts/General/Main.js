$(document).ready(function () {
    DisplayBrowserBanner();
    SetSpinner();


    $(document).on("click", ".BackToHomePageButton_js", function (e) {
        e.preventDefault();
        GoBackToHomePage();
    });
    

    //Reset the validation forms of the page
    $('form').each(function (index, value) {
        SetValidationForm($(this).attr('id'));
    });



    SetDateTimeFields();

    $(document).not(".noAjaxSpinner_js")
        .ajaxStart(function () {
            ShowSpinner();
        })
        .ajaxStop(function () {
            HideSpinner();
        });

    if ($("#loginOrSignInModal").length > 0) {
        if (typeof (SetLoginForm) === "function") {
            SetLoginForm();
        }
        if ($("#SignUpForm").length > 0) {
            if (typeof (SetSignUpForm) === "function") {
                SetSignUpForm();
            }
        }
    }

});


function SetSpinner() {
    $('.formSpinnerLoad').each(function (index, value) {
        $(this).submit(function () {
            if ($(this).valid()) {
                ShowSpinner();
            }
        });
    });

    $(document).on("click", "a.showSpinnerBeforeRedirect_js", function () {
        ShowSpinner();
    });

}
