
(function ($) {
    "use strict";

    // Check for Mobile device
    var mobileDetected;
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        mobileDetected = true;
    } else {
        mobileDetected = false;
    }

    // Check for placeholder support
    jQuery.support.placeholder = (function () {
        var i = document.createElement('input');
        return 'placeholder' in i;
    })();

    // if Placeholder is not supported call plugin
    if (!jQuery.support.placeholder && $.fn.placeholder) {
        $('input, textarea').placeholder();
    }


    // function check for window width
    function checkWindowWidth() {
        return $(window).width();
    }


    /* =========================================
    ---- Create Responsive Menu
    =========================================== */
    var menu = $('.menu').clone(true).removeClass('menu').addClass('responsive-nav'),
        container = $('#responsive-nav');

    container.append(menu);



    container.find('li, .col-2, .col-3, .col-4, .col-5').each(function () {

        var $this = $(this);


        if ($this.hasClass('mega-menu-container')) {
            $this.removeClass('mega-menu-container');
        }



        $this.has('ul, .megamenu').prepend('<span class="menu-button"></span>');

    });


    $('span.menu-button').on('click', function () {
        var $this = $(this);

        if (!$this.hasClass('active')) {
            $(this)
                .addClass('active')
                .siblings('ul, .mega-menu')
                .slideDown('800');
        } else {
            $(this)
                .removeClass('active')
                .siblings('ul, .mega-menu')
                .slideUp('800');
        }
    });


    $('#responsive-nav-button').on('click', function () {
        var $this = $(this);

        if ($this.hasClass('active')) {
            $('#responsive-nav').find('.responsive-nav').slideUp(300, function () {
                $this.removeClass('active');
            });

        } else {
            $('#responsive-nav').find('.responsive-nav').slideDown(300, function () {
                $this.addClass('active');
            });
        }
    });


    // Sub menu show/hide with hoverIntent plugin
    if ($.fn.hoverIntent) {
        $('ul.menu').hoverIntent(function () {
            $(this).children('ul, .mega-menu').fadeIn(100);

        }, function () {
            $(this).children('ul, .mega-menu').fadeOut(50);
        },
            'li');

    } else {

        $('ul.menu').find('li').mouseover(function () {
            $(this).children('ul, .mega-menu').css('display', 'block');

        }).mouseout(function () {
            $(this).children('ul, .mega-menu').css('display', 'none');
        });
    }


    /* =========================================
    ---- Search bar input animation for Better Responsive
    ----- if not empty send form
    =========================================== */
    var formInputOpen = true;
    $('#quick-search').on('click', function (e) {
        var $this = $(this),
            parentForm = $this.closest('.quick-search-form'),
            searchInput = parentForm.find('.form-control'),
            searchInputVal = $.trim(searchInput.val());

        if (searchInputVal === '') {
            var hiddenGroup = parentForm.find(':hidden.form-group'),
                formGroup = parentForm.find('.form-group ');

            if (formInputOpen) {
                hiddenGroup.animate({ width: 'show' }, 400, function () {
                    formInputOpen = false;
                });
            } else {
                formGroup.animate({ width: 'hide' }, 400, function () {
                    formInputOpen = true;
                });
            }

            e.preventDefault();
        }

    });


    /* =========================================
    ---- Item hover animation
    =========================================== */

    function itemAnimationIn() {
        var $this = $(this),
            itemText = $this.find('.icon-cart-text'),
            itemWidth = $this.width(),
            ratingAmount = $this.find('.ratings-amount'),
            moreActionBtns = $this.find('.item-action-inner');


        if (itemWidth < 220) {
            itemText.animate({ width: 'hide' }, 100, function () {
                $(this).closest('.item-add-btn').addClass('icon-cart');
            });
        }
        ratingAmount.animate({ width: 'show' }, 300);
        moreActionBtns.css({ 'visibility': 'visible', 'overflow': 'hidden' }).animate({ width: 90 }, 300);
    }

    function itemAnimationOut() {
        var $this = $(this),
            itemText = $this.find('.icon-cart-text'),
            itemWidth = $this.width(),
            ratingAmount = $this.find('.ratings-amount'),
            moreActionBtns = $this.find('.item-action-inner');


        if (itemWidth < 220) {
            // be careful about this duration
            // make sure that it is the same as below's
            itemText.animate({ width: 'show' }, 300).closest('.item-add-btn').removeClass('icon-cart');
        }

        ratingAmount.animate({ width: 'hide' }, 300);
        moreActionBtns.animate({ width: 0 }, 300).css({ 'visibility': 'hidden', 'overflow': 'hidden' });
    }

    // Don't forget to use hoverIntent plugin for better ainmation!
    if ($.fn.hoverIntent) {
        $('.item').hoverIntent(itemAnimationIn, itemAnimationOut);
    } else {
        $('.item').on('mouseover', itemAnimationIn).on('mouseleave', itemAnimationOut);

    }




    function stickyMenu() {
        var windowTop = $(window).scrollTop(),
            windowWidth = checkWindowWidth(),
            header = $('#header'),
            navContainer = $('#main-nav-container'),
            navDist = navContainer.offset().top,
            headerHeight = header.height();

        if (windowTop >= navDist && windowTop > headerHeight && windowWidth > 768) {
            navContainer.addClass('fixed');
        } else {
            navContainer.removeClass('fixed');
        }
    }

    // NEW HOMEPAGE CHANGES
    //$(window).on('scroll resize', stickyMenu);







    // Blog Sidebar Widget Collapse with plugin's custom events
    $('.panel-title').on('click', function () {
        var $this = $(this),
            targetAcc = $this.find('a').attr('href');

        $(targetAcc).on('shown.bs.collapse', function () {
            $this.find('.icon-box').html('&plus;');
        }).on('hidden.bs.collapse', function () {
            $this.find('.icon-box').html('&minus;');
        });
    });


    // Checkout Collapse//Accordion
    $('.accordion-btn').on('click', function () {
        var $this = $(this),
            targetAcc = $this.data('target');

        $(targetAcc).on('shown.bs.collapse', function () {
            $this.addClass('opened');
        }).on('hidden.bs.collapse', function () {
            if ($this.hasClass('opened')) {
                $this.removeClass('opened');
            }

        });

    });


    $(window).on('scroll', function () {
        var windowTop = $(window).scrollTop(),
            scrollTop = $('#scroll-top');

        if (windowTop >= 300) {
            scrollTop.addClass('fixed');
        } else {
            scrollTop.removeClass('fixed');
        }
    });


    $('#scroll-top').on('click', function (e) {
        $('html, body').animate({
            'scrollTop': 0
        }, 1200);
        e.preventDefault();
    });

    var w = $('.container').width();

    $('#Canada-map').css('height', w / 2);
    $('#map_inner').css('top', -(w / 2));


    $(window).resize(function () {
        rsz();
    });


    function rsz() {
        var w = $('.container').width();

        //Top header
        var header_w = $('.container').width();
        var header_right_w = $('#top-links').width();
        $('.header-top-left').css('width', header_w - header_right_w - 1);

        if (header_w < 750) {
            $('.header-top-left').css('width', header_w);
        }

        //Video
        var video_w = $('.video').width();
        $('.video iframe').css('width', video_w);
        $('.video iframe').css('height', video_w * .5625);

        //Map
        $('#Canada-map').css('height', w * 0.5);
        if (w == 1140) {
            $('#map_inner').css('top', -(w * 0.38));
            $('#Canada-map .map-info').css('top', (w * 0.3));
        }
        if (w == 940) {
            $('#map_inner').css('top', -(w * 0.5));
            $('#Canada-map .map-info').css('top', (w * 0.25));
        }
        if (w <= 720) {
            $('#Canada-map').css('height', w * 0.5 + 180);
            $('#map_inner').css('top', -(w * 0.5));
            $('#Canada-map .map-info').css('top', 0);
        }

        //Sectors
        $('.sectors').each(function () {
            var window_w = $('#wrapper').width();
            var sectors_h = $(this).find('.section-contents').height() + 100;
            var sector_background_h = (window_w * .3);

            if ((sectors_h - sector_background_h) < 0) {
                $(this).find('.image').css('margin-top', sectors_h - sector_background_h);
            }

        });

        //Persents bar            

        var progress_bar_w = $('#projects .project-progress').width();
        var progress_w = $('#projects .project-progress .progress-bar').width();
        var persents_w = $('#projects .project-progress .persents').width();
        var rest_w = $('#projects .project-progress .rest').width();
        if ((progress_w + rest_w) > progress_bar_w) {
            $('#projects .project-progress .persents').css('left', progress_bar_w - rest_w - persents_w);
            if (progress_w == progress_bar_w) {
                $('#projects .project-progress .persents').css('left', progress_bar_w - persents_w);
                $('#projects .project-progress .rest').hide();

            }
        }
        if ((progress_w - persents_w / 2) < 0) {
            $('#projects .project-progress .persents').css('left', 0);
        }
        else {
            $('#projects .project-progress .persents').css('left', progress_w - persents_w / 2);
        }

        var header_H = $('#header').height();
        $('#top_space').css('height', header_H);
    }; //rsz end.

    rsz();

    setTimeout(function () {
        $('#new_way').animate({
            opacity: 1
        }, 1500, 'swing');
    }, 1000);



    /*
    $('.step-odd').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeInDown',
        offset: 500
    });

    $('.step-even').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeInDown',
        offset: 500
    });

    $('.how-it-works .steps.step-1').animate({
        opacity: .5,
        top: -10,
    }, 500, 'swing', function () {
        $('.how-it-works .steps.step-1').animate({
            opacity: 1,
            top: 0,
            }, 1000, 'swing');
        
    });

    $('.how-it-works #Entrepreneur .line-position-1 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Entrepreneur .line-position-1 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });

    $('.how-it-works #Entrepreneur .line-position-2 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Entrepreneur .line-position-2 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    $('.how-it-works #Entrepreneur .line-position-3 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Entrepreneur .line-position-3 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });
    $('.how-it-works #Entrepreneur .line-position-4 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Entrepreneur .line-position-4 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    $('.how-it-works #Entrepreneur .line-position-5 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Entrepreneur .line-position-5 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });
    $('.how-it-works .line-position-6 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('.line-position-6 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    $('.how-it-works .line-position-7 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('.line-position-7 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });
    $('.how-it-works .line-position-8 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('.line-position-8 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    

    $('.how-it-works #Investor .line-position-1 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Investor .line-position-1 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });

    $('.how-it-works #Investor .line-position-2 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Investor .line-position-2 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    $('.how-it-works #Investor .line-position-3 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Investor .line-position-3 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });
    $('.how-it-works #Investor .line-position-4 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Investor .line-position-4 .line-cover').animate({
                left: -880,
            }, 500);
        }
    });
    $('.how-it-works #Investor .line-position-5 .blue').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeIn',
        offset: 400,
        callbackFunction: function () {
            $('#Investor .line-position-5 .line-cover').animate({
                left: 880,
            }, 500);
        }
    });
    $('.how-it-works .sign_up').addClass("hidden_item").viewportChecker({
        classToAdd: 'visible animated fadeInDown',
        offset: 300
    });

    */

    $('.sidebar-widget .widget-title h4.button').on('click', function () {
        $(this).parent().parent('.sidebar-widget').toggleClass('open');
        if ($(this).parent().parent('.sidebar-widget').hasClass('open')) {
            $(this).parent().siblings('.content-box').slideDown('1000', function () {
                $(this).siblings('.widget-title').children('.fa').removeClass('fa-plus').addClass('fa-minus');
            });
        } else {
            $(this).parent().siblings('.content-box').slideUp('1000', function () {
                $(this).siblings('.widget-title').children('.fa').removeClass('fa-minus').addClass('fa-plus');
            });
        }
    });
}(jQuery));