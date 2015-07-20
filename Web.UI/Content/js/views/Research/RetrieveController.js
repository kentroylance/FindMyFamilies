define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var msgBox = require('msgBox');
    var constants = require('constants');
    var researchHelper = require("researchHelper");

    // models
    var user = require("user");
    var person = require("person");
    var retrieve = require("retrieve");


    function updateForm() {
        if (person.id) {
            $("#retrievePersonId").val(person.id);
        }
        if (person.researchType) {
            $("#retrieveResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#retrieveGeneration").val(person.generation);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function retrieveFamilySearchData() {
        if (system.isAuthenticated()) {
            if (person.id) {
                person.save();
            } else {
                msgBox.message("You must first select a person from Family Search");
            }

            msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine. ", "Question", function(result) {
                if (result) {
                    system.initSpinner(startingPoint.spinner);
                    retrieve.callerSpinner = retrieve.spinner;
                    $.ajax({
                        url: constants.RETRIEVE_DATA_URL,
                        data: { "personId": person.id, "name": person.name, "generation": person.generation, "researchType": person.researchType, "title": retrieve.title, "addChildren": person.addChildren },
                        success: function(data) {
                            if (data) {
                                retrieve.retrievedRecords = data.RetrievedRecords;
                                retrieve.reportId = data.ReportId;
//                                if (retrieve.caller === "IncompleteOrdinances") {
//                                    IncompleteOrdinances.reportId = data.ReportId;
//                                    IncompleteOrdinances.loadReports(true);
//                                } else if (retrieve.caller === "PossibleDuplicates") {
//                                    PossibleDuplicates.reportId = data.ReportId;
//                                    PossibleDuplicates.loadReports(true);
//                                } else if (retrieve.caller === "Hints") {
//                                    Hints.reportId = data.ReportId;
//                                    Hints.loadReports(true);
//                                } else if (retrieve.caller === "StartingPoint") {
//                                    StartingPoint.reportId = data.ReportId;
//                                    StartingPoint.loadReports(true);
//                                } else if (retrieve.caller === "DateProblems") {
//                                    DateProblems.reportId = data.ReportId;
//                                    DateProblems.loadReports(true);
//                                } else if (retrieve.caller === "FindClues") {
//                                    FindClues.reportId = data.ReportId;
//                                    FindClues.loadReports(true);
//                                }
                                msgBox.message("Successfully retrieved <b>" + data.RetrievedRecords + "</b> " + person.researchType + ".");

                                if (retrieve.popup) {
                                    retrieve.form.dialog("close");
                                }
                            } else {
                                msgBox.message("Retrieved no " + person.researchType + ".");
                            }
                        }
                    });
                }
            });
        } else {
            retrieve.form.dialog("close");
            relogin();
        }
        return false;
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"retrieveGeneration\"\>");
        $.each(options, function (a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#retrieveGenerationDiv').empty();
        $("#retrieveGenerationDiv").append(select);
        $('#retrieveGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#retrieveGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: 1em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#retrieveGeneration").change(function (e) {
            var generation = $("#retrieveGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#retrieveGeneration").val();
            person.resetReportId($("#retrieveReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#retrieveGeneration").val(person.generation);
    }

    function addAncestorGenerationOptions() {
        var options = [];
        options[0] = "2";
        options[1] = "3";
        options[2] = "4";
        options[3] = "5";
        options[4] = "6";
        options[5] = "7";
        options[6] = "8";
        addGenerationOptions(options);
        $("#retrieveGeneration").val(person.generation);
    }

    function updateResearchData() {
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }
    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("retrieveGeneration").val(person.generation);
    }

    function open() {
        var currentSpinnerTarget = system.target.id;
        if (system.target) {
            retrieve.callerSpinner = system.target.id;
        }
        retrieve.form = $("#retrieveForm");
        loadEvents();
        person.loadPersons($("#retrievePersonId"), false);
        updateResearchData();
        updateForm();
        system.openForm(retrieve.form, retrieve.formTitleImage, retrieve.spinner);
        //        $('#firstName').focus();
        if (currentSpinnerTarget !== constants.DEFAULT_SPINNER_AREA) {
            //            var retrieveButtons = document.getElementById("retrieveButtons");
            //            retrieveButtons.style.display = 'block';
        }
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        retrieve.reset();
        updateForm();
    }

    function loadEvents() {
        $("#retrievePerson").change(function(e) {
            debugger;
        });

        $("#retrieveTitle").change(function(e) {
            retrieve.title = $("#retrieveTitle").val();
        });

        $('#retrievePersonId').change(function (e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            updateResearchData();
        });

        $('#retrieveResearchType').change(function(e) {
            person.researchType = $("#retrieveResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = retrieve.generationDescendants;
                addDecendantGenerationOptions();
            } else {
                retrieve.generationDescendants = person.generation;
                person.generation = retrieve.generationAncestors;
                addAncestorGenerationOptions();
            }

            setHiddenFields();
            person.resetReportId($("#retrieveReportId"));
            updateResearchData();
        });

        $("#retrieveFindPersonButton").unbind('click').bind('click', function(e) {
            researchHelper.findPerson(e, function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#retrievePersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                    }
                    retrieve.save();
                    person.loadPersons($("#retrievePersonId"), true);
                    if (changed) {
                        person.resetReportId($("#retrieveReportId"));
                        updateResearchData();
                    }
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#retrieveButton").unbind('click').bind('click', function (e) {
            retrieveFamilySearchData();
            return false;
        });

        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#startingPointReportId"));
            updateResearchData();
        });
        $("#retrieveHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#retrieveHelpReset").unbind('click').bind('click', function (e) {
            person.reset();
            updateResearchData();
        });

        $("#retrieveCloseButton").unbind('click').bind('click', function(e) {
            retrieve.form.dialog(constants.CLOSE);
        });

        $("#retrieveResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#retrieveSelectButton").unbind('click').bind('click', function (e) {
            if (!person.selected) {
                msgBox.message("You must first select a person by checking the checkbox before proceeding");
                $('#eventsTable').focus();
            } else {
                retrieve.form.dialog(constants.CLOSE);
            }
        });

        $("#retrieveCancelButton").unbind('click').bind('click', function (e) {
            person.selected = false;
            retrieve.form.dialog(constants.CLOSE);
        });

        $("#retrieveCloseButton").unbind('click').bind('click', function (e) {
            retrieve.form.dialog(constants.CLOSE);
        });

        retrieve.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.initSpinner(retrieve.callerSpinner, true);
            person.save();
            if (retrieve.callback) {
                if (typeof (retrieve.callback) === "function") {
                    retrieve.callback(person.selected);
                }
            }
            retrieve.reset();
        });
    }

    var retrieveController = {
        open: function () {
            open();
        }
    };

    researchHelper.retrieveController = retrieveController;
    open();

    return retrieveController;


});
//# sourceURL=retrieveController.js
