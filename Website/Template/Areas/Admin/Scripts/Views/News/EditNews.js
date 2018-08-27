$(document).ready(function () {
    SetEditNewsForm();
});


function SetEditNewsForm()
{
    TypeOnChange();
    $("#TypeId").unbind("change");
    $("#TypeId").on("change", function (e) { TypeOnChange(); });

    $("#MailPreview").unbind("click");
    $("#MailPreview").on("click", function (e) { PreviewMailOnClick(); });

    $("#MailSubject").addClass("RequireSubject");
    $.validator.addMethod("RequireSubject",
        function (value, element, options) {
            return !(($("#MailSubject").val().trim().length == 0) && IsMailNeeded())
        }, "[[[Mail Subject is a required field.]]]");
    $.validator.classRuleSettings.RequireSubject = { RequireSubject: true };


    $("#TypeUserMailingId").addClass("RequireTypeUserMailing");
    $.validator.addMethod("RequireTypeUserMailing",
        function (value, element, options) {
            return !(($("#TypeUserMailingId").val() == 0 || $("#TypeUserMailingId").val() == '') && IsMailNeeded())
        }, "[[[Type User Mailing is a required field.]]]");
    $.validator.classRuleSettings.RequireTypeUserMailing = { RequireTypeUserMailing: true };

    SetEnterKey("SubmitNews");
    SetGenericAjaxForm('EditNewsForm', EditNewsSuccess, EditNewsFailure, null);
}

function EditNewsFailure() {
    ErrorActions();
}

function EditNewsSuccess(data) {



    if (data != null && data.Result) {
        if (data.IsCreation) {
            window.location.href = GetHomePageUrl() + '/Admin/News';
        }
        else {
            notificationOK('[[[The news has been successfully saved.]]]');
            BackToTop();
            HideSpinner();
        }
    }
    else {
        ErrorActions();
    }
}


function  PreviewMailOnClick() {

    ShowSpinner();

    var _Title = $("#NewsTitle").val();
    var _Description = $("#NewsDescription").val();

    $.ajax({
        url: "/Admin/News/_PreviewNewsMail",
        type: "POST",
        data: { Title: _Title, Description: _Description},
        success: function (data) {
            if (data == null || data.trim() == "") {
                notificationKO(Constants.ErrorMessages.UnknownError);
            }
            else {
                $("#_PreviewMailModalDiv").html(data);
                $('#PreviewMailModal').animate({ scrollTop: 0 }, 'slow');
                $('#PreviewMailModal').modal('show');
            }
            HideSpinner();
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function TypeOnChange() {

    if (IsMailNeeded()) {
        $(".RelatedToNewsMail").fadeIn();
    }
    else {
        $(".RelatedToNewsMail").fadeOut();
    }
}


function IsMailNeeded() {
    var result = false;
    var TypeVal = $("#TypeId").val();
    if (TypeVal != null && (TypeVal == Constants.NewsType.PublishAndMail || TypeVal.toString() == Constants.NewsType.MailOnly)) {
        result = true;
    }

    return result;
}