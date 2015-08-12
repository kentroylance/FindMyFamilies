define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
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
            system.spinnerArea = dateProblems.spinner;
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
                    $(this).append(findPersonHelper.getMenuOptions(row));
            },
            'mouseout .personAction1': function (e, value, row, index) {
                $('#personInfoDiv').hide();
            },
            'mouseover .personAction1': function (e, value, row, index) {

                $('#content').empty();

                var html = "<label><span style=\"color: " + _dateProblemsPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _dateProblemsPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _dateProblemsPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _dateProblemsPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#content').append(html);
                $('#personInfoDiv').show();
                $("#personInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#dateProblemsReportForm")
                });


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
                    if (data && data.errorMessage) {
                        system.spinnerArea = dateProblems.spinner;
                        system.stopSpinner(force);
                        msgBox.error(data.errorMessage);
                    } else {
                        dateProblems.previous = data.list;
                        if (!data.list || data.list < 1) {
                            $('.no-records-found').show();
                        } else {
                            $("#dateProblemsReportTable").bootstrapTable("append", data.list);
                            system.openForm(dateProblemsReport.form, dateProblemsReport.formTitleImage, dateProblemsReport.spinner);
                        }
                    }
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