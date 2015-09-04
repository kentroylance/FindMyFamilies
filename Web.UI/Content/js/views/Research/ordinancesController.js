define(function (require) {

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

    function updateResearchData() {
        retrieve.populatePersonFromReport("ordinancesReportId", "ordinancesPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("ordinancesGeneration", "ordinancesReportId", "ordinancesPersonId", "ordinancesGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("ordinancesGeneration", "ordinancesReportId", "ordinancesPersonId", "ordinancesGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("ordinancesReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        ordinances.form = $("#ordinancesForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#ordinancesPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(ordinances.form, ordinances.formTitleImage, ordinances.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".ordinancesAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, ordinances);
    }

    function mouseOut() {
        person.mouseOut(ordinances);
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
        $('#ordinancesPerson').change(function (e) {
            debugger;
        });

        $("#ordinancesReportId").change(function (e) {
            person.reportId = $("#ordinancesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#ordinancesPersonId').change(function (e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("ordinancesReportId");
        });

        $('#ordinancesResearchType').change(function (e) {
            person.researchType = $("#ordinancesResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("ordinancesGeneration", "ordinancesReportId", "ordinancesPersonId", "ordinancesGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("ordinancesGeneration", "ordinancesReportId", "ordinancesPersonId", "ordinancesGenerationDiv");
            }

            person.setHiddenFields("ordinancesGeneration");
            retrieve.checkReports("ordinancesReportId", "ordinancesPersonId");
        });


        $("#ordinancesFindPersonButton").unbind('click').bind('click', function () {
            researchHelper.findPerson(function (result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#ordinancesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#ordinancesPersonId").val(person.id);
                        retrieve.checkReports("ordinancesReportId", "ordinancesPersonId");
                    }
                    person.loadPersons($("#ordinancesPersonId"));
                    ordinances.save();
                }
                findPersonModel.reset();
                system.spinnerArea = ordinances.spinnerArea;
            });
            return false;
        });

        $("#ordinancesRetrieveButton").unbind('click').bind('click', function () {
            researchHelper.retrieve(function (result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    ordinances.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = ordinances.spinnerArea;
            });
            return false;
        });

        $("#ordinancesHelpButton").unbind('click').bind('click', function (e) {
        });

        $("#ordinancesCloseButton").unbind('click').bind('click', function (e) {
            ordinances.form.dialog(constants.CLOSE);
        });

        $("#ordinancesResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#ordinancesPreviousButton").unbind('click').bind('click', function (e) {
            if (window.localStorage) {
                ordinances.previous = JSON.parse(localStorage.getItem(constants.ORDINANCES_PREVIOUS));
            }
            ordinances.displayType = "previous";
            if (ordinances.previous) {
                $.ajax({
                    url: constants.ORDINANCES_REPORT_HTML_URL,
                    success: function (data) {
                        var $dialogContainer = $('#ordinancesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"ordinancesReportForm\"></div>').dialog({
                            title: " Ordinances",
                            width: 975,
                            open: function () {
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
            system.spinnerArea = ordinances.spinnerArea;
        });

        $("#ordinancesSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                ordinances.displayType = "start";
                ordinances.save();

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function (result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function () {
                        }, function () {
                            $.ajax({
                                url: constants.ORDINANCES_REPORT_HTML_URL,
                                success: function (data) {
                                    var $dialogContainer = $('#ordinancesReportForm');
                                    var $detachedChildren = $dialogContainer.children().detach();
                                    $("<div id=\"ordinancesReportForm\"></div>").dialog({
                                        title: " Ordinances",
                                        width: 975,
                                        height: 515,
                                        open: function () {
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


        $("#addChildren").change(function (e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#ordinancesReportId"));
            updateResearchData();
        });



        $("#ordinancesCancelButton").unbind('click').bind('click', function (e) {
            ordinances.form.dialog(constants.CLOSE);
        });

        ordinances.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (ordinances.callerSpinner) {
                system.spinnerArea = ordinances.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            ordinances.save();
        });
    }

    var ordinancesController = {
        open: function () {
            open();
        },
        loadReports: function (refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.ordinancesController = ordinancesController;
    open();

    return ordinancesController;
});


//# sourceURL=ordinancesController.js