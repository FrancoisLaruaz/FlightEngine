$(document).ready(function () {
    RefreshData(true);
    SetEnterKey("SearchIcon");


    //BEGIN get edit update form
    $(document).on("click", ".element-card", function () {
        ShowSpinner();
        var elementId = $(this).data("news_id");
        window.location.href = GetHomePageUrl() + '/Admin/News/EditNews/' + elementId;

    })
    //END get edit update form

    $(document).on("click", ".delPastnewsBtn", function (e) {
        e.stopPropagation();
        var elementId = $(this).data("news_id");
        DeleteNews(elementId, true);
    });

    $(document).on("click", ".delFuturenewsBtn", function (e) {
        e.stopPropagation();
        var elementId = $(this).data("news_id");
        DeleteNews(elementId, false);
    });
    //END delete update

});


function DeleteNews(elementId, isPast) {
    console.log("elementId to del->", elementId);
    sweetConfirmation("[[[Are you sure about deleting this news ?]]]", null, function () {
        $("#spinner").fadeIn();
        //BEGIN ajax delete
        $.ajax({
            url: "/Admin/News/DeleteNews",
            type: "POST",
            data: { id: elementId },
            success: function (data) {
                if (data.Success) {
                    notificationOK("[[[The news has been successfully deleted.]]]");
                    var divIdToRemove = "#elementWrap_" + data.Id;
                    $(divIdToRemove).fadeOut(function () {
                        $(this).remove();
                    });

                    if (isPast) {
                        var NewCount = parseInt($("#numPastRowCount").html()) - 1;
                        $("#numPastRowCount").html(NewCount);
                    }
                } else {
                    notificationKO(data.Err);

                }
            },
            error: function (xhr, error) {
                console.log(" : error" + error);
                notificationKO(Constants.ErrorMessages.UnknownError);
            }
        });
        //END ajax delete
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
        url: "/Admin/News/_DisplayPublishedNews",
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