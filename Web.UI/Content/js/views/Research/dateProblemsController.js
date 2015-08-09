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

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"dateProblemsGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#dateProblemsGenerationDiv').empty();
        $("#dateProblemsGenerationDiv").append(select);
        $('#dateProblemsGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#dateProblemsGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#dateProblemsGeneration").change(function (e) {
            var generation = $("#dateProblemsGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#dateProblemsGeneration").val();
            person.resetReportId($("#dateProblemsReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#dateProblemsGeneration").val(person.generation);
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
        $("#dateProblemsGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#dateProblemsReportId").val(person.reportId);
        var reportText = $("#dateProblemsReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#dateProblemsPersonId"));
            //                person.reportId = $("#dateProblemsReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#dateProblemsReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("dateProblemsGeneration").val(person.generation);
    }

    function open() {
        dateProblems.form = $("#dateProblemsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#dateProblemsPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(dateProblems.form, dateProblems.formTitleImage, dateProblems.spinner);
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
            if (retrieve.findReport()) {
                updateResearchData();
            } else {
                resetReportId();
            }
        });

        $('#dateProblemsResearchType').change(function(e) {
            person.researchType = $("#dateProblemsResearchType").val();
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

        $("#dateProblemsFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#dateProblemsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        if (retrieve.findReport()) {
                            updateResearchData();
                        } else {
                            resetReportId();
                        }
                    }
                    dateProblems.save();
                    person.loadPersons($("#dateProblemsPersonId"));
                }
                findPersonModel.reset();
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
            if (!dateProblems.previous) {
                if (window.localStorage) {
                    dateProblems.previous = JSON.parse(localStorage.getItem(constants.DATE_PROBLEMS_PREVIOUS));
                }
            }
            dateProblemsReport.displayType = "previous";
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
        });

        $("#dateProblemsSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                dateProblemsReport.displayType = "start";
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
        }
    };

    researchHelper.dateProblemsController = dateProblemsController;
    open();

    return dateProblemsController;
});


//# sourceURL=dateProblemsController.js