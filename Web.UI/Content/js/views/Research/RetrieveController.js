define(function(require) {

    var $ = require("jquery");
    var spinner = require("spinner");
    var system = require("system");
    var msgbox = require("msgBox");
    var constants = require("constants");
    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();

    // models
    var user = require("user");
    var person = require("person");
    var retrieve = require("retrieve");

    var _formName = "retrieveForm";
    var _formTitleImage = "fmf24-retrieve";
    var _form = $("#retrieveForm");

    function updateForm() {
        if (person.id) {
            $("#retrievePersonId").val(person.id + " - " + person.name);
        }
        if (person.researchType) {
            $("#retrieveResearchType").val(person.researchType);
        }
        if (person.generation) {
            $("#retrieveGeneration").val(person.generation);
        }
        if (person.addChildren) {
            $("#addChildren").prop("checked", person.addChildren);
        }
    }

    function reset() {
        person.reset();
        retrieve.reset();
        updateForm();
    }


    function submit() {
        if (isAuthenticated()) {
            if (person.id) {
                person.save();
            } else {
                msgBox.message("You must first select a person from Family Search");
            }

            msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine. ", "Question", function(result) {
                if (result) {
                    $.ajax({
                        url: "/Home/RetrieveData",
                        data: { "personId": person.id, "name": person.name, "generation": person.generation, "researchType": person.researchType, "title": retrieve.title, "addChildren": person.addChildren },
                        success: function(data) {
                            if (data) {
                                retrieve.retrievedRecords = data.RetrievedRecords;
                                retrieve.reportId = data.ReportId;
                                if (retrieve.caller === "IncompleteOrdinances") {
                                    IncompleteOrdinances.reportId = data.ReportId;
                                    IncompleteOrdinances.loadReports(true);
                                } else if (retrieve.caller === "PossibleDuplicates") {
                                    PossibleDuplicates.reportId = data.ReportId;
                                    PossibleDuplicates.loadReports(true);
                                } else if (retrieve.caller === "Hints") {
                                    Hints.reportId = data.ReportId;
                                    Hints.loadReports(true);
                                } else if (retrieve.caller === "StartingPoint") {
                                    StartingPoint.reportId = data.ReportId;
                                    StartingPoint.loadReports(true);
                                } else if (retrieve.caller === "DateProblems") {
                                    DateProblems.reportId = data.ReportId;
                                    DateProblems.loadReports(true);
                                } else if (retrieve.caller === "FindClues") {
                                    FindClues.reportId = data.ReportId;
                                    FindClues.loadReports(true);
                                }
                                msgBox.message("Successfully retrieved <b>" + data.RetrievedRecords + "</b> " + person.researchType + ".");

                                if (retrieve.popup) {
                                    _form.dialog("close");
                                }
                            } else {
                                msgBox.message("Retrieved no " + person.researchType + ".");
                            }
                        }
                    });
                }
            });
        } else {
            _form.dialog("close");
            relogin();
        }
        return false;
    }

    function resetReportId() {
        person.reportId = constants.REPORT_ID;
        $("#retrieveReportId").val(constants.REPORT_ID);
    }

    function setHiddenFields() {
        if (person.researchType === constants.ANCESTORS) {
        } else {
            person.generation = "2";
        }
        $("retrieveGeneration").val(person.generation);
    }

    function initialize() {
        if (!_initialized) {
            loadData();
        }
        if (person.researchType === constants.DESCENDANTS) {
            addDecendantGenerationOptions();
        } else {
            addAncestorGenerationOptions();
        }
        updateForm();
    }

    function addDecendantGenerationOptions() {
        var options = [];
        options[0] = "1";
        options[1] = "2";
        options[2] = "3";
        addGenerationOptions(options);
        $("#retrieveGeneration").val(person.generation);
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
        $("#retrieveGeneration").val(person.generation);
    }

    function addGenerationOptions(options) {
        var select = $("<select class=\"form-control select1Digit\" id=\"retrieveGeneration\"\>");
        $.each(options, function(a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        $("#retrieveGenerationDiv").empty();
        $("#retrieveGenerationDiv").append(select);
        $("#retrieveGenerationDiv").nextAll().remove();
        if (person.researchType === "Ancestors") {
            $("#retrieveGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: top; margin-top: -0.625em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -0.965em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
        }
        $("#retrieveGeneration").change(function(e) {
            var generation = $("#retrieveGeneration").val();
            if (person.researchType === constants.DESCENDANTS) {
                if (generation > 1) {
                    msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generation > person.generation) {
                    msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            person.generation = $("#retrieveGeneration").val();
            resetReportId();
        });

    }

    $("#retrievePerson").change(function(e) {
        debugger;
    });

    $("#retrieveReportId").change(function(e) {
        var reportText = $("#retrieveReportId").text();
        if (reportText && reportText.length > 8) {
            var nameIndex = reportText.indexOf("Name: ") + 6;
            var dateIndex = reportText.indexOf(", Date:  ");
            var researchTypeIndex = reportText.indexOf(", Research Type: ");
            var generationoIndex = reportText.indexOf(",  Generations: ");
            Retrieve.personId = reportText.substring(nameIndex, nameIndex + 8);
            Retrieve.personName = reportText.substring(nameIndex + 11, dateIndex);
            Retrieve.researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
            Retrieve.generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
            Retrieve.ReportId = $("#retrieveReportId").val();
            Retrieve.updateForm();
        }

    });

    $("#retrieveTitle").change(function(e) {
        Retrieve.title = $("#retrieveTitle").val();
    });


    $("#retrieveResearchType").change(function(e) {
        Retrieve.researchType = $("#retrieveResearchType").val();
        if (Retrieve.researchType === "Descendants") {
            Retrieve.generationAncestors = Retrieve.generation;
            Retrieve.generation = Retrieve.generationDescendants;
            Retrieve.addDecendantGenerationOptions();
        } else {
            Retrieve.generationDescendants = Retrieve.generation;
            Retrieve.generation = Retrieve.generationAncestors;
            Retrieve.addAncestorGenerationOptions();
        }

        Retrieve.setHiddenFields();
        Retrieve.resetReportId();
    });

    $("#retrieveFindPersonButton").unbind("click").bind("click", function(e) {
        findPerson(e, $(this)).then(function() {
            if (FindPerson.selected) {
                $("#retrievePersonId").val(FindPerson.personId + " - " + FindPerson.personName);
                Retrieve.personId = FindPerson.personId;
                Retrieve.personName = FindPerson.personName;
            }
        });
        return false;
    });

    $("#addChildren").change(function(e) {
        Retrieve.addChildren = $("#addChildren").prop("checked");
        if (Retrieve.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will more double the time to retrieve ancestors.");
        }
    });

    Retrieve.form.unbind("dialogclose").bind("dialogclose", function(e) {
        person.save();
    });

    if (spinnerTarget) {
        retrieve.callerSpinner = spinnerTarget.id;
    }


    openForm(Retrieve.form, Retrieve.formTitleImage, "retrieveSpinner");

    function open() {
        loadReports();
        person.loadPersons($("#startingPointPersonId"), false);
        updateForm();
        openForm(startingPoint.form, startingPoint.formTitleImage, startingPoint.spinner);
    }


    //    Retrieve.form = $("#retrieveForm");

    Retrieve.initialize();



    startingPoint.form = $("#startingPointForm");


    open();

//    return {
//        formName: _formName,
//        formTitleImage: _formTitleImage,
//        get form() {
//            return _form;
//        },
//        set form(value) {
//            _form = value;
//        },
//        setHiddenFields: function () {
//            setHiddenFields();
//        },
//        resetReportId: function () {
//            resetReportId();
//        },
//        updateForm: function () {
//            updateForm();
//        },
//        save: function () {
//            save();
//        },
//        loadData: function () {
//            loadData();
//        },
//        reset: function () {
//            reset();
//        },
//        submit: function () {
//            submit();
//        }
//    };


    var retrieveController = {
        open: function() {
            open();
        }
    };

    return retrieveController;
});


//# sourceURL=retrieveController.js


//# sourceURL=Retrieve.js