define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var possibleDuplicates = require('possibleDuplicates');
    var possibleDuplicatesReport = require('possibleDuplicatesReport');

    function loadEvents() {

        $("#possibleDuplicatesReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, possibleDuplicatesReport);
        });

        $("#possibleDuplicatesReportSaveButton").unbind('click').bind('click', function (e) {
            possibleDuplicates.savePrevious();
            possibleDuplicatesReport.form.dialog(constants.CLOSE);
        });


        $("#possibleDuplicatesReportCancelButton").unbind('click').bind('click', function (e) {
            possibleDuplicatesReport.form.dialog(constants.CLOSE);
        });

        $("#possibleDuplicatesReportCloseButton").unbind('click').bind('click', function(e) {
            possibleDuplicatesReport.form.dialog(constants.CLOSE);
        });

        possibleDuplicatesReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.spinnerArea = possibleDuplicates.spinner;
            person.save();
            if (possibleDuplicatesReport.callback) {
                if (typeof (possibleDuplicatesReport.callback) === "function") {
                    possibleDuplicatesReport.callback(person.selected);
                }
            }
//            possibleDuplicatesReport.reset();
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

                var html = "<label><span style=\"color: " + _possibleDuplicatesPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _possibleDuplicatesPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _possibleDuplicatesPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _possibleDuplicatesPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#content').append(html);
                $('#personInfoDiv').show();
                $("#personInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#possibleDuplicatesReportForm")
                });


            }
        };

        var $result = $('#eventsResult');

        $('#possibleDuplicatesTable').on('all.bs.table', function (e, name, args) {
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
        possibleDuplicatesReport.form = $("#possibleDuplicatesReportForm");
        loadEvents();

        if (possibleDuplicates.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "possibleDuplicates": possibleDuplicates.possibleDuplicates, "possibleMatches": possibleDuplicates.possibleMatches, "reportId": person.reportId },
                url: constants.POSSIBLE_DUPLICATES_REPORT_DATA_URL,
                success: function (data) {
                    if (data && data.errorMessage) {
                        system.spinnerArea = possibleDuplicates.spinner;
                        system.stopSpinner(true);
                        msgBox.error(data.errorMessage);
                    } else {
                        possibleDuplicates.previous = data.list;
                        $("#possibleDuplicatesReportTable").bootstrapTable("append", data.list);
                        system.openForm(possibleDuplicatesReport.form, possibleDuplicatesReport.formTitleImage, possibleDuplicatesReport.spinner);
                        if (person.reportId === constants.REPORT_ID) {
                            possibleDuplicatesController.loadReports(true);
                        }
                    }
                }
            });
        } else {
            system.spinnerArea = possibleDuplicates.spinner;
            system.stopSpinner(true);
            $("#possibleDuplicatesReportTable").bootstrapTable("append", possibleDuplicates.previous);
            system.openForm(possibleDuplicatesReport.form, possibleDuplicatesReport.formTitleImage, possibleDuplicatesReport.spinner);
        }

    }

    var possibleDuplicatesReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.possibleDuplicatesReportController = possibleDuplicatesReportController;
    open();

    return possibleDuplicatesReportController;
});

var _possibleDuplicatesPerson = require('person');
var _possibleDuplicatesSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default dropdown-btn personAction1\"><span style=\"color: " + _possibleDuplicatesPerson.getPersonColor(row.gender) + "\">" + _possibleDuplicatesPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function linkFormatter(value) {
    var result = "";
    if (value) {
        result = _possibleDuplicatesSystem.familySearchSystem() + "/tree/#view=possibleDuplicates&person=" + value;
        result = "<a style=\"color: rgb(50,205,50)\" href=\"" + result + "\" target=\"_tab\">Duplicate</a>&nbsp;";
    }
    return result;
}

//# sourceURL=possibleDuplicatesReportController.js