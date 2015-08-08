define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var _featuresController;
    var _spinnerArea;
    var _tryItNowButton;
    var _featureName;

    function loadSpinner() {
        _spinnerArea = system.getSpinnerArea();
        system.startSpinner();
    }

    function loadFeature() {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["features", "jqueryUiOptions"], function (Features) {
                Features.callerSpinner = _spinnerArea;
                Features.tryItNowButton = _tryItNowButton;
                Features.featureName = _featureName;
            }, function () {
                $.ajax({
                    url: constants.FEATURES_URL,
                    success: function (data) {
                        var $dialogContainer = $("#featuresForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"featuresForm\"></div>").dialog({
                            width: 900,
                            title: "Features",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#featuresForm").empty().append(data);
                        if (_featuresController) {
                            _featuresController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }

    }

    $("#feature1").unbind("click").bind("click", function (e) {     //  starting point
        _tryItNowButton = "Try Starting Point";
        _featureName = constants.STARTING_POINT;
        loadFeature();
        return false;
    });

    $("#feature2").unbind("click").bind("click", function (e) {     //  starting point
        _tryItNowButton = "Try Find Persoon";
        _featureName = constants.FIND_PERSON;
        loadFeature();
        return false;
    });

    $("#feature3").unbind("click").bind("click", function (e) {     //  starting point
        _tryItNowButton = "Try Hints";
        _featureName = constants.HINTS;
        loadFeature();
        return false;
    });

    $("#feature4").unbind("click").bind("click", function (e) {     //  starting point
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

    var indexController = {
        get featuresController() {
            return _featuresController;
        },
        set featuresController(value) {
            _featuresController = value;
        }
    };

    system.deleteCookie(constants.LAST_CALLED);

    return indexController;




});