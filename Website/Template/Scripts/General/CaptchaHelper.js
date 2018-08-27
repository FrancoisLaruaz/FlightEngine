$(document).ready(function () {
    $.getScript("https://www.google.com/recaptcha/api.js?hl=" + (typeof getLanguageWebsite == "function" ? getLanguageWebsite() : Constants.Const.DefaultCulture));
});