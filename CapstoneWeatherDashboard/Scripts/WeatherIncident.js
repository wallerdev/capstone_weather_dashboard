google.load("maps", "2");

var urls = [];
var allIncidents = [];
var totalUrls = 0;

function displayIncidents(incidents) {
    for (var i in incidents) {
        incidents[i].newRow = true;
        incidents[i].order = allIncidents.length;
        incidents[i].html = $('<div id="result' + i + '" class="result ' + incidents[i].EventTypeString + '" style="display: none">' +
                                '<p class="distance">' + i + ' miles away</p>' +
                                '<p class="date">' + incidents[i].StartDateString + '</p>' +
                                '<p class="eventType">' + incidents[i].EventTypeInWords + '</p>' +
                                '<div class="additionalInfo">' +
                                    '<p>' +
                                        '<label>Source:</label>' +
                                        '<a href="' + incidents[i].MoreInformationUrl + '">' +
                                            incidents[i].MoreInformationUrl +
                                        '</a>' +
                                    '</p>' +
                                    '<div>' +
                                        '<label>Map:</label>' +
                                        '<div class="map" title="Eagle, MI"></div>' +
                                    '</div>' + 
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
        if (allIncidents[i].newRow) {
            allIncidents[i].html.fadeIn(1000);
            allIncidents[i].newRow = false;
            allIncidents[i].html.click(function() {
                $(this).find(".additionalInfo").slideToggle();

                // setup google map
                $(this).find('.map').each(function() {
                    var geocoderEach = new GClientGeocoder();
                    var address = $(this).attr("title");

                    var mapDiv = $(this)[0];

                    geocoderEach.getLatLng(address, function(point) {
                        if (!point) {
                            //alert(address + " not found");
                        } else {
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
        }
    }

    if (urls.length > 0) {
        $.getJSON(urls.shift(), displayIncidents);
        $("#percent").html(Math.round((100.0 * (totalUrls - urls.length - 1) / totalUrls)) + '%');
    } else {
        $("#progress").hide();
    }
    
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