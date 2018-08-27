var placeSearch, autocomplete;
var componentForm = {
    street_number: 'long_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'long_name',
    country: 'long_name',
    postal_code: 'long_name',
    country: 'short_name'
};

var componentFullAddressForm = {
    street_number: 'long_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'long_name',
    country: 'long_name',
    postal_code: 'long_name'
};


var googleMapLoaded = false;
var FullAdressNeeded = false;

$(document).ready(function () {
    var gmUrl = "https://maps.googleapis.com/maps/api/js?key=" + Constants.APIKeys.GoogleMap + "&callback=initAutocomplete&language=" + (typeof getLanguageWebsite == "function" ? getLanguageWebsite() : Constants.Const.DefaultCulture);
    $.getScript(gmUrl, function () {
        $.getScript("/Scripts/General/InfoBox.js", function () {
            $.getScript("/Scripts/General/CustomGoogleMapMarker.js", function () {
                googleMapLoaded = true;
            });
        });
        $('.addressAutoComplete').each(function (index, value) {
            $(this).on("focus", function (e) {
                e.preventDefault();
                geolocate();
            });
        });
    });
});

function initAutocomplete() {
    setTimeout(function () {

        var _autocomplete = document.getElementsByClassName('addressAutoComplete');
        if (_autocomplete != null && typeof _autocomplete != "undefined" && _autocomplete.length > 0) {
            // Create the autocomplete object, restricting the search to geographical
            // location types.
            autocomplete = new google.maps.places.Autocomplete(
                (_autocomplete[0]),
                { types: ['geocode'] });

            // When the user selects an address from the dropdown, populate the address
            // fields in the form.

            if ($(".FullAdressAutoComplete").length > 0) {
                FullAdressNeeded = true;
            }

            autocomplete.addListener('place_changed', fillInAddress);

            $(".addressAutoComplete").attr("placeholder", "");


        }

    }, 700);
}

function removeValidationError(addressType) {
    if ($("." + addressType).length > 0) {
        var Element = $("." + addressType)[0];

        var ParentElement = $(Element).parent();

        var Validationerror = $(ParentElement).find(".field-validation-error");

        if ($(Validationerror) != null) {
            $(Validationerror).hide();
        }
    }


}

