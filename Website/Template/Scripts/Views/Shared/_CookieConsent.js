$(function () {

    // https://codepen.io/ytran/pen/xRdgjQ
    var is_cookie_compliance_accepted = getCookie(Constants.JavascriptCookies.ComplianceAccepted);
    if (is_cookie_compliance_accepted == null || is_cookie_compliance_accepted.toLowerCase()!='true') {
        $('.cookie-notification').show();
        $('body').on('click', '.btn-close-cookie-notification', function () {
            setCookie(Constants.JavascriptCookies.ComplianceAccepted, true, 3000);
            $(this).parent().fadeOut(250);
            SetLiveChat();
        });
    }
});