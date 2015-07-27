define(function(require) {
    var $ = require("jquery");
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var researchHelper;
    var msgBox;
    var system;

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

    $("#researchFamily").unbind("click").bind("click", function (e) {
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


    var researchController = {
    };

    return researchController;

});


//# sourceURL=researchController.js