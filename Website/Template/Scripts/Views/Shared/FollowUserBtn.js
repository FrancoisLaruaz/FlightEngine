function SetFollowBtn(CustomCallBackFunction) {
    $(".FollowUnfollowBtnUser_js").unbind("click");
    $(".FollowUnfollowBtnUser_js").on("click", function (e) {
        e.stopPropagation();
        e.preventDefault();
        var UserId = $(this).data("userid");
        if (UserId > 0) {
            $.ajax({
                url: "/Profile/ToggleUserFollow",
                type: "POST",
                data: { UserToFollowId: UserId },
                success: function (data) {

                    if (data == null || !data.Result) {

                        if (data.Errors == Constants.PartialViewResults.UnknownError) {
                            notificationKO(Constants.ErrorMessages.UnknownError);
                        }
                        else if (data == Constants.PartialViewResults.NotAuthorized) {
                            notificationKO(Constants.ErrorMessages.NotAuthorized);
                        }
                        notificationKO();
                    }
                    else {
                        var BtnTab = $(".FollowUnfollowBtn[data-userid='" + UserId + "']:visible");

                        if (BtnTab.length > 0) {
                            var Btn = $(BtnTab).first();
                            if (data.UserFollowed) {
                                $(Btn).html('[[[Following]]] <i class="fa fa-user-plus followBtnIcon"></i>').addClass('FFgreenButton').removeClass('FForangeButton');
                            }
                            else {
                                $(Btn).html('[[[Follow]]] <i class="fa fa-user-plus followBtnIcon"></i>').addClass('FForangeButton').removeClass('FFgreenButton');;
                            }
                        }

                        if (typeof (CustomCallBackFunction) === "function") {
                            CustomCallBackFunction.apply(this, null);
                        }
                    }
                },
                error: function (xhr, error) {
                    notificationKO(Constants.ErrorMessages.UnknownError);
                }
            });
        }
        else {
            notificationKO(Constants.ErrorMessages.UnknownError);
        }
    });
}