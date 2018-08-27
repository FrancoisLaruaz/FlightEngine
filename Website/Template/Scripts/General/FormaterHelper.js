Number.prototype.format = function (n, x) {
    var re = '\\d(?=(\\d{' + (x || 3) + '})+' + (n > 0 ? '\\.' : '$') + ')';
    return this.toFixed(Math.max(0, ~~n)).replace(new RegExp(re, 'g'), '$&,');
};


function GetEnglishAmount(number) {
    return number.format(0);
}

function GetFrenchAmount(number) {
    return replaceAll(GetEnglishAmount(number), ',', ' ');
}


function GetAmountFormat(number) {
    if (getLanguageWebsite() == 'fr') {
        return replaceAll(GetEnglishAmount(number), ',', ' ');
    }
    else {
        return GetEnglishAmount(number);
    }
}

function GetAmountFormatWithCurrency(number) {
    if (getLanguageWebsite() != 'en') {
        return replaceAll(GetEnglishAmount(number), ',', ' ') + ' $';
    }
    else {
        return '$' + GetEnglishAmount(number);
    }
}


function GetIntegerFormat(number) {
    if (getLanguageWebsite() != 'en') {
        var integer = GetEnglishInteger(number);
        return replaceAll(integer, ',', ' ');
    }
    else {
        return GetEnglishInteger(number);
    }
}

function GetEnglishInteger(number) {
    return number.format(0);
}
