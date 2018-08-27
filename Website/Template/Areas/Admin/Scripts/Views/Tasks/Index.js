$(document).ready(function () {
    $('#SearchBox').focus();
    RefreshData(true);
    SetEnterKey("SearchIcon");


    ShowSpinner();

    $("#ResetTasks").unbind("click");
    $("#ResetTasks").on("click", function (e) {
        e.preventDefault();
        AskConfirmationToResetTasks();
    });
});

function AskConfirmationToResetTasks() {
    sweetConfirmation("[[[Are you sure you want to reset the tasks?]]]", null, ResetTasks, null);
}

var ResetTasks=function ResetTasks()
{
    $.ajax({
        url: "/Admin/Tasks/ResetTasks",
        type: "POST",
        success: function (data) {
     
            if (data == null || !data.Result) {
                ErrorActions();
            }
            else {
                window.location.href = window.location.href;
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}

function RefreshData(IsNewResearch) {

    if (IsNewResearch === undefined) {
        IsNewResearch = true;
    }
    if (IsNewResearch) {
        $('#StartAt').val(0);
    }
    StartAt = parseInt($('#StartAt').val());
    ShowSpinner();
    var _Pattern = $('#SearchBox').val().replace("'", "''");
    $.ajax({
        url: "/Admin/Tasks/_DisplayTasks",
        type: "POST",
        data: { Pattern: _Pattern, StartAt: StartAt },
        success: function (data) {
            BackToTop();
            if (data == "ERROR") {
                ErrorActions();
            }
            else {
                $("#targetContainer").fadeOut(500, function () {
                    $("#targetContainer").html(data).fadeIn(500);
                });
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}