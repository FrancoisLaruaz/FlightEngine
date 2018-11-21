$(function () {
    var is_cookie_compliance_accepted = getCookie(Constants.JavascriptCookies.ComplianceAccepted);

    if (is_cookie_compliance_accepted != null && is_cookie_compliance_accepted != 'null' && is_cookie_compliance_accepted.toLowerCase().indexOf('true') > -1) {
        SetLiveChat();
    }
});

function SetLiveChat() {
    var Tawk_API = Tawk_API || {}, Tawk_LoadStart = new Date();
    (function () {
        var s1 = document.createElement("script"), s0 = document.getElementsByTagName("script")[0];
        s1.async = true;
        s1.src = 'https://embed.tawk.to/5aa2e3f94b401e45400d94fc/default';
        s1.charset = 'UTF-8';
        s1.setAttribute('crossorigin', '*');
        s0.parentNode.insertBefore(s1, s0);
    })();
}