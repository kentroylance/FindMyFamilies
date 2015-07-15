define(function (require) {

    var $ = require('jquery');
    var startingPoint = require('startingPoint');
    var lazyRequire = require('lazyRequire');
    var user = require('user');
    var person = require('person');
    var constants = require('constants');
    var requireOnce = lazyRequire.once();
    var msgBox = require('msgBox');
    var system = require('system');

    $('#startingPointPerson').change(function (e) {
        debugger;
    });

    $("#startingPointReportId").change(function (e) {
        startingPoint.reportId = $("#startingPointReportId option:selected").val();
        if (startingPoint.reportId === constants.REPORT_ID) {
            msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
        }
        startingPoint.updateResearchData();
    });

    $('#startingPointResearchType').change(function (e) {
        startingPoint.researchType = $("#startingPointResearchType").val();
        if (startingPoint.researchType === constants.DESCENDANTS) {
            startingPoint.generationAncestors = startingPoint.generation;
            startingPoint.generation = startingPoint.generationDescendants;
            startingPoint.addDecendantGenerationOptions();
        } else {
            startingPoint.generationDescendants = startingPoint.generation;
            startingPoint.generation = startingPoint.generationAncestors;
            startingPoint.addAncestorGenerationOptions();
        }

        startingPoint.setHiddenFields();
        startingPoint.resetReportId();
        startingPoint.updateResearchData();
    });

    $("#startingPointFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (person.selected) {
                var changed = (person.personId === $("#startingPointPersonId").val()) ? false : true;
//               PersonInfo.updateFromFindPerson();
//                startingPoint.personId = FindPerson.personId;
//                startingPoint.personName = FindPerson.personName;
                startingPoint.save();
                person.loadPersons($("#startingPointPersonId"), true);
                if (changed) {
                    startingPoint.resetReportId();
                    startingPoint.updateResearchData();
                }
            }
        });
        return false;
    });

    $("#startingPointRetrieveButton").unbind('click').bind('click', function (e) {
        if (startingPoint.personId) {
            retrieve.caller = constants.STARTING_POINT;
            retrieve.retrievedRecords = 0;
            retrieve.popup = true;
            startingPoint.updateResearchData();
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#addChildren").change(function (e) {
        startingPoint.addChildren = $("#addChildren").prop("checked");
        if (startingPoint.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
        }
        startingPoint.resetReportId();
        startingPoint.updateResearchData();
    });

    $("#startingPointNonMormon").change(function (e) {
        startingPoint.nonMormon = $("#startingPointNonMormon").prop("checked");
        if (startingPoint.nonMormon) {
            $('#startingPointOrdinances').prop('checked', true);
            msgBox.warning("Selecting <b>NonMormon</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
        }
    });

    $("#startingPointBorn18101850").change(function (e) {
        startingPoint.born18101850 = $("#startingPointBorn18101850").prop("checked");
    });

    $("#startingPointLivedInUSA").change(function (e) {
        startingPoint.livedInUSA = $("#startingPointLivedInUSA").prop("checked");
    });

    $("#startingPointOrdinances").change(function (e) {
        startingPoint.ordinances = $("#startingPointOrdinances").prop("checked");
        if (startingPoint.ordinances) {
            $('#startingPointNonMormon').prop('checked', true);
            msgBox.warning("Selecting <b>IncompleteOrdinances</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
        }

    });

    $("#startingPointHints").change(function (e) {
        startingPoint.hints = $("#startingPointHints").prop("checked");
        if (startingPoint.hints) {
            msgBox.warning("Selecting <b>Hints</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
        }
    });

    $("#startingPointDuplicates").change(function (e) {
        startingPoint.duplicates = $("#startingPointDuplicates").prop("checked");
        if (startingPoint.duplicates) {
            msgBox.warning("Selecting <b>Possible Duplicates</b> will add 1-3 seconds more time for each " + startingPoint.researchType.substring(0, startingPoint.researchType.length - 1) + " that is processed.");
        }
    });

    $("#startingPointCancelButton").unbind('click').bind('click', function (e) {
        startingPoint.form.dialog(constants.CLOSE);
    });

    startingPoint.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
        startingPoint.save();
    });

    var startingPointEvents = {
    };

    return startingPointEvents;
});
//# sourceURL=startingPoint.js