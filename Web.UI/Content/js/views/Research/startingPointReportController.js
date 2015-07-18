define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');

    // models
    var person = require('person');
    var startingPoint = require('startingPoint');
    var research = require('research');

    function loadEvents() {

        $("#startingPointReportOptionsButton").unbind('click').bind('click', function(e) {
            system.initSpinner(startingPointReport.spinner);
            startingPointReport.callerSpinner = startingPointReport.spinner;
            $.ajax({
                url: constants.FIND_PERSON_OPTIONS_URL,
                success: function(data) {
                    var $dialogContainer = $("#findPersonOptionsForm");
                    var $detachedChildren = $dialogContainer.children().detach();
                    $("<div id=\"findPersonOptionsForm\"></div>").dialog({
                        width: 450,
                        title: "Find Person Options",
                        open: function() {
                            $detachedChildren.appendTo($dialogContainer);
                        }
                    });
                    $("#findPersonOptionsForm").empty().append(data);
                    if (research && research.findPersonOptionsController) {
                        research.findPersonOptionsController.open();
                    }
                }
            });
        });

        $("#startingPointReportCloseButton").unbind('click').bind('click', function(e) {
            startingPointReport.form.dialog(constants.CLOSE);
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                if ($(this).children().length <= 1) {
                    $(this).append(findPersonHelper.getMenuOptions(row));
                }
            }
        };
    }

    function open() {
        if (system.target) {
            startingPointReport.callerSpinner = system.target.id;
        }

        startingPointReport.form = $("#startingPointReportForm");
        loadEvents();

        if (startingPointReport.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "nonMormon": startingPoint.nonMormon, "born18101850": startingPoint.born18101850, "livedInUSA": startingPoint.livedInUSA, "needOrdinances": startingPoint.ordinances, "hint": startingPoint.hints, "duplicate": startingPoint.duplicates, "reportId": startingPoint.reportId },
                url: constants.STARTING_POINT_REPORT_DATA_URL,
                success: function (data) {
                    startingPoint.previous = data;
                    $("#startingPointReportTable").bootstrapTable("append", data);
                    system.openForm(startingPointReport.form, startingPointReport.formTitleImage, startingPointReport.spinner);
                }
            });
        } else {
            $("#startingPointReportTable").bootstrapTable("append", startingPoint.previous);
            system.openForm(startingPointReport.form, startingPointReport.formTitleImage, startingPointReport.spinner);
        }

    }

    function close() {
        system.initSpinner(startingPointReport.callerSpinner, true);
    }

    var startingPointReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    research.startingPointReportController = startingPointReportController;
    open();

    return startingPointReportController;
});

var _startingPointPerson = require('person');
var _startingPointSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row && row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default\"><span style=\"color: " + _startingPointPerson.getPersonColor(row.gender) + "\">" + _startingPointPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return [result].join('');
//    if (value != null) {
//        var idNumber = value.substring(0, value.indexOf("~"));
//        var fullname = value.substring(value.indexOf("~") + 1, value.size);
//        var idNumberUrl = "<p><a style=\"color: rgb(0,0,255)\" href=\"" + getFamilySearchSystem() + "/tree/#view=ancestor&person=" + idNumber + "\" target=\"_tab\">" + idNumber + "</a></p>";
//        var fullnameUrl = "<p><a href= \"#\" onClick=\" displayPerson('" + idNumber + "'); \" style= \" color: rgb(0, 153, 0)\" value= \"" + idNumber + "\" data-toggle=\" tooltip\" data-placement= \"top \" title=\" Select to display more info about this person\" >" + fullname + "</a></p>";
//        result = fullnameUrl + idNumberUrl;
//    }
    return result;
}

function reasonsFormatter(value) {
    var result = "";
    if (value) {
        var reasons = value.split("~");
        for (var i = 0; i < reasons.length - 1; i++) {
            var reason = reasons[i];
            if (reason.indexOf("BornBetween1810and1850") > -1) {
                var birthDate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Born between 1810 and 1850 - <b>" + birthDate + "</b></p>";
            } else if (reason.indexOf("DiedInUSA") > -1) {
                var deathPlace = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Died in the United States - <b>" + deathPlace + "</b></p>";
            } else if (reason.indexOf("BornInUSA") > -1) {
                var birthPlace = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Born in United States - <b>" + birthPlace + "</b></p>";
            } else if (reason.indexOf("NoBirthDate") > -1) {
                var noBirthDate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Invalid birth date - <b>" + noBirthDate + "</b></p>";
            } else if (reason.indexOf("NonMormon") > -1) {
                result += "<p>Non-Mormon</p>";
            } else if (reason.indexOf("Hint") > -1) {
                var hint = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Hint - <b>" + hint + "</b></p>";
            } else if (reason.indexOf("PossibleDuplicate") > -1) {
                var possibleDuplicate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Possible Duplicate - <b>" + possibleDuplicate + "</b></p>";
            } else if (reason.indexOf("NoBirthPlace") > -1) {
                result += "<p>No birth place</p>";
            } else if (reason.indexOf("IncompleteOrdinances") > -1) {
                var ordinances = reason.substring(reason.indexOf("[") + 1, reason.length - 2);
                result += "<p>IncompleteOrdinances - <b>" + ordinances + "</b></p>";
            } else {
                result = value;
            }

        }
    }
    return result;
}

//# sourceURL=StartingPointReport.js