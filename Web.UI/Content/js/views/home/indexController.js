define(function(require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var researchHelper;

    function loadFeature(tryItNowButton, featureName) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.features(tryItNowButton, featureName);
        });
    }
    
    $("#feature1").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Starting Point", constants.STARTING_POINT);
        return false;
    });

    $("#feature2").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Find Person", constants.FIND_PERSON);
        return false;
    });

    $("#feature3").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Hints", constants.HINTS);
        return false;
    });

    $("#feature4").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Incomplete Ordinances", constants.INCOMPLETE_ORDINANCES);
        return false;
    });

    $("#feedback").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.feedback();
        });
        return false;
    });


    system.requireQueue([
        'lazyload',
        'greensock',
        'transitions',
        'layerslider'
    ], function() {
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

    var indexController = {
    };

    system.deleteCookie(constants.LAST_CALLED);

    return indexController;


});