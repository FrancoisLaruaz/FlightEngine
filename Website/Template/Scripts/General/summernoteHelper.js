$(document).ready(function () {

    $('.summernote').each(function (index, value) {

        var SummerNoteId = $(this).attr('id');

        $(this).summernote({
            height: 300,                 // set editor height
            minHeight: 150,             // set minimum height of editor
            maxHeight: null,             // set maximum height of editor
            focus: true,              // set focus to editable area after initializing summernote
            lang: 'fr',
            callbacks: {
                onInit: function () {
                    $('.note-editor').show();
                },
                onImageUpload: function (files) {                   
                    var formData = new FormData();
                    formData.append("file", files[0]);
                    formData.append("Purpose", "news");
                    ShowSpinner();
                    $.ajax({
                        url: "/Upload/UploadDecryptedDocument",
                        data: formData,
                        type: 'POST',
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function (data) {
                            if (data != null && data.PathFile != null) {
                                var AjustedUrl = data.PathFile.replace('~', GetHomePageUrl());
                                $("#" + SummerNoteId).summernote('editor.insertImage', AjustedUrl);
                            }
                            else {
                                ErrorActions();
                            }
                        },
                        error: function () {
                            ErrorActions();
                        }
                    });
                    
                }
                ,
                onMediaDelete: function (target) {
                    if (target != null) {
                        DeleteFileDirectPath(target[0].src);
                    }
                }

            }
        });

    });

});
