var imgArray = [],
    curIndex = 0;
imgDuration = 10000,
imgTransition = 650;

$(document).ready(function () {

    SetSliderImages();

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

    if ($("#LogOff").prop("checked")) {
        setCookie('LanguageCookie', null, 3000);
    }

    SetNavBar();
    setTimeout(slideShow, imgDuration);
    HideSpinner();


});


function SetSliderImages() {
    var SliderHomePageJson = $("#SliderHomePageJson").val();
    if (SliderHomePageJson != null) {
        imgArray = JSON.parse(SliderHomePageJson);
    }
}

function slideShow() {
    var background_homepage = $('#background_homepage');
    background_homepage.addClass("fadeOut");
    setTimeout(function () {
        var newImage = imgArray[curIndex].replace('~','');
        background_homepage.css('background-image', 'url(' + newImage + ')');
        background_homepage.removeClass("fadeOut");
    }, imgTransition);
    curIndex++;
    if (curIndex == imgArray.length) { curIndex = 0; }
    setTimeout(slideShow, imgDuration);
}

function SetNavBar() {
    var wind = $(window);
    var navBar = $("#homeNavDefault");

    wind.bind('scroll', function () {
        var scrolHeight = wind.scrollTop();
        if (scrolHeight > 480) {
            $(navBar).removeClass("navBarBackgroundColor").removeClass("navBarBackgroundColorTransparent").addClass("navBarBackgroundColorWhite").removeClass("navBarBottomBorder").removeClass("navBarBottomBorderNone").addClass("navBarBottomBorderGrey");
        }
        else {
            $(navBar).removeClass("navBarBackgroundColor").removeClass("navBarBackgroundColorWhite").addClass("navBarBackgroundColorTransparent").removeClass("navBarBottomBorder").removeClass("navBarBottomBorderGrey").addClass("navBarBottomBorderNone");
        }
    });
}