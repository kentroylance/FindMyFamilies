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
    var startingPoint = require('startingPoint');
    var startingPointReport = require('startingPointReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#startingPointPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#startingPointResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#startingPointGeneration").val(person.generation);
        }
        if (startingPoint.nonMormon) {
            $('#startingPointNonMormon').prop('checked', startingPoint.nonMormon);
        }
        if (startingPoint.born18101850) {
            $('#startingPointBorn18101850').prop('checked', startingPoint.born18101850);
        }
        if (startingPoint.livedInUSA) {
            $('#startingPointLivedInUSA').prop('checked', startingPoint.livedInUSA);
        }
        if (startingPoint.ordinances) {
            $('#startingPointOrdinances').prop('checked', startingPoint.ordinances);
        }
        if (startingPoint.hints) {
            $('#startingPointHints').prop('checked', startingPoint.hints);
        }
        if (startingPoint.duplicates) {
            $('#startingPointDuplicates').prop('checked', startingPoint.duplicates);
        }
        if (startingPoint.clues) {
            $('#startingPointClues').prop('checked', startingPoint.clues);
        }
        if (person.reportId) {
            $("#startingPointReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("startingPointReportId", "startingPointPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("startingPointGeneration", "startingPointReportId", "startingPointPersonId", "startingPointGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("startingPointGeneration", "startingPointReportId", "startingPointPersonId", "startingPointGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("startingPointReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        startingPoint.form = $("#startingPointForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#startingPointPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(startingPoint.form, startingPoint.formTitleImage, startingPoint.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".startingPointAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, startingPoint);
    }

    function mouseOut() {
        person.mouseOut(startingPoint);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#startingPointReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#startingPointPerson').change(function(e) {
            debugger;
        });

        $("#startingPointReportId").change(function(e) {
            person.reportId = $("#startingPointReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#startingPointPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("startingPointReportId");
        });

        $('#startingPointResearchType').change(function(e) {
            person.researchType = $("#startingPointResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("startingPointGeneration", "startingPointReportId", "startingPointPersonId", "startingPointGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("startingPointGeneration", "startingPointReportId", "startingPointPersonId", "startingPointGenerationDiv");
            }

            person.setHiddenFields("startingPointGeneration");
            retrieve.checkReports("startingPointReportId", "startingPointPersonId");
        });


        $("#startingPointFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#startingPointPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#startingPointPersonId").val(person.id);
                        retrieve.checkReports("startingPointReportId", "startingPointPersonId");
                    }
                    person.loadPersons($("#startingPointPersonId"));
                    startingPoint.save();
                }
                findPersonModel.reset();
                system.spinnerArea = startingPoint.spinnerArea;
            });
            return false;
        });

        $("#startingPointRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    startingPoint.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = startingPoint.spinnerArea;
            });
            return false;
        });

        $("#startingPointHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#startingPointCloseButton").unbind('click').bind('click', function(e) {
            startingPoint.form.dialog(constants.CLOSE);
        });

        $("#startingPointResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#startingPointPreviousButton").unbind('click').bind('click', function(e) {
            if (window.localStorage) {
                startingPoint.previous = JSON.parse(localStorage.getItem(constants.STARTING_POINT_PREVIOUS));
            }
            startingPoint.displayType = "previous";
            if (startingPoint.previous) {
                $.ajax({
                    url: constants.STARTING_POINT_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#startingPointReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"startingPointReportForm\"></div>').dialog({
                            title: "Starting Points",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#startingPointReportForm").empty().append(data);
                        if (researchHelper && researchHelper.startingPointReportController) {
                            researchHelper.startingPointReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = startingPoint.spinnerArea;
        });

        $("#startingPointSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                startingPoint.displayType = "start";

                startingPoint.save();
                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                        }, function () {
                                $.ajax({
                                    url: constants.STARTING_POINT_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#startingPointReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"startingPointReportForm\"></div>").dialog({
                                            title: "Starting Points",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#startingPointReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.startingPointReportController) {
                                            researchHelper.startingPointReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                startingPointReport.form.dialog("close");
                system.relogin();
            }
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

        $("#startingPointNonMormon").change(function(e) {
            startingPoint.nonMormon = $("#startingPointNonMormon").prop("checked");
            if (startingPoint.nonMormon) {
                $('#startingPointOrdinances').prop('checked', true);
                msgBox.warning("Selecting <b>NonMormon</b> will add 1-3 seconds more time for each " + person.researchType.substring(0, person.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointBorn18101850").change(function(e) {
            startingPoint.born18101850 = $("#startingPointBorn18101850").prop("checked");
        });


        $("#startingPointClues").change(function (e) {
            startingPoint.clues = $("#startingPointClues").prop("checked");
        });

        $("#startingPointLivedInUSA").change(function (e) {
            startingPoint.livedInUSA = $("#startingPointLivedInUSA").prop("checked");
        });

        $("#startingPointOrdinances").change(function(e) {
            startingPoint.ordinances = $("#startingPointOrdinances").prop("checked");
            if (startingPoint.ordinances) {
                $('#startingPointNonMormon').prop('checked', true);
                msgBox.warning("Selecting <b>Ordinances</b> will add 1-3 seconds more time for each " + person.researchType.substring(0, person.researchType.length - 1) + " that is processed.");
            }

        });

        $("#startingPointHints").change(function(e) {
            startingPoint.hints = $("#startingPointHints").prop("checked");
            if (startingPoint.hints) {
                msgBox.warning("Selecting <b>Hints</b> will add 1-3 seconds more time for each " + person.researchType.substring(0, person.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointDuplicates").change(function(e) {
            startingPoint.duplicates = $("#startingPointDuplicates").prop("checked");
            if (startingPoint.duplicates) {
                msgBox.warning("Selecting <b>Possible Duplicates</b> will add 1-3 seconds more time for each " + person.researchType.substring(0, person.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointCancelButton").unbind('click').bind('click', function (e) {
            startingPoint.form.dialog(constants.CLOSE);
        });

        startingPoint.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (startingPoint.callerSpinner) {
                system.spinnerArea = startingPoint.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            startingPoint.save();
        });
    }

    var startingPointController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.startingPointController = startingPointController;
    open();

    return startingPointController;
});


//# sourceURL=startingPointController.js