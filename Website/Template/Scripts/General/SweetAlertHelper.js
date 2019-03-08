$(document).ready(function () {
    $(window).keydown(function (event) {
        if (event.keyCode == 27) {
            swal.close();
            return false;
        }
    });
}); //closing doc ready




function sweetConfirmation(text, title, ConfirmFunction, ConfirmFunctionParams , RejectFunction , RejectFunctionParams) {

    if (ConfirmFunctionParams === undefined) {
        ConfirmFunctionParams = null;
    }
    if (RejectFunction === undefined) {
        RejectFunction = null;
    }
    if (RejectFunctionParams === undefined) {
        RejectFunctionParams = null;
    }

    if (text == null)
        text = "[[[Are you sure you want to perform this action ?]]]";
    if (title == null)
        title = "[[[Confirmation needed]]]";

    swal({
        title: title,
        text: text,
        type: "warning",
        showCancelButton: true,
        allowEscapeKey: false,
        confirmButtonColor: Constants.Const.ColorWebsite,
        confirmButtonText: "[[[Yes]]]",
        cancelButtonText: "[[[No]]]",
        closeOnConfirm: true,
        closeOnCancel: true
    },
        function (isReject) {



            if (isReject)
            {
                if (typeof (ConfirmFunction) === "function") {
                    ConfirmFunction.apply(this, ConfirmFunctionParams);
                }
                else {
                    notificationKO("Invalid confirmation function");
                }
            }
            else if (RejectFunction!=null)
            {
                if (typeof (RejectFunction) === "function") {
                    RejectFunction.apply(this, RejectFunctionParams);
                }
                else {
                    notificationKO("Invalid reject function");
                }
            }
        });
}