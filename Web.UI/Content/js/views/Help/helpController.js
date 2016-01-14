define(function(require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var msgBox;

    $(document).ready(function () {
        requireOnce(["jqueryUiOptions", 'fancybox', 'fancyboxMedia', "css!/Content/js/vendor/fancybox/jquery.fancybox.css"], function () {
        }, function () {
            $('.fancyboxvideo').fancybox({
                openEffect: 'none',
                closeEffect: 'none',
                helpers: {
                    media: true,
                    title: {
                        type: 'inside'
                    }
                },
                fitToView: false,
                aspectRatio: true,
                maxWidth: "100%",
                maxHeight: "100%",
                beforeLoad: function () {
                    this.title = $(this.element).attr('caption');
                },
                afterLoad: function () {
                    this.width = $(this.element).data("width");
                    this.height = $(this.element).data("height");
                }
            });
            $(".helpDialog").fancybox({
                helpers: {
                    title: null,
                    overlay: null,
                    padding: [20, 20, 20, 20]
                },
                'transitionIn': 'none',
                'transitionOut': 'none',
                'changeFade': 0,
                openEffect: 'none',
                closeEffect: 'none'
            });
        });
    });


    var helpController = {
    };

    return helpController;

});


//# sourceURL=helpController.js