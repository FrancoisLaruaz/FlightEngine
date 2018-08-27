var UserId = -1;
var CanUserEdit = false;
var currentViewid = "profile";
var SettingsTab = ["email", "password", "emailpreferences", "generalsettings"];
var ProfileTab = ["profile",  "settings"];






$(document).ready(function () {
    DisplayBrowserBanner();

    var Show = $("#TabToShow").val().toLowerCase();

    if (Show != null && Show != '') {
        currentViewid = Show;
    }
    $("#SettingsTabToGo").val("");
    if (SettingsTab.indexOf(currentViewid) > -1) {
        $("#SettingsTabToGo").val(currentViewid);
        currentViewid = "settings";
    }
    else if (ProfileTab.indexOf(currentViewid) <= -1) {
        currentViewid = "profile";
    }



    UserId = $("#HiddenUserId").val();
    CanUserEdit = $("#CanUserEdit").val() == "True" ? true : false;

    //BEGIN subnav stick to top 
    SetNavBar();
    SetTabOnClick();
}); //closing doc ready



function SetTabOnClick() {
    $("a.profiletabs").unbind("click");
    $("a.profiletabs").on("click", function (e) {
        e.preventDefault();
        var clickedViewid = $(this).data("viewid");
        if (clickedViewid != currentViewid) {
            getHtmlString(clickedViewid);
        }

    });
    $("#tab_" + currentViewid).click();
}

function getHtmlString(clickedViewid) {
    ShowSpinner();
    $.ajax({
        url: "/Profile/" + clickedViewid,
        type: "GET",
        data: { id: UserId },
        success: function (data) {
            if (data == null || (HasValue(Constants.PartialViewResults.UnknownError) && data == Constants.PartialViewResults.UnknownError)) {
                notificationKO(Constants.ErrorMessages.UnknownError);
            }
            else if (HasValue(Constants.PartialViewResults.NotAuthorized) && data == Constants.PartialViewResults.NotAuthorized) {
                notificationKO(Constants.ErrorMessages.NotAuthorized);
            }
            else {
                currentViewid = clickedViewid;

                $("#targetContainer").fadeOut(500, function () {
                    $("#targetContainer").html(data).fadeIn(500);
                });
                $("#MyProfileNavBar li").removeClass("active");
                $("#MyProfileNavBar li[data-viewid='" + clickedViewid + "']").addClass("active");
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}
function SetNavBar() {
    if (CanUserEdit) {
        $('.nav-2017').show();
        $('.nav-2017 li a').on('click', function (e) {
            e.preventDefault();
            $("body, html").animate({
                'scrollTop': 0
            }, 800);
        });

        var masterNavHeight = 0;
        var navToFix = $('.navbar.nav-2017');
        var gapNowToTop = navToFix.offset().top;
        var gapReserved = gapNowToTop - masterNavHeight;
        var wind = $(window);
        wind.bind('scroll', function () {
            if (wind.scrollTop() > gapReserved) {
                if (!navToFix.hasClass('fix-at-top')) {
                    navToFix.addClass('fix-at-top');
                }
            } else {
                if (navToFix.hasClass('fix-at-top')) {
                    navToFix.removeClass('fix-at-top');
                }
            }
        });
    }
}

