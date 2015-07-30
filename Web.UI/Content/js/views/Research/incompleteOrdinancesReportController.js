define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var incompleteOrdinances = require('incompleteOrdinances');
    var incompleteOrdinancesReport = require('incompleteOrdinancesReport');

    function loadEvents() {

        $("#incompleteOrdinancesReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, incompleteOrdinancesReport);
        });

        $("#incompleteOrdinancesReportSaveButton").unbind('click').bind('click', function (e) {
            incompleteOrdinances.savePrevious();
            incompleteOrdinancesReport.form.dialog(constants.CLOSE);
        });


        $("#incompleteOrdinancesReportCancelButton").unbind('click').bind('click', function (e) {
            incompleteOrdinancesReport.form.dialog(constants.CLOSE);
        });

        $("#incompleteOrdinancesReportCloseButton").unbind('click').bind('click', function(e) {
            incompleteOrdinancesReport.form.dialog(constants.CLOSE);
        });

        incompleteOrdinancesReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.initSpinner(incompleteOrdinancesReport.callerSpinner, true);
            person.save();
            if (incompleteOrdinancesReport.callback) {
                if (typeof (incompleteOrdinancesReport.callback) === "function") {
                    incompleteOrdinancesReport.callback(person.selected);
                }
            }
//            incompleteOrdinancesReport.reset();
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                if ($(this).children().length <= 1) {
                    $(this).append(findPersonHelper.getMenuOptions(row));
                }
            }
        };

        var $result = $('#eventsResult');

        $('#incompleteOrdinancessTable').on('all.bs.table', function (e, name, args) {
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
        incompleteOrdinancesReport.form = $("#incompleteOrdinancesReportForm");
        loadEvents();

        if (incompleteOrdinances.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "nonMormon": incompleteOrdinances.nonMormon, "born18101850": incompleteOrdinances.born18101850, "livedInUSA": incompleteOrdinances.livedInUSA, "needOrdinances": incompleteOrdinances.ordinances, "hint": incompleteOrdinances.hints, "duplicate": incompleteOrdinances.duplicates, "reportId": person.reportId },
                url: constants.INCOMPLETE_ORDINANCES_REPORT_DATA_URL,
                success: function (data) {
                    incompleteOrdinances.previous = data;
                    $("#incompleteOrdinancesReportTable").bootstrapTable("append", data);
                    system.openForm(incompleteOrdinancesReport.form, incompleteOrdinancesReport.formTitleImage, incompleteOrdinancesReport.spinner);
                }
            });
        } else {
            $("#incompleteOrdinancesReportTable").bootstrapTable("append", incompleteOrdinances.previous);
            system.openForm(incompleteOrdinancesReport.form, incompleteOrdinancesReport.formTitleImage, incompleteOrdinancesReport.spinner);
        }

    }

    var incompleteOrdinancesReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.incompleteOrdinancesReportController = incompleteOrdinancesReportController;
    open();

    return incompleteOrdinancesReportController;
});

var _incompleteOrdinancesPerson = require('person');
var _incompleteOrdinancesSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default\"><span style=\"color: " + _incompleteOrdinancesPerson.getPersonColor(row.gender) + "\">" + _incompleteOrdinancesPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function ordinanceFormatter(value) {
    var result = "";
    if (value != null) {
        if (value.indexOf("~") > -1) {
            var status = value.substring(0, value.indexOf("~"));
            var reservable = value.substring(value.indexOf("~") + 1, value.size);
            result = "<p>" + status + "</p><p>" + reservable + "</p>";
        } else {
            result = value;
        }
    }
    return result;
}

//# sourceURL=incompleteOrdinancesReportController.js