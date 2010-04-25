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
        var allGeocoded = true;
        for(var j in incidents[i].Locations) {
            if(incidents[i].Locations[j].Geocode == null) {
                allGeocoded = false;
            }
        }
        if (allGeocoded) {
            incidents[i].staticMap = GetIncidentStaticMapImage(incidents[i]);
        } else {
            incidents[i].staticMap = "";
        }

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
                                    '<form action="/demo/IncidentPdf/IncidentAsPdf" method="post" style="float:left; padding: 10px;" class="pdfForm">' +
                                        '<input type="hidden" name="d" value="' + encodeURIComponent(incidents[i].DateString) + '" />' +
                                        '<input type="hidden" name="et" value="' + encodeURIComponent(incidents[i].EventTypeInWords) + '" />' +
                                        '<input type="hidden" name="mi" value="' + encodeURIComponent(incidents[i].MoreInformationUrl) + '" />' +
                                        '<input type="hidden" name="i" value="' + encodeURIComponent(incidents[i].staticMap) + '" />' +
                                        '<input type="submit" value="Create Pdf" />' +
                                    '</form>' +
                                    '<form style="float:right; padding: 10px;" class="emailSendForm">' +
                                        '<label for="e">Email Addresses (Separated by commas): </label>' +
                                        '<input type="text" name="e" style="width:300px;" />' +
                                        '<input type="hidden" name="d" value="' + encodeURIComponent(incidents[i].DateString) + '" />' +
                                        '<input type="hidden" name="et" value="' + encodeURIComponent(incidents[i].EventTypeInWords) + '" />' +
                                        '<input type="hidden" name="mi" value="' + encodeURIComponent(incidents[i].MoreInformationUrl) + '" />' +
                                        '<input type="hidden" name="i" value="' + encodeURIComponent(incidents[i].staticMap) + '" />' +
                                        '<input type="submit" value="Send Pdf" class="emailSend" />' +
                                    '</form>' +
                                    '<div style="clear:both;" />' +
                                '</div>' +
                                '</div>');
        allIncidents.push(incidents[i]);
    }

    allIncidents = allIncidents.sort(function(a, b) {
    if (a.DateString < b.DateString) {
            return -1;
        } else if (a.DateString > b.DateString) {
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
                incident.html.find('.pdfForm').submit(function() {
                    if ($(this).find('input[name=i]').val() != '') {
                        return true;
                    } else {
                        GetIncidentStaticMapImageThenCall(incident, function(incident, data) {
                            var form = incident.html.find('.pdfForm');
                            form.find('input[name=i]').val(data);
                            form.unbind('submit');
                            form.submit();
                        });
                    }
                    return false;
                });
                incident.html.find('.emailSendForm').submit(function() {
                    GetIncidentStaticMapImageThenCall(incident, function(incident, data) {
                        var form = incident.html.find('.emailSendForm');
                        form.find('input[name=i]').val(data);
                        form.unbind('submit');
                        var serializedForm = form.serialize();
                        $.post('/demo/IncidentPdf/IncidentAsEmail',
                            serializedForm,
                            function() {
                                alert('Email Sent Successfully');
                            }
                        );
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
        var percent = Math.round((100.0 * (totalUrls - urls.length - 1) / totalUrls)) + '%';
        $("#percent").html("Loading Incidents: " + percent);
    } else {
        $("#progress").hide();
    }
}

function setupMarker(map, marker, html) {
    map.addOverlay(marker);
    GEvent.addListener(marker, "mouseover", function(point) {
        marker.openInfoWindowHtml(html);
    });
    GEvent.addListener(marker, "mouseout", function(point) {
        marker.closeInfoWindow();
    });
}

function GetIncidentStaticMapImageThenCall(incident, fcn) {
    var returnText = "&markers=color:blue|label:H|" + latitude + "," + longitude;
    for (var j in incident.Locations) {
        var location = incident.Locations[j];
        var incidentMarker = null;
        if (location.Geocode == null) {
            geocoder.getLatLng(location.FullAddress, function(point) {
                if (!point) {
                    alert(location.FullAddress + " not found");
                    returnText = returnText + "&markers=color:red|label:O|&sensor=false";
                    fcn(incident, returnText);
                } else {
                    returnText = returnText + "&markers=color:red|label:O|" + point.y + "," + point.x;
                    fcn(incident, returnText);
                }
            });
            break;
        }
        else {
            returnText = returnText + "&markers=color:blue|label:O|" + location.Geocode.Latitude + "," + location.Geocode.Longitude;
            fcn(incident, returnText);
        }
    }
}

function GetIncidentStaticMapImage(incident) {
    var returnText = "&markers=color:blue|label:H|" + latitude + "," + longitude;
    for (var j in incident.Locations) {
        var location = incident.Locations[j];
        returnText = returnText + "&markers=color:red|label:O|" + location.Geocode.Latitude + "," + location.Geocode.Longitude;        
    }
    return returnText;
}

function generateAllStaticMapsThenCall(fcn) {
    var geocoding = false;
    var geocoded = 0;
    var toGeocode = 0;
    for (var i in allIncidents) {
        var incident = allIncidents[i];
        if (incident.staticMap === "") {
            if (!geocoding) {
                GetIncidentStaticMapImageThenCall(incident, function(inc, data) {
                    inc.staticMap = data;
                });
                setTimeout(function() { generateAllStaticMapsThenCall(fcn) }, 1700);
                geocoding = true;
            }
            toGeocode++;
        } else {
            geocoded++;
        }
    }
    
    if (toGeocode > 0) {
        var percent = Math.round(100.0 * geocoded / (geocoded + toGeocode)) + '%';
        $("#percent").html("Generating Maps: " + percent);
        $("#progress").show();
    } else {
        $("#progress").hide();
        fcn();
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

    $("#allIncidentsPdf").submit(function() {
        generateAllStaticMapsThenCall(function() {
            var data = "";
            for (var i in allIncidents) {
                data += "<input type='hidden' name='d[]' value='" + encodeURI(allIncidents[i].DateString) + "'/>" +
                    "<input type='hidden' name='et[]' value='" + encodeURI(allIncidents[i].EventTypeInWords) + "'/>" +
                    "<input type='hidden' name='mi[]' value='" + encodeURI(allIncidents[i].MoreInformationUrl) + "'/>" +
                    "<input type='hidden' name='i[]' value='" + encodeURI(allIncidents[i].staticMap) + "'/>";
            }
            var form = $("#allIncidentsPdf");
            form.prepend(data);
            form.unbind('submit');
            form.submit();
        });
        return false;
    });

    $("#allIncidentsEmail").submit(function() {
        var data = "";
        for (var i in allIncidents) {
            data += "<input type='hidden' name='d[]' value='" + encodeURI(allIncidents[i].DateString) + "'/>" +
                        "<input type='hidden' name='et[]' value='" + encodeURI(allIncidents[i].EventTypeInWords) + "'/>" +
                        "<input type='hidden' name='mi[]' value='" + encodeURI(allIncidents[i].MoreInformationUrl) + "'/>" +
                        "<input type='hidden' name='i[]' value='" + encodeURI(allIncidents[i].staticMap) + "'/>";
        }
        var form = $("#allIncidentsEmail");
        form.prepend(data);
        form.unbind('submit');
        form.submit();
        return false;
    });

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



    totalUrls = urls.length;
    if (urls.length > 0) {
        $.getJSON(urls.shift(), displayIncidents);
    }
});