$(function () {
    $('form').submit(function () {
        $(this).children('input[type=submit],button[type=submit]').attr('disabled', 'disabled');
    });

    var $tables = $("table.bt");
    if ($tables.length > 0) {
        $tables.basictable({
            breakpoint: 768,
        });
    }
});