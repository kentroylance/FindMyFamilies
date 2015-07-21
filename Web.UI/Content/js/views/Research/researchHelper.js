define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var person = require('person');
    var personUrlOptionsModel = require('personUrlOptions');
    var msgBox;
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();
    require("lazyload");

    var _startingPointController;
    var _findPersonController;
    var _startingPointReportController;
    var _possibleDuplicatesController;
    var _retrieveController;
    var _personUrlOptionsController;
    var _personUrlsController;


    function startingPoint() {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                $.ajax({
                    url: constants.STARTING_POINT_URL,
                    success: function (data) {
                        var $dialogContainer = $("#startingPointForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"startingPointForm\"></div>").dialog({
                            width: 775,
                            title: "Starting Point",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#startingPointForm").empty().append(data);
                        if (_startingPointController) {
                            _startingPointController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function retrieve(e, callback) {
        e.preventDefault();
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                var retrieve = require('retrieve');
                retrieve.callback = callback;
                retrieve.callerSpinner = system.target.id;
                $.ajax({
                    url: constants.RETRIEVE_URL,
                    success: function (data) {
                        var $dialogContainer = $("#retrieveForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"retrieveForm\"></div>").dialog({
                            width: 825,
                            title: "Retrieve",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        }
                        );
                        $("#retrieveForm").empty().append(data);
                        if (_retrieveController) {
                            _retrieveController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }


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
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        }
                        );
                        $("#findPersonForm").empty().append(data);
                        if (_findPersonController) {
                            _findPersonController.open();
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
                        if (_possibleDuplicatesController) {
                            _possibleDuplicatesController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function displayPersonUrls() {
        if (personUrlOptionsModel.id && system.isAuthenticated()) {
            $.ajax({
                url: constants.DISPLAY_PERSON_URLS_URL,
                data: {
                    "personId": personUrlOptionsModel.id,
                    "includeMaidenName": person.includeMaidenName,
                    "includeMiddleName": person.includeMiddleName,
                    "includePlace": person.includePlace,
                    "yearRange": person.yearRange
                },
                success: function (data) {
                    var $dialogContainer = $("#personUrlsForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"personUrlsForm\"></div>").dialog({
                        width: 600,
                        title: "Research Family",
                        resizable: false,
                        minHeight: 0,
                        maxHeight: $(window).height(),
                        create: function () {
                            $(this).css("maxHeight", 700);
                        },
                        open: function () {
                            $detachedChildren.appendTo($dialogContainer);
                            $(this).dialog("option", "maxHeight", $(window).height());
                        },
                        close: function (event, ui) {
                            event.preventDefault();
                            $(this).dialog("destroy").remove();
                        }
                    });
                    $("#personUrlsForm").empty().append(data);
                }
            });
        } else {
            system.relogin();
        }
        return false;
    }

    function personUrlOptions(id, name) {
        if (id && system.isAuthenticated()) {
            system.startSpinner();
            $.ajax({
                url: constants.PERSON_URL_OPTIONS_URL,
                success: function (data) {
                    var $dialogContainer = $("#personUrlOptionsForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"personUrlOptionsForm\"></div>").dialog({
                        width: 400,
                        title: "Search Options",
                        open: function () {
                            $detachedChildren.appendTo($dialogContainer);
                        }

                    });
                    $("#personUrlOptionsForm").empty().append(data);
                    personUrlOptionsModel.id = id;
                    personUrlOptionsModel.name = name;
                    if (_personUrlOptionsController) {
                        _personUrlOptionsController.open();
                    }
                }
            });

        } else {
            system.relogin();
        }
    }

    function hints(e) {
        e.preventDefault();
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                var hints = require('hints');
                hints.callerSpinner = system.target.id;
                $.ajax({
                    url: constants.HINTS_URL,
                    success: function (data) {
                        var $dialogContainer = $("#hintsForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"hintsForm\"></div>").dialog({
                            width: 775,
                            title: "Hints",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#hintsForm").empty().append(data);
                        if (research && research.hintsController) {
                            research.hintsController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    //function incompleteOrdinances(e) {
    //    e.preventDefault();
    //    system.startSpinner();
    //    if (system.isAuthenticated()) {
    //        requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
    //        }, function () {
    //            var incompleteOrdinances = require('incompleteOrdinances');
    //            incompleteOrdinances.callerSpinner = system.target.id;
    //            $.ajax({
    //                url: constants.INCOMPLETE_ORDINANCES_URL,
    //                success: function (data) {
    //                    var $dialogContainer = $("#incompleteOrdinancesForm");
    //                    var $detachedChildren = $dialogContainer.children().detach();
    //                    $("<div id=\"incompleteOrdinancesForm\"></div>").dialog({
    //                        width: 775,
    //                        title: "Incomplete Ordinances",
    //                        open: function () {
    //                            $detachedChildren.appendTo($dialogContainer);
    //                        }
    //                    });
    //                    $("#incompleteOrdinancesForm").empty().append(data);
    //                    if (research && research.incompleteOrdinancesController) {
    //                        research.incompleteOrdinancesController.open();
    //                    }
    //                }
    //            });
    //        }
    //        );
    //    } else {
    //        system.relogin();
    //    }
    //}

    function dateProblems(e) {
        e.preventDefault();
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                var dateProblems = require('dateProblems');
                dateProblems.callerSpinner = system.target.id;
                $.ajax({
                    url: constants.DATE_PROBLEMS_URL,
                    success: function (data) {
                        var $dialogContainer = $("#dateProblemsForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"dateProblemsForm\"></div>").dialog({
                            width: 775,
                            title: "Date Problems",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#dateProblemsForm").empty().append(data);
                        if (research && research.dateProblemsController) {
                            research.dateProblemsController.open();
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
        retrieve: function (e, deferred) {
            return retrieve(e, deferred);
        },
        startingPoint: function (e) {
            return startingPoint(e);
        },
        possibleDuplicates: function (e) {
            return possibleDuplicates(e);
        },
        displayPersonUrls: function () {
            displayPersonUrls();
        },
        personUrlOptions: function (id, name) {
            personUrlOptions(id, name);
        },
        get startingPointController() {
            return _startingPointController;
        },
        set startingPointController(value) {
            _startingPointController = value;
        },
        get findPersonController() {
            return _findPersonController;
        },
        set findPersonController(value) {
            _findPersonController = value;
        },
        get startingPointReportController() {
            return _startingPointReportController;
        },
        set startingPointReportController(value) {
            _startingPointReportController = value;
        },
        get possibleDuplicatesController() {
            return _possibleDuplicatesController;
        },
        set possibleDuplicatesController(value) {
            _possibleDuplicatesController = value;
        },
        get retrieveController() {
            return _retrieveController;
        },
        set retrieveController(value) {
            _retrieveController = value;
        },
        get personUrlOptionsController() {
            return _personUrlOptionsController;
            },
        set personUrlOptionsController(value) {
            _personUrlOptionsController = value;
        },
        get personUrlsController() {
            return _personUrlsController;
            },
        set personUrlsController(value) {
            _personUrlsController = value;
        },
        hints: function (e) {
            return hints(e);
        },
        incompleteOrdinances: function (e) {
            return incompleteOrdinances(e);
        },
        dateProblems: function(e) {
            return dateProblems(e);
        }

    };

    return researchHelper;
});


//# sourceURL=findPersonHelper.js