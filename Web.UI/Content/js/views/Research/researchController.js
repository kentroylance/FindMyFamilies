define(function(require) {
    var $ = require("jquery");
    var system = require("system");
    var constants = require("constants");

//    require("css!/Content/css/vendor/formValidation.min.css");
//    var formValidation = require("formValidation");
//    var bootstrapValidation = require("bootstrapValidation");
//    


    var msgBox;
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();
    require("lazyload");

    // models
    var user = require("user");
    var person = require("person");
    var research = require("research");

    function findPerson(e) {
        var defer = $.Deferred();
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["bootstrapTable", "formValidation", "bootstrapValidation", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css"], function () {
//                require("css!/Content/css/vendor/formValidation.min.css");
//                var formValidation = require("formValidation");
//                var bootstrapValidation = require("bootstrapValidation");

                //                requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css", "css!/Content/css/vendor/formValidation.min.css", "bootstrapTable", "formValidation", "bootstrapValidation"], function () {
                }, function() {
                    $.ajax({
                        url: constants.FIND_PERSON_URL,
                        success: function(data) {
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
                                    open: function() {
                                        $detachedChildren.appendTo($dialogContainer);
                                        $(this).css("maxHeight", 700);
                                    },
                                    close: function(event, ui) {
                                        event.preventDefault();
                                        research.findPersonController.close();
                                        $(this).dialog("destroy").remove();
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


    function retrieveData(e) {
        var defer = $.Deferred();
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            var retrieveData;
            requireOnce(["retrieveData"], function(RetrieveData) {
                    retrieveData = RetrieveData;
                },
                function() {
                    retrieveData.retrievedRecords = 0;
                    $.ajax({
                        url: constants.RETRIEVE_DATA_URL,
                        success: function(data) {
                            var $dialogContainer = $("#retrieveForm");
                            var $detachedChildren = $dialogContainer.children().detach();
                            $("<div id=\"retrieveForm\"></div>").dialog({
                                width: 825,
                                title: "Retrieve Family Search Data",
                                open: function() {
                                    $(this).css("maxHeight", 700);
                                    $detachedChildren.appendTo($dialogContainer);
                                },
                                close: function(event, ui) {
                                    event.preventDefault();
                                    retrieveData.popup = false;
                                    retrieveData.clear();
                                    system.initSpinner(Retrieve.callerSpinner, true);
                                    $(this).dialog("destroy").remove();
                                },
                                buttons: {
                                    "0": {
                                        id: "submit",
                                        text: "Retrieve",
                                        icons: { primary: "submitIcon" },
                                        click: function(event) {
                                            event.preventDefault();
                                            Retrieve.submit();
                                            defer.resolve(true);
                                        },
                                        "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                                    },
                                    "1": {
                                        id: "reset",
                                        text: "Reset",
                                        icons: { primary: "resetIcon" },
                                        click: function(event) {
                                            event.preventDefault();
                                            Retrieve.reset();
                                        },
                                        "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                                    },
                                    "2": {
                                        id: "close",
                                        text: "Close",
                                        icons: { primary: "closeIcon" },
                                        click: function(event) {
                                            event.preventDefault();
                                            $(this).dialog("close");
                                        },
                                        "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                                    },
                                    "3": {
                                        id: "help",
                                        text: "Help",
                                        icons: { primary: "helpIcon" },
                                        click: function(event) {
                                            event.preventDefault();
                                        },
                                        "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                                    }
                                }
                            });
                            $("#retrieveForm").empty().append(data);
                        }
                    });
                }
            );
        } else {
            system.relogin();
        }

        return defer.promise();
    }

    function startingPoint() {
        if (system.isAuthenticated()) {
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            requireOnce(["jqueryUiOptions"], function () {
                }, function () {
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
                            if (research && research.startingPointController) {
                                research.startingPointController.open();
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
        if (person.personId && system.isAuthenticated()) {
            $.ajax({
                url: constants.DISPLAY_PERSON_URLS_URL,
                data: {
                    "personId": person.personId,
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
                }
            });
        } else {
            system.relogin();
        }
        return false;
    }

    function personUrlOptions(personId) {
        if (personId && system.isAuthenticated()) {
            person.personId = personId;
            system.initSpinner(constants.DEFAULT_SPINNER_AREA);
            $.ajax({
                url: constants.PERSON_URL_OPTIONS_URL,
                success: function(data) {
                    var $dialogContainer = $("#personUrlOptionsForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"personUrlOptionsForm\"></div>").dialog({
                        width: 350,
                        title: "Search Options",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        },
                        buttons: {
                            "0": {
                                id: "ok",
                                text: "Ok",
                                icons: { primary: "okIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PersonUrlOptions.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#personUrlOptionsForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
    }

    var domReady = require("domReady");
    domReady(function() {
        $("img.lazy").lazyload({
            threshold: 200
        });
        require("_layoutController");
    });


    $("#displayPerson").on("click", function(e) {
        e.preventDefault();
        $.ajax({
            url: constants.DISPLAY_PERSON_URL,
            success: function(data) {
                $("#displayPersonForm").dialog({
                    height: "auto",
                    autoOpen: true,
                    my: "center",
                    at: "center",
                    of: window,
                    modal: true,
                    width: 650,
                    title: "&nbsp;&nbsp;Family Search Login Information",
                    closeOnEscape: true,
                    close: function(event, ui) {
                        $("#displayPersonForm").dialog("destroy");
                    }
                });

                $("#displayPersonForm").empty().append(data);
            }
        });
        return false;
    });

    $("#retrieve").unbind("click").bind("click", function(e) {

        retrieveData(e, $(this)).then(function() {
        });
    });


    $("#incompleteOrdinances").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);

            $.ajax({
                url: constants.INCOMPLETE_ORDINANCES_URL,
                success: function(data) {
                    var $dialogContainer = $("#ordinancesForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"ordinancesForm\"></div>").dialog({
                        width: 775,
                        title: "IncompleteOrdinances",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Submit",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    IncompleteOrdinances.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    IncompleteOrdinances.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    IncompleteOrdinances.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#ordinancesForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });

    $("#findClues").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);
            $.ajax({
                url: constants.FIND_CLUES_URL,
                success: function(data) {
                    var $dialogContainer = $("#findCluesForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"findCluesForm\"></div>").dialog({
                        width: 950,
                        title: "Find Clues",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Find Clues",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    FindClues.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    FindClues.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    FindClues.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#findCluesForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });

    $("#researchFamily").unbind("click").bind("click", function (e) {
        person.callerSpinner = 
        findPerson(e);
        return false;
    });

    $("#possibleDuplicates").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);
            $.ajax({
                url: constants.POSSIBLE_DUPLICATES_URL,
                success: function(data) {
                    var $dialogContainer = $("#possibleDuplicatesForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"possibleDuplicatesForm\"></div>").dialog({
                        width: 775,
                        title: "PossibleDuplicates",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Possible Duplicates",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PossibleDuplicates.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PossibleDuplicates.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PossibleDuplicates.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#possibleDuplicatesForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });

    $("#dateProblems").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);
            $.ajax({
                url: constants.DATE_PROBLEMS_URL,
                success: function(data) {
                    var $dialogContainer = $("#datesForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"datesForm\"></div>").dialog({
                        width: 775,
                        title: "Date Problems",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Find DateProblems",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    DateProblems.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    DateProblems.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    DateProblems.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#datesForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });

    $("#placeProblems").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);
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
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Find PlaceProblems",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PlaceProblems.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PlaceProblems.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    PlaceProblems.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#placeProblemsForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });


    $("#hints").unbind("click").bind("click", function(e) {
        e.preventDefault();
        if (system.isAuthenticated()) {
            system.startSpinner(constants.DEFAULT_SPINNER_AREA);
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
                        },
                        buttons: {
                            "0": {
                                id: "submit",
                                text: "Find Hints",
                                icons: { primary: "submitIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    Hints.submit();
                                },

                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                            },
                            "1": {
                                id: "previous",
                                text: "Previous",
                                icons: { primary: "previousIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    Hints.displayPrevious();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "2": {
                                id: "reset",
                                text: "Reset",
                                icons: { primary: "resetIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    Hints.reset();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "3": {
                                id: "close",
                                text: "Close",
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            },
                            "4": {
                                id: "help",
                                text: "Help",
                                icons: { primary: "helpIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }

                        }

                    });
                    $("#hintsForm").empty().append(data);
                }
            });

        } else {
            system.relogin();
        }
        return false;
    });

    $("#startingPoint").unbind("click").bind("click", function(e) {
        e.preventDefault();

        startingPoint();
        return false;
    });

    $("#clearStorage").unbind("click").bind("click", function(e) {
        e.preventDefault();
        system.clearStorage();
        //        $.fancybox.message.info("Cleared Storage Successfully");
        return false;
    });

    $("#refreshAndReset").unbind("click").bind("click", function(e) {
        e.preventDefault();
        system.clearStorage();
        system.relogin();
        return false;
    });

    $("#refresh").unbind("click").bind("click", function(e) {
        e.preventDefault();
        system.relogin();
        return false;
    });


    var researchController = {
        openForm: function(form, image, spinnerTarget) {
            openForm(form, image, spinnerTarget);
        },
        findPerson: function(e) {
            findPerson(e);
        },
        retrieveData: function(e) {
            retrieveData(e);
        },
        displayPersonUrls: function() {
            displayPersonUrls();
        },
        personUrlOptions: function(personId) {
            personUrlOptions(personId);
        },
        test: function() {
            //            $.fancybox.message.info("Thanks for subscribing to our monthly newsletter.");
            require(["msgBox"], function(msgBox) {
                msgBox.message("No rows are selected");
            });
            alert("test");
        }
    };

    return researchController;

});


//# sourceURL=Research.js