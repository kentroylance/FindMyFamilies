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
            $('#findCluesSearchCriteria').prop('checked', findClues.searchCriteria);
        }
        if (findClues.gapInChildren) {
            $('#findCluesGapInChildren').prop('checked', findClues.gapInChildren);
        }
        if (findClues.ageLimit) {
            $('#findAgeLimit').prop('checked', findClues.ageLimit);
        }
        if (person.reportId) {
            $('#findCluesReportId').prop('checked', person.reportId);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"findCluesGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#findCluesGenerationDiv').empty();
        $("#findCluesGenerationDiv").append(select);
        $('#findCluesGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#findCluesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        $("#findCluesGeneration").change(function(e) {
            var generation = $("#findCluesGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#findCluesGeneration").val();
            person.resetReportId($("#findCluesReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#findCluesGeneration").val(person.generation);
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
        $("#findCluesGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#findCluesReportId").val(person.reportId);
        var reportText = $("#findCluesReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#findCluesPersonId"));
            //                person.reportId = $("#findCluesReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#findCluesReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("findCluesGeneration").val(person.generation);
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
        $.each(findClues.searchCriteriaList, function (i) {
            var optionhtml = '<option value="' + findClues.searchCriteriaList[i].ValueMember + '">' + findClues.searchCriteriaList[i].DisplayMember + '</option>';
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
            if (retrieve.findReport()) {
                updateResearchData();
            } else {
                resetReportId();
            }
        });

        $('#findCluesResearchType').change(function(e) {
            person.researchType = $("#findCluesResearchType").val();
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

        $("#findCluesFindPersonButton").unbind('click').bind('click', function() {
            researchHelper.findPerson(function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#findCluesPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                        if (retrieve.findReport()) {
                            updateResearchData();
                        } else {
                            resetReportId();
                        }
                    }
                    findClues.save();
                    person.loadPersons($("#findCluesPersonId"));
                }
                findPersonModel.reset();
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
            if (!findClues.previous) {
                if (window.localStorage) {
                    findClues.previous = JSON.parse(localStorage.getItem(constants.FIND_CLUES_PREVIOUS));
                }
            }
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
                        findClues.displayType = "previous";
                        $("#findCluesReportForm").empty().append(data);
                        if (researchHelper && researchHelper.findCluesReportController) {
                            researchHelper.findCluesReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#findCluesSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

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
                                        findCluesReport.displayType = "start";
                                        findClues.save();
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
                msgBox.warning("Selecting <b>IncompleteOrdinances</b> will add 1-3 seconds more time for each " + findClues.researchType.substring(0, findClues.researchType.length - 1) + " that is processed.");
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
        }
    };

    researchHelper.findCluesController = findCluesController;
    open();

    return findCluesController;
});


//# sourceURL=findCluesController.js