$(document).ready(function () {




    $('#LogInHeader').on('click', function (e) {
        e.preventDefault();
        $("#loginOrSignInModalBody #SignUpForm").hide();
        ShowLogInForm(true);
    });

    $('#SignUpHeader').on('click', function (e) {
        e.preventDefault();
        ShowSignUpFormNow(true);
    });

    $("#LinkLogOff").unbind("click");
    $('#LinkLogOff').on('click', function (e) {
        e.preventDefault();
        ShowSpinner();
        document.getElementById('logoutForm').submit();
    });

    $('ul.nav li.dropdown').addClass('open');
    $('ul.nav li.dropdown').hover(function (e) {
        // $(this).find('.dropdown-menu').first().stop(true, true).fadeToggle(500); 
        if ($(window).width() > 1070) {
            $('ul.nav li.dropdown').addClass('open');

            $(this).addClass('li_hover');
        }
    }, function (e) {
        if ($(window).width() > 1070) {
            $(this).removeClass('li_hover');
        }
    });

    $('li.dropdown :first-child').on('click', function () {
        var $el = $(this).parent();
        if ($el.hasClass('open')) {
            var $a = $el.children('a.dropdown-toggle');
            if ($a.length && $a.attr('href')) {
                location.href = $a.attr('href');
            }
        }
    });
});

function showGuidePg(pgId) {
    var visibleGuide = $('#loginOrSignInModalBody');
    $("#loginOrSignInModal").modal('show');
    var contentHtml = $('#' + pgId).html();
    visibleGuide.fadeOut(500, function () {
        visibleGuide.html(contentHtml).fadeIn()
    });
}

function hideAndShowGuidePg(toHideId, toShowId, mode) {

    if (mode == null || typeof mode == "undefined") {
        mode = "slow";
    }

    $('#' + toHideId).fadeOut(mode, function () {
        $('#' + toShowId).fadeIn();
    });
}

function LogOffBegin() {
    ShowSpinner();
}

function LogOffSuccess() {

    NotificationOK("[[[See you soon !]]]");
    HideSpinner();
    window.location.href = GetHomePageUrl();
}


function LogOffFailure() {
    ErrorActions();
    window.location.href = GetHomePageUrl();
}

function ShowPasswordForgotForm() {

    if ($("#loginOrSignInModalBody").length > 0) {

        $("#DivConfirmationForgotPassword").hide();
        $("#DivFormForgotPassword").show();

        $(".SignUpProcessPages_js").hide();
        $("#loginOrSignInModalBody #SignUpForm").hide();
        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #LoginForm").fadeOut(500, function () {
            $("#loginOrSignInModalBody #ForgotPassword").fadeIn(500);
        });
    }
}

function ShowSignUpForm() {
    if ($("#loginOrSignInModalBody").length > 0) {
        recordGoToUrl(null);

        $(".SignUpProcessPages_js").hide();
        $("#loginOrSignInModalBody #ForgotPassword").hide();
        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #LoginForm").fadeOut(500, function () {
            $("#loginOrSignInModalBody #SignUpForm").fadeIn(500);

            if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
                $("#div_SignUpFormLinks").show();
                //   setTimeout(function () { SetPasswordForm(); }, 1000);
                //  SetPasswordForm();
            }
            else {
                $("#div_SignUpFormLinks").hide();
            }

        });
    }
}

function ShowSignUpFormNow(RecordUrl) {
    if ($("#loginOrSignInModalBody").length > 0) {
        $(".SignUpProcessPages_js").hide();
        if (RecordUrl)
            recordGoToUrl(null);
        $("#loginOrSignInModalBody #ForgotPassword").hide();
        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #LoginForm").hide();
        $("#loginOrSignInModalBody #SignUpForm").show();
        if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
            $("#div_SignUpFormLinks").show();
        }
        else {
            $("#div_SignUpFormLinks").hide();
        }
    }
}




function ShowLogInForm(RecordUrl) {
    if ($("#loginOrSignInModalBody #SignUpForm").length > 0) {
        if (RecordUrl)
            recordGoToUrl(null);
        if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
            $(".SignUpProcessPages_js").hide();
            $("#loginOrSignInModalBody #ForgotPassword").hide();
            $("#loginOrSignInModal").modal('show');
            $("#loginOrSignInModalBody #SignUpForm").fadeOut(500, function () {
                $("#loginOrSignInModalBody #LoginForm").fadeIn(500);
            });
        }
        else {
            $("#loginOrSignInModal").modal('hide');
        }
    }
}

function RefreshHeader() {

    $.ajax({
        url: "/Home/RefreshHeader",
        success: function (data) {
            if (data == null) {
                ErrorActions();
            }
            else {
                $("#divHeader").html(data);
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function recordGoToUrl(shouldGoTo) {

    if (shouldGoTo == null) {
        $('#CentralGoToUrl').val(window.location.href); // optional param not passed in, so use current page url
    } else {
        $('#CentralGoToUrl').val(shouldGoTo);
    }
}


function ShowSecretlyLogInForm() {

    if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
        $("#loginOrSignInModalBody #ForgotPassword").hide();
        $("#loginOrSignInModalBody #SignUpForm").hide();
        $("#loginOrSignInModalBody #LoginForm").show();
    }
}