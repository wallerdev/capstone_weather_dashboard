$(document).ready(function() {
    $(".details").colorbox({ innerWidth: "50%", html: function() {
        return $(this).prev().html();
    }
    });
});