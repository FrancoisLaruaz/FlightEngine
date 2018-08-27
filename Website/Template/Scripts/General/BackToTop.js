$(document).ready(function () {
    if ($('#back-to-top') != undefined) {
        $(window).scroll(function () {
            if ($(this).scrollTop() > 50) {
                $('#back-to-top').fadeIn();
            } else {
                $('#back-to-top').fadeOut();
            }
        });

        // scroll body to 0px on click
        $('#back-to-top').click(function () {
            BackToTop();
            return false;
        });
    }

});

