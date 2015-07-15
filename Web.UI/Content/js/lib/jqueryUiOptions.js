define(function(require) {
    var $ = require('jquery');
    require('css!/Content/css/vendor/jquery-ui-1.11.4');
    require('css!/Content/css/dialog');
    require('jqueryui');

    $.extend($.ui.dialog.prototype.options, {
        height: "auto",
        autoOpen: false,
        modal: true,
        resizable: false,
        minHeight: 0,
        closeOnEscape: true,
        close: function(event, ui) {
            event.preventDefault();
            $(this).dialog('destroy').remove();
        }
    });

});