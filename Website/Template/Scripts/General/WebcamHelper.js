function SetCamera(Purpose, IdCamera, IdFileToSave, IdPicturePreview, showWithWebCamPicture) {


    if (IdPicturePreview === undefined) {
        IdPicturePreview = null;
    }

    if (IdCamera != null && IdCamera.trim() != '') {
        var WebcamSessionId = GetRandomInt(0, 99999999);
    
        $(".webcamBtn").unbind("click");
        $(".webcamBtn").on("click", function (e) {
            e.preventDefault();
            ShowSpinner();
            WebcamCapture();
        });

        var UrlSave = "/Upload/SaveWebcamCapture?Purpose=" + Purpose + "&WebcamSessionId=" + WebcamSessionId.toString();
        var UrlGet = "/Upload/GetWebcamCapture?Purpose=" + Purpose + "&WebcamSessionId=" + WebcamSessionId.toString();

        $("#XwebcamXobjectX").css("width", "100%");
        $("#" + IdCamera).webcam({
            mode: "save",
            quality: 90,
            swffile: "/Scripts/OtherFormats/jscam.swf",
            onTick: function (remain) {
            },
            onSave: function (data) {
                if (data != null) {
                    ShowSpinner();
                    $.ajax({
                        type: "POST",
                        url: UrlGet,
                        data: '',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data != null) {
                            
                                if (data.Result) {
                                  
                                    if (IdPicturePreview != null && $("#" + IdPicturePreview).length > 0 && data.PathFilePreview != null) {

                                        if ($("#" + IdPicturePreview).is("img")) {
                                            $("#" + IdPicturePreview).attr("src", data.PathFilePreview);
                                        }
                                        else {
                                            $("#" + IdPicturePreview).css('backgroundImage', 'url(' + data.PathFilePreview + ')');
                                        }
                                        $("#" + IdPicturePreview).fadeIn();
                                    }
                                    if (IdFileToSave != null && $("#" + IdFileToSave).length > 0 && data.PathFile != null) {
                                        $("#" + IdFileToSave).val(data.PathFile);
                                    }

                                    if (showWithWebCamPicture != null && typeof showWithWebCamPicture != "undefined")
                                        $("." + showWithWebCamPicture).fadeIn();
                                }
                            }
                            HideSpinner();
                        },
                        failure: function (response) {
                            ErrorActions();
                        }
                    });
                }
            },
            noCameraFound: function () {
                notificationKO('[[[Web camera is not available]]]');
                HideSpinner();
            },
            error: function (e) {
                notificationKO('[[[Internal camera plugin error]]]');
                HideSpinner();
            },
            onCapture: function () {
                ShowSpinner();
                webcam.save(UrlSave);
            },
            debug: function () {
            },
            onLoad: function () {
           
                if (!CanUserTakeWebcamPicture()) {
                    $("#" + IdCamera).hide();
                    if ($("#CameraNotFound").length > 0) {
                        $("#CameraNotFound").fadeIn();
                    }
                    $(".webcamBtn").hide();
                }
                else {
         
                    if ($("#CameraNotFound").length > 0) {
                        $("#CameraNotFound").hide();
                    }
                    $("#" + IdCamera).fadeIn();
                    $(".webcamBtn").show();
                }
            }
        });
        $("#XwebcamXobjectX").css("width", "100%");

    }
}




function WebcamCapture() {
    PlayAudio(WebCamPictureAudio);
    ShowSpinner();
    webcam.capture();
    return false;
}


function CanUserTakeWebcamPicture() {


    var cams = webcam.getCameraList();

    var isFlashExists = swfobject.hasFlashPlayerVersion('1') ? true : false;


    if (cams == null || cams.length == 0 || !isFlashExists) {
        return false;
    }
    else {
        return true;
    }
}