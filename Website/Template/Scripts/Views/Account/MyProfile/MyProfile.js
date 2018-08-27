var actionActive = '';

$(document).ready(function () {

    SetNavActions();
});


function SetNavActions()
{

    $(".navVerticalItem").unbind("click");
    $(".navVerticalItem").on("click", function (e) {
        var SelectedNav = $(this);
     
        var action = $(this).data("actiontogo");
        var _UserId = $("#HiddenUserId").val();

        if (actionActive != action && _UserId > 0 && typeof action != "undefined")
        {
            
            $.ajax({
                url: "/Account/" + action,
                type: "GET",
                data: { UserId: _UserId },
                success: function (data) {
                    if (data == "ERROR" || data==null) {
                        notificationKO(Constants.ErrorMessages.UnknownError);
                    }
                    else if (data == "NotLoggedIn") {
                        notificationKO("[[[Please log in to access your profile.]]]");
                    }
                    else {
                        $(".navVerticalItem").removeClass('navActive');
                        $(SelectedNav).addClass('navActive');
                        actionActive = action;
                        $("#divMyProfileAction").fadeOut(500,function () {
                            $("#divMyProfileAction").html(data).fadeIn(500);
                        });
                    }
                },
                error: function (xhr, error) {
                    ErrorActions();
                }
            });

        }

    });

    $("#EditProfilLink").click();

}