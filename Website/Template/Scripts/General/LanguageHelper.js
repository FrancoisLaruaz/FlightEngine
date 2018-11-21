var LanguageCookie = 'LanguageCookie';
var EnglishSuffix = '/en/'
var FrenchSuffix = '/fr/'

$(document).ready(function () {
    ChangeURlWithLanguage();
    $('#LanguageSelector').unbind('change');
    $('#LanguageSelector').on('change', function (e) {
        e.preventDefault();
        var urlToGo = $(this).find(":checked").val();
        var Language = 'en';
        if (urlToGo.toLowerCase().indexOf('langtag=fr') > -1) {
            Language = 'fr';
        }
        setCookie(LanguageCookie, Language, 3000);
        window.location.href = urlToGo;
    });

}); //closing doc ready


function getLanguageWebsite() {
    var result = Constants.Const.DefaultCulture;
    var LanguageSelector = $('#LanguageSelector');
    if (LanguageSelector.length > 0) {
        var language = LanguageSelector.find(":selected").text();
        if (HasValue(language)) {
            result = language.substr(0, 2).toLowerCase();
        }
    }
    return result;
}

function ChangeURlWithLanguage() {

    var IsAuthenticated = null;
    if (typeof Constants != "undefined") {

        if (typeof Constants.User != "undefined") {

            IsAuthenticated = Constants.User.IsAuthenticated;
        }
    }

    var language = 'en';
    if (!IsAuthenticated) {
        language = getCookie(LanguageCookie);
        if (typeof language == "undefined" || language == null || language == 'null') {

            var language = navigator.language || navigator.browserLanguage; //for IE

            if (typeof language != "undefined" && language != null && language.indexOf("fr") > -1) {
                language = "fr";
            }
            else {
                language = "en";
            }
        }
    }
    else {
        language = getLanguageWebsite();
    }

    var currentUrl = window.location.href.toLowerCase();

    var suffix = EnglishSuffix;
    if (language == 'fr') {
        suffix = FrenchSuffix;
    }

    if (currentUrl.split(suffix.toLowerCase().slice(0, -1))[1] == '') {
        currentUrl = currentUrl + '/';
    }
    currentUrl = currentUrl.replace('//', '/');

    var host = '';
    if (isLocalhost()) {
        host = 'localhost:54808';
    }
    else if (isStaging()) {
        host = 'staging.zaural.com';
    }
    else {
        host = 'zaural.com';
    }

    if (currentUrl.indexOf('?logoff=true') > -1) {
        var newUrl = currentUrl.replace('?logoff=true', '').replace('//', '/').replace(FrenchSuffix.toLowerCase(), FrenchSuffix);
        history.pushState(null, null, newUrl.split(host)[1]);
    }
    else if ((currentUrl.indexOf(suffix.toLowerCase()) == -1 && language != 'en')) {
        currentUrl = currentUrl.replace(FrenchSuffix.toLowerCase(), '').replace(EnglishSuffix.toLowerCase(), '').replace('?logoff=true', '');

        var replaceHost = host + suffix;


        var newUrl = currentUrl.replace(host, replaceHost).replace('//', '/');

        if (newUrl.split(suffix)[1] == '/') {
            newUrl = newUrl.slice(0, -1);
        }
        var newUrlCut = newUrl.split(host)[1];

        if (getLanguageWebsite() == language) {
            var newUrlCut = newUrl.split(host)[1];
            history.pushState(null, null, newUrlCut);
        }
        else {
            window.location.href = newUrlCut;
        }
    }

}