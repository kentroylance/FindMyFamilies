define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var person = require('person');
    var featuresModel = require('features');
    var feedbackModel = require('feedback');
    var retrieveModel = require('retrieve');
    var personUrlOptionsModel = require('personUrlOptions');
    var msgBox = require('msgBox');
    var timezone = require('jstz');
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();
    require("findPerson");
    require("fancybox");
    require("fancyboxMedia");

    var _startingPointController;
    var _findPersonController;
    var _googleSearchController;
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
    var _ordinancesController;
    var _ordinancesReportController;
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
                                person.reportId = constants.REPORT_ID;
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
            requireOnce(["findClues"], function(FindClues) {
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
                                person.reportId = constants.REPORT_ID;
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

            requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
            }, function() {
                retrieveModel.callback = callback;
                retrieveModel.callerSpinner = spinnerArea;
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
                            person.reportId = constants.REPORT_ID;
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
            requireOnce(["formValidation", "jqueryUiOptions", "bootstrapValidation", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function() {
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


    function startingPoint(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["startingPoint", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function (StartingPoint) {
                StartingPoint.callerSpinner = spinnerArea;
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
                        if (id) {
                            person.id = id;
                            person.name = name;
                            person.reportId = constants.REPORT_ID;
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

    function googleSearch(id, fullName, firstName,  middleName, lastName, birthYear, deathYear, birthPlace) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["googleSearch", "formValidation", "jqueryUiOptions", "bootstrapValidation", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function (GoogleSearch) {
                GoogleSearch.callerSpinner = spinnerArea;
            }, function () {
                $.ajax({
                    url: constants.GOOGLE_SEARCH_URL,
                    success: function (data) {
                        var $dialogContainer = $("#googleSearchForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"googleSearchForm\"></div>").dialog({
                            width: 1100,
                            title: "Google Search",
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        }
                        );
                        $("#googleSearchForm").empty().append(data);
                        if (id || fullName || firstName || middleName || lastName || birthYear || deathYear || birthPlace) {
                            person.id = id;
                            person.fullName = fullName;
                            person.firstName = firstName;
                            person.middleName = middleName;
                            person.lastName = lastName;
                            person.birthYear = birthYear;
                            person.deathYear = deathYear;
                            person.birthPlace = birthPlace;
                            var googleSearch = require('googleSearch');
                            googleSearch.copyFromPerson();
                        }
                        if (_googleSearchController) {
                            _googleSearchController.open();
                        }
                    }
                });
            }
            );
        } else {
            system.relogin();
        }
    }

    function displayPersonUrls(id, name) {
        loadSpinner();
        if (system.isAuthenticated()) {
            person.id = id;
            person.name = name;
            person.save();
            $.ajax({
                url: constants.DISPLAY_PERSON_URLS_URL,
                data: {
                    "personId": id,
                    "includeMaidenName": person.includeMaidenName,
                    "includeMiddleName": person.includeMiddleName,
                    "includePlace": person.includePlace,
                    "yearRange": person.yearRange
                },
                success: function(data) {
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
                }
            });
        } else {
            system.relogin();
        }
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
                                person.reportId = constants.REPORT_ID;
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
            requireOnce(["hints", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(Hints) {
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
                                person.reportId = constants.REPORT_ID;
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

    function feedback() {
        loadSpinner();
        requireOnce(["formValidation", "jqueryUiOptions", "bootstrapValidation", "css!/Content/css/vendor/formValidation.min.css"], function() {
            }, function() {
                feedbackModel.callerSpinner = spinnerArea;
                $.ajax({
                    url: constants.FEEDBACK_URL,
                    success: function(data) {
                        var $dialogContainer = $("#feedbackForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"feedbackForm\"></div>").dialog({
                            width: 775,
                            title: "Feedback",
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                            }
                        });
                        $("#feedbackForm").empty().append(data);
                        if (_feedbackController) {
                            _feedbackController.open();
                        }
                    }
                });
            }
        );
    }


    function features(tryItNowButton, featureName, dialogVideos) {
        loadSpinner();
        requireOnce(["jqueryUiOptions"], function() {
            }, function() {
                featuresModel.callerSpinner = spinnerArea;
                featuresModel.tryItNowButton = tryItNowButton;
                featuresModel.featureName = featureName;
                featuresModel.dialogVideos = dialogVideos;
                $.ajax({
                    url: constants.FEATURES_URL,
                    success: function(data) {
                        var $dialogContainer = $("#featuresForm");
                        var $detachedChildren = $dialogContainer.children().detach();
                        $("<div id=\"featuresForm\"></div>").dialog({
                            width: 930,
                            title: "Features",
                            open: function() {
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
    }

    function dateProblems(id, name) {
        if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["dateProblems", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(DateProblems) {
                    DateProblems.callerSpinner = spinnerArea;
                }, function() {
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
                                person.reportId = constants.REPORT_ID;
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

    function ordinances(id, name) {
          if (system.isAuthenticated()) {
            loadSpinner();
            requireOnce(["ordinances", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(Ordinances) {
                    Ordinances.callerSpinner = spinnerArea;
                }, function() {
                    $.ajax({
                        url: constants.ORDINANCES_URL,
                        success: function(data) {
                            var $dialogContainer = $("#ordinancesForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"ordinancesForm\"></div>").dialog({
                                width: 775,
                                title: "Ordinances",
                                open: function() {
                                    $detachedChildren.appendTo($dialogContainer);
                                }
                            });
                            $("#ordinancesForm").empty().append(data);
                            if (id) {
                                person.id = id;
                                person.name = name;
                                person.reportId = constants.REPORT_ID;
                            }
                            if (_ordinancesController) {
                                _ordinancesController.open();
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
            requireOnce(["placeProblems", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function(PlaceProblems) {
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
                                person.reportId = constants.REPORT_ID;
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
            });
        } else {
            system.relogin();
        }
    }

    var researchHelper = {
        findPerson: function(deferred) {
            return findPerson(deferred);
        },
        googleSearch: function (id, fullName, firstName, middleName, lastName, birthYear, deathYear, birthPlace) {
            return googleSearch(id, fullName, firstName, middleName, lastName, birthYear, deathYear, birthPlace);
        },
        retrieve: function (deferred, id, name) {
            return retrieve(deferred, id, name);
        },
        startingPoint: function(id, name) {
            return startingPoint(id, name);
        },
        feedback: function() {
            return feedback();
        },
        findClues: function (id, name) {
            return findClues(id, name);
        },
        possibleDuplicates: function(id, name) {
            return possibleDuplicates(id, name);
        },
        hints: function(id, name) {
            return hints(id, name);
        },
        get feedbackModel() {
            return feedbackModel;
        },
        set feedbackModel(value) {
            feedbackModel = value;
        },
        get featuresModel() {
            return featuresModel;
        },
        set featuresModel(value) {
            featuresModel = value;
        },
        features: function(tryItNowButton, featureName, dialogVideos) {
            return features(tryItNowButton, featureName, dialogVideos);
        },
        dateProblems: function(id, name) {
            return dateProblems(id, name);
        },
        placeProblems: function(id, name) {
            return placeProblems(id, name);
        },
        ordinances: function(id, name) {
            return ordinances(id, name);
        },
        displayPersonUrls: function(id, name) {
            displayPersonUrls(id, name);
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
        get googleSearchController() {
            return _googleSearchController;
        },
        set googleSearchController(value) {
            _googleSearchController = value;
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
        get ordinancesController() {
            return _ordinancesController;
        },
        set ordinancesController(value) {
            _ordinancesController = value;
        },
        get ordinancesReportController() {
            return _ordinancesReportController;
        },
        set ordinancesReportController(value) {
            _ordinancesReportController = value;
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
        },
        get person() {
            return person;
        },
        get msgBox() {
            return msgBox;
        },
        get timezone() {
            return timezone;
        }
    };

    person.msgBox = msgBox;
    person.retrieve = retrieveModel;

    return researchHelper;
});


//# sourceURL=findPersonHelper.js