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

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"hintsGeneration\"\>");
        $.each(options, function (a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#hintsGenerationDiv').empty();
        $("#hintsGenerationDiv").append(select);
        $('#hintsGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#hintsGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#hintsGeneration").change(function (e) {
            var generation = $("#hintsGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#hintsGeneration").val();
            person.resetReportId($("#hintsReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#hintsGeneration").val(person.generation);
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
            $("#hintsGeneration").val(person.generation);
        }

    function updateResearchData() {
        $("#hintsReportId").val(person.reportId);
        var reportText = $("#hintsReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#hintsPersonId"));
            //                person.reportId = $("#hintsReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#hintsReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("hintsGeneration").val(person.generation);
    }

    function open() {
        hints.form = $("#hintsForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#hintsPersonId"));
        updateForm();
        system.openForm(hints.form, hints.formTitleImage, hints.spinner);
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
            resetReportId();
        });

        $('#hintsResearchType').change(function(e) {
            person.researchType = $("#hintsResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                hints.generationAncestors = person.generation;
                person.generation = hints.generationDescendants;
                addDecendantGenerationOptions();
            } else {
                hints.generationDescendants = person.generation;
                person.generation = hints.generationAncestors;
                addAncestorGenerationOptions();
            }

            setHiddenFields();
            resetReportId();
        });

        $("#hintsFindPersonButton").unbind('click').bind('click', function(e) {
            researchHelper.findPerson(e, function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#hintsPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                    }
                    hints.save();
                    person.loadPersons($("#hintsPersonId"));
                    if (changed) {
                        resetReportId();
                    }
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#hintsRetrieveButton").unbind('click').bind('click', function(e) {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    hints.save();
                    loadReports(true);
                }
                retrieve.reset();
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
            if (!hints.previous) {
                if (window.localStorage) {
                    hints.previous = JSON.parse(localStorage.getItem(constants.HINTS_PREVIOUS));
                }
            }
            if (hints.previous) {
                system.initSpinner('hints.spinner');
                hints.callerSpinner = hints.spinner;
                $.ajax({
                    url: constants.HINTS_REPORT_HTML_URL,
                    success: function (data) {
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
                        hints.displayType = "previous";
                        $("#hintsReportForm").empty().append(data);
                        if (researchHelper && researchHelper.hintsReportController) {
                            researchHelper.hintsReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#hintsSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        system.initSpinner(hints.spinner);
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                        }, function () {
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
                                        hintsReport.displayType = "start";
                                        hints.save();
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
                system.initSpinner(hints.callerSpinner, true);
            }
            hints.save();
        });
    }

    var hintsController = {
        open: function () {
            open();
        }
    };

    researchHelper.hintsController = hintsController;
    open();

    return hintsController;
});


//# sourceURL=hintsController.js