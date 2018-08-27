function getUTCNow() {
    var now = new Date();
    var utc_now = new Date(now.getUTCFullYear(), now.getUTCMonth(), now.getUTCDate(), now.getUTCHours(), now.getUTCMinutes(), now.getUTCSeconds(), now.getUTCMilliseconds());
    return utc_now;
}

function convertUTCDateToLocalDate(d) {
    var timeOffset = -((new Date()).getTimezoneOffset() / 60);
    d.setHours(d.getHours() + timeOffset);
    return d;
}


function addDays(date, days) {
    var result = new Date(date);
    result.setDate(result.getDate() + days);
    return result;
}


function formatDate(date) {

    var months = new Array();
    months[0] = "[[[Jan]]]";
    months[1] = "[[[Feb]]]";
    months[2] = "[[[Mar]]]";
    months[3] = "[[[Apr]]]";
    months[4] = "[[[May]]]";
    months[5] = "[[[Jun]]]";
    months[6] = "[[[Jul]]]";
    months[7] = "[[[Aug]]]";
    months[8] = "[[[Sep]]]";
    months[9] = "[[[Oct]]]";
    months[10] = "[[[Nov]]]";
    months[11] = "[[[Dec]]]";

    var year = date.getFullYear(),
        month = date.getMonth() + 1, // months are zero indexed
        day = date.getDate(),
        hour = date.getHours(),
        minute = date.getMinutes(),
        second = date.getSeconds(),
        hourFormatted = hour % 12 || 12, // hour returned in 24 hour format
        minuteFormatted = minute < 10 ? "0" + minute : minute,
        morning = hour < 12 ? " am" : " pm";

    if (day < 10)
        day = '0' + day;

    return day + " " + months[month - 1] + " " + year + " " + hourFormatted + ":" +
        minuteFormatted + morning;
}