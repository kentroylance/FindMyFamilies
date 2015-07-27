define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var person = require('person');
    var personUrlOptions;
    var retrieve;
    var msgBox;
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var _startingPointController;
    var _startingPointReportController;
    var _findPersonController;
    var _possibleDuplicatesController;
    var _possibleDuplicatesReportController;
    var _retrieveController;
    var _personUrlOptionsController;
    var _personUrlsController;
    var _hintsController;
    var _hintsReportController;
    var _incompleteOrdinancesController;
    var _incompleteOrdinancesReportController;
    var _dateProblemsController;
    var _dateProblemsReportController;
    var _findCluesController;
    var _findCluesReportController;


    function startingPoint(id, name) {
        if (system.isAuthenticated()) {
            system.startSpinner();
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                }, function() {
                    $.ajax({
                        url: constants.STARTING_POINT_URL,
                        success: function(data) {
                            var $dialogContainer = $("#startingPointForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"startingPointForm\"></div>").dialog({
                                width: 775,
                                title: "Starting Point",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#startingPointForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
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

    function findClues(id, name) {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                }, function() {
                    $.ajax({
                        url: constants.FIND_CLUES_URL,
                        success: function(data) {
                            var $dialogContainer = $("#findCluesForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"findCluesForm\"></div>").dialog({
                                width: 775,
                                title: "Find Clues",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#findCluesForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
                            if (_findCluesController) {
                                _findCluesController.open();
                            }
                        }
                    });
                }
            );
        } else {
            system.relogin();
        }
    }

    function retrieve(callback, id, name) {
        if (system.isAuthenticated()) {
            system.startSpinner();
            requireOnce(["retrieve", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(Retrieve) {
                retrieve = Retrieve;
            }, function() {
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
                        if (id) {
                            person.id = id;
                            person.name = name;
                        }
                        if (_retrieveController) {
                            _retrieveController.open();
                        }
                    }
                });
            });
        } else {
            system.relogin();
        }
//
//
//        if (system.isAuthenticated()) {
//            requireOnce(["retrieve", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(Retrieve) {
//                retrieveModel = Retrieve;
//            }, function() {
//                retrieveModel.callback = callback;
//                retrieveModel.callerSpinner = system.target.id;
//                $.ajax({
//                    url: constants.RETRIEVE_URL,
//                    success: function(data) {
//                        var $dialogContainer = $("#retrieveForm");
//                        var $detachedChildren = $dialogContainer.children().detach();
//                        $("<div id=\"retrieveForm\"></div>").dialog({
//                                width: 825,
//                                title: "Retrieve",
//                                open: function() {
//                                    $detachedChildren.appendTo($dialogContainer);
//                                    $(this).css("maxHeight", 700);
//                                }
//                            }
//                        );
//                        $("#retrieveForm").empty().append(data);
//                        if (id) {
//                            person.id = id;
//                            person.name = name;
//                        }
//                        if (_retrieveController) {
//                            _retrieveController.open();
//                        }
//                    }
//                });
//            });
//        } else {
//            system.relogin();
//        }
    }

    function findPerson(callback) {
        system.startSpinner();
        if (system.isAuthenticated()) {
            requireOnce(["formValidation", "bootstrapValidation", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function() {
                    //                require("css!/Content/css/vendor/formValidation.min.css");
                    //                var formValidation = require("formValidation");
                    //                var bootstrapValidation = require("bootstrapValidation");

                    //                requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css", "bootstrapTable", "formValidation", "bootstrapValidation"], function () {
                }, function() {
                    var findPerson = require('findPerson');
                    findPerson.callback = callback;
                    findPerson.callerSpinner = system.target.id;
                    $.ajax({
                        url: constants.FIND_PERSON_URL,
                        success: function(data) {
                            var $dialogContainer = $("#findPersonForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"findPersonForm\"></div>").dialog({
                                    width: 1100,
                                    title: "Find Person",
                                    open: function() {
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

    function displayPersonUrls() {
        e.preventDefault();
        system.startSpinner();
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
        }, function() {
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function hints(e) {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
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
                        if (_hintsController) {
                            _hintsController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function dateProblems(e) {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
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
                        if (_dateProblemsController) {
                            _dateProblemsController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function incompleteOrdinances(e) {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions", "bootstrapTable", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                $.ajax({
                    url: constants.INCOMPLETE_ORDINANCES_URL,
                    success: function (data) {
                        var $dialogContainer = $("#incompleteOrdinancesForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"incompleteOrdinancesForm\"></div>").dialog({
                            width: 775,
                            title: "Incomplete Ordinances",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#incompleteOrdinancesForm").empty().append(data);
                        if (_incompleteOrdinancesController) {
                            _incompleteOrdinancesController.open();
            if (personUrlOptions.id && system.isAuthenticated()) {
                $.ajax({
                    url: constants.DISPLAY_PERSON_URLS_URL,
                    data: {
                        "personId": personUrlOptions.id,
                        "includeMaidenName": person.includeMaidenName,
                        "includeMiddleName": person.includeMiddleName,
                        "includePlace": person.includePlace,
                        "yearRange": person.yearRange
                    },
                    success: function(data) {
                        var $dialogContainer = $("#personUrlsForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"personUrlsForm\"></div>").dialog({
                            width: 600,
                            title: "Research Family",
                            resizable: false,
                            minHeight: 0,
                            maxHeight: $(window).height(),
                            create: function() {
                                $(this).css("maxHeight", 700);
                            },
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).dialog("option", "maxHeight", $(window).height());
                            },
                            close: function(event, ui) {
                                event.preventDefault();
                                $(this).dialog("destroy").remove();
                            }
                        });
                        $("#personUrlsForm").empty().append(data);
                        if (_personUrlsController) {
                            _personUrlsController.open();
                        }

                    }
                });
            } else {
                system.relogin();
            }
        });
        return false;
    }

    function personUrlOptions(id, name) {
        if (id && system.isAuthenticated()) {
            system.startSpinner();
            requireOnce(['personUrlOptions'], function(PersonUrlOptions) {
                personUrlOptions = PersonUrlOptions;
            }, function() {
                $.ajax({
                    url: constants.PERSON_URL_OPTIONS_URL,
                    success: function(data) {
                        var $dialogContainer = $("#personUrlOptionsForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"personUrlOptionsForm\"></div>").dialog({
                            width: 400,
                            title: "Search Options",
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                            }

                        });
                        $("#personUrlOptionsForm").empty().append(data);
                        personUrlOptions.id = id;
                        personUrlOptions.name = name;
                        if (_personUrlOptionsController) {
                            _personUrlOptionsController.open();
                        }
                    }
                });
    var researchHelper = {
        findPerson: function(deferred) {
            return findPerson(deferred);
        },
        retrieve: function(deferred, id, name) {
            return retrieve(deferred, id, name);
        },
        startingPoint: function(id, name) {
            return startingPoint(id, name);
        },
        findClues: function(id, name) {
            return findClues(id, name);
        },
        possibleDuplicates: function(id, name) {
            return possibleDuplicates(id, name);
        },
        hints: function (e) {
            return hints(e);
        },
        dateProblems: function (e) {
            return dateProblems(e);
        },
        incompleteOrdinances: function (e) {
            return incompleteOrdinances(e);
        },
        displayPersonUrls: function() {
            displayPersonUrls();
        },
        personUrlOptions: function(id, name) {
            personUrlOptions(id, name);
        },
        get findPersonController() {
            return _findPersonController;
        },
        set findPersonController(value) {
            _findPersonController = value;
        },
        get startingPointController() {
            return _startingPointController;
        },
        set startingPointController(value) {
            _startingPointController = value;
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
        get possibleDuplicatesReportController() {
            return _possibleDuplicatesReportController;
        },
        set possibleDuplicatesReportController(value) {
            _possibleDuplicatesReportController = value;
        },
        get hintsController() {
            return _hintsController;
        },
        set hintsController(value) {
            _hintsController = value;
        },
        get hintsReportController() {
            return _hintsReportController;
        },
        set hintsReportController(value) {
            _hintsReportController = value;
        },
        get incompleteOrdinancesController() {
            return _incompleteOrdinancesController;
        },
        set incompleteOrdinancesController(value) {
            _incompleteOrdinancesController = value;
        },
        get incompleteOrdinancesReportController() {
            return _incompleteOrdinancesReportController;
        },
        set incompleteOrdinancesReportController(value) {
            _incompleteOrdinancesReportController = value;
        },
        get dateProblemsController() {
            return _dateProblemsController;
        },
        set dateProblemsController(value) {
            _dateProblemsController = value;
        },
        get dateProblemsReportController() {
            return _dateProblemsReportController;
        },
        set dateProblemsReportController(value) {
            _dateProblemsReportController = value;
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
        get findCluesController() {
            return _findCluesController;
        },
        set findCluesController(value) {
            _findCluesController = value;
        },
        get findCluesReportController() {
            return _findCluesReportController;
        },
        set findCluesReportController(value) {
            _findCluesReportController = value;
        }
    };

    return researchHelper;
});


//# sourceURL=findPersonHelper.js