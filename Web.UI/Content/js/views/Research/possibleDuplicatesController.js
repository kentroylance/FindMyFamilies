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
    var possibleDuplicates = require('possibleDuplicates');
    var possibleDuplicatesReport = require('possibleDuplicatesReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#possibleDuplicatesPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#possibleDuplicatesResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#possibleDuplicatesGeneration").val(person.generation);
        }
        if (possibleDuplicates.includePossibleDuplicates) {
            $("#includePossibleDuplicates").prop('checked', possibleDuplicates.includePossibleDuplicates);
            }
        if (possibleDuplicates.includePossibleMatches) {
            $("#includePossibleMatches").prop('checked', possibleDuplicates.includePossibleMatches);
            }
        if (person.reportId) {
            $("#possibleDuplicatesReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("possibleDuplicatesReportId", "possibleDuplicatesPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("possibleDuplicatesGeneration", "possibleDuplicatesReportId", "possibleDuplicatesPersonId", "possibleDuplicatesGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("possibleDuplicatesGeneration", "possibleDuplicatesReportId", "possibleDuplicatesPersonId", "possibleDuplicatesGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("possibleDuplicatesReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        possibleDuplicates.form = $("#possibleDuplicatesForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#possibleDuplicatesPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(possibleDuplicates.form, possibleDuplicates.formTitleImage, possibleDuplicates.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".possibleDuplicatesAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, possibleDuplicates);
    }

    function mouseOut() {
        person.mouseOut(possibleDuplicates);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#possibleDuplicatesReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#possibleDuplicatesPerson').change(function (e) {
            debugger;
        });

        $("#possibleDuplicatesReportId").change(function (e) {
            person.reportId = $("#possibleDuplicatesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#possibleDuplicatesPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("possibleDuplicatesReportId");
        });

        $('#possibleDuplicatesResearchType').change(function(e) {
            person.researchType = $("#possibleDuplicatesResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("possibleDuplicatesGeneration", "possibleDuplicatesReportId", "possibleDuplicatesPersonId", "possibleDuplicatesGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("possibleDuplicatesGeneration", "possibleDuplicatesReportId", "possibleDuplicatesPersonId", "possibleDuplicatesGenerationDiv");
            }

            person.setHiddenFields("possibleDuplicatesGeneration");
            retrieve.checkReports("possibleDuplicatesReportId", "possibleDuplicatesPersonId");
        });

        $("#possibleDuplicatesFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#possibleDuplicatesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#possibleDuplicatesPersonId").val(person.id);
                        retrieve.checkReports("possibleDuplicatesReportId", "possibleDuplicatesPersonId");
                    }
                    person.loadPersons($("#possibleDuplicatesPersonId"));
                    possibleDuplicates.save();
                }
                findPersonModel.reset();
                system.spinnerArea = possibleDuplicates.spinnerArea;
            });
            return false;
        });

        $("#possibleDuplicatesRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    possibleDuplicates.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = possibleDuplicates.spinnerArea;
            });
            return false;
        });

        $("#possibleDuplicatesHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#possibleDuplicatesCloseButton").unbind('click').bind('click', function (e) {
            possibleDuplicates.form.dialog(constants.CLOSE);
        });

        $("#possibleDuplicatesResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#possibleDuplicatesPreviousButton").unbind('click').bind('click', function (e) {
            if (window.localStorage) {
                placeProblems.previous = JSON.parse(localStorage.getItem(constants.PLACE_PROBLEMS_PREVIOUS));
            }
            possibleDuplicates.displayType = "previous";
            if (possibleDuplicates.previous) {
                $.ajax({
                    url: constants.POSSIBLE_DUPLICATES_REPORT_HTML_URL,
                    success: function (data) {
                        var $dialogContainer = $('#possibleDuplicatesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"possibleDuplicatesReportForm\"></div>').dialog({
                            title: "Possible Duplicates",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#possibleDuplicatesReportForm").empty().append(data);
                        if (researchHelper && researchHelper.possibleDuplicatesReportController) {
                            researchHelper.possibleDuplicatesReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = possibleDuplicates.spinnerArea;
        });

        $("#possibleDuplicatesSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                possibleDuplicates.displayType = "start";
                possibleDuplicates.save();

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.POSSIBLE_DUPLICATES_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#possibleDuplicatesReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"possibleDuplicatesReportForm\"></div>").dialog({
                                            title: "Possible Duplicates",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#possibleDuplicatesReportForm").empty().append(data);
                                if (researchHelper && researchHelper.possibleDuplicatesReportController) {
                                    researchHelper.possibleDuplicatesReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                possibleDuplicatesReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#possibleDuplicatesReportId"));
            updateResearchData();
        });

    $("#includePossibleDuplicates").change(function (e) {
        possibleDuplicates.includePossibleDuplicates = $("#includePossibleDuplicates").prop("checked");
    });

    $("#includePossibleMatches").change(function (e) {
        possibleDuplicates.includePossibleMatches = $("#includePossibleMatches").prop("checked");
    });


        $("#possibleDuplicatesCancelButton").unbind('click').bind('click', function (e) {
            possibleDuplicates.form.dialog(constants.CLOSE);
        });

        possibleDuplicates.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (possibleDuplicates.callerSpinner) {
                system.spinnerArea = possibleDuplicates.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            possibleDuplicates.save();
        });
    }

    var possibleDuplicatesController = {
        open: function () {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.possibleDuplicatesController = possibleDuplicatesController;
    open();

    return possibleDuplicatesController;
});


//# sourceURL=possibleDuplicatesController.js