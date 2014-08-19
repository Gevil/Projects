/**
 * Kiválasztja a megye listából azt a megyét, amire a térképen kattintottak.
 */
$(document).ready(function() {
        $('#mapHu').maphilight();
        $('area').click(function(e) {
            $('#megye').val($(this).attr('title'));
        });
		/*.mouseout(function(e) {
            $('#squidhead').mouseout();
        }).click(function(e) { e.preventDefault(); });*/
    });