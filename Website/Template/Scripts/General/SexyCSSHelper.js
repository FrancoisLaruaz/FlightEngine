$(document).ready(function () {
    SetSexyCSS();
});

function SetSexyCSS() {
    DeleteSexyCSSElements();
    SetStyledCheckboxes();
    SetSexyLabelsForTB();
    SetSexyTB();
    SetSexySelect();
    SetToggleMessages();
}

function SetToggleMessages() {
    $('.switch').each(function (index, value) {
        var spanMessage = $(this).find('.sliderMessage');
        displayToggleMessage(spanMessage);
    });
}


function ToggleCheckbox(switchElement) {

    var spanMessage = $(switchElement).find('.sliderMessage');
    var checkBox = $(spanMessage).parent().parent().find('.toggleSwitch');
    if ($(checkBox).length > 0) {
        if (!$(checkBox).is(':checked')) {
            $(spanMessage).addClass('sliderON').removeClass('sliderOFF');
            $(spanMessage).html('ON');
        }
        else {
            $(spanMessage).addClass('sliderOFF').removeClass('sliderON');
            $(spanMessage).html('OFF');
        }
    }
}
function displayToggleMessage(spanMessage) {
    var checkBox = $(spanMessage).parent().parent().find('.toggleSwitch');

    if ($(checkBox).length > 0) {
        if ($(checkBox).is(':checked')) {
            $(spanMessage).addClass('sliderON').removeClass('sliderOFF');
            $(spanMessage).html('ON');
        }
        else {
            $(spanMessage).addClass('sliderOFF').removeClass('sliderON');
            $(spanMessage).html('OFF');
        }
    }
}

function DeleteSexyCSSElements() {
    $(".highlightSexyCSS").remove();
}


function GetToggleValue(name) {
    var result = false;
    var element = $("#" + name);
    if ($(element).length > 0) {
        result=$(element).find(".sliderMessage").html() == "ON" ? true : false;
    }

    return result;
}

function SetSexySelect() {
    $('.select-text').each(function (index, value) {
        var FieldToAdd = $(this).parent();
        $(FieldToAdd).append('<span class="select-highlight highlightSexyCSS"></span><span class="select-bar highlightSexyCSS"></span >');
    });
}

function SetSexyTB() {

    $('.sexyTB').each(function (index, value) {

        var parentElement = document.querySelector("#" + $(this).attr('id')).parentNode;
        parentElement.children[1].insertAdjacentHTML("afterEnd", '<span class="bar highlightSexyCSS"></span><span class="highlight highlightSexyCSS"></span >');
    });


}


function SetSexyLabelsForTB() {
    $('.sexyTB').each(function (index, value) {
        var Id = $(this).attr('id');
        SetSexyTbLabel(Id);

        $(this).blur(function (element, value) {
            $('.sexyTB').each(function (index, value) {
                var Id = $(this).attr('id');
                SetSexyTbLabel(Id);
            });
        });

        $(this).change(function (element, value) {
            $('.sexyTB').each(function (index, value) {
                var Id = $(this).attr('id');
                SetSexyTbLabel(Id);
            });
        });

    });
}

function SetStyledCheckboxes() {
    $('.styled-checkbox').each(function (index, value) {
        var name = $(this).attr('name');
        var HiddenInput = $('input:not(.styled-checkbox)[name=' + name + ']');


        if (HiddenInput != "undefined" && HiddenInput != null) {
            $(HiddenInput).remove();
            var parentElement = document.querySelector("#" + $(this).attr('id')).parentNode;
            parentElement.children[1].insertAdjacentHTML("afterEnd", ' <input name="' + name + '" value="false" type="hidden">');
        }
    });
}


function SetSexyTbLabel(Id) {
    var element = $("#" + Id);
    tmpval = $(element).val();

    if (tmpval == '' || tmpval == null) {
        $(element).addClass('empty');
        $(element).removeClass('not-empty');
    } else {
        $(element).addClass('not-empty');
        $(element).removeClass('empty');
    }
}
