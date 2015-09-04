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
    var findClues = require('findClues');
    var findCluesReport = require('findCluesReport');
    var retrieve = require('retrieve');

    function updateForm() {
        if (person.id) {
            $("#findCluesPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#findCluesResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#findCluesGeneration").val(person.generation);
        }
        if (findClues.searchCriteria) {
            $("#findCluesSearchCriteria").val(findClues.searchCriteria);
        }
        if (findClues.gapInChildren) {
            $('#findCluesGapInChildren').prop('checked', findClues.gapInChildren);
        }
        if (findClues.ageLimit) {
            $('#findAgeLimit').prop('checked', findClues.ageLimit);
        }
        if (person.reportId) {
            $("#findCluesReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function updateResearchData() {
        retrieve.populatePersonFromReport("findCluesReportId", "findCluesPersonId");
        if (person.researchType === constants.DESCENDANTS) {
            person.addDecendantGenerationOptions("findCluesGeneration", "findCluesReportId", "findCluesPersonId", "findCluesGenerationDiv");
        } else {
            person.addAncestorGenerationOptions("findCluesGeneration", "findCluesReportId", "findCluesPersonId", "findCluesGenerationDiv");
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports("findCluesReportId", refreshReport);
        updateResearchData();
    }

    function open() {
        findClues.form = $("#findCluesForm");
        loadEvents();
        loadSearchCriteria();
        loadReports();
        person.loadPersons($("#findCluesPersonId"));
        retrieve.findReport();
        updateForm();
        system.openForm(findClues.form, findClues.formTitleImage, findClues.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOver,
                out: mouseOut
            }

            $(".findCluesAction").hoverIntent(hoverIntentConfig);
        });
    }

    function mouseOver() {
        person.mouseOver(this, findClues);
    }

    function mouseOut() {
        person.mouseOut(findClues);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#findCluesReportId"));
        updateResearchData();
    }

    function loadSearchCriteria() {
        if (!findClues.searchCriteria) {
            findClues.searchCriteria = "0";
        }
        $.each(findClues.searchCriteriaList, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            $("#findCluesSearchCriteria").append(optionhtml);
        });
    }

    function loadEvents() {
        $("#findCluesSearchCriteria").change(function (e) {
            findClues.searchCriteria = $("#findCluesSearchCriteria").val();
        });

        $("#findCluesGapInChildren").change(function (e) {
            findClues.gapInChildren = $("#findCluesGapInChildren").val();
        });

        $("#findCluesAgeLimit").change(function (e) {
            findClues.ageLimit = $("#findCluesAgeLimit").val();
        });

        $("#findCluesReportId").change(function(e) {
            person.reportId = $("#findCluesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#findCluesPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            retrieve.checkReports("findCluesReportId");
        });

        $('#findCluesResearchType').change(function(e) {
            person.researchType = $("#findCluesResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                person.generationAncestors = person.generation;
                person.generation = person.generationDescendants;
                person.addDecendantGenerationOptions("findCluesGeneration", "findCluesReportId", "findCluesPersonId", "findCluesGenerationDiv");
            } else {
                person.generationDescendants = person.generation;
                person.generation = person.generationAncestors;
                person.addAncestorGenerationOptions("findCluesGeneration", "findCluesReportId", "findCluesPersonId", "findCluesGenerationDiv");
            }

            person.setHiddenFields("findCluesGeneration");
            retrieve.checkReports("findCluesReportId", "findCluesPersonId");
        });

        $("#findCluesFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#findCluesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        $("#findCluesPersonId").val(person.id);
                        retrieve.checkReports("findCluesReportId", "findCluesPersonId");
                    }
                    person.loadPersons($("#findCluesPersonId"));
                    findClues.save();
                }
                findPersonModel.reset();
                system.spinnerArea = findClues.spinnerArea;
            });
            return false;
        });

        $("#findCluesRetrieveButton").unbind('click').bind('click', function() {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    findClues.save();
                    loadReports(true);
                }
                retrieve.reset();
                system.spinnerArea = findClues.spinnerArea;
            });
            return false;
        });

        $("#findCluesHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#findCluesCloseButton").unbind('click').bind('click', function(e) {
            findClues.form.dialog(constants.CLOSE);
        });

        $("#findCluesResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#findCluesPreviousButton").unbind('click').bind('click', function(e) {
            if (window.localStorage) {
                findClues.previous = JSON.parse(localStorage.getItem(constants.FIND_CLUES_PREVIOUS));
            }
            findClues.displayType = "previous";
            if (findClues.previous) {
                $.ajax({
                    url: constants.FIND_CLUES_REPORT_HTML_URL,
                    success: function(data) {
                        var $dialogContainer = $('#findCluesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"findCluesReportForm\"></div>').dialog({
                            title: "Find Clues",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        $("#findCluesReportForm").empty().append(data);
                        if (researchHelper && researchHelper.findCluesReportController) {
                            researchHelper.findCluesReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            system.spinnerArea = findClues.spinnerArea;
        });

        $("#findCluesSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }
                findClues.displayType = "start";
                findClues.save();

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                $.ajax({
                                    url: constants.FIND_CLUES_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#findCluesReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"findCluesReportForm\"></div>").dialog({
                                            title: "Find Clues",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        $("#findCluesReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.findCluesReportController) {
                                            researchHelper.findCluesReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                findCluesReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#findCluesReportId"));
            updateResearchData();
        });

        $("#findCluesNonMormon").change(function(e) {
            findClues.nonMormon = $("#findCluesNonMormon").prop("checked");
            if (findClues.nonMormon) {
                $('#findCluesOrdinances').prop('checked', true);
                msgBox.warning("Selecting <b>NonMormon</b> will add 1-3 seconds more time for each " + findClues.researchType.substring(0, findClues.researchType.length - 1) + " that is processed.");
            }
        });

        $("#findCluesBorn18101850").change(function(e) {
            findClues.born18101850 = $("#findCluesBorn18101850").prop("checked");
        });

        $("#findCluesLivedInUSA").change(function(e) {
            findClues.livedInUSA = $("#findCluesLivedInUSA").prop("checked");
        });

        $("#findCluesOrdinances").change(function(e) {
            findClues.ordinances = $("#findCluesOrdinances").prop("checked");
            if (findClues.ordinances) {
                $('#findCluesNonMormon').prop('checked', true);
                msgBox.warning("Selecting <b>Ordinances</b> will add 1-3 seconds more time for each " + findClues.researchType.substring(0, findClues.researchType.length - 1) + " that is processed.");
            }

        });

        $("#findCluesHints").change(function(e) {
            findClues.hints = $("#findCluesHints").prop("checked");
            if (findClues.hints) {
                msgBox.warning("Selecting <b>Hints</b> will add 1-3 seconds more time for each " + findClues.researchType.substring(0, findClues.researchType.length - 1) + " that is processed.");
            }
        });

        $("#findCluesDuplicates").change(function(e) {
            findClues.duplicates = $("#findCluesDuplicates").prop("checked");
            if (findClues.duplicates) {
                msgBox.warning("Selecting <b>Possible Duplicates</b> will add 1-3 seconds more time for each " + findClues.researchType.substring(0, findClues.researchType.length - 1) + " that is processed.");
            }
        });

        $("#findCluesCancelButton").unbind('click').bind('click', function(e) {
            findClues.form.dialog(constants.CLOSE);
        });

        findClues.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (findClues.callerSpinner) {
                system.spinnerArea = findClues.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            findClues.save();
        });
    }

    var findCluesController = {
        open: function() {
            open();
        },
        loadReports: function(refreshReport) {
            loadReports(refreshReport);
        }

    };

    researchHelper.findCluesController = findCluesController;
    open();

    return findCluesController;
});


//# sourceURL=findCluesController.js