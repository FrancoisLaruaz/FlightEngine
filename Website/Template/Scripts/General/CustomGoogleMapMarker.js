var mapObject = null;

// https://www.w3schools.com/bootstrap/tryit.asp?filename=trybs_ref_js_carousel_js&stacked=h

function CustomMarker(latlng, map, args) {
    this.latlng = latlng;
    this.args = args;
    mapObject = map;
    this.setMap(map);
}


CustomMarker.prototype = new google.maps.OverlayView();

CustomMarker.prototype.draw = function () {

    var self = this;

    var div = this.div;
    if (!div) {

        div = this.div = document.createElement('div');
        div.className = 'GMmarker';

        var subDiv = document.createElement('div');
        var spanDate = document.createElement('span');
        spanDate.innerHTML = "25/02/18";
        spanDate.className = "spanDateClass";

        var spanPrice = document.createElement('span');
        spanPrice.innerHTML = "$2 500";
        spanPrice.className = "spanPriceClass";


        var categoryImage = document.createElement("IMG");
        categoryImage.setAttribute("src", "/Ressources/Files/Images/ProductIcon/drinks.png");
        categoryImage.setAttribute("width", "20");
        categoryImage.setAttribute("height", "20");
        categoryImage.setAttribute("align", "left");
        categoryImage.setAttribute("alt", "Category");
        categoryImage.className = "categoryImage";


        if (typeof (self.args.marker_id) !== 'undefined') {
            div.dataset.marker_id = self.args.marker_id;
        }

        google.maps.event.addDomListener(div, "mouseover", function (event) {
            div.style.background = Constants.Const.ColorWebsite;
            div.style.color = 'white';
            google.maps.event.trigger(self, "mouseover");
        });

        google.maps.event.addDomListener(div, "mouseout", function (event) {
            div.style.background = 'white';
            div.style.color = Constants.Const.ColorWebsite;
            google.maps.event.trigger(self, "mouseout");
        });

        google.maps.event.addDomListener(div, "click", function (event) {
            closeInfoBox();

            if (!div.open) {
                var loc = self.latlng;
                var item = { map_image_url: "https://cloud.netlifyusercontent.com/assets/344dbf88-fdf9-42bb-adb4-46f01eedd629/242ce817-97a3-48fe-9acd-b1bf97930b01/09-posterization-opt.jpg", rate: "5", name_point: "Name", type_point: "type", description_point: "description", url_detail: "https://frontfundr.com", get_directions_start_address: "address", location_latitude: 0, location_longitude: 0, phone: "604 345 34 06" };
                var infobox = getInfoBox(item);
                infobox.openRevised(mapObject, this, loc);
                InitCarousel("myCarousel");
        //    mapObject.setCenter(loc);
                div.open = true;
            }
            else {
                closeInfoBox();
                div.open = false;
            }
            google.maps.event.addListener(mapObject, 'click', function () {
                closeInfoBox();
                div.open = false;
            });

            google.maps.event.trigger(self, "click");
            event.stopPropagation();
        });

        var panes = this.getPanes();
        subDiv.appendChild(categoryImage);
        subDiv.appendChild(spanDate);
        subDiv.appendChild(spanPrice);
        div.appendChild(subDiv);
        panes.overlayImage.appendChild(div);
    }

    var point = this.getProjection().fromLatLngToDivPixel(this.latlng);

    if (point) {
        div.style.left = (point.x - 10) + 'px';
        div.style.top = (point.y - 20) + 'px';
    }
};

CustomMarker.prototype.remove = function () {
    if (this.div) {
        this.div.parentNode.removeChild(this.div);
        this.div = null;
    }
};

CustomMarker.prototype.getPosition = function () {
    return this.latlng;
};




function closeInfoBox() {
    $('div.infoBox').remove();
};

function getInfoBox(item) {
    return new InfoBox({
        content:
            '<div class="marker_info" id="marker_info">' +
          //  '<div><div class="marker_image" style="background-image:  url(' + item.map_image_url +')" /></div>' +
            '<div class="marker_carousel">' + GetCarousel(item)+'</div>' +
            '<div class="marker_infotext">' +
            '<div class="marker_titletext">' + item.name_point +'</div>' +
            '<em>' + item.type_point + '</em>' +
            '<strong>' + item.description_point + '</strong>' +
            '<a href="' + item.url_detail + '" class="btn_infobox_detail"></a>' +
            '<form action="http://maps.google.com/maps" method="get" target="_blank"><input name="saddr" value="' + item.get_directions_start_address + '" type="hidden"><input type="hidden" name="daddr" value="' + item.location_latitude + ',' + item.location_longitude + '"><button type="submit" value="Get directions" class="btn_infobox_get_directions">Get directions</button></form>' +
            '<a href="tel://' + item.phone + '" class="btn_infobox_phone">' + item.phone + '</a>' +
            '</div>' +
            '</div>',
        disableAutoPan: true,
        maxWidth: 0,
        pixelOffset: new google.maps.Size(35, -160),
        closeBoxMargin: '5px 5px 0 0',
        closeBoxURL:null,
        isHidden: false,
        pane: 'floatPane',
        enableEventPropagation: false
    });
};

InfoBox.prototype.openRevised = function (map, anchor, loc) {

    if (anchor) {
        this.position_ = loc;
    }
    this.setMap(map);
};


function GetCarousel(item) {
    var html = '<div id="myCarousel" class="carousel slide"><ol class="carousel-indicators"> <li class="item1 active" onclick="carouselItemOnClick(0,this)"></li><li class="item2" onclick="carouselItemOnClick(1,this)"></li>'
        + ' <li class="item3" onclick="carouselItemOnClick(2,this)"></li>'
        + ' <li class="item4" onclick="carouselItemOnClick(3,this)"></li></ol>'
        + '<div class="carousel-inner" role="listbox">'
        + '<div class="item active">'
        +  '<div class="marker_image" style="background-image:  url(' + item.map_image_url +')" /></div>'
        + ' </div>'
        + '  <div class="item"><div class="marker_image" style="background-image:  url(' + item.map_image_url + ')" /></div>'
        + ' </div> <div class="item">'
        + ' <div class="marker_image" style="background-image:  url(https://bloximages.chicago2.vip.townnews.com/tribdem.com/content/tncms/assets/v3/editorial/3/83/38384be2-3ba5-11e8-adec-bf48bc62810f/5acadc92f3c7d.image.jpg?resize=1200%2C1070)" /></div>'
        + ' </div>'
        + ' <div class="item">'
        + ' <div class="marker_image" style="background-image:  url(https://bloximages.chicago2.vip.townnews.com/tribdem.com/content/tncms/assets/v3/editorial/3/83/38384be2-3ba5-11e8-adec-bf48bc62810f/5acadc92f3c7d.image.jpg?resize=1200%2C1070)" /></div>'
        + '</div>'
        + ' </div>'
        + ' <a class="left carousel-control"  onclick="carouselLeftOnClick(this)"  role="button">'
        + ' <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>'
        + ' <span class="sr-only">Previous</span>'
        + '</a>'
        + ' <a class="right carousel-control" onclick="carouselRightOnClick(this)"  role="button">'
        + '<span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>'
        + '<span class="sr-only">Next</span>'
        + '</a>'
        + '</div>';
    return html;
}

