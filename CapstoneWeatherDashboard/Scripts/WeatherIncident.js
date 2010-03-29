google.load("maps", "2");

var urls = [];
var allIncidents = [];
var totalUrls = 0;
var geocoder = new GClientGeocoder();

function displayIncidents(incidents) {
    if (!incidentFound && incidents.length < 1 && urls.length < 1) {
        $('#results').append("<h2>No results found for search '" + searchString + "'.</h2>");
        $("#progress").hide();
        $("#searchString").hide();
        return;
    }
    for (var i in incidents) {
        incidentFound = true;
        incidents[i].newRow = true;
        incidents[i].order = allIncidents.length;
        incidents[i].html = $('<div id="result' + i + '" class="result ' + incidents[i].EventTypeString + '" style="display: none">' +
                                '<div class="topInfo">' +
                                    '<p class="distance">' + i + ' miles away</p>' +
                                    '<p class="date">' + incidents[i].DateString + '</p>' +
                                    '<p class="eventType">' + incidents[i].EventTypeInWords + '</p>' +
                                '</div>' +
                                '<div class="additionalInfo">' +
                                    '<p>' +
                                        '<label>Source:</label>' +
                                        '<a href="' + incidents[i].MoreInformationUrl + '">' +
                                            incidents[i].MoreInformationUrl +
                                        '</a>' +
                                    '</p>' +
                                    '<div>' +
                                        '<div class="map"></div>' +
                                    '</div>' +
                                    '<a href="/demo/IncidentPdf/IncidentAsPdf"> Create PDF </a>' +
                                '</div>' +
                                '</div>');
        allIncidents.push(incidents[i]);
    }

    allIncidents = allIncidents.sort(function(a, b) {
        if (a.StartDateString < b.StartDateString) {
            return -1;
        } else if (a.StartDateString > b.StartDateString) {
            return 1;
        } else {
            // prevent fields with the same date from getting rearranged
            return a.order - b.order;
        }
    });

    $('#results').html();

    for (var i in allIncidents) {
        $('#results').append(allIncidents[i].html);
    }

    for (var i in allIncidents) {
        var closure = function(incident) {
            if (incident.newRow) {
                incident.html.fadeIn(1000);
                incident.newRow = false;
                incident.html.find('.topInfo').click(function() {
                    var additionalInfo = $(this).next(".additionalInfo");
                    additionalInfo.slideToggle();

                    // setup google map
                    additionalInfo.find('.map').each(function() {
                        var mapDiv = $(this);
                        var map = new google.maps.Map2(mapDiv[0]);

                        var searchLocation = new GLatLng(latitude, longitude);
                        map.setCenter(searchLocation, 9);
                        map.addControl(new GLargeMapControl());

                        var marker = new GMarker(searchLocation);
                        setupMarker(map, marker, '<b>Location Searched For:</b><br/>' + homeAddress);
                        marker.openInfoWindowHtml('<b>Location Searched For:</b><br/>' + homeAddress);

                        for (var j in incident.Locations) {
                            var location = incident.Locations[j];
                            var incidentMarker = null;
                            if (location.Geocode == null) {
                                geocoder.getLatLng(location.FullAddress, function(point) {
                                    if (!point) {
                                        alert(location.FullAddress + " not found");
                                    } else {
                                        incidentMarker = new GMarker(point);
                                        setupMarker(map, incidentMarker, '<b>Incident Observed At:</b><br/>' + location.FullAddress);
                                    }
                                });
                            } else {
                                incidentMarker = new GMarker(new GLatLng(location.Geocode.Latitude, location.Geocode.Longitude));
                                setupMarker(map, incidentMarker, '<b>Incident Observed At:</b><br/>' + location.FullAddress);
                            }
                        }
                    });
                });
            }
        };
        closure(allIncidents[i]);
    }

    if (urls.length > 0) {
        $.getJSON(urls.shift(), displayIncidents);
        $("#percent").html(Math.round((100.0 * (totalUrls - urls.length - 1) / totalUrls)) + '%');
    } else {
        $("#progress").hide();
    }
}

function setupMarker(map, marker, html) {
    map.addOverlay(marker);
    GEvent.addListener(marker, "click", function(point) {
        marker.openInfoWindowHtml(html);
    });
}

$(document).ready(function() {

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
});