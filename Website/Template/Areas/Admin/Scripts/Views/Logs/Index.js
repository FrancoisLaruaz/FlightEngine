$(document).ready(function () {
    $('#SearchBox').focus();
    RefreshData(true);
    SetEnterKey("SearchIcon");
});

function RefreshData(IsNewResearch) {

    if (IsNewResearch === undefined) {
        IsNewResearch = true;
    }
    if (IsNewResearch)
    {
        $('#StartAt').val(0);
    }
    StartAt = parseInt($('#StartAt').val());
    ShowSpinner();
    var _Pattern = $('#SearchBox').val().replace("'","''");
    $.ajax({
        url: "/Admin/Logs/_DisplayLogs",
        type: "POST",
        data: { Pattern: _Pattern, StartAt: StartAt },
        success: function (data) {
            BackToTop();
            if (data == "ERROR") {
                ErrorActions();
            }
            else {
                $("#targetContainer").fadeOut(500,function () {
                    $("#targetContainer").html(data).fadeIn(500);
                });
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}