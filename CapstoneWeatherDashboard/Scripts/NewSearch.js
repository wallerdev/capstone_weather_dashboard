function updateButtons() {
    var searchTypes = ['addressSearch', 'geocodeSearch', 'policySearch'];

    for (var i in searchTypes) {
        var type = searchTypes[i];
        var searched = $('#' + type + ' input:text').filter(function() {
            return $(this).val() !== '';
        }).size() > 0;

        if (searched) {
            $('#' + type + ' input:submit').removeAttr('disabled');

        } else {
            $('#' + type + ' input:submit').attr('disabled', 'disabled');
        }
    }

    
}

$(document).ready(function() {
    $('#startDate').datepicker({
        changeMonth: true,
        changeYear: true,
        showOn: 'button',
        buttonImage: 'Content/images/calendar.gif',
        buttonImageOnly: true
    });

    $('#endDate').datepicker({
        changeMonth: true,
        changeYear: true,
        showOn: 'button',
        buttonImage: 'Content/images/calendar.gif',
        buttonImageOnly: true
    });

    $('input[placeholder]').placeholder();

    $('input').focus(updateButtons).keyup(updateButtons).blur(updateButtons).change(updateButtons)
    setInterval('updateButtons()', 1000);

    $('#addressSearchSubmit').click(function() {
        var city = $('#city').val();
        var state = $('#state').val();
        var zipCode = $('#zipCode').val();

        if ((city != "" && state != "") || zipCode != "") {
            return true;
        }
        else {
            $('#addrError').text("You must select at least a zip code or a city and state");
            return false;
        }
    });

    $('#geocodeSearchSubmit').click(function() {
        var latitude = $('#latitude').val();
        var longtitude = $('#longitude').val();

        if ( latitude != "" && longtitude != "" ) {
            return true;
        }
        else {
            $('#geoError').text("You must select a longitude and a latitude");
            return false;
        }
    });

    $('#policySearchSubmit').click(function() {
        var policyNumber = $('#policyNumber').val();

        if (policyNumber != "") {
            return true;
        }
        else {
            $('#policyError').text("You must select at lease a policy number");
            return false;
        }
    });
});