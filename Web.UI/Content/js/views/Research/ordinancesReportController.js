define(function(require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var ordinances = require('ordinances');
    var ordinancesController = require('ordinancesController');
    var ordinancesReport = require('ordinancesReport');

    function loadEvents() {

        $("#ordinancesReportOptionsButton").unbind('click').bind('click', function(e) {
            findPersonHelper.findOptions(e, ordinancesReport);
        });

        $("#ordinancesReportSaveButton").unbind('click').bind('click', function(e) {
            ordinances.savePrevious();
            ordinancesReport.form.dialog(constants.CLOSE);
        });


        $("#ordinancesReportCancelButton").unbind('click').bind('click', function(e) {
            ordinancesReport.form.dialog(constants.CLOSE);
        });

        $("#ordinancesReportCloseButton").unbind('click').bind('click', function(e) {
            ordinancesReport.form.dialog(constants.CLOSE);
        });

        ordinancesReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            system.spinnerArea = ordinances.spinner;
            person.save();
            if (ordinancesReport.callback) {
                if (typeof (ordinancesReport.callback) === "function") {
                    ordinancesReport.callback(person.selected);
                }
            }
//            ordinancesReport.reset();
        });

        window.nameEvents = {
            'click .ordinancesReportOptionsAction': function (e, value, row, index) {
                 $(this).append(findPersonHelper.getMenuOptions(row));
            },
            'mouseout .ordinancesReportAction': function (e, value, row, index) {
                $('#ordinancesReportPersonInfoDiv').hide();
            },
            'mouseover .ordinancesReportAction': function (e, value, row, index) {

                $('#ordinancesReportPersonInfoContent').empty();

                var html = "<label><span style=\"color: " + _ordinancesPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _ordinancesPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _ordinancesPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _ordinancesPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#ordinancesReportPersonInfoContent').append(html);
                $('#ordinancesReportPersonInfoDiv').show();
                $("#ordinancesReportPersonInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#ordinancesReportForm")
                });


            }
        };

        var $result = $('#eventsResult');

        $('#ordinancessTable').on('all.bs.table', function(e, name, args) {
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
        ordinancesReport.form = $("#ordinancesReportForm");
        loadEvents();

        if (ordinances.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "reportId": person.reportId, "reportFile": person.reportFile },
                url: constants.ORDINANCES_REPORT_DATA_URL,
                success: function(data) {
                    system.stopSpinner(true);
                    if (data && data.errorMessage) {
                        system.spinnerArea = ordinances.spinner;
                        msgBox.error(data.errorMessage);
                    } else {
                        ordinances.previous = data.list;
                        $("#ordinancesReportTable").bootstrapTable("append", data.list);
                        system.openForm(ordinancesReport.form, ordinancesReport.formTitleImage, ordinancesReport.spinner);
                        if (person.reportId === constants.REPORT_ID) {
                            person.reportId = data.reportId;
                            person.reportFile = data.reportFile;
                            ordinancesController.loadReports(true);
                        }
                    }
                }
            });
        } else {
            system.spinnerArea = ordinances.spinner;
            system.stopSpinner(true);
            $("#ordinancesReportTable").bootstrapTable("append", ordinances.previous);
            system.openForm(ordinancesReport.form, ordinancesReport.formTitleImage, ordinancesReport.spinner);
        }

    }

    var ordinancesReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.ordinancesReportController = ordinancesReportController;
    open();

    return ordinancesReportController;
});

var _ordinancesPerson = require('person');
var _ordinancesSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group \"><button type=\"button\" class=\"btn btn-default dropdown-btn ordinancesReportAction\"><span style=\"color: " + _ordinancesPerson.getPersonColor(row.gender) + "\">" + _ordinancesPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"ordinancesReportOptionsAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function statusFormatter(value) {
    var result = "";
    if (value) {
        result = value;
    }
    return result;
}

//# sourceURL=ordinancesReportController.js