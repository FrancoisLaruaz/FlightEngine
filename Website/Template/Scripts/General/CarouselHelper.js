function InitCarousel(carouselId) {
    $("#" + carouselId).carousel();
}

function carouselRightOnClick(element) {
    var _carousel = $(element).closest(".carousel");
    _carousel.carousel("next")
}

function carouselLeftOnClick(element) {
    var _carousel = $(element).closest(".carousel");
    _carousel.carousel("prev");
}

function carouselItemOnClick(i, element) {
    var _carousel = $(element).closest(".carousel");
    _carousel.carousel(i);
}