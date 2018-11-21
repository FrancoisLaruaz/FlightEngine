var CanUserEdit = false;
var ProfilePictureHover = false;
var BackgroundPictureHover = false;
var IsLoggedUserProfile = false;

$(document).ready(function () {
    CanUserEdit = $("#CanUserEdit").val() == "True" ? true : false;
    IsLoggedUserProfile = $("#IsLoggedUserProfile").val() == "True" ? true : false;
});


function SetProfileTab() {

}

function SetGeneralInformation() {
    SetEditProfileBtn();
    if (CanUserEdit) {
        SetProfilePictureEvents();
        SetBackgroundPictureEvents();
    }


    if (typeof (SetFollowBtn) === "function") {
        SetFollowBtn(RefreshFollowersDiv);
    }
    else {
        setTimeout(function () { SetFollowBtn(RefreshFollowersDiv); }, 500);
    }

    HideSpinner();
}


var RefreshFollowersDiv = function RefreshFollowersDiv() {
    var UserId = $("#HiddenUserId").val();
    $.ajax({
        url: "/Profile/_Followers",
        type: "GET",
        data: { id: UserId },
        success: function (data) {
            if (data == null || data == Constants.PartialViewResults.UnknownError) {
                notificationKO(Constants.ErrorMessages.UnknownError);
                HideSpinner();
            }
            else if (data == Constants.PartialViewResults.NotAuthorized) {
                notificationKO(Constants.ErrorMessages.NotAuthorized);
                HideSpinner();
            }
            else {
                $("#divFollowers").html(data);
                HideSpinner();
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function SetAccordionActions() {


    $(".accordionShow").unbind("click");
    $(".accordionShow").on("click", function (e) {
        e.preventDefault();
        var followerPannel = $(this).parent().parent().find(".followerPannel");
        $(followerPannel).fadeIn(500);
        $(this).fadeOut(500);
    });

    $(".accordionHide").unbind("click");
    $(".accordionHide").on("click", function (e) {
        e.preventDefault();
        var followerPannel = $(this).parent().parent().parent().find(".followerPannel");
        $(followerPannel).fadeOut(500);
        $(this).parent().parent().parent().find(".accordionShow").fadeIn(500);
    });

}


function HidePictureOverlay() {
    $(".portraitUploadOverlay").css("display", "none");
    $(".portraitUploadOverlay").css("opacity", "1");
}

function ShowPictureOverlay() {
    $(".portraitUploadOverlay").css("display", "block");
    $(".portraitUploadOverlay").css("opacity", "1");
}

function HideBackgroundOverlay() {
    $(".backgroundUploadOverlay").css("display", "none");
    $(".backgroundUploadOverlay").css("opacity", "1");
}

function ShowBackgroundOverlay() {
    $(".backgroundUploadOverlay").css("display", "block");
    $(".backgroundUploadOverlay").css("opacity", "1");
}

function SetBackgroundPictureEvents() {
    $("#divBackgroundPicture").attr("title", "[[[Upload a new background picture.]]]");
    $("#divBackgroundPicture").addClass("pointerCursor");
    $("#divBackgroundPicture").hover(function () {
   
        if (!ProfilePictureHover) {
            BackgroundPictureHover = true;
            ShowBackgroundOverlay();
            HidePictureOverlay();
        }
        else {
            HideBackgroundOverlay();
        }
    },
        function () {

            if (BackgroundPictureHover) {
                BackgroundPictureHover = false;
                HideBackgroundOverlay();
            }
            else {
                HideBackgroundOverlay();
                HidePictureOverlay();
            }
        });

    $("#divBackgroundPicture").unbind("click");
    $("#divBackgroundPicture").on("click", function (e) {
        e.preventDefault();
        if (!ProfilePictureHover) {
            $("#backgroundPictureInputFile").click();
        }
    });
}


function SetProfilePictureEvents() {
    $("#divProfilePicture").attr("title", "[[[Upload a new profile picture.]]]");
    $("#divProfilePicture").addClass("pointerCursor");
    $("#divProfilePicture").hover(function () {
        ProfilePictureHover = true;
        ShowPictureOverlay();
        HideBackgroundOverlay();
    },
        function () {
            ProfilePictureHover = false;
            ShowBackgroundOverlay();
            HidePictureOverlay();
        });

    $("#divProfilePicture").unbind("click");
    $("#divProfilePicture").on("click", function (e) {
        e.preventDefault();
        $("#portraitInputFile").click();
    });
}


function loadBackgroundPicture(event) {
    if (event.target != null && event.target.files.length > 0) {

        var file = event.target.files[0];
        if (file != null && typeof file != "undefined") {
            var sizefileKB = file.size / 1024 / 1024 * 1000000;
            var IsSizeOk = sizefileKB <= Constants.FileSize.LightPicture ? true : false;

            if (IsSizeOk) {
                ShowSpinner();
                var model = new FormData();
                model.append("UserId", $("#HiddenUserId").val());
                model.append("Picture", file);

                $.ajax({
                    url: "/Profile/UpdateBackgroundPicture",
                    type: 'POST',
                    dataType: 'json',
                    data: model,
                    processData: false,
                    contentType: false,// not json
                    success: function (data) {
                        if (data.Result) {
                            var url = URL.createObjectURL(file);
                            $('#divBackgroundPicture').css('background-image', 'url(' + url + ')');
                        }
                        else if (data.Error != null && data.Error.trim() != "") {
                            notificationKO(data.Error);
                        }
                        else {
                            notificationKO(Constants.ErrorMessages.UploadError);
                        }
                        HideSpinner();
                    },
                    error: function (response) {
                        notificationKO(Constants.ErrorMessages.UploadError);
                        HideSpinner();
                    }
                });
            }
            else {
                notificationKO('[[[Your file size exceeds 2MB. Please upload another picture.]]]');
                $('#backgroundInputFile').val('');
            }
        }
    }
}


function loadProfilePicture(event) {
    if (event.target != null && event.target.files.length > 0) {

        var file = event.target.files[0];
        if (file != null && typeof file != "undefined") {
            var sizefileKB = file.size / 1024 / 1024 * 1000000;
            var IsSizeOk = sizefileKB <= Constants.FileSize.LightPicture ? true : false;

            if (IsSizeOk) {
                ShowSpinner();
                var model = new FormData();
                model.append("UserId", $("#HiddenUserId").val());
                model.append("Picture", file);

                $.ajax({
                    url: "/Profile/UpdateProfilePicture",
                    type: 'POST',
                    dataType: 'json',
                    data: model,
                    processData: false,
                    contentType: false,// not json
                    success: function (data) {
                        if (data.Result) {
                            var url = URL.createObjectURL(file);
                            $('#divProfilePicture').css('background-image', 'url(' + url + ')');

                            if (IsLoggedUserProfile) {
                                $('#UserPic').css('background-image', 'url(' + url + ')');
                            }
                        }
                        else if (data.Error != null && data.Error.trim() != "") {
                            notificationKO(data.Error);
                        }
                        else {
                            notificationKO(Constants.ErrorMessages.UploadError);
                        }
                        HideSpinner();
                    },
                    error: function (response) {
                        notificationKO(Constants.ErrorMessages.UploadError);
                        HideSpinner();
                    }
                });
            }
            else {
                notificationKO('[[[Your file size exceeds 2 MB. Please upload another picture.]]]');
                $('#portraitInputFile').val('');
            }
        }
    }
}


function SetUrlRedirect() {
    $(".redirectToUrl_js").unbind("click");
    $(".redirectToUrl_js").on("click", function (e) {
        e.preventDefault();
        ShowSpinner();
        var redirectUrl = $(this).data("redirecturl");
        if (redirectUrl != null) {
            window.location.href = FixPartialUrlWithCulture(redirectUrl);
        }
    });
}

function SetEditProfileBtn() {
    $(".EditProfileBtn").unbind("click");
    $(".EditProfileBtn").on("click", function (e) {
        e.preventDefault();
        ShowSpinner();
        var UserId = $("#HiddenUserId").val();
        $('#EditProfileModal').data('modal', null);
        $.ajax({
            url: "/Profile/_EditGeneralInformation",
            type: "GET",
            data: { UserId: UserId },
            success: function (data) {
                if (data == null || data == Constants.PartialViewResults.UnknownError) {
                    notificationKO(Constants.ErrorMessages.UnknownError);
                }
                else if (data == Constants.PartialViewResults.NotAuthorized) {
                    notificationKO(Constants.ErrorMessages.NotAuthorized);
                }
                else {
                    $("#EditProfileDiv").html(data);
                    $('#EditProfileModal').modal('show');
                }
            },
            error: function (xhr, error) {
                ErrorActions();
            }
        });
    });
}

function SetEditGeneralInformationForm() {
    SetSexyCSS();
    SetGenericAjaxForm('EditGeneralInformationForm', EditGeneralInformationFormSuccess, EditGeneralInformationFormFailure, EditGeneralInformationFormBegin);
    HideSpinner();
}

function EditGeneralInformationFormBegin() {
    ShowSpinner();
}

function EditGeneralInformationFormFailure() {
    notificationKO();
    HideSpinner();
}

function EditGeneralInformationFormSuccess(data) {
    $("#GeneralInformationFormError").html('');
    if (data == null) {
        $("#GeneralInformationFormError").html(Constants.ErrorMessages.UnknownError);
        HideSpinner();
    }
    else if (data.Result) {
        $("#closeEditProfileModal").click();
        $('#EditProfileModal').modal('hide');
        setTimeout(function () { RefreshProfilGeneralInformation(); }, 300);
        if (IsLoggedUserProfile) {
            $("#UserFirstName").html(data.FirstName);
        }
        notificationOK("[[[Your profile has been successfully saved.]]]");
    }
    else {
        if (data.Errors == "" || data.Errors == null) {
            $("#GeneralInformationFormError").html(Constants.ErrorMessages.UnknownError);
        }
        else {
            $("#GeneralInformationFormError").html(data.Errors);
        }
        HideSpinner();
    }
}

function RefreshProfilGeneralInformation() {
    var UserId = $("#HiddenUserId").val();
    $.ajax({
        url: "/Profile/_GeneralInformation",
        type: "GET",
        data: { id: UserId },
        success: function (data) {
            if (data == null || data == Constants.PartialViewResults.UnknownError) {
                notificationKO(Constants.ErrorMessages.UnknownError);
                HideSpinner();
            }
            else if (data == Constants.PartialViewResults.NotAuthorized) {
                notificationKO(Constants.ErrorMessages.NotAuthorized);
                HideSpinner();
            }
            else {
                $("#GeneralInformationDiv").html(data);
                HideSpinner();
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}

