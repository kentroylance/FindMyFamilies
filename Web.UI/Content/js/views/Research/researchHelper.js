define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var person = require('person');
    var research = require("research");

    var msgBox;
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();
    require("lazyload");

    function findPerson(e, callback) {
        e.preventDefault();
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["bootstrapTable", "formValidation", "bootstrapValidation", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function () {
                //                require("css!/Content/css/vendor/formValidation.min.css");
                //                var formValidation = require("formValidation");
                //                var bootstrapValidation = require("bootstrapValidation");

                //                requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css", "bootstrapTable", "formValidation", "bootstrapValidation"], function () {
            }, function () {
                var findPerson = require('findPerson');
                findPerson.callback = callback;
                findPerson.callerSpinner = system.target.id;
                $.ajax({
                    url: constants.FIND_PERSON_URL,
                    success: function (data) {
                        var $dialogContainer = $("#findPersonForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"findPersonForm\"></div>").dialog({
                            width: 1100,
                            title: "Find Person",
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .15)),
                                collision: "none"
                            },
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        }
                        );
                        $("#findPersonForm").empty().append(data);
                        if (research && research.findPersonController) {
                            research.findPersonController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }


    function possibleDuplicates(e) {
        e.preventDefault();
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                var possibleDuplicate = require('possibleDuplicates');
                possibleDuplicate.callerSpinner = system.target.id;
                $.ajax({
                    url: constants.POSSIBLE_DUPLICATES_URL,
                    success: function (data) {
                        var $dialogContainer = $("#possibleDuplicatesForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"possibleDuplicatesForm\"></div>").dialog({
                            width: 775,
                            title: "Possible Duplicates",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#possibleDuplicatesForm").empty().append(data);
                        if (research && research.possibleDuplicatesController) {
                            research.possibleDuplicatesController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }


    var researchHelper = {
        findPerson: function (e, deferred) {
            return findPerson(e, deferred);
        },
        possibleDuplicates: function (e) {
            return possibleDuplicates(e);
        }

    };


    return researchHelper;
});


//# sourceURL=findPersonHelper.js