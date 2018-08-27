/*
Notes:
Files that use this js file:
Views\Investment\Pledge.cshtml,
Views/Investment/Verify.cshtml,
Views\Investor2\Index.cshtml,
*/

$(function () {

    $(document).on('click', '.popup', function () {
        var box = $(this).find('.popuptext');
        box.toggleClass('show');
        //begin control when to add squeezed class
        if ($(window).width() < 993) {
            if (box.hasClass("show")) {
                box.addClass("squeezed");
            }
        }
        if (!box.hasClass("show")) {
            box.removeClass("squeezed");
        }
        //end control when to add squeezed class
    })

    $(document).on('mouseover', '.popuphover', function () {
        var box = $(this).find('.popuptext');
        box.toggleClass('show');
    })

    //BEGIN dealing with users who resize windows while some pops are open
    $(window).on('resize', function () {
        var boxesOpened = $(".popuptext.show");
        if ($(this).width() < 993) {
            boxesOpened.addClass("squeezed");
        } else {
            boxesOpened.removeClass("squeezed");
        }
    });
    //END dealing with users who resize windows while some pops are open


})