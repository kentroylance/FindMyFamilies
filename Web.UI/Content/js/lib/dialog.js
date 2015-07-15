define(function(require) {

    var $ = require('jquery');
    var lazyRequire = require('lazyRequire');
    var requireOnce = lazyRequire.once();
    var jqueryui;

//    require('jqueryui');
//    require('css');
//    require('css!/Content/css/lib/common/jquery-ui-1.11.2');


//    require('lib/fancybox');
//    $(".fancybox").fancybox({
//        padding: 5,
//        width: 1050,
//        height: 800,
//        openEffect: 'none',
//        closeEffect: 'none',
//        iframe: {
//            preload: false
//        }
//    });
//
//    $("#helpDialog").fancybox({
//        helpers: {
//            title: null,
//            overlay: null,
//            padding: [20, 20, 20, 20]
//        },
//        'transitionIn': 'none',
//        'transitionOut': 'none',
//        'changeFade': 0,
//        openEffect: 'none',
//        closeEffect: 'none'
//    });
    function initJqueryUI() {
        requireOnce(['jqueryui', 'css!/Content/css/lib/common/jquery-ui-1.11.2'], function (Jqueryui) {
                jqueryui = Jqueryui;
            },
            function () {
                if (!jqueryui) {
                    $.extend($.ui.dialog.prototype.options, {
                        height: "auto",
                        autoOpen: false,
                        modal: true,
                        resizable: false,
                        minHeight: 0,
                        closeOnEscape: true,
                        close: function (event, ui) {
                            event.preventDefault();
                            $(this).dialog('destroy').remove();
                        }
                    });
                }
            }
        );
    }


    function openForm(form, image, spinnerTarget) {
        requireOnce(['jqueryui', 'css!/Content/css/lib/common/jquery-ui-1.11.2'], function (Jqueryui) {
            jqueryui = Jqueryui;
        },
            function () {
                if (!jqueryui) {
                    $.extend($.ui.dialog.prototype.options, {
                        height: "auto",
                        autoOpen: false,
                        modal: true,
                        resizable: false,
                        minHeight: 0,
                        closeOnEscape: true,
                        close: function (event, ui) {
                            event.preventDefault();
                            $(this).dialog('destroy').remove();
                        }
                    });
                }
            }
        );

        initJqueryUI();
    }

    var dialog = {
        initJqueryUI: function() {
            initJqueryUI();
        },
        openForm: function(form, image, spinnerTarget) {
            openForm(form, image, spinnerTarget);
        },
        test: function() {
            //            $.fancybox.message.info("Thanks for subscribing to our monthly newsletter.");
            var msgBox = require('msgBox');
            msgBox.message("No rows are selected");
        }

    };

    return dialog;
});