google.load("maps", "2");

$(document).ready(function() {
    $(".details").colorbox({ innerWidth: "50%", html: function() {
        return $(this).prev().html();
    }
    });
    $(".result").click(function() {
        $(this).find(".additionalInfo").slideToggle();
    });

    var geocoder = new GClientGeocoder();

    var homePoint;
    var homeMarker;
    geocoder.getLocations(homeAddress, function(response) {
        // Retrieve the object
        var place = response.Placemark[0];
        // Retrieve the latitude and longitude
        homePoint = new GLatLng(place.Point.coordinates[1],
            place.Point.coordinates[0]);

        homeMarker = new GMarker(homePoint);
    });


    $(".map").each(function() {
        var geocoderEach = new GClientGeocoder();
        var address = $(this).attr("title");

        var mapDiv = $(this)[0];

        geocoderEach.getLatLng(address, function(point) {
            if (!point) {
                //alert(address + " not found");
            }
            else {
                var map = new google.maps.Map2(mapDiv);
                map.setCenter(point, 10);
                var marker = new GMarker(point);
                map.addOverlay(marker);
                map.addOverlay(homeMarker);
                marker.openInfoWindowHtml(address);
                map.addControl(new GLargeMapControl());
            }
        });
    });
});