function fillInAddress() {
    // Get the place details from the autocomplete object.
    var place = autocomplete.getPlace();

    for (var component in componentForm) {
        $("." + component).val('');
    }


    if (place != null && place.address_components != "undefined" && place.address_components != null) {

        if (FullAdressNeeded) {
            var FieldAutoComplete = $(".addressAutoComplete")[0];
            if ($(FieldAutoComplete).length > 0) {
                var associativeArray = {};

                for (var i = 0; i < place.address_components.length; i++) {
                    var addressType = place.address_components[i].types[0];

                    if (componentFullAddressForm[addressType]) {

                        var val = place.address_components[i][componentFullAddressForm[addressType]];

                        if (typeof val == "undefined" || val == null) {
                            val = '';
                        }

                        if (addressType == 'postal_code') {
                            val = replaceAll(val, ' ', '');
                        }

                        associativeArray[addressType] = val;

                    }
                }
                var LongAddress = '';
                if (typeof associativeArray["street_number"] != "undefined") {
                    LongAddress = LongAddress + associativeArray["street_number"];
                }
                if (typeof associativeArray["route"] != "undefined") {
                    LongAddress = LongAddress + ' ' + associativeArray["route"];
                }
                if (typeof associativeArray["locality"] != "undefined") {
                    LongAddress = LongAddress + ', ' + associativeArray["locality"];
                }
                if (typeof associativeArray["administrative_area_level_1"] != "undefined") {
                    LongAddress = LongAddress + ', ' + associativeArray["administrative_area_level_1"];
                }
                if (typeof associativeArray["postal_code"] != "undefined") {
                    LongAddress = LongAddress + ' ' + associativeArray["postal_code"];
                }
                if (typeof associativeArray["country"] != "undefined") {
                    LongAddress = LongAddress + ', ' + associativeArray["country"];
                }

                if (LongAddress.indexOf(",") <= 1) {
                    LongAddress = LongAddress.substr(1);
                }
                if ($("#ShortLocation").length > 0) {
                    var City = associativeArray["locality"];
                    var Province = associativeArray["administrative_area_level_1"];
                    var newShortLocation = ''
                    if (typeof City != "undefined" && typeof Province != "undefined") {
                        newShortLocation = City + ', ' + Province;
                    }
                    else if (typeof City != "undefined" && typeof Province == "undefined") {
                        newShortLocation = City;
                    }
                    else if (typeof City == "undefined" && typeof Province != "undefined") {
                        newShortLocation = Province;
                    }


                    $("#ShortLocation").val(newShortLocation.trim());
                    setTimeout(function () {
                        $("#ShortLocation").val(newShortLocation.trim());
                    }, 500);
                }

                $(FieldAutoComplete).val(LongAddress.trim());
            }
        }
        else {


            // Get each component of the address from the place details
            // and fill the corresponding field on the form.
            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];

                if (componentForm[addressType] && $("." + addressType).length > 0) {

                    var val = place.address_components[i][componentForm[addressType]];

                    if (addressType == 'postal_code') {
                        val = replaceAll(val, ' ', '');
                    }


                    if (!FullAdressNeeded) {
                        $("." + addressType).val($("." + addressType).val() + ' ' + val);
                    }
                }
            }


            for (var i = 0; i < place.address_components.length; i++) {
                var addressType = place.address_components[i].types[0];

                if (componentForm[addressType] && $("." + addressType).length > 0) {
                    $("." + addressType).val($("." + addressType).val().trim());
                }
            }

            if ($("#Hiddencountry").length > 0) {
                var NewCountryAcronym = $("#Hiddencountry").val();
                var NewProvinceName = $("#Hiddenprovince").val();
                var postal_codeTbx = $(".postal_code")[0];
                var CountryId = -1;

                ShowSpinner();;
                setTimeout(function () {
                    HideSpinner();
                }, 2500);
                var url = '/Ajax/GetCountryByAcronym';
                $.post(url, { acronym: NewCountryAcronym }, function (data) {

                    if (data != null && parseInt(data) > 0 && $(".CountryDDL").length > 0) {
                        $(".ProvinceDDL").prop('disabled', false);
                        CountryId = parseInt(data);
                        var CountryDDL = $(".CountryDDL")[0];
                        var ProvinceDDL = $(".ProvinceDDL")[0];



                        if (CountryDDL != "undefined" && CountryDDL != null) {
                            $(".CountryDDL").val(data);
                            if (typeof (SetTrullioLabel) === "function") {
                                SetTrullioLabel();
                            }
                        }

                        loadProvinceGooglePlace(CountryId, ".ProvinceDDL");


                        if (NewProvinceName != null && NewProvinceName.trim() != '') {
                            var url = '/Ajax/GetProvinceByName';
                            $.post(url, { name: NewProvinceName, countryId: CountryId }, function (data) {
                                if (data != null && parseInt(data) > 0 && $(".ProvinceDDL").length > 0) {


                                    if (ProvinceDDL != "undefined" && ProvinceDDL != null) {
                                        $(".ProvinceDDL").val(data);
                                    }
                                }
                                else if ($(".ProvinceDDL").length > 0) {
                                    $(".ProvinceDDL").val('');
                                }
                                HideSpinner();
                            });
                        }
                        else {
                            HideSpinner();
                        }

                    }
                    else {
                        $(".ProvinceDDL").val('');
                        $(".CountryDDL").val('');
                        if (typeof (SetTrullioLabel) === "function") {
                            SetTrullioLabel();
                        }
                        $(".ProvinceDDL").prop('disabled', true);
                        HideSpinner();
                    }


                });

            }
        }
    }
    if (typeof (AdressFormOnChange) === "function") {
        AdressFormOnChange();
    }


    setTimeout(function () {
        var ValidationErrorsKO = $('.field-validation-error').length;

        if (ValidationErrorsKO > 0 && !FullAdressNeeded) {
            $('.addressValidateForm').validate().form();
        }
    }, 1100);

    if (typeof (SetSexyCSS) === "function") {
        SetSexyCSS();
    }

}


function loadProvinceGooglePlace(countryId, inputId) {
    var url = '/Ajax/GetProvinces';
    $.post(url, { id: countryId }, function (data) {
        var items = [];
        items.push("<option>--Select--</option>");
        $.each(data, function () {
            items.push("<option value=" + this.Id + ">" + this.Name + "</option>");
        });
        $(inputId).html(items.join(' '));
    });
}

// Bias the autocomplete object to the user's geographical location,
// as supplied by the browser's 'navigator.geolocation' object.
function geolocate() {


    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            if (autocomplete != null && typeof (autocomplete) != "undefined") {
                autocomplete.setBounds(circle.getBounds());
            }
        });
    }
}

function getDistance(lat1, lon1, lat2, lon2) {
    var R = 6371; // km
    var dLat = toRad(lat2 - lat1);
    var dLon = toRad(lon2 - lon1);
    var lat1 = toRad(lat1);
    var lat2 = toRad(lat2);

    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.sin(dLon / 2) * Math.sin(dLon / 2) * Math.cos(lat1) * Math.cos(lat2);
    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    var d = R * c;
    return d;
}

// Converts numeric degrees to radians
function toRad(Value) {
    return Value * Math.PI / 180;
}


function getDistanceToSearch(zoomLevel)
{
    var result = Constants.Const.DefaultGoogleMapSearchDistance;

    if (zoomLevel == 13) {
        result = 6;
    }
    else if (zoomLevel == 11) {
        result = 10;
    }
    else if (zoomLevel == 10) {
        result = 14;
    }
    else if (zoomLevel == 8) {
        result = 20;
    }
    else if (zoomLevel == 8) {
        result = 20;
    }
    else if (zoomLevel == 7) {
        result = 20;
    }
    else if (zoomLevel == 6) {
        result = 20;
    }
    else if (zoomLevel == 5) {
        result = 20;
    }
    else if (zoomLevel < 5) {
        result = 20;
    }

   return result;

}