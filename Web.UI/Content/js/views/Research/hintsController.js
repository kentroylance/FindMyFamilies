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
    var hints = require('hints');
    var hintsReport = require('hintsReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
                $("#hintsPersonId").val(person.id);
        }
        if (person.researchType) {
                $("#hintsResearchType").val(person.researchType);
        }
        if (person.generation) {
                $("#hintsGeneration").val(person.generation);
        }
            if (hints.topScore) {
                $("#hintsTopScore").prop('checked', hints.topScore);
            }
            if (hints.count) {
                $("#hintsCount").prop('checked', hints.count);
            }
        if (person.reportId) {
            $("#hintsReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("hintsReportId", "hintsPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("hintsGeneration", "hintsReportId", "hintsPersonId", "hintsGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("hintsGeneration", "hintsReportId", "hintsPersonId", "hintsGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("hintsReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        hints.form = $("#hintsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#hintsPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(hints.form, hints.formTitleImage, hints.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".hintsAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, hints);
    }

    function mouseOut() {
        person.mouseOut(hints);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#hintsReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#hintsPerson').change(function (e) {
            debugger;
        });

        $("#hintsReportId").change(function (e) {
            person.reportId = $("#hintsReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#hintsPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("hintsReportId");
        });

        $('#hintsResearchType').change(function(e) {
            person.researchType = $("#hintsResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("hintsGeneration", "hintsReportId", "hintsPersonId", "hintsGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("hintsGeneration", "hintsReportId", "hintsPersonId", "hintsGenerationDiv");
            }

            person.setHiddenFields("hintsGeneration");
            retrieve.checkReports("hintsReportId", "hintsPersonId");
        });

        $("#hintsFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#hintsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#hintsPersonId").val(person.id);
                        retrieve.checkReports("hintsReportId", "hintsPersonId");
                    }
                    person.loadPersons($("#hintsPersonId"));
                    hints.save();
                }
                findPersonModel.reset();
                system.spinnerArea = hints.spinnerArea;
            });
            return false;
        });

        $("#hintsRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    hints.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = hints.spinnerArea;
            });
            return false;
        });

        $("#hintsHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#hintsCloseButton").unbind('click').bind('click', function (e) {
            hints.form.dialog(constants.CLOSE);
        });

        $("#hintsResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#hintsPreviousButton").unbind('click').bind('click', function (e) {
            if (window.localStorage) {
                hints.previous = JSON.parse(localStorage.getItem(constants.HINTS_PREVIOUS));
            }
            hints.displayType = "previous";
            if (hints.previous) {
                $.ajax({
                    url: constants.HINTS_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#hintsReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"hintsReportForm\"></div>').dialog({
                            title: "Hints",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#hintsReportForm").empty().append(data);
                        if (researchHelper && researchHelper.hintsReportController) {
                            researchHelper.hintsReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = hints.spinnerArea;
        });

        $("#hintsSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

                hints.displayType = "start";
                hints.save();
                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.HINTS_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#hintsReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"hintsReportForm\"></div>").dialog({
                                            title: "Hints",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#hintsReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.hintsReportController) {
                                            researchHelper.hintsReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                hintsReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#hintsReportId"));
            updateResearchData();
        });

    $("#hintsTopScore").change(function (e) {
        hints.topScore = $("#hintsTopScore").prop("checked");
        if (hints.topScore) {
            hints.count = false;
        } else {
            hints.count = true;
        }
    });
    $("#hintsCount").change(function (e) {
        hints.count = $('#hintsCount').prop("checked");
        if (hints.count) {
            hints.topScore = false;
        } else {
            hints.topScore = true;
        }
    });


        $("#hintsCancelButton").unbind('click').bind('click', function (e) {
            hints.form.dialog(constants.CLOSE);
        });

        hints.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (hints.callerSpinner) {
                system.spinnerArea = hints.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            hints.save();
        });
    }

    var hintsController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.hintsController = hintsController;
    open();

    return hintsController;
});


//# sourceURL=hintsController.js