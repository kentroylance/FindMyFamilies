define(function (require) {

    var $ = require('jquery');
    var system = require('system');
    var msgBox = require('msgBox');
    var retrieve = require('retrieve');
    var constants = require('constants');
    var researchHelper = require("researchHelper");


    // models
    var person = require('person');
    var possibleDuplicates = require('possibleDuplicates');
//    var possibleDuplicatesReport = require('possibleDuplicatesReport');
    var research = require('research');

    function updateForm() {
        if (person.id) {
            $("#possibleDuplicatesPersonId").val(person.id);
        }
        if (person.researchType) {
            $("#possibleDuplicatesResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#possibleDuplicatesGeneration").val(person.generation);
        }
        if (possibleDuplicates.includePossibleDuplicates) {
            $("#includePossibleDuplicates").prop('checked', possibleDuplicates.includePossibleDuplicates);
            }
        if (possibleDuplicates.includePossibleMatches) {
            $("#includePossibleMatches").prop('checked', possibleDuplicates.includePossibleMatches);
            }
        if (person.reportId) {
            $("#possibleDuplicatesReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"possibleDuplicatesGeneration\"\>");
        $.each(options, function (a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#possibleDuplicatesGenerationDiv').empty();
        $("#possibleDuplicatesGenerationDiv").append(select);
        $('#possibleDuplicatesGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#possibleDuplicatesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: 1em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#possibleDuplicatesGeneration").change(function (e) {
            var generation = $("#possibleDuplicatesGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#possibleDuplicatesGeneration").val();
            person.resetReportId($("#possibleDuplicatesReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#possibleDuplicatesGeneration").val(person.generation);
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
        $("#possibleDuplicatesGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#possibleDuplicatesReportId").val(person.reportId);
        var reportText = $("#possibleDuplicatesReportId option:selected").text();
        if (reportText && reportText.length > 8) {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            //                person.reportId = $("#possibleDuplicatesReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#possibleDuplicatesReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("possibleDuplicatesGeneration").val(person.generation);
    }

    function open() {
        possibleDuplicates.form = $("#possibleDuplicatesForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#possibleDuplicatesPersonId"), false);
        updateForm();
        system.openForm(possibleDuplicates.form, possibleDuplicates.formTitleImage, possibleDuplicates.spinner);
    }


    function loadEvents() {
        $('#possibleDuplicatesPerson').change(function (e) {
            debugger;
        });

        $("#possibleDuplicatesReportId").change(function (e) {
            person.reportId = $("#possibleDuplicatesReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#possibleDuplicatesResearchType').change(function (e) {
            person.researchType = $("#possibleDuplicatesResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                possibleDuplicates.generationAncestors = person.generation;
                person.generation = possibleDuplicates.generationDescendants;
                addDecendantGenerationOptions();
            } else {
                possibleDuplicates.generationDescendants = person.generation;
                person.generation = possibleDuplicates.generationAncestors;
                addAncestorGenerationOptions();
            }

            setHiddenFields();
            person.resetReportId($("#possibleDuplicatesReportId"));
            updateResearchData();
        });

        $("#possibleDuplicatesFindPersonButton").unbind('click').bind('click', function (e) {
            researchHelper.findPerson(e, function (result) {
                if (result) {
                    var changed = (person.id === $("#possibleDuplicatesPersonId").val()) ? false : true;
                    //               PersonInfo.updateFromFindPerson();
                    //                possibleDuplicates.id = FindPerson.id;
                    //                possibleDuplicates.personName = FindPerson.personName;
                    possibleDuplicates.save();
                    person.loadPersons($("#possibleDuplicatesPersonId"), true);
                    if (changed) {
                        person.resetReportId($("#possibleDuplicatesReportId"));
                        updateResearchData();
                    }
                }
                var findPerson = require('findPerson');
                findPerson.reset();
            });
            return false;
        });

        $("#possibleDuplicatesRetrieveButton").unbind('click').bind('click', function (e) {
            if (possibleDuplicates.id) {
                retrieve.caller = constants.POSSIBLE_DUPLICATES;
                retrieve.retrievedRecords = 0;
                retrieve.popup = true;
                updateResearchData();
            }
            //  add method outside of researchController
            retrieveData(e, $(this)).then(function () {
            });
            return false;
        });

        function clear() {
            person.clear();
        }

        function reset() {
            person.reset();
        }


        $("#possibleDuplicatesHelpButton").unbind('click').bind('click', function (e) {
        });

        $("#possibleDuplicatesCloseButton").unbind('click').bind('click', function (e) {
            possibleDuplicates.form.dialog(constants.CLOSE);
        });

        $("#possibleDuplicatesResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#possibleDuplicatesPreviousButton").unbind('click').bind('click', function (e) {
            if (!possibleDuplicates.previous) {
                if (window.localStorage) {
                    possibleDuplicates.previous = JSON.parse(localStorage.getItem(constants.POSSIBLE_DUPLICATES_PREVIOUS));
                }
            }
            if (possibleDuplicates.previous) {
                system.initSpinner('possibleDuplicatesSpinner');
                $.ajax({
                    url: constants.POSSIBLE_DUPLICATES_REPORT_HTML_URL,
                    success: function (data) {
                        var $dialogContainer = $('#possibleDuplicatesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="possibleDuplicatesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Starting Points",
                            width: 975,
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            },
                            close: function (event, ui) {
                                event.preventDefault();
                                $(this).dialog('destroy').remove();
                            }
                        });
                        possibleDuplicates.displayType = "previous";
                        $("#possibleDuplicatesReportForm").empty().append(data);
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#possibleDuplicatesSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (person.id) {
                    possibleDuplicates.save();
                } else {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function (result) {
                    if (result) {
                        system.initSpinner(possibleDuplicates.spinner);
                        possibleDuplicates.callerSpinner = possibleDuplicates.spinner;
                        $.ajax({
                            url: constants.POSSIBLE_DUPLICATES_REPORT_HTML_URL,
                            success: function (data) {
                                var $dialogContainer = $('#possibleDuplicatesReportForm');
                                var $detachedChildren = $dialogContainer.children().detach();
                                $('<div id="possibleDuplicatesReportForm"></div>').dialog({
                                    position: {
                                        my: "center top",
                                        at: ("center top+" + (window.innerHeight * .07)),
                                        collision: "none"
                                    },
                                    title: "Starting Points",
                                    width: 975,
                                    open: function () {
                                        $detachedChildren.appendTo($dialogContainer);
                                        $(this).css("maxHeight", 700);
                                    }
                                });
                                possibleDuplicatesReport.displayType = "start";
                                possibleDuplicatesReport.form.empty().append(data);
                                if (research && research.possibleDuplicatesReportController) {
                                    research.possibleDuplicatesReportController.open();
                                }
                            }
                        });
                    }
                });
            } else {
                possibleDuplicatesReport.form.dialog("close");
                relogin();
            }
            return false;
        });

        $("#addChildren").change(function (e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#possibleDuplicatesReportId"));
            updateResearchData();
        });

    $("#includePossibleDuplicates").change(function (e) {
        possibleDuplicates.includePossibleDuplicates = $("#includePossibleDuplicates").prop("checked");
    });

    $("#includePossibleMatches").change(function (e) {
        possibleDuplicates.includePossibleMatches = $("#includePossibleMatches").prop("checked");
    });


        $("#possibleDuplicatesCancelButton").unbind('click').bind('click', function (e) {
            possibleDuplicates.form.dialog(constants.CLOSE);
        });

        possibleDuplicates.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            possibleDuplicates.save();
        });
    }

    var possibleDuplicatesController = {
        open: function () {
            open();
        }
    };

    research.possibleDuplicatesController = possibleDuplicatesController;
    open();

    return possibleDuplicatesController;
});


//# sourceURL=possibleDuplicatesController.js