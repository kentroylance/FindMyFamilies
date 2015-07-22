define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var msgBox = require('msgBox');
    var constants = require('constants');
    var researchHelper = require("researchHelper");

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();
    require("lazyload");


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
        if (person.reportId) {
            $("#startingPointReportId").val(person.reportId);
        }
        if (person.addChildren) {
            $('#addChildren').prop('checked', person.addChildren);
        }
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"startingPointGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $('#startingPointGenerationDiv').empty();
        $("#startingPointGenerationDiv").append(select);
        $('#startingPointGenerationDiv').nextAll().remove();
        if ((person.researchType === "Ancestors") && (person.reportId === constants.REPORT_ID)) {
            $("#startingPointGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        $("#startingPointGeneration").change(function(e) {
            var generation = $("#startingPointGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#startingPointGeneration").val();
            person.resetReportId($("#startingPointReportId"));
        });

    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#startingPointGeneration").val(person.generation);
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
        $("#startingPointGeneration").val(person.generation);
    }

    function updateResearchData() {
        $("#startingPointReportId").val(person.reportId);
        var reportText = $("#startingPointReportId option:selected").text();
        if (reportText && reportText.length > 8 && reportText !== "Select") {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            person.id = reportText.substring(nameIndex, nameIndex + 8);
            person.name = reportText.substring(nameIndex + 11, dateIndex);
            person.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            person.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            person.loadPersons($("#startingPointPersonId"));
            //                person.reportId = $("#startingPointReportId option:selected").val();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function loadReports(refreshReport) {
        retrieve.loadReports($("#startingPointReportId"), refreshReport);
        updateResearchData();
    }


    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("startingPointGeneration").val(person.generation);
    }

    function open() {
        startingPoint.form = $("#startingPointForm");
        loadEvents();
        loadReports();
        person.loadPersons($("#startingPointPersonId"));
        updateForm();
        system.openForm(startingPoint.form, startingPoint.formTitleImage, startingPoint.spinner);
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
            resetReportId();
        });

        $('#startingPointResearchType').change(function(e) {
            person.researchType = $("#startingPointResearchType").val();
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

        $("#startingPointFindPersonButton").unbind('click').bind('click', function(e) {
            researchHelper.findPerson(e, function(result) {
                var findPersonModel = require('findPerson');
                if (result) {
                    var changed = (findPersonModel.id === $("#startingPointPersonId").val()) ? false : true;
                    if (changed) {
                        person.id = findPersonModel.id;
                        person.name = findPersonModel.name;
                    }
                    startingPoint.save();
                    person.loadPersons($("#startingPointPersonId"));
                    if (changed) {
                        resetReportId();
                    }
                }
                findPersonModel.reset();
            });
            return false;
        });

        $("#startingPointRetrieveButton").unbind('click').bind('click', function(e) {
            researchHelper.retrieve(e, function(result) {
                var retrieve = require('retrieve');
                if (result) {
                    person.reportId = retrieve.reportId;
                    startingPoint.save();
                    loadReports(true);
                }
                retrieve.reset();
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
            if (!startingPoint.previous) {
                if (window.localStorage) {
                    startingPoint.previous = JSON.parse(localStorage.getItem(constants.STARTING_POINT_PREVIOUS));
                }
            }
            if (startingPoint.previous) {
                system.initSpinner(startingPoint.spinner);
                startingPoint.callerSpinner = startingPoint.spinner;
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
                        startingPoint.displayType = "previous";
                        $("#startingPointReportForm").empty().append(data);
                        if (researchHelper && researchHelper.startingPointReportController) {
                            researchHelper.startingPointReportController.open();
                        }
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
        });

        $("#startingPointSubmitButton").unbind('click').bind('click', function(e) {
            if (system.isAuthenticated()) {
                if (!person.id) {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.", "Question", function(result) {
                    if (result) {
                        requireOnce(["bootstrapTable", "jqueryUiOptions", "css!/Content/css/lib/research/bootstrap-table.min.css"], function() {
                            }, function() {
                                system.initSpinner(startingPoint.spinner);
                                startingPoint.callerSpinner = startingPoint.spinner;
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
                                        startingPointReport.displayType = "start";
                                        startingPoint.save();
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
                msgBox.warning("Selecting <b>NonMormon</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointBorn18101850").change(function(e) {
            startingPoint.born18101850 = $("#startingPointBorn18101850").prop("checked");
        });

        $("#startingPointLivedInUSA").change(function(e) {
            startingPoint.livedInUSA = $("#startingPointLivedInUSA").prop("checked");
        });

        $("#startingPointOrdinances").change(function(e) {
            startingPoint.ordinances = $("#startingPointOrdinances").prop("checked");
            if (startingPoint.ordinances) {
                $('#startingPointNonMormon').prop('checked', true);
                msgBox.warning("Selecting <b>IncompleteOrdinances</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
            }

        });

        $("#startingPointHints").change(function(e) {
            startingPoint.hints = $("#startingPointHints").prop("checked");
            if (startingPoint.hints) {
                msgBox.warning("Selecting <b>Hints</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointDuplicates").change(function(e) {
            startingPoint.duplicates = $("#startingPointDuplicates").prop("checked");
            if (startingPoint.duplicates) {
                msgBox.warning("Selecting <b>Possible Duplicates</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
            }
        });

        $("#startingPointCancelButton").unbind('click').bind('click', function(e) {
            startingPoint.form.dialog(constants.CLOSE);
        });

        startingPoint.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            startingPoint.save();
        });
    }

    var startingPointController = {
        open: function() {
            open();
        }
    };

    researchHelper.startingPointController = startingPointController;
    open();

    return startingPointController;
});


//# sourceURL=startingPointController.js