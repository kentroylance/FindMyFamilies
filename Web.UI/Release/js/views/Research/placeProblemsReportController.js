define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var placeProblems = require('placeProblems');
    var placeProblemsReport = require('placeProblemsReport');

    function loadEvents() {

        $("#placeProblemsReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, placeProblemsReport);
        });

        $("#placeProblemsReportSaveButton").unbind('click').bind('click', function (e) {
            placeProblems.savePrevious();
            placeProblemsReport.form.dialog(constants.CLOSE);
        });


        $("#placeProblemsReportCancelButton").unbind('click').bind('click', function (e) {
            placeProblemsReport.form.dialog(constants.CLOSE);
        });

        $("#placeProblemsReportCloseButton").unbind('click').bind('click', function(e) {
            placeProblemsReport.form.dialog(constants.CLOSE);
        });

        placeProblemsReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.initSpinner(placeProblemsReport.callerSpinner, true);
            person.save();
            if (placeProblemsReport.callback) {
                if (typeof (placeProblemsReport.callback) === "function") {
                    placeProblemsReport.callback(person.selected);
                }
            }
//            placeProblemsReport.reset();
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                if ($(this).children().length <= 1) {
                    $(this).append(findPersonHelper.getMenuOptions(row));
                }
            }
        };

        var $result = $('#eventsResult');

        $('#placeProblemssTable').on('all.bs.table', function (e, name, args) {
                console.log('Event:', name, ', data:', args);
            })
            .on('click-row.bs.table', function(e, row, $element) {
                $result.text('Event: click-row.bs.table');
            })
            .on('dbl-click-row.bs.table', function(e, row, $element) {
                $result.text('Event: dbl-click-row.bs.table');
            })
            .on('sort.bs.table', function(e, name, order) {
                $result.text('Event: sort.bs.table');
            })
            .on('check.bs.table', function(e, row) {
                $result.text('Event: check.bs.table');
            })
            .on('uncheck.bs.table', function(e, row) {
                $result.text('Event: uncheck.bs.table');
            })
            .on('check-all.bs.table', function(e) {
                $result.text('Event: check-all.bs.table');
            })
            .on('uncheck-all.bs.table', function(e) {
                $result.text('Event: uncheck-all.bs.table');
            })
            .on('load-success.bs.table', function(e, data) {
                $result.text('Event: load-success.bs.table');
            })
            .on('load-error.bs.table', function(e, status) {
                $result.text('Event: load-error.bs.table');
            })
            .on('column-switch.bs.table', function(e, field, checked) {
                $result.text('Event: column-switch.bs.table');
            })
            .on('page-change.bs.table', function(e, size, number) {
                $result.text('Event: page-change.bs.table');
            })
            .on('search.bs.table', function(e, text) {
                $result.text('Event: search.bs.table');
            });
    }

    function open() {
        var currentSpinnerTarget = system.target.id;
        if (system.target) {
            placeProblemsReport.callerSpinner = system.target.id;
        }

        placeProblemsReport.form = $("#placeProblemsReportForm");
        loadEvents();

        if (placeProblems.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "nonMormon": placeProblems.nonMormon, "born18101850": placeProblems.born18101850, "livedInUSA": placeProblems.livedInUSA, "needOrdinances": placeProblems.ordinances, "hint": placeProblems.hints, "duplicate": placeProblems.duplicates, "reportId": person.reportId },
                url: constants.PLACE_PROBLEMS_REPORT_DATA_URL,
                success: function (data) {
                    placeProblems.previous = data;
                    $("#placeProblemsReportTable").bootstrapTable("append", data);
                    system.openForm(placeProblemsReport.form, placeProblemsReport.formTitleImage, placeProblemsReport.spinner);
                }
            });
        } else {
            $("#placeProblemsReportTable").bootstrapTable("append", placeProblems.previous);
            system.openForm(placeProblemsReport.form, placeProblemsReport.formTitleImage, placeProblemsReport.spinner);
        }

    }

    var placeProblemsReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.placeProblemsReportController = placeProblemsReportController;
    open();

    return placeProblemsReportController;
});

var _placeProblemsPerson = require('person');
var _placeProblemsSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
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
            } else if (reason.indexOf("PlaceProblems") > -1) {
                var ordinances = reason.substring(reason.indexOf("[") + 1, reason.length - 2);
                result += "<p>PlaceProblems - <b>" + ordinances + "</b></p>";
            } else {
                result = value;
            }

        }
    }
    return result;
}

//# sourceURL=placeProblemsReportController.js