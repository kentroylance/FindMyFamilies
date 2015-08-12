define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var startingPoint = require('startingPoint');
    var startingPointController = require('startingPointController');
    var startingPointReport = require('startingPointReport');

    function loadEvents() {

        $("#startingPointReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, startingPointReport);
        });

        $("#startingPointReportSaveButton").unbind('click').bind('click', function (e) {
            startingPoint.savePrevious();
            startingPointReport.form.dialog(constants.CLOSE);
        });


        $("#startingPointReportCancelButton").unbind('click').bind('click', function (e) {
            startingPointReport.form.dialog(constants.CLOSE);
        });

        $("#startingPointReportCloseButton").unbind('click').bind('click', function(e) {
            startingPointReport.form.dialog(constants.CLOSE);
        });

        startingPointReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.spinnerArea = startingPoint.spinner;
            person.save();
            if (startingPointReport.callback) {
                if (typeof (startingPointReport.callback) === "function") {
                    startingPointReport.callback(person.selected);
                }
            }
//            startingPointReport.reset();
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

                var html = "<label><span style=\"color: " + _startingPointPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _startingPointPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _startingPointPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _startingPointPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#content').append(html);
                $('#personInfoDiv').show();
                $("#personInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#startingPointForm")
                });


            }
        };

        var $result = $('#eventsResult');

        $('#startingPointsTable').on('all.bs.table', function (e, name, args) {
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
        startingPointReport.form = $("#startingPointReportForm");
        loadEvents();

        if (startingPointReport.displayType === "start") {
            $.ajax({
                data: { "id": person.id, "fullName": person.name, "generation": person.generation, "researchType": person.researchType, "nonMormon": startingPoint.nonMormon, "born18101850": startingPoint.born18101850, "livedInUSA": startingPoint.livedInUSA, "needOrdinances": startingPoint.ordinances, "hint": startingPoint.hints, "duplicate": startingPoint.duplicates, "reportId": person.reportId },
                url: constants.STARTING_POINT_REPORT_DATA_URL,
                success: function (data) {
                    if (data && data.errorMessage) {
                        system.spinnerArea = startingPoint.spinner;
                        system.stopSpinner(true);
                        msgBox.error(data.errorMessage);
                    } else {
                        startingPoint.previous = data.list;
                        $("#startingPointReportTable").bootstrapTable("append", data.list);
                        system.openForm(startingPointReport.form, startingPointReport.formTitleImage, startingPointReport.spinner);
                        if (person.reportId === constants.REPORT_ID) {
                            startingPointController.loadReports(true);
                        }
                    }
                }
            });
        } else {
            $("#startingPointReportTable").bootstrapTable("append", startingPoint.previous);
            system.openForm(startingPointReport.form, startingPointReport.formTitleImage, startingPointReport.spinner);
        }

    }

    var startingPointReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.startingPointReportController = startingPointReportController;
    open();

    return startingPointReportController;
});

var _startingPointPerson = require('person');
var _startingPointSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group \"><button type=\"button\" class=\"btn btn-default dropdown-btn personAction1\"><span style=\"color: " + _startingPointPerson.getPersonColor(row.gender) + "\">" + _startingPointPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
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
                result += "<p>IncompleteOrdinances - </p>" + ordinances + "<p></p>";
            } else {
                result = value;
            }

        }
    }
    return result;
}

//# sourceURL=startingPointReportController.js