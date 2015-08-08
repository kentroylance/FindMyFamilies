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
    var incompleteOrdinances = require('incompleteOrdinances');
    var incompleteOrdinancesReport = require('incompleteOrdinancesReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#incompleteOrdinancesPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#incompleteOrdinancesResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#incompleteOrdinancesGeneration").val(person.generation);
        }
        
        if (person.reportId) {
            $("#incompleteOrdinancesReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"incompleteOrdinancesGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#incompleteOrdinancesGenerationDiv').empty();
        $("#incompleteOrdinancesGenerationDiv").append(select);
        $('#incompleteOrdinancesGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#incompleteOrdinancesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        $("#incompleteOrdinancesGeneration").change(function(e) {
            var generation = $("#incompleteOrdinancesGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#incompleteOrdinancesGeneration").val();
            person.resetReportId($("#incompleteOrdinancesReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#incompleteOrdinancesGeneration").val(person.generation);
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
        $("#incompleteOrdinancesGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#incompleteOrdinancesReportId").val(person.reportId);
        var reportText = $("#incompleteOrdinancesReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#incompleteOrdinancesPersonId"));
            //                person.reportId = $("#incompleteOrdinancesReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#incompleteOrdinancesReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("incompleteOrdinancesGeneration").val(person.generation);
    }

    function open() {
        incompleteOrdinances.form = $("#incompleteOrdinancesForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#incompleteOrdinancesPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(incompleteOrdinances.form, incompleteOrdinances.formTitleImage, incompleteOrdinances.spinner);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#incompleteOrdinancesReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#incompleteOrdinancesPerson').change(function(e) {
            debugger;
        });

        $("#incompleteOrdinancesReportId").change(function(e) {
            person.reportId = $("#incompleteOrdinancesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#incompleteOrdinancesPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            if (retrieve.findReport()) {
                updateResearchData();
            } else {
                resetReportId();
            }
        });

        $('#incompleteOrdinancesResearchType').change(function(e) {
            person.researchType = $("#incompleteOrdinancesResearchType").val();
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

        $("#incompleteOrdinancesFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#incompleteOrdinancesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        if (retrieve.findReport()) {
                            updateResearchData();
                        } else {
                            resetReportId();
                        }
                    }
                    incompleteOrdinances.save();
                    person.loadPersons($("#incompleteOrdinancesPersonId"));
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#incompleteOrdinancesRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    incompleteOrdinances.save();
                    loadReports(true);
                }
                retrieve.reset();
            });
            return false;
        });

        $("#incompleteOrdinancesHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#incompleteOrdinancesCloseButton").unbind('click').bind('click', function(e) {
            incompleteOrdinances.form.dialog(constants.CLOSE);
        });

        $("#incompleteOrdinancesResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#incompleteOrdinancesPreviousButton").unbind('click').bind('click', function(e) {
            if (!incompleteOrdinances.previous) {
                if (window.localStorage) {
                    incompleteOrdinances.previous = JSON.parse(localStorage.getItem(constants.INCOMPLETE_ORDINANCES_PREVIOUS));
                }
            }
            if (incompleteOrdinances.previous) {
                system.initSpinner(incompleteOrdinances.spinner);
                incompleteOrdinances.callerSpinner = incompleteOrdinances.spinner;
                $.ajax({
                    url: constants.INCOMPLETE_ORDINANCES_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#incompleteOrdinancesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"incompleteOrdinancesReportForm\"></div>').dialog({
                            title: "Incomplete Ordinances",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        incompleteOrdinances.displayType = "previous";
                        $("#incompleteOrdinancesReportForm").empty().append(data);
                        if (researchHelper && researchHelper.incompleteOrdinancesReportController) {
                            researchHelper.incompleteOrdinancesReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#incompleteOrdinancesSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                system.initSpinner(incompleteOrdinances.spinner);
                                incompleteOrdinances.callerSpinner = incompleteOrdinances.spinner;
                                $.ajax({
                                    url: constants.INCOMPLETE_ORDINANCES_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#incompleteOrdinancesReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"incompleteOrdinancesReportForm\"></div>").dialog({
                                            title: "Incomplete Ordinances",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        incompleteOrdinancesReport.displayType = "start";
                                        incompleteOrdinances.save();
                                        $("#incompleteOrdinancesReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.incompleteOrdinancesReportController) {
                                            researchHelper.incompleteOrdinancesReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                incompleteOrdinancesReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#incompleteOrdinancesReportId"));
            updateResearchData();
        });



        $("#incompleteOrdinancesCancelButton").unbind('click').bind('click', function(e) {
            incompleteOrdinances.form.dialog(constants.CLOSE);
        });

        incompleteOrdinances.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (incompleteOrdinances.callerSpinner) {
                system.spinnerArea = incompleteOrdinances.callerSpinner;
            }
            incompleteOrdinances.save();
        });
    }

    var incompleteOrdinancesController = {
        open: function() {
            open();
        }
    };

    researchHelper.incompleteOrdinancesController = incompleteOrdinancesController;
    open();

    return incompleteOrdinancesController;
});


//# sourceURL=incompleteOrdinancesController.js