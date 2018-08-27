$(document).ready(function () {


    $("#SignUpWelcomeBtn").unbind("click");
    $("#SignUpWelcomeBtn").on("click", function (e) {
        e.preventDefault();
        SignUpPictureForm();
    });
});


function SignUpPictureForm()
{
    hideAndShowGuidePg('SignUpWelcomePage', 'SignUpProcessPages');
}