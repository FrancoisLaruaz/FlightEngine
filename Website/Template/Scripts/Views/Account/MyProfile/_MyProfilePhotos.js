$(document).ready(function () {
    SetInputFile();
    SetBtnActions();
    SetCamera('UserPicture', 'MyProfileCameraPictureUser', 'MyProfilePictureSrc', 'MyProfileCameraPictureUserPreview', 'showWithWebCamPicture');
    HideSpinner();
});


function SetBtnActions() {
    $("#UserUploadFileBtn").unbind("click");
    $("#UserUploadFileBtn").on("click", function (e) {
        e.preventDefault();
        $('#HiddenPic').click();
    });



    $("#UserWebcamBtn").unbind("click");
    $("#UserWebcamBtn").on("click", function (e) {
        e.preventDefault();
        $("#WebcamModal").modal('show');
    });


    $("#UserWebcamBtn").unbind("click");
    $("#UserWebcamBtn").on("click", function (e) {
        e.preventDefault();
        $("#WebcamModal").modal('show');
    });

}

function SetInputFile() {
    $('#HiddenPic').change(function (event) {

        if (event.target != null && event.target.files.length > 0) {

            var file = event.target.files[0];
            var IsSizeOk = file.size / 1024 / 1024 <= 0.5 ? true : false;
            if (IsSizeOk) {
                var model = new FormData();

                model.append("UserId", $("#HiddenUserId").val());
                model.append("Picture", file);


                $.ajax({
                    url: "/Account/UpdateMyProfilePicture",
                    type: 'POST',
                    dataType: 'json',
                    data: model,
                    processData: false,
                    contentType: false,// not json
                    success: function (data) {
                        if (data.Result) {
                            var url = URL.createObjectURL(file);
                            $('#PictureDiv').css('background-image', 'url(' + url + ')');
                            RefreshHeader();
                        }
                        else if (data.Error != null && data.Error.trim() != "") {
                            notificationKO(data.Error);
                        }
                        else {
                            notificationKO(Constants.ErrorMessages.UploadError);
                        }
                    },
                    error: function (response) {
                        notificationKO(Constants.ErrorMessages.UploadError);
                    }
                });

            }
            else {
                notificationKO('[[[Your file size exceeds 500 KB. Please upload another picture.]]]');
                $('#HiddenPic').val('');
            }
        }
    });
}

function handleMyProfilePhotosBegin() {

}




function MyProfilePhotosFailure() {
    ErrorActions();
}



function MyProfilePhotosSuccess(data) {
    $("#ErrorMyProfilePhotosForm").html('');

    if (data != null && data.Result) {

        RefreshHeader();
        $("#PictureDiv" ).css('backgroundImage', 'url(' + data.PreviewPath + ')');
        notificationOK('[[[Your picture has been successfully saved.]]]');
        $("#CloseModalPhotoX").click();
    }
    else if (data.Error != null && data.Error.trim() != '') {
        $("#ErrorMyProfilePhotosForm").html(data.Error);
    }
    else {
        MyProfilePhotosFailure();
    }
    BackToTop();
}
