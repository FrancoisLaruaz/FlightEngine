$(document).ready(function () {
   
    $('#SearchBox').focus();
    RefreshData(true);
    SetEnterKey("SearchIcon");
});

function RefreshData(IsNewResearch)
{

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
        url: "/Admin/Users/_DisplayUsers",
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





function AddUserRole(roleid, userid) {
    ShowSpinner();
    $.ajax({
        url: "/Admin/Users/AddUserRole",
        type: "POST",
        data: { roleid: roleid, userid: userid },
        success: function (data) {
            if (data != null && data.Result) {
                RefreshUserRoles(userid)
                notificationOK("[[[The role has been successfully added to the user.]]]");
            }
            else if (data != null && data.Error != null) {
                notificationKO(data.Error);
                HideSpinner();
            }
            else {
                ErrorActions();
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function DeleteUserRole(roleid, userid) {

        ShowSpinner();
        $.ajax({
            url: "/Admin/Users/DeleteUserRole",
            type: "POST",
            data: { roleid: roleid, userid: userid },
            success: function (data) {
                if (data != null && data.Result) {
                    RefreshUserRoles(userid)
                    notificationOK("[[[The role has been successfully deleted.]]]");
                }
                else if (data != null && data.Error != null) {
                    notificationKO(data.Error);
                    HideSpinner();
                }
                else {
                    ErrorActions();
                }
            },
            error: function (xhr, error) {
                ErrorActions();
            }
        });

}

function RefreshUserRoles(UserIdentityId) {
    if (UserIdentityId != null && UserIdentityId != '') {

        $.ajax({
            url: "/Admin/Users/_DisplayUsersModifications",
            type: "POST",
            data: { UserIdentityId: UserIdentityId },
            success: function (data) {
                if (data == "ERROR") {
                    ErrorActions();
                }
                else {
                    $("#div_userroles_" + UserIdentityId).fadeOut(500, function () {
                        $("#div_userroles_" + UserIdentityId).html(data).fadeIn(500);
                    });
                    HideSpinner();
                }
            },
            error: function (xhr, error) {
                ErrorActions();
            }
        });
    }
    else {
        HideSpinner();
    }
}




function AskConfirmationToDeleteUser(UserId) {
    if (UserId > 0) {
        sweetConfirmation("[[[Are you sure you want to delete this user ?]]]", null, DeleteUser, [UserId]);
    }
}

var DeleteUser = function DeleteUser(UserId) {
    $.ajax({
        url: "/Admin/Users/DeleteUser",
        type: "POST",
        data: { UserId: UserId },
        success: function (data) {

            if (data == null || !data.Result) {
                ErrorActions();
            }
            else {
                notificationOK("[[[The user has been successfully deleted.]]]");
                var divIdToRemove = "#UserTr_" + UserId;
                $(divIdToRemove).fadeOut(function () {
                    $(this).remove();
                });
                var NewCount = parseInt($("#numRowCount").html()) - 1;
                $("#numRowCount").html(NewCount);
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}