define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    $("#startingPoint").unbind("click").bind("click", function (e) {
        system.setCookie(constants.LAST_CALLED, constants.STARTING_POINT, 1);
        window.location.href = constants.RESEARCH_URL;
        return false;
    });

    system.requireQueue([
        'lazyload',
        'greensock',
        'transitions',
        'layerslider'
    ], function () {
        $('#layerslider').layerSlider({
            imgPreload: false,
            lazyLoad: true,
            pauseOnHover: true,
            autoPlayVideos: false,
            autoStart: true,
            firstLayer: 1,
            showBarTimer: true,
            showCircleTimer: false,
            responsive: true,
            skin: 'borderlesslight',
            skinsPath: '/Content/LayerSlider/layerslider/skins/'
        });
        $("img.lazy").lazyload({
            threshold: 200
        });

    });


});