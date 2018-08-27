$(document).ready(function () {

    if ($("#VideoHomePageDiv").is(":visible")) {

        $('ul.nav').hover(function () {
            $('.hero-video-container').addClass('blur-container');
            $('.hero-inner').addClass('blur');
        }, function () {
                $('.hero-video-container').removeClass('blur-container');
                $('.hero-inner').removeClass('blur');
        });

        $('.home-nav a').on('click', function () {
            $('.hero-video-container').removeClass('blur-container');
            $('.hero-inner').removeClass('blur');
        });



        $('.navbar-collapse').on('hidden.bs.collapse', function () {
            $('.hero-video-container').removeClass('blur-container');
            $('.hero-inner').removeClass('blur');
        });

        $('#playHomePageButton').on('click', function () {
            if ($(window).width() > 768) {
                $("#videoplayer")[0].src = $("#videoplayer")[0].src.replace("autoplay=0", "autoplay=1");

                setTimeout(
                    function () {
                        $('#videoPlayerOverlay').addClass('video-player-overlay-show');
                    },
                    300);
            } else {
                window.location.href = "https://www.youtube.com/watch?v=nkhpGC10OVw";
            }
        });

        $('#videoPlayerClose').on('click', function () {
            $('#videoPlayerOverlay').removeClass('video-player-overlay-show');

            setTimeout(
                function () {
                    var url = $('#videoplayer').attr('src');
                    $('#videoplayer').attr('src', '');

                    url = url.replace("autoplay=1", "autoplay=0");
                    $('#videoplayer').attr('src', url);
                },
                800);
        });

        if ($('#homepageHeroVideo') != "undefined" && $('#homepageHeroVideo') != null) {
            if ($(window).width() < 768) {
                $('#homepageHeroVideo').removeAttr("autoplay");
                $('#homepageHeroVideo').removeAttr("preload");

                if (!$('#homepageHeroVideo').get(0).paused && typeof $('#homepageHeroVideo').get(0).pause === "function") {
                    $('#homepageHeroVideo').get(0).pause();
                }
            } else if (typeof $('#homepageHeroVideo').get(0).play === "function") {
                $('#homepageHeroVideo').get(0).play();
            }
        }

        $(window).resize(function () {

            if ($('#homepageHeroVideo').length > 0) {

                if ($(window).width() < 768) {
                    $('#homepageHeroVideo').removeAttr("autoplay");
                    $('#homepageHeroVideo').removeAttr("preload");

                    if (!$('#homepageHeroVideo').get(0).paused) {
                        $('#homepageHeroVideo').get(0).pause();
                    }
                }
                else {
                    //$('#homepageHeroVideo').attr('autoplay', 'autoplay');
                    $('#homepageHeroVideo').attr('preload', 'auto');

                    if ($('#homepageHeroVideo').get(0).paused) {
                        $('#homepageHeroVideo').get(0).play();
                    }
                }
            }
        });


    }

});