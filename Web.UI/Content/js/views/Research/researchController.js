define(function(require) {
    var $ = require("jquery");
    var system = require("system");
    var constants = require("constants");
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var researchHelper;
    var msgBox;

    $("#startingPoint").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.startingPoint();
        });

        return false;
    });

    $("#findClues").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.findClues();
        });
        return false;
    });

    $("#retrieve").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.retrieve();
        });
        return false;
    });

    $("#findPerson").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.findPerson();
        });
        return false;
    });

    $("#possibleDuplicates").unbind("click").bind("click", function (e) {
         requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.possibleDuplicates();
        });
        return false;
    });

    $("#hints").unbind("click").bind("click", function (e) {
         requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.hints();
        });
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
    
    $("#features").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.features();
        });
        return false;
    });

    $("#incompleteOrdinances").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.incompleteOrdinances();
        });
        return false;
    });

    $("#dateProblems").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.dateProblems();
        });
        return false;
    });

    $("#placeProblems").unbind("click").bind("click", function (e) {
        requireOnce(['researchHelper'], function (ResearchHelper) {
            researchHelper = ResearchHelper;
        }, function () {
            researchHelper.placeProblems();
        });
        return false;
    });

    $("#clearStorage").unbind("click").bind("click", function(e) {
        requireOnce(['system'], function (System) {
            system = System;
        }, function () {
            system.clearStorage();
            //        $.fancybox.message.info("Cleared Storage Successfully");
        });
        return false;
    });

    $("#refreshAndReset").unbind("click").bind("click", function(e) {
        requireOnce(['system'], function (System) {
            system = System;
        }, function () {
            system.clearStorage();
            system.relogin();
            //        $.fancybox.message.info("Cleared Storage Successfully");
        });
        return false;
    });

    $("#refresh").unbind("click").bind("click", function(e) {
        requireOnce(['system'], function (System) {
            system = System;
        }, function () {
            system.relogin();
            //        $.fancybox.message.info("Cleared Storage Successfully");
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

    var lastCalled = system.getCookie(constants.LAST_CALLED);

    if (lastCalled && lastCalled === constants.STARTING_POINT) {
        system.deleteCookie(constants.LAST_CALLED);
        $('#startingPoint').click();
    }

    var researchController = {
    };

    return researchController;

});


//# sourceURL=researchController.js