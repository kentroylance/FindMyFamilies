define(function(require) {
    var $ = require("jquery");
    var system = require("system");
    var constants = require("constants");
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var researchHelper;
    var msgBox;

    $("#startingPoint").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.startingPoint();
        });

        return false;
    });

    $("#findClues").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.findClues();
        });
        return false;
    });

    $("#retrieve").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.retrieve();
        });
        return false;
    });

    $("#findPerson").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.findPerson();
        });
        return false;
    });

    $("#possibleDuplicates").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.possibleDuplicates();
        });
        return false;
    });

    $("#hints").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.hints();
        });
        return false;
    });


    $("#features").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.features();
        });
        return false;
    });

    $("#ordinances").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.ordinances();
        });
        return false;
    });

    $("#dateProblems").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.dateProblems();
        });
        return false;
    });

    $("#placeProblems").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.placeProblems();
        });
        return false;
    });

    $("#clearStorage").unbind("click").bind("click", function(e) {
        system.clearStorage();
        return false;
    });

    $("#refreshAndReset").unbind("click").bind("click", function(e) {
        system.clearStorage();
//        researchHelper.msgBox.message("Cleared Storage Successfully");
        system.relogin();
        return false;
    });

    $("#refresh").unbind("click").bind("click", function(e) {
       system.relogin();
       return false;
    });

    $("#feedback").unbind("click").bind("click", function(e) {
        requireOnce(['researchHelper'], function(ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function() {
            researchHelper.feedback();
        });
        return false;
    });


//    function startSessionAlive() {
//        create delay 10 second
//        system.KeepSessionAlive();
//
//    }
//
//
//    startSessionAlive();
    //    

    function loadLastCalled() {
        var lastCalled = system.getCookie(constants.LAST_CALLED);
        system.deleteCookie(constants.LAST_CALLED);

        if (lastCalled) {
            switch (lastCalled) {
            case constants.STARTING_POINT:
                $('#startingPoint').click();
                break;
            case constants.FIND_PERSON:
                $('#findPerson').click();
                break;
            case constants.HINTS:
                $('#hints').click();
                break;
            case constants.INCOMPLETE_ORDINANCES:
                $('#ordinances').click();
                break;
            default:
                break;
            }

        }

    }

    function delayLoading() {
        system.keepSessionAlive();

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

    $(document).ready(function() {
        loadLastCalled();
        setTimeout(delayLoading, 1000);
    });

    var researchController = {
    
    };

    return researchController;

});


//# sourceURL=researchController.js