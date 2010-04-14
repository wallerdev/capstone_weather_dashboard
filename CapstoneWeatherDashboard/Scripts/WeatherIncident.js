google.load("maps", "2");

var urls = [];
var allIncidents = [];
var totalUrls = 0;
var geocoder = new GClientGeocoder();
var allIncidentInputs = "";

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

        allIncidentInputs += '<input type="hidden" name="d[]" value="' + encodeURIComponent(incidents[i].DateString) + '" />' +
            '<input type="hidden" name="et[]" value="' + encodeURIComponent(incidents[i].EventTypeInWords) + '" />' +
            '<input type="hidden" name="mi[]" value="' + encodeURIComponent(incidents[i].MoreInformationUrl) + '" />' +
            '<input type="hidden" name="i[]" value="' + encodeURIComponent(GetIncidentStaticMapImage(incidents[i])) + '" />';
        
        incidents[i].html = $('<div id="result' + i + '" class="result ' + incidents[i].EventTypeString + '" style="display: none">' +
                                '<div class="topInfo">' +
                                    '<p class="distance">' + incidents[i].Locations[0].FullAddress + '</p>' +
                                    '<p class="date">' + incidents[i].DateString + '</p>' +
                                    '<p class="eventType">' + incidents[i].EventTypeInWords + '</p>' +
                                    '<div style="clear:both;" />' +
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
                                    '<form action="/demo/IncidentPdf/IncidentAsPdf" method="post" style="float:left; padding: 10px;">' +
                                        '<input type="hidden" name="d" value="' + encodeURIComponent(incidents[i].DateString) + '" />' +
                                        '<input type="hidden" name="et" value="' + encodeURIComponent(incidents[i].EventTypeInWords) + '" />' +
                                        '<input type="hidden" name="mi" value="' + encodeURIComponent(incidents[i].MoreInformationUrl) + '" />' +
                                        '<input type="hidden" name="i" value="' + encodeURIComponent(GetIncidentStaticMapImage(incidents[i])) + '" />' +
                                        '<input type="submit" value="Create Pdf" />' +
                                    '</form>' +
                                    '<form style="float:right; padding: 10px;">' +
                                        '<label for="e">Email Addresses (Separated by commas): </label>' +
                                        '<input type="text" name="e" style="width:300px;" />' +
                                        '<input type="hidden" name="d" value="' + encodeURIComponent(incidents[i].DateString) + '" />' +
                                        '<input type="hidden" name="et" value="' + encodeURIComponent(incidents[i].EventTypeInWords) + '" />' +
                                        '<input type="hidden" name="mi" value="' + encodeURIComponent(incidents[i].MoreInformationUrl) + '" />' +
                                        '<input type="hidden" name="i" value="' + encodeURIComponent(GetIncidentStaticMapImage(incidents[i])) + '" />' +
                                        '<input type="submit" value="Send Pdf" class="emailSend" />' +
                                    '</form>' +
                                    '<div style="clear:both;" />' +
                                '</div>' +
                                '</div>');
        allIncidents.push(incidents[i]);
    }

    $("#allIncidentsPdf").html(allIncidentInputs + '<input type="submit" value="Save All Incidents to PDF" style="float: right;" />');
    $("#allIncidentsEmail").html(allIncidentInputs + 
        '<label for="e">Email Addresses (Separated by commas): </label>' +
            '<input type="text" id="e" name="e" style="width:300px;" />' +
            '<input type="submit" value="Email All Incidents" />');
    $("#allIncidentsEmail input[type=submit]").click(function() {
        var parentForm = $(this).parent('form');
        var serializedForm = parentForm.serialize();
        $.post('/demo/IncidentPdf/AllIncidentsAsEmail',
                        serializedForm,
                        function() {
                            alert('Email Sent Successfully');
                        });
        return false;
    });

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
                incident.html.find('.emailSend').click(function() {
                    var parentForm = $(this).parent('form');
                    var serializedForm = parentForm.serialize();
                    $.post('/demo/IncidentPdf/IncidentAsEmail',
                        serializedForm,
                        function() {
                            alert('Email Sent Successfully');
                        });
                    return false;
                });
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
                                        alert(location.FullAddress + " not found!");
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

function GetIncidentStaticMapImage(incident) {
    var returnText = latitude + "," + longitude +
        "&markers=color:blue|label:H|" + latitude + "," + longitude;
    for (var j in incident.Locations) {
        var location = incident.Locations[j];
        var incidentMarker = null;
        if (location.Geocode == null) {
            geocoder.getLatLng(location.FullAddress, function(point) {
                if (!point) {
                    // alert(location.FullAddress + " not found");
                    returnText = returnText + "&markers=color:blue|label:O|&sensor=false";
                } else {
                    returnText = returnText + "&markers=color:blue|label:O|" + point + "&sensor=false";
                }
            });
        }
        else {
            returnText = returnText + "&markers=color:blue|label:O|" + location.Geocode.Latitude + "," + location.Geocode.Longitude;
        }
        break;
    }
    return returnText;
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