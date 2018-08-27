$(document).ready(function () {
    SetSexyCSS();
    SetValidationForm('MyProfileEditForm');
    SetDeleteLinkAction();
    HideSpinner();
});

function MyProfileEditFailure() {
    ErrorActions();
}

function SetDeleteLinkAction() {
    $("#DeleteProfile").unbind("click");
    $("#DeleteProfile").on("click", function (e) {
        e.preventDefault();
        var UserId = $(this).data("userid");
        if (UserId > 0) {
            AskConfirmationToDeleteProfile(UserId);
        }
    });
}

function AskConfirmationToDeleteProfile(UserId) {
    sweetConfirmation("[[[Are you sure you want to delete your profile ?]]]", null, DeleteProfile, [UserId]);
}

var DeleteProfile = function DeleteProfile(UserId) {
    $.ajax({
        url: "/Account/DeleteProfile",
        type: "POST",
        data: { UserId: UserId },
        success: function (data) {

            if (data == null || !data.Result) {
                ErrorActions();
            }
            else {
                window.location.href = GetHomePageUrl();
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function MyProfileEditSuccess(data) {
    $("#ErrorMyProfileEditForm").html('');

    if (data != null && data.Result) {
        if (data.LanguageRedirect != null) {

            var url = window.location.href.toLowerCase();
            var urlTab = url.split('/myprofile');
            if (urlTab.length == 1) {
                window.location.href = GetHomePageUrl() + '/' + data.LanguageRedirect + '/MyProfile';
            }
            else {
                window.location.href = GetHomePageUrl() + '/' + data.LanguageRedirect + '/MyProfile' + urlTab[1];
            }

            
        }
        else {
            RefreshHeader();
            notificationOK('[[[Your profile has been successfully saved.]]]');
        }
    }
    else if (data.Error != null && data.Error.trim() != '') {
        $("#ErrorMyProfileEditForm").html(data.Error);
    }
    else {
        MyProfileEditFailure();
    }
    BackToTop();
}

function handleMyProfileEditBegin() {

}

