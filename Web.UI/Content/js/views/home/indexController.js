define(function(require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var features;

    var _spinnerArea;
    var _tryItNowButton;
    var _featureName;

    function loadSpinner() {
        _spinnerArea = system.spinnerArea;
        system.startSpinner();
    }

    function loadFeature() {
        loadSpinner();
        requireOnce(["features", "jqueryUiOptions"], function (Features) {
                features = Features;
            }, function() {
                features.callerSpinner = _spinnerArea;
                features.tryItNowButton = _tryItNowButton;
                features.featureName = _featureName;
                $.ajax({
                    url: constants.FEATURES_URL,
                    success: function(data) {
                        var $dialogContainer = $("#featuresForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"featuresForm\"></div>").dialog({
                            width: 900,
                            title: "Features",
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#featuresForm").empty().append(data);
                        if (features.featuresController) {
                            features.featuresController.open();
                        }
                    }
                });
            }
        );

    }

    $("#feature1").unbind("click").bind("click", function(e) { //  starting point
        _tryItNowButton = "Try Starting Point";
        _featureName = constants.STARTING_POINT;
        loadFeature();
        return false;
    });

    $("#feature2").unbind("click").bind("click", function(e) { //  starting point
        _tryItNowButton = "Try Find Person";
        _featureName = constants.FIND_PERSON;
        loadFeature();
        return false;
    });

    $("#feature3").unbind("click").bind("click", function(e) { //  starting point
        _tryItNowButton = "Try Hints";
        _featureName = constants.HINTS;
        loadFeature();
        return false;
    });

    $("#feature4").unbind("click").bind("click", function(e) { //  starting point
        _tryItNowButton = "Try Incomplete Ordinances";
        _featureName = constants.INCOMPLETE_ORDINANCES;
        loadFeature();
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