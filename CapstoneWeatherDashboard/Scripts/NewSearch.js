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
    $('input[placeholder]').placeholder();

    $('input').focus(updateButtons).keyup(updateButtons).blur(updateButtons).change(updateButtons)
    setInterval('updateButtons()', 1000);
});