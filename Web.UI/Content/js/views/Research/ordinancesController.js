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
    var ordinances = require('ordinances');
    var ordinancesReport = require('ordinancesReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#ordinancesPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#ordinancesResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#ordinancesGeneration").val(person.generation);
        }

        if (person.reportId) {
            $("#ordinancesReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"ordinancesGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#ordinancesGenerationDiv').empty();
        $("#ordinancesGenerationDiv").append(select);
        $('#ordinancesGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#ordinancesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        $("#ordinancesGeneration").change(function(e) {
            var generation = $("#ordinancesGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#ordinancesGeneration").val();
            person.resetReportId($("#ordinancesReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#ordinancesGeneration").val(person.generation);
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
        $("#ordinancesGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#ordinancesReportId").val(person.reportId);
        var reportText = $("#ordinancesReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#ordinancesPersonId"));
            //                person.reportId = $("#ordinancesReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#ordinancesReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("ordinancesGeneration").val(person.generation);
    }

    function open() {
        ordinances.form = $("#ordinancesForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#ordinancesPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(ordinances.form, ordinances.formTitleImage, ordinances.spinner);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#ordinancesReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#ordinancesPerson').change(function(e) {
            debugger;
        });

        $("#ordinancesReportId").change(function(e) {
            person.reportId = $("#ordinancesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#ordinancesPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            if (retrieve.findReport()) {
                updateResearchData();
            } else {
                resetReportId();
            }
        });

        $('#ordinancesResearchType').change(function(e) {
            person.researchType = $("#ordinancesResearchType").val();
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

        $("#ordinancesFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#ordinancesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        if (retrieve.findReport()) {
                            updateResearchData();
                        } else {
                            resetReportId();
                        }
                    }
                    ordinances.save();
                    person.loadPersons($("#ordinancesPersonId"));
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#ordinancesRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    ordinances.save();
                    loadReports(true);
                }
                retrieve.reset();
            });
            return false;
        });

        $("#ordinancesHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#ordinancesCloseButton").unbind('click').bind('click', function(e) {
            ordinances.form.dialog(constants.CLOSE);
        });

        $("#ordinancesResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#ordinancesPreviousButton").unbind('click').bind('click', function(e) {
            if (!ordinances.previous) {
                if (window.localStorage) {
                    ordinances.previous = JSON.parse(localStorage.getItem(constants.ORDINANCES_PREVIOUS));
                }
            }
            ordinancesReport.displayType = "previous";
            if (ordinances.previous) {
                $.ajax({
                    url: constants.ORDINANCES_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#ordinancesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"ordinancesReportForm\"></div>').dialog({
                            title: " Ordinances",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#ordinancesReportForm").empty().append(data);
                        if (researchHelper && researchHelper.ordinancesReportController) {
                            researchHelper.ordinancesReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#ordinancesSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                ordinancesReport.displayType = "start";
                ordinances.save();

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.ORDINANCES_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#ordinancesReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"ordinancesReportForm\"></div>").dialog({
                                            title: " Ordinances",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#ordinancesReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.ordinancesReportController) {
                                            researchHelper.ordinancesReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                ordinancesReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#ordinancesReportId"));
            updateResearchData();
        });



        $("#ordinancesCancelButton").unbind('click').bind('click', function(e) {
            ordinances.form.dialog(constants.CLOSE);
        });

        ordinances.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (ordinances.callerSpinner) {
                system.spinnerArea = ordinances.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            ordinances.save();
        });
    }

    var ordinancesController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.ordinancesController = ordinancesController;
    open();

    return ordinancesController;
});


//# sourceURL=ordinancesController.js