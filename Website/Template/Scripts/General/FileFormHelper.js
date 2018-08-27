function DownloadFileDirectPath(Path, IsAdminArea) {
    if (Path != null && Path.trim() != '') {
        var url = '../../Upload/DownloadFile?PathFile=' + Path;
        if (typeof IsAdminArea !== "undefined" && IsAdminArea) {
            url = '../../../Upload/DownloadFile?PathFile=' + Path;
        }
        $('#form_download_file').attr('action', url);
        $('#form_download_file').submit();
        $('#form_download_file').attr('action', '');
    }
}

function DownloadFileInElement(idElementSrc, IsAdminArea) {

    var Path = $("#" + idElementSrc).val();
    if (Path != null && Path.trim() != '') {
        var url = '../../Upload/DownloadFile?PathFile=' + Path;
        if (typeof IsAdminArea !== "undefined" && IsAdminArea) {
            url = '../../../Upload/DownloadFile?PathFile=' + Path;
        }
        $('#form_download_file').attr('action', url);
        $('#form_download_file').submit();
        $('#form_download_file').attr('action', '');
    }
}


function DeleteFileDirectPath(Path, IsAdminArea) {
    if (Path != null && Path.trim() != '') {
        var url = '../../Upload/DeleteFile?PathFile=' + Path;
        if (typeof IsAdminArea !== "undefined" && IsAdminArea) {
            url = '../../../Upload/DeleteFile?PathFile=' + Path;
        }
        $('#form_delete_file').attr('action', url);
        SetGenericAjaxForm('form_delete_file', null, null, null);
        $('#form_delete_file').submit();
        $('#form_delete_file').attr('action', '');
    }
}