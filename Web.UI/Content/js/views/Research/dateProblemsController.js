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
    var dateProblems = require('dateProblems');
    var dateProblemsReport = require('dateProblemsReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#dateProblemsPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#dateProblemsResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#dateProblemsGeneration").val(person.generation);
        }
        if (dateProblems.empty) {
            $("#dateProblemsEmpty").prop('checked', dateProblems.empty);
            }
        if (dateProblems.invalid) {
            $("#dateProblemsInvalid").prop('checked', dateProblems.invalid);
        }
        if (dateProblems.invalidFormat) {
            $("#dateProblemsInvalidFormat").prop('checked', dateProblems.invalidFormat);
        }
        if (dateProblems.incomplete) {
            $("#dateProblemsIncomplete").prop('checked', dateProblems.incomplete);
        }
        if (person.reportId) {
            $("#dateProblemsReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("dateProblemsReportId", "dateProblemsPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("dateProblemsGeneration", "dateProblemsReportId", "dateProblemsPersonId", "dateProblemsGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("dateProblemsGeneration", "dateProblemsReportId", "dateProblemsPersonId", "dateProblemsGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("dateProblemsReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        dateProblems.form = $("#dateProblemsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#dateProblemsPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(dateProblems.form, dateProblems.formTitleImage, dateProblems.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".dateProblemsAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, dateProblems);
    }

    function mouseOut() {
        person.mouseOut(dateProblems);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#dateProblemsReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#dateProblemsPerson').change(function (e) {
            debugger;
        });

        $("#dateProblemsReportId").change(function (e) {
            person.reportId = $("#dateProblemsReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#dateProblemsPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("dateProblemsReportId");
        });

        $('#dateProblemsResearchType').change(function(e) {
            person.researchType = $("#dateProblemsResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("dateProblemsGeneration", "dateProblemsReportId", "dateProblemsPersonId", "dateProblemsGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("dateProblemsGeneration", "dateProblemsReportId", "dateProblemsPersonId", "dateProblemsGenerationDiv");
            }

            person.setHiddenFields("dateProblemsGeneration");
            retrieve.checkReports("dateProblemsReportId", "dateProblemsPersonId");
        });


        $("#dateProblemsFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#dateProblemsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#dateProblemsPersonId").val(person.id);
                        retrieve.checkReports("dateProblemsReportId", "dateProblemsPersonId");
                    }
                    person.loadPersons($("#dateProblemsPersonId"));
                    dateProblems.save();
                }
                findPersonModel.reset();
                system.spinnerArea = dateProblems.spinnerArea;
            });
            return false;
        });

        $("#dateProblemsRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    dateProblems.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = dateProblems.spinnerArea;
            });
            return false;
        });

        $("#dateProblemsHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#dateProblemsCloseButton").unbind('click').bind('click', function (e) {
            dateProblems.form.dialog(constants.CLOSE);
        });

        $("#dateProblemsResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#dateProblemsPreviousButton").unbind('click').bind('click', function (e) {
            if (window.localStorage) {
                dateProblems.previous = JSON.parse(localStorage.getItem(constants.DATE_PROBLEMS_PREVIOUS));
            }
            dateProblems.displayType = "previous";
            if (dateProblems.previous) {
                $.ajax({
                    url: constants.DATE_PROBLEMS_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#dateProblemsReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"dateProblemsReportForm\"></div>').dialog({
                            title: "Date Problems",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#dateProblemsReportForm").empty().append(data);
                        if (researchHelper && researchHelper.dateProblemsReportController) {
                            researchHelper.dateProblemsReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = dateProblems.spinnerArea;
        });

        $("#dateProblemsSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                dateProblems.displayType = "start";

                dateProblems.save();
                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.DATE_PROBLEMS_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#dateProblemsReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"dateProblemsReportForm\"></div>").dialog({
                                            title: "Date Problems",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#dateProblemsReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.dateProblemsReportController) {
                                            researchHelper.dateProblemsReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                dateProblemsReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#dateProblemsReportId"));
            updateResearchData();
        });

    $("#dateProblemsEmpty").change(function (e) {
        dateProblems.empty = $("#dateProblemsEmpty").prop("checked");
    });

    $("#dateProblemsInvalid").change(function (e) {
        dateProblems.invalid = $("#dateProblemsInvalid").prop("checked");
    });

    $("#dateProblemsInvalidFormat").change(function (e) {
        dateProblems.invalidFormat = $("#dateProblemsInvalidFormat").prop("checked");
    });

    $("#dateProblemsIncomplete").change(function (e) {
        dateProblems.incomplete = $("#dateProblemsIncomplete").prop("checked");
    });


        $("#dateProblemsCancelButton").unbind('click').bind('click', function (e) {
            dateProblems.form.dialog(constants.CLOSE);
        });

        dateProblems.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (dateProblems.callerSpinner) {
                system.spinnerArea = dateProblems.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            dateProblems.save();
        });
    }

    var dateProblemsController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }
    };

    researchHelper.dateProblemsController = dateProblemsController;
    open();

    return dateProblemsController;
});


//# sourceURL=dateProblemsController.js