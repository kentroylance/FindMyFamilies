﻿define(function(require) {
    var $ = require('jquery');
    var system = require('system');

    system.requireQueue([
        'lazyload',
        'greensock',
        'transitions',
        'layerslider'
    ], function() {

        var domReady = require('domReady');
        domReady(function() {

            $('#layerslider').layerSlider({
                imgPreload: false,
                lazyLoad: true,
                pauseOnHover: false,
                autoPlayVideos: false,
                autoStart: false,
                firstLayer: 2,
                skin: 'borderlesslight',
                skinsPath: '/Content/LayerSlider/layerslider/skins/'
            });
            $("img.lazy").lazyload({
                threshold: 200
            });
            require('_layoutController');
        });

    });


});