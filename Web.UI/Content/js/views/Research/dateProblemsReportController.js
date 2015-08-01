define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var dateProblems = require('dateProblems');
    var dateProblemsReport = require('dateProblemsReport');

    function loadEvents() {

        $("#dateProblemsReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, dateProblemsReport);
        });

        $("#dateProblemsReportSaveButton").unbind('click').bind('click', function (e) {
            dateProblems.savePrevious();
            dateProblemsReport.form.dialog(constants.CLOSE);
        });


        $("#dateProblemsReportCancelButton").unbind('click').bind('click', function (e) {
            dateProblemsReport.form.dialog(constants.CLOSE);
        });

        $("#dateProblemsReportCloseButton").unbind('click').bind('click', function(e) {
            dateProblemsReport.form.dialog(constants.CLOSE);
        });

        dateProblemsReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.initSpinner(dateProblemsReport.callerSpinner, true);
            person.save();
            if (dateProblemsReport.callback) {
                if (typeof (dateProblemsReport.callback) === "function") {
                    dateProblemsReport.callback(person.selected);
                }
            }
//            dateProblemsReport.reset();
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                if ($(this).children().length <= 1) {
                    $(this).append(findPersonHelper.getMenuOptions(row));
                }
            }
        };

        var $result = $('#eventsResult');

        $('#dateProblemssTable').on('all.bs.table', function (e, name, args) {
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
        dateProblemsReport.form = $("#dateProblemsReportForm");
        loadEvents();

        if (dateProblems.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "empty": dateProblems.empty, "invalid": dateProblems.invalid, "invalidFormat": dateProblems.invalidFormat, "incomplete": dateProblems.incomplete, "reportId": person.reportId },
                url: constants.DATE_PROBLEMS_REPORT_DATA_URL,
                success: function (data) {
                    dateProblems.previous = data;
                    $("#dateProblemsReportTable").bootstrapTable("append", data);
                    system.openForm(dateProblemsReport.form, dateProblemsReport.formTitleImage, dateProblemsReport.spinner);
                }
            });
        } else {
            $("#dateProblemsReportTable").bootstrapTable("append", dateProblems.previous);
            system.openForm(dateProblemsReport.form, dateProblemsReport.formTitleImage, dateProblemsReport.spinner);
        }

    }

    var dateProblemsReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.dateProblemsReportController = dateProblemsReportController;
    open();

    return dateProblemsReportController;
});

var _dateProblemsPerson = require('person');
var _dateProblemsSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default dropdown-btn\"><span style=\"color: " + _dateProblemsPerson.getPersonColor(row.gender) + "\">" + _dateProblemsPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function linkFormatter(value) {
    var result = "";
    if (value) {
        result = _dateProblemsSystem.familySearchSystem() + "/tree/#view=datesPlaces&person=" + value;
        result = "<a style=\"color: rgb(50,205,50)\" href=\"" + result + "\" target=\"_tab\">Dates</a>&nbsp;";
    }
    return result;
}


//# sourceURL=dateProblemsReportController.js