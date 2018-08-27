$(document).ready(function () {
    SetSexyCSS();
    SetValidationForm('MyProfileAddressForm');
    SetCountryOnChange();
    HideSpinner();
});

function MyProfileAddressFailure() {
    ErrorActions();
}

function MyProfileAddressSuccess(data) {
    $("#ErrorMyProfileAddressForm").html('');

    if (data != null && data.Result) {

        notificationOK('[[[Your address has been successfully saved.]]]');
    }
    else if (data.Error != null && data.Error.trim() != '') {
        $("#ErrorMyProfileAddressForm").html(data.Error);
    }
    else {
        MyProfileEditFailure();
    }
    BackToTop();
}

function handleMyProfileAddressBegin() {

}

function SetCountryOnChange() {
    CountryOnChange();
    $("#CountryId").unbind("change");
    $("#CountryId").on("change", function (e) {
        e.preventDefault();
        CountryOnChange();
    });
}

function CountryOnChange() {
    var CountryId = $("#CountryId").val();

    var url = "/Country/GetProvinces";
    if (CountryId != null && CountryId != '' && CountryId > 0) {
        $.post(url, { CountryId: CountryId }, function (data) {
            var ProvinceId = $("#ProvinceId").val();
            var items = [];
            items.push("<option value='0'>--- [[[Select]]] ---</option>");
            $.each(data, function () {
                if (ProvinceId != null && ProvinceId!='' && this.Id == parseInt(ProvinceId)) {
                    items.push("<option selected value=" + this.Id + ">" + this.Name + "</option>");
                }
                else {
                    items.push("<option value=" + this.Id + ">" + this.Name + "</option>");
                }
            });
            $("#ProvinceId").html(items.join(' '));
        });
        $("#ProvinceId").toggleClass("disabled", false);
    }
    else {
        var items = [];
        items.push("<option value='0'>--- [[[Select]]] ---</option>");
        $("#ProvinceId").html(items.join(' '));
        $("#ProvinceId").toggleClass("disabled", true);
    }
}