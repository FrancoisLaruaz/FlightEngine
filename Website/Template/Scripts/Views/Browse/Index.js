var Latitude = 48.8695866;
var Longitude = 2.34850449999999;
var zoomLevel = 14;
var distanceToSearch = Constants.Const.DefaultGoogleMapSearchDistance;

$(document).ready(function () {
    InitMap();
});




function InitMap() {
    //google.maps.event.addDomListener(window, 'load', InitMap);
    if (googleMapLoaded) {
        setTimeout(function () { SetMap(); }, 50);
        ShowSpinner();
    }
    else {
        setTimeout(function () { InitMap(); }, 100);
    } 
}


function SetMap() {
    var LocalizationJson = $("#LocalizationJson").val();

    var DefaultLocalization = GetDefaultLocalization(LocalizationJson);

    if (DefaultLocalization != null && typeof DefaultLocalization != "undefined") {
        Latitude = DefaultLocalization.numLat;
        Longitude = DefaultLocalization.numLon;
    }
   
    var myLatlng = new google.maps.LatLng(Latitude, Longitude);
    // Options
    var mapProp = {
        center: myLatlng,
        zoom: zoomLevel,
        disableDefaultUI: true,
        minZoom: 6, // Minimum zoom level allowed (0-20)
        maxZoom: 17, // Maximum soom level allowed (0-20)
        zoomControl: true, // Set to true if using zoomControlOptions below, or false to remove all zoom controls.
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.DEFAULT // Change to SMALL to force just the + and - buttons.
        },
        mapTypeId: google.maps.MapTypeId.ROADMAP, // Set the type of Map
        scrollwheel: false, // Disable Mouse Scroll zooming (Essential for responsive sites!)
        // All of the below are set to true by default, so simply remove if set to true:
        panControl: false, // Set to false to disable
        mapTypeControl: false, // Disable Map/Satellite switch
        scaleControl: false, // Set to false to hide scale
        streetViewControl: false, // Set to disable to hide street view
        overviewMapControl: false, // Set to false to remove overview control
        rotateControl: false // Set to false to disable rotate control
    };
    // Création de la map
    var map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    // Création du marker sur le local

    var overlay1 = new CustomMarker(
        myLatlng,
        map,
        {
            marker_id: '1'
        }
    );

    var overlay2 = new CustomMarker(
        new google.maps.LatLng(Latitude + 0.006, Longitude + 0.006),
        map,
        {
            marker_id: '2'
        }
    );

    var overlay2 = new CustomMarker(
        new google.maps.LatLng(Latitude - 0.007, Longitude + 0.006),
        map,
        {
            marker_id: '3'
        }
    );
    google.maps.event.addListener(map, 'zoom_changed', function () {
        zoomLevel = map.getZoom();
        var newDistanceToSearch = getDistanceToSearch(zoomLevel);
        if (newDistanceToSearch > distanceToSearch) {
            distanceToSearch = newDistanceToSearch;
            refreshMap();
        }
    });

    google.maps.event.addListener(map, 'drag', function (evt) {
        var newCenterPosition=map.getCenter();
        var newLat = newCenterPosition.lat();
        var newLon = newCenterPosition.lng();
        var distance = getDistance(Latitude, Longitude, newLat, newLon);
        if (distance > distanceToSearch) {
            Latitude = newLat;
            Longitude = newLon;
            refreshMap();
        }
    }); 
  
    HideSpinner();
}

function refreshMap() {
    alert('refreshMap');
}

function GetDefaultLocalization(LocalizationJson) {
    var MyLocalizationObject = [];
    if (LocalizationJson != null && LocalizationJson != '') {
         MyLocalizationObject = JSON.parse(LocalizationJson);
    }
    return MyLocalizationObject;
}
