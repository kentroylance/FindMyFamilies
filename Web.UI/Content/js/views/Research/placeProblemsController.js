define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var msgBox = require('msgBox');
    var constants = require('constants');
    var researchHelper = require("researchHelper");

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();


    // models
    var person = require('person');
    var placeProblems = require('placeProblems');
    var placeProblemsReport = require('placeProblemsReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#placeProblemsPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#placeProblemsResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#placeProblemsGeneration").val(person.generation);
        }

        if (person.reportId) {
            $("#placeProblemsReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("placeProblemsReportId", "placeProblemsPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("placeProblemsGeneration", "placeProblemsReportId", "placeProblemsPersonId", "placeProblemsGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("placeProblemsGeneration", "placeProblemsReportId", "placeProblemsPersonId", "placeProblemsGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("placeProblemsReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        placeProblems.form = $("#placeProblemsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#placeProblemsPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(placeProblems.form, placeProblems.formTitleImage, placeProblems.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".placeProblemsAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, placeProblems);
    }

    function mouseOut() {
        person.mouseOut(placeProblems);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#placeProblemsReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#placeProblemsPerson').change(function(e) {
            debugger;
        });

        $("#placeProblemsReportId").change(function(e) {
            person.reportId = $("#placeProblemsReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#placeProblemsPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("placeProblemsReportId");
        });

        $('#placeProblemsResearchType').change(function(e) {
            person.researchType = $("#placeProblemsResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("placeProblemsGeneration", "placeProblemsReportId", "placeProblemsPersonId", "placeProblemsGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("placeProblemsGeneration", "placeProblemsReportId", "placeProblemsPersonId", "placeProblemsGenerationDiv");
            }

            person.setHiddenFields("placeProblemsGeneration");
            retrieve.checkReports("placeProblemsReportId", "placeProblemsPersonId");
        });

        $("#placeProblemsFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#placeProblemsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#placeProblemsPersonId").val(person.id);
                        retrieve.checkReports("placeProblemsReportId", "placeProblemsPersonId");
                    }
                    person.loadPersons($("#placeProblemsPersonId"));
                    placeProblems.save();
                }
                findPersonModel.reset();
                system.spinnerArea = placeProblems.spinnerArea;
            });
            return false;
        });

        $("#placeProblemsRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    placeProblems.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = placeProblems.spinnerArea;
            });
            return false;
        });

        $("#placeProblemsHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#placeProblemsCloseButton").unbind('click').bind('click', function(e) {
            placeProblems.form.dialog(constants.CLOSE);
        });

        $("#placeProblemsResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#placeProblemsPreviousButton").unbind('click').bind('click', function(e) {
            if (window.localStorage) {
                placeProblems.previous = JSON.parse(localStorage.getItem(constants.PLACE_PROBLEMS_PREVIOUS));
            }
            placeProblems.displayType = "previous";
            if (placeProblems.previous) {
                $.ajax({
                    url: constants.PLACE_PROBLEMS_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#placeProblemsReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"placeProblemsReportForm\"></div>').dialog({
                            title: "Place Problems",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#placeProblemsReportForm").empty().append(data);
                        if (researchHelper && researchHelper.placeProblemsReportController) {
                            researchHelper.placeProblemsReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = placeProblems.spinnerArea;
        });

        $("#placeProblemsSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                placeProblems.displayType = "start";
                placeProblems.save();

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.PLACE_PROBLEMS_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#placeProblemsReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"placeProblemsReportForm\"></div>").dialog({
                                            title: "Place Problems",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#placeProblemsReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.placeProblemsReportController) {
                                            researchHelper.placeProblemsReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                placeProblemsReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#placeProblemsReportId"));
            updateResearchData();
        });



        $("#placeProblemsCancelButton").unbind('click').bind('click', function(e) {
            placeProblems.form.dialog(constants.CLOSE);
        });

        placeProblems.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (placeProblems.callerSpinner) {
                system.spinnerArea = placeProblems.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            placeProblems.save();
        });
    }

    var placeProblemsController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }
    };

    researchHelper.placeProblemsController = placeProblemsController;
    open();

    return placeProblemsController;
});


//# sourceURL=placeProblemsController.js