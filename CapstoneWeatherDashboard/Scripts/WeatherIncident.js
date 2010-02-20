$(document).ready(function() {
    $(".details").colorbox({ innerWidth: "50%", html: function() {
        return $(this).prev().html();
    }
    });
    $(".result").click(function() {
        $(this).find(".additionalInfo").slideToggle();
    });
});