define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var person = require('person');
    var personUrlOptions;
    var msgBox;
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    var _startingPointController;
    var _findPersonController;
    var _startingPointReportController;
    var _possibleDuplicatesController;
    var _possibleDuplicatesReportController;
    var _retrieveController;
    var _personUrlOptionsController;
    var _personUrlsController;
    var _hintsController;
    var _hintsReportController;
    var _feedbackController;
    var _featuresController;
    var _incompleteOrdinancesController;
    var _incompleteOrdinancesReportController;
    var _dateProblemsController;
    var _dateProblemsReportController;
    var _placeProblemsController;
    var _placeProblemsReportController;
    var _findCluesController;
    var _findCluesReportController;
    var spinnerArea;

    function loadSpinner() {
        spinnerArea = system.spinnerArea;
        system.startSpinner();
    }

    function startingPoint(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["startingPoint", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(StartingPoint) {
                    StartingPoint.callerSpinner = spinnerArea;
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
            loadSpinner();
            requireOnce(["findClues", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(FindClues) {
                    FindClues.callerSpinner = spinnerArea;
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
            loadSpinner();

            requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
            }, function () {
                var retrieve = require('retrieve');
                retrieve.callback = callback;
                retrieve.callerSpinner = spinnerArea;
                $.ajax({
                    url: constants.RETRIEVE_URL,
                    success: function(data) {
                        var $dialogContainer = $("#retrieveForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"retrieveForm\"></div>").dialog({
                                width: 825,
                                title: "Retrieve",
                                open: function() {
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
    }

    function findPerson(callback) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["formValidation", "jqueryUiOptions", "bootstrapValidation", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function () {
                }, function() {
                    var findPerson = require('findPerson');
                    findPerson.callback = callback;
                    findPerson.callerSpinner = spinnerArea;
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
        loadSpinner();
        requireOnce(['personUrlOptions'], function(PersonUrlOptions) {
            personUrlOptions = PersonUrlOptions;
            personUrlOptions.callerSpinner = spinnerArea;
        }, function() {
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
                    success: function (data) {
                        if (data && data.errorMessage) {
                            msgBox.error(data.errorMessage);
                        } else {
                            var $dialogContainer = $("#personUrlsForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"personUrlsForm\"></div>").dialog({
                                width: 850,
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
                            if (_personUrlsController) {
                                _personUrlsController.open();
                            }
                        }
                    }
                });
            } else {
                system.relogin();
            }
        });
        return false;
    }

    function possibleDuplicates(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["possibleDuplicates", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(PossibleDuplicates) {
                    PossibleDuplicates.callerSpinner = spinnerArea;
                }, function() {
                    $.ajax({
                        url: constants.POSSIBLE_DUPLICATES_URL,
                        success: function(data) {
                            var $dialogContainer = $("#possibleDuplicatesForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"possibleDuplicatesForm\"></div>").dialog({
                                width: 775,
                                title: "Possible Duplicates",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#possibleDuplicatesForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
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

    function hints(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["hints", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function (Hints) {
                    Hints.callerSpinner = spinnerArea;
                }, function() {
                    $.ajax({
                        url: constants.HINTS_URL,
                        success: function(data) {
                            var $dialogContainer = $("#hintsForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"hintsForm\"></div>").dialog({
                                width: 775,
                                title: "Hints",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#hintsForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
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

    function feedback(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["feedback", "jqueryUiOptions"], function (Feedback) {
                Feedback.callerSpinner = spinnerArea;
            }, function () {
                $.ajax({
                    url: constants.FEEDBACK_URL,
                    success: function (data) {
                        var $dialogContainer = $("#feedbackForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"feedbackForm\"></div>").dialog({
                            width: 775,
                            title: "Feedback",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#feedbackForm").empty().append(data);
                        if (id) {
                            person.id = id;
                            person.name = name;
                        }
                        if (_feedbackController) {
                            _feedbackController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function features(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["features", "jqueryUiOptions"], function (Features) {
                Features.callerSpinner = spinnerArea;
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
                        if (id) {
                            person.id = id;
                            person.name = name;
                        }
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

    function dateProblems(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["dateProblems", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function (DateProblems) {
                DateProblems.callerSpinner = spinnerArea;
            }, function () {
                    $.ajax({
                        url: constants.DATE_PROBLEMS_URL,
                        success: function(data) {
                            var $dialogContainer = $("#dateProblemsForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"dateProblemsForm\"></div>").dialog({
                                width: 775,
                                title: "Date Problems",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#dateProblemsForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
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

    function incompleteOrdinances(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["incompleteOrdinances", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function (IncompleteOrdinances) {
                IncompleteOrdinances.callerSpinner = spinnerArea;
            }, function () {
                    $.ajax({
                        url: constants.INCOMPLETE_ORDINANCES_URL,
                        success: function(data) {
                            var $dialogContainer = $("#incompleteOrdinancesForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"incompleteOrdinancesForm\"></div>").dialog({
                                width: 775,
                                title: "Incomplete Ordinances",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#incompleteOrdinancesForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
                            if (_incompleteOrdinancesController) {
                                _incompleteOrdinancesController.open();
                            }
                        }
                    });
                }
            );
        } else {
            system.relogin();
        }
    }

    function placeProblems(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["placeProblems", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function (PlaceProblems) {
                    PlaceProblems.callerSpinner = spinnerArea;
                }, function() {
                    $.ajax({
                        url: constants.PLACE_PROBLEMS_URL,
                        success: function(data) {
                            var $dialogContainer = $("#placeProblemsForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"placeProblemsForm\"></div>").dialog({
                                width: 775,
                                title: "Place Problems",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#placeProblemsForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                            }
                            if (_placeProblemsController) {
                                _placeProblemsController.open();
                            }
                        }
                    });
                }
            );
        } else {
            system.relogin();
        }
    }

    function personUrlOptions(id, name) {
        if (id && system.isAuthenticated()) {
            loadSpinner();
            requireOnce(['personUrlOptions'], function(PersonUrlOptions) {
                personUrlOptions = PersonUrlOptions;
                personUrlOptions.callerSpinner = spinnerArea;
            }, function () {
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
            });
        } else {
            system.relogin();
        }
    }

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
        hints: function(id, name) {
            return hints(id, name);
        },
        feedback: function (id, name) {
            return feedback(id, name);
        },
        features: function (id, name) {
            return features(id, name);
        },
        dateProblems: function(id, name) {
            return dateProblems(id, name);
        },
        placeProblems: function(id, name) {
            return placeProblems(id, name);
        },
        incompleteOrdinances: function(id, name) {
            return incompleteOrdinances(id, name);
        },
        displayPersonUrls: function() {
            displayPersonUrls();
        },
        personUrlOptions: function(id, name) {
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
        get feedbackController() {
            return _feedbackController;
        },
        set feedbackController(value) {
            _feedbackController = value;
        },
        get featuresController() {
            return _featuresController;
        },
        set featuresController(value) {
            _featuresController = value;
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
        get placeProblemsController() {
            return _placeProblemsController;
        },
        set placeProblemsController(value) {
            _placeProblemsController = value;
        },
        get placeProblemsReportController() {
            return _placeProblemsReportController;
        },
        set placeProblemsReportController(value) {
            _placeProblemsReportController = value;
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