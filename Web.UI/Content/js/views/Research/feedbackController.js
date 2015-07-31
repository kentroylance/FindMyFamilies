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
    var feedback = require('feedback');
    var feedbackReport = require('feedbackReport');

    var retrieve = require('retrieve');

        function updateForm() {
            if (person.id) {
                $("#feedbackPersonId").val(person.id);
            }
            if (person.researchType) {
                $("#feedbackResearchType").val(person.researchType);
            }
            if (person.generation) {
                $("#feedbackGeneration").val(person.generation);
            }
            if (feedback.topScore) {
                $("#feedbackTopScore").prop('checked', feedback.topScore);
            }
            if (feedback.count) {
                $("#feedbackCount").prop('checked', feedback.count);
            }
        if (person.reportId) {
            $("#feedbackReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"feedbackGeneration\"\>");
        $.each(options, function (a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#feedbackGenerationDiv').empty();
        $("#feedbackGenerationDiv").append(select);
        $('#feedbackGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#feedbackGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#feedbackGeneration").change(function (e) {
            var generation = $("#feedbackGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#feedbackGeneration").val();
            person.resetReportId($("#feedbackReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#feedbackGeneration").val(person.generation);
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
            $("#feedbackGeneration").val(person.generation);
        }

    function updateResearchData() {
        $("#feedbackReportId").val(person.reportId);
        var reportText = $("#feedbackReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#feedbackPersonId"));
            //                person.reportId = $("#feedbackReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#feedbackReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("feedbackGeneration").val(person.generation);
    }

    function open() {
        feedback.form = $("#feedbackForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#feedbackPersonId"));
        updateForm();
        system.openForm(feedback.form, feedback.formTitleImage, feedback.spinner);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
        updateResearchData();
    }

    function resetReportId() {
        person.resetReportId($("#feedbackReportId"));
        updateResearchData();
    }

    function loadEvents() {
        $('#feedbackPerson').change(function (e) {
            debugger;
        });

        $("#feedbackReportId").change(function (e) {
            person.reportId = $("#feedbackReportId option:selected").val();
            if (person.reportId === constants.REPORT_ID) {
                msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
            }
            updateResearchData();
        });

        $('#feedbackPersonId').change(function(e) {
            person.id = $('option:selected', $(this)).val();
            person.name = $('option:selected', $(this)).text();
            resetReportId();
        });

        $('#feedbackResearchType').change(function(e) {
            person.researchType = $("#feedbackResearchType").val();
            if (person.researchType === constants.DESCENDANTS) {
                feedback.generationAncestors = person.generation;
                person.generation = feedback.generationDescendants;
                addDecendantGenerationOptions();
            } else {
                feedback.generationDescendants = person.generation;
                person.generation = feedback.generationAncestors;
                addAncestorGenerationOptions();
            }

            setHiddenFields();
            resetReportId();
        });

        $("#feedbackFindPersonButton").unbind('click').bind('click', function(e) {
            researchHelper.findPerson(e, function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#feedbackPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                    }
                    feedback.save();
                    person.loadPersons($("#feedbackPersonId"));
                    if (changed) {
                        resetReportId();
                    }
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#feedbackRetrieveButton").unbind('click').bind('click', function(e) {
            researchHelper.retrieve(function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    feedback.save();
                    loadReports(true);
                }
                retrieve.reset();
            });
            return false;
        });

        $("#feedbackHelpButton").unbind('click').bind('click', function(e) {
        });

        $("#feedbackCloseButton").unbind('click').bind('click', function (e) {
            feedback.form.dialog(constants.CLOSE);
        });

        $("#feedbackResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#feedbackPreviousButton").unbind('click').bind('click', function (e) {
            if (!feedback.previous) {
                if (window.localStorage) {
                    feedback.previous = JSON.parse(localStorage.getItem(constants.FEEDBACK_PREVIOUS));
                }
            }
            if (feedback.previous) {
                system.initSpinner('feedback.spinner');
                feedback.callerSpinner = feedback.spinner;
                $.ajax({
                    url: constants.FEEDBACK_REPORT_HTML_URL,
                    success: function (data) {
                        var $dialogContainer = $('#feedbackReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id=\"feedbackReportForm\"></div>').dialog({
                            title: "Feedback",
                            width: 975,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            }
                        });
                        feedback.displayType = "previous";
                        $("#feedbackReportForm").empty().append(data);
                        if (researchHelper && researchHelper.feedbackReportController) {
                            researchHelper.feedbackReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#feedbackSubmitButton").unbind('click').bind('click', function (e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        system.initSpinner(feedback.spinner);
                        requireOnce(["css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                        }, function () {
                                $.ajax({
                                    url: constants.FEEDBACK_REPORT_HTML_URL,
                                    success: function(data) {
                                        var $dialogContainer = $('#feedbackReportForm');
                                        var $detachedChildren = $dialogContainer.children().detach();
                                        $("<div id=\"feedbackReportForm\"></div>").dialog({
                                            title: "Feedback",
                                            width: 975,
                                            height: 515,
                                            open: function() {
                                                $detachedChildren.appendTo($dialogContainer);
                                            }
                                        });
                                        feedbackReport.displayType = "start";
                                        feedback.save();
                                        $("#feedbackReportForm").empty().append(data);
                                        if (researchHelper && researchHelper.feedbackReportController) {
                                            researchHelper.feedbackReportController.open();
                                        }
                                    }
                                });
                            }
                        );
                    }
                });
            } else {
                feedbackReport.form.dialog("close");
                system.relogin();
            }
            return false;
        });


        $("#addChildren").change(function(e) {
            person.addChildren = $("#addChildren").prop("checked");
            if (person.addChildren) {
                msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
            }
            person.resetReportId($("#feedbackReportId"));
            updateResearchData();
        });

    $("#feedbackTopScore").change(function (e) {
        feedback.topScore = $("#feedbackTopScore").prop("checked");
        if (feedback.topScore) {
            feedback.count = false;
        } else {
            feedback.count = true;
        }
    });
    $("#feedbackCount").change(function (e) {
        feedback.count = $('#feedbackCount').prop("checked");
        if (feedback.count) {
            feedback.topScore = false;
        } else {
            feedback.topScore = true;
        }
    });


        $("#feedbackCancelButton").unbind('click').bind('click', function (e) {
            feedback.form.dialog(constants.CLOSE);
        });

        feedback.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            feedback.save();
        });
    }

    var feedbackController = {
        open: function () {
            open();
        }
    };

    researchHelper.feedbackController = feedbackController;
    open();

    return feedbackController;
});


//# sourceURL=feedbackController.js