var LoggedUserName = "";




$(document).ready(function () {
    DisplayBrowserBanner();

    var LoggedUserName = $("#HiddenLoggedUserName").val();

    SetSpinner();

    $(".BackToHomePageButton_js").unbind("click");
    $(".BackToHomePageButton_js").on("click", function (e) { e.preventDefault(); GoBackToHomePage(); });

    

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


    $("a.showSpinnerBeforeRedirect_js").on("click", function (e) {
        ShowSpinner();
    });
}
