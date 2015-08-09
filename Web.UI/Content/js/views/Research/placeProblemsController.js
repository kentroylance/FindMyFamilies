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

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"placeProblemsGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#placeProblemsGenerationDiv').empty();
        $("#placeProblemsGenerationDiv").append(select);
        $('#placeProblemsGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#placeProblemsGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        $("#placeProblemsGeneration").change(function(e) {
            var generation = $("#placeProblemsGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#placeProblemsGeneration").val();
            person.resetReportId($("#placeProblemsReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#placeProblemsGeneration").val(person.generation);
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
        $("#placeProblemsGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#placeProblemsReportId").val(person.reportId);
        var reportText = $("#placeProblemsReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#placeProblemsPersonId"));
            //                person.reportId = $("#placeProblemsReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#placeProblemsReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("placeProblemsGeneration").val(person.generation);
    }

    function open() {
        placeProblems.form = $("#placeProblemsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#placeProblemsPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(placeProblems.form, placeProblems.formTitleImage, placeProblems.spinner);
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
            if (retrieve.findReport()) {
                updateResearchData();
            } else {
                resetReportId();
            }
        });

        $('#placeProblemsResearchType').change(function(e) {
            person.researchType = $("#placeProblemsResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                addDecendantGenerationOptions();
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                addAncestorGenerationOptions();
            }

            setHiddenFields();
            resetReportId();
        });

        $("#placeProblemsFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#placeProblemsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        if (retrieve.findReport()) {
                            updateResearchData();
                        } else {
                            resetReportId();
                        }
                    }
                    placeProblems.save();
                    person.loadPersons($("#placeProblemsPersonId"));
                }
                findPersonModel.reset();
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
            if (!placeProblems.previous) {
                if (window.localStorage) {
                    placeProblems.previous = JSON.parse(localStorage.getItem(constants.PLACE_PROBLEMS_PREVIOUS));
                }
            }
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
                        placeProblems.displayType = "previous";
                        $("#placeProblemsReportForm").empty().append(data);
                        if (researchHelper && researchHelper.placeProblemsReportController) {
                            researchHelper.placeProblemsReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#placeProblemsSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

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
                                        placeProblemsReport.displayType = "start";
                                        placeProblems.save();
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
        }
    };

    researchHelper.placeProblemsController = placeProblemsController;
    open();

    return placeProblemsController;
});


//# sourceURL=placeProblemsController.js