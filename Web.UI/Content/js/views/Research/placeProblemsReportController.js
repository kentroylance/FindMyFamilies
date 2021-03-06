define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var placeProblems = require('placeProblems');
    var placeProblemsController = require('placeProblemsController');
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
            system.spinnerArea = placeProblems.spinner;
            person.save();
            if (placeProblemsReport.callback) {
                if (typeof (placeProblemsReport.callback) === "function") {
                    placeProblemsReport.callback(person.selected);
                }
            }
//            placeProblemsReport.reset();
        });

        window.nameEvents = {
            'click .placeProblemsReportOptionsAction': function (e, value, row, index) {
                 $(this).append(findPersonHelper.getMenuOptions(row));
            },
            'mouseout .placeProblemsReportAction': function (e, value, row, index) {
                $('#placeProblemsReportPersonInfoDiv').hide();
            },
            'mouseover .placeProblemsReportAction': function (e, value, row, index) {

                $('#placeProblemsReportPersonInfoContent').empty();

                var html = "<label><span style=\"color: " + _placeProblemsPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _placeProblemsPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _placeProblemsPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _placeProblemsPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#placeProblemsReportPersonInfoContent').append(html);
                $('#placeProblemsReportPersonInfoDiv').show();
                $("#placeProblemsReportPersonInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#placeProblemsReportForm")
                });


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
        placeProblemsReport.form = $("#placeProblemsReportForm");
        loadEvents();

        if (placeProblems.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "nonMormon": placeProblems.nonMormon, "born18101850": placeProblems.born18101850, "livedInUSA": placeProblems.livedInUSA, "needOrdinances": placeProblems.ordinances, "hint": placeProblems.hints, "duplicate": placeProblems.duplicates, "reportId": person.reportId, "reportFile": person.reportFile },
                url: constants.PLACE_PROBLEMS_REPORT_DATA_URL,
                success: function (data) {
                    system.stopSpinner(true);
                    if (data && data.errorMessage) {
                        system.spinnerArea = placeProblems.spinner;
                        msgBox.error(data.errorMessage);
                    } else {
                        placeProblems.previous = data.list;
                        $("#placeProblemsReportTable").bootstrapTable("append", data.list);
                        system.openForm(placeProblemsReport.form, placeProblemsReport.formTitleImage, placeProblemsReport.spinner);
                        if (person.reportId === constants.REPORT_ID) {
                            person.reportId = data.reportId;
                            person.reportFile = data.reportFile;
                            placeProblemsController.loadReports(true);
                        }
                    }
                }
            });
        } else {
            system.spinnerArea = placeProblems.spinner;
            system.stopSpinner(true);
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
        result = "<div class=\"btn-group \"><button type=\"button\" class=\"btn btn-default dropdown-btn placeProblemsReportAction\"><span style=\"color: " + _placeProblemsPerson.getPersonColor(row.gender) + "\">" + _placeProblemsPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"placeProblemsReportOptionsAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function linkFormatter(value) {
    var result = "";
    if (value) {
        result = _placeProblemsSystem.familySearchSystem() + "/tree/#view=places&person=" + value;
        result = "<a style=\"color: rgb(50,205,50)\" href=\"" + result + "\" target=\"_tab\">Duplicate</a>&nbsp;";
    }
    return result;
}


//# sourceURL=placeProblemsReportController.js