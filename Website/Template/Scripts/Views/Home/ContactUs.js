$(document).ready(function () {


    HideSpinner();
});

function ContactUsFailure() {
    ErrorActions();
}

function ContactUsSuccess(data) {
    $("#ErrorContactUsForm").html('');

    if (data != null && data.Result) {

        $("#DivFormContactUs").fadeOut(500, function () {
            $("#DivConfirmationContactUs").fadeIn(500);
        });
    }
    else if(data.Error!=null && data.Error.trim()!='')
    {
        $("#ErrorContactUsForm").html(data.Error);
    }
    else {
        ContactUsFailure();
    }
}

function handleContactUsBegin() {

}