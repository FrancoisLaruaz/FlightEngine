$(document).ready(function () {

    

    $("#EmailUserWelcomeBtn").unbind("click");
    $("#EmailUserWelcomeBtn").on("click", function (e) {
        e.preventDefault();
        DisplaySignUpPictureForm('newUserEmailPg');
    });


 

    $("#CompleteProfileBtn").unbind("click");
    $("#CompleteProfileBtn").on("click", function (e) {
        e.preventDefault();
        window.location.href = GetHomePageUrl()+'/MyProfile';
    });


    $("#ContinueBrowsing").unbind("click");
    $("#ContinueBrowsing").on("click", function (e) {
        e.preventDefault();
        window.location.href = GetHomePageUrl();
    });

 
});

function DisplaySignUpPictureForm(ElementToHide)
{

    $.ajax({
        url: "/Account/_SignUpPictureForm",
        type: "GET",
        success: function (data) {
            if (data == null) {
           
                notificationKO(Constants.ErrorMessages.UnknownError);
            }
            else {
                $("#" + ElementToHide).fadeOut(500, function () {
                    $("#newUserPicturePg").html(data).fadeIn(500);
                });

            }
        },
        error: function (xhr, error) {
            notificationKO(Constants.ErrorMessages.UnknownError);
        }
    });
}
