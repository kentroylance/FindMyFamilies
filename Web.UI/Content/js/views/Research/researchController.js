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
    var researchHelper = require("researchHelper");

    // models
    var user = require("user");
    var person = require("person");


    $("#startingPoint").unbind("click").bind("click", function (e) {
        researchHelper.startingPoint(e);
        return false;
    });

    $("#retrieve").unbind("click").bind("click", function (e) {
        researchHelper.retrieve(e);
        return false;
    });

    $("#researchFamily").unbind("click").bind("click", function (e) {
        researchHelper.findPerson(e);
        return false;
    });

    $("#possibleDuplicates").unbind("click").bind("click", function (e) {
        researchHelper.possibleDuplicates(e);
        return false;
    });

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

    $("#clearStorage").unbind("click").bind("click", function(e) {
        system.clearStorage();
        //        $.fancybox.message.info("Cleared Storage Successfully");
        return false;
    });

    $("#refreshAndReset").unbind("click").bind("click", function(e) {
        system.clearStorage();
        system.relogin();
        return false;
    });

    $("#refresh").unbind("click").bind("click", function(e) {
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