define(function(require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var researchHelper;


    function loadFeature(tryItNowButton, featureName, dialogVideos) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.features(tryItNowButton, featureName, dialogVideos);
        });
    }
    
    $("#feature1").unbind("click").bind("click", function (e) { //  starting point
        loadFeature("Try Google Search", constants.GOOGLE_SEARCH, "https://player.vimeo.com/video/96324885?title=0&amp;byline=0&amp;portrait=0");
        return false;
    });

    $("#feature2").unbind("click").bind("click", function (e) { //  starting point
        loadFeature("Try Starting Point", constants.STARTING_POINT, "https://player.vimeo.com/video/96324885?title=0&amp;byline=0&amp;portrait=0");
        return false;
    });

    $("#feature3").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Find Person", constants.FIND_PERSON, "https://player.vimeo.com/video/136109637?title=0&amp;byline=0&amp;portrait=0");
        return false;
    });

    $("#feature4").unbind("click").bind("click", function(e) { //  starting point
        loadFeature("Try Hints", constants.HINTS, "https://player.vimeo.com/video/136152023?title=0&amp;byline=0&amp;portrait=0");
        return false;
    });


//    $("#feature4").unbind("click").bind("click", function(e) { //  starting point
//        loadFeature("Try Incomplete Ordinances", constants.INCOMPLETE_ORDINANCES, "https://player.vimeo.com/video/136109637?title=0&amp;byline=0&amp;portrait=0");
//        return false;
//    });

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

    });

    function delayLoading() {
        $("img.lazy").lazyload({
            threshold: 200
        });
        requireOnce(['researchHelper', 'findPersonHelper', 'findPerson', 'retrieve', 'string', "formValidation", "jqueryUiOptions", "bootstrapValidation", 'fancybox', 'fancyboxMedia', "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css", "css!/Content/js/vendor/fancybox/jquery.fancybox.css"], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.person.getPersonInfo();

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
            $("#helpDialog").fancybox({
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

    }

    $(document).ready(function () {
        setTimeout(delayLoading, 1000);
    });


    var indexController = {
    };

    system.deleteCookie(constants.LAST_CALLED);

    return indexController;


});