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

/* Google maps code


<script src="http://maps.google.com/maps?file=api&amp;v=2.x&amp;key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"
type="text/javascript"></script>

<script type="text/javascript">
var geocoder = null;
geocoder = new GClientGeocoder();
var address = "grand ledge, mi 48837";
var marker;
var lat;
var lon;

//        document.write(address);
geocoder.getLocations(address, Set);

function Set(response) {
// Retrieve the object
var place = response.Placemark[0];
// Retrieve the latitude and longitude
var point = new GLatLng(place.Point.coordinates[1],
place.Point.coordinates[0]);
lat = place.Point.coordinates[1];
lon = place.Point.coordinates[0];
//            document.write(point);

}
</script>

<script type="text/javascript" src="http://www.google.com/jsapi?key=ABQIAAAAam0nwuIjjXo0_gZGpAyU2hRCy4l6b2RPYQNXTJn1LO8P79-4LxTFJKh9yf0ov08TsXwL824gW69e8w"></script>

<script type="text/javascript">

google.load("maps", "2");
// Call this function when the page has been loaded
function initialize() {
var map = new google.maps.Map2(document.getElementById("map"))
var point3 = new GLatLng(lat, lon);
var marker = new GMarker(point3);
map.setCenter(point3, 11);

map.addOverlay(marker);

map.addControl(new GLargeMapControl());
}
google.setOnLoadCallback(initialize); 
</script>
    
    
<form id="form1" runat="server">
<div>
<div id="map" style="width: 400px; height: 400px">
</div>
</div>
</form>
*/