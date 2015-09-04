define(function(require) {

    var $ = require("jquery");
    var person = require("person");
    var constants = require("constants");
    var msgBox = require("msgBox");

    var _formName = "retrieveForm";
    var _formTitleImage = "fmf-retrieve24";
    var _form = $("#retrieveForm");
    var _spinner = "retrieveSpinner";
    var _callerSpinner;
    var _callback;
    var _addSelect = true;

    var _title;
    var _retrievedRecords = 0;
    var _reports;
    var _selected = false;
    var _reportId = constants.REPORT_ID;
    var _reportFile;

    function RetrieveDO(reports) {
        this.reports = reports;
    }

    function save() {
        if (window.localStorage) {
            var retrieve = new RetrieveDO(_reports);
            localStorage.setItem(constants.RETRIEVE, JSON.stringify(retrieve));
        }
    }

    function load() {
        if (window.localStorage) {
            var retrieve = JSON.parse(localStorage.getItem(constants.RETRIEVE));
            if (!retrieve) {
                retrieve = new RetrieveDO();
            }
            _reports = retrieve.reports;
        }
    }

    function reset() {
        _selected = false;
        _callback = null;
    }

    function clear() {
        _retrievedRecords = 0;
        _title = "";
    }


    function findReport() {
        var found = false;
        if (_reports) {
            $.each(_reports, function(i) {
                if (_reports[i].id === person.id) {
                    person.reportId = _reports[i].reportId;
                    person.reportFile = _reports[i].reportFile;
                    found = true;
                    return false;
                }
            });
        }
        return found;
    }

    function loadReportData() {
        $.ajax({
//            'async': false,
            url: constants.GET_REPORT_LIST_URL,
            success: function(data) {
                if (data && data.errorMessage) {
                    msgBox.error(data.errorMessage);
                } else {
                    if (data && data.list) {
                        _reports = data.list;
                        save();
                    }

                }

            },
            'beforeSend': function() {
            },
            'complete': function() {
            }
        });
        return false;
    }


    function checkReports(reportInput) {
        person.resetReportId($('#' + reportInput));
        var data = retrieve.reports;
        $.each(data, function(i) {
            if ((data[i].id === person.id) && (data[i].researchType === person.researchType) && (data[i].generation === person.generation)) {
                person.reportId = data[i].reportId;
                person.reportFile = data[i].reportFile;
                return false;
            }
        });
        $('#' + reportInput).val(person.reportId);
    }


    function populatePersonFromReport(reportInput, personInput) {
        $('#' + reportInput).val(person.reportId);
        var data = retrieve.reports;
        $.each(data, function(i) {
            if (data[i].id === person.id) {
                person.reportId = data[i].reportId;
                person.reportFile = data[i].reportFile;
                if ((data[i].researchType === person.researchType) && (data[i].generation === person.generation)) {
                    person.id = data[i].id;
                    person.name = data[i].fullName;
                    person.researchType = data[i].researchType;
                    person.generation = data[i].generation;
                    person.loadPersons($('#' + personInput));
                    return false;
                }
            }
        });

    }

    function loadReports(reportInput, refreshReport) {
        var report = $('#' + reportInput);
        var optionhtml;
        if (_reports === undefined || _reports.length == 0) {
            $.ajax({
                'async': false,
                url: constants.GET_REPORT_LIST_URL,
                success: function(data) {
                    if (data && data.errorMessage) {
                        msgBox.error(data.errorMessage);
                    } else {
                        if (data && (data.list !== undefined && data.list.length > 0)) {
                            _reports = data.list;
                            var found = false;
                            save();
                            $.each(_reports, function(i) {
                                if (!found && (_reports[i].reportId === person.reportId)) {
                                    found = true;
                                    person.reportFile = _reports[i].reportFile;
                                } else if (!found && (person.reportId === '0') && _reports[i].id === person.id) {
                                    found = true;
                                    person.reportId = _reports[i].reportId;
                                    person.reportFile = _reports[i].reportFile;
                                }
                                optionhtml = '<option value="' + _reports[i].reportId + '">' + _reports[i].fullName + " - " + _reports[i].id + '</option>';
                                report.append(optionhtml);
                            });
                            if (!found) {
                                person.reportId = constants.REPORT_ID;
                                person.reportFile = "";
                            }
                        }
                    }
                }
            });
        } else {
            if (refreshReport) {
                report.empty();
                if (_addSelect) {
                    optionhtml = '<option value="' + constants.REPORT_ID + '">' + constants.SELECT + '</option>';
                    report.append(optionhtml);
                }
                $.ajax({
                    'async': false,
                    url: constants.GET_REPORT_LIST_URL,
                    success: function(data) {
                        if (data && data.errorMessage) {
                            msgBox.error(data.errorMessage);
                        } else {
                            if (data && (data.list !== undefined && data.list.length > 0)) {
                                _reports = data.list;
                                $.each(_reports, function(i) {
                                    if (person.reportId === _reports[i].reportId) {
                                        optionhtml = '<option value="' + _reports[i].reportId + '" selected>' + _reports[i].fullName + " - " + _reports[i].id + '</option>';
                                        person.reportId = _reports[i].reportId;
                                        person.reportFile = _reports[i].reportFile;
                                    } else {
                                        optionhtml = '<option value="' + _reports[i].reportId + '">' + _reports[i].fullName + " - " + _reports[i].id + '</option>';
                                    }
                                    report.append(optionhtml);
                                });
                            }
                        }
                    }
                });
            } else if (_reports !== undefined && _reports.length > 0) {
                report.empty();
                if (_addSelect) {
                    optionhtml = '<option value="' + constants.REPORT_ID + '">Select</option>';
                    report.append(optionhtml);
                }
                var found = false;
                $.each(_reports, function(i) {
                    if (_reports[i].id === person.id) {
                        person.reportId = _reports[i].reportId;
                        person.reportFile = _reports[i].reportFile;
                        found = true;
                        optionhtml = '<option value="' + _reports[i].reportId + '">' + _reports[i].fullName + " - " + _reports[i].id + '</option>';
                        report.append(optionhtml);
                        return false;
                    }
                });
                if (!found) {
                    person.reportId = constants.REPORT_ID;
                    person.reportFile = "";
                }
            }
        }
        _addSelect = true;
    }

    //load();
    loadReportData();

    var retrieve = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        get reports() {
            return _reports;
        },
        set reports(value) {
            _reports = value;
        },
        get addSelect() {
            return _addSelect;
        },
        set addSelect(value) {
            _addSelect = value;
        },
        get retrievedRecords() {
            return _retrievedRecords;
        },
        set retrievedRecords(value) {
            _retrievedRecords = value;
        },
        findReport: function() {
            return findReport();
        },
        loadReports: function(reportInput, refreshReport) {
            loadReports(reportInput, refreshReport);
        },
        checkReports: function(reportInput) {
            checkReports(reportInput);
        },
        populatePersonFromReport: function(reportInput, personInput) {
            populatePersonFromReport(reportInput, personInput);
        },
        clear: function() {
            clear();
        },
        reset: function() {
            reset();
        },
        save: function() {
            save();
        },
        loadReportData: function() {
            loadReportData();
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        get callback() {
            return _callback;
        },
        set callback(value) {
            _callback = value;
        },
        get selected() {
            return _selected;
        },
        set selected(value) {
            _selected = value;
        },
        get reportId() {
            return _reportId;
        },
        set reportId(value) {
            _reportId = value;
        },
        get reportFile() {
            return _reportFile;
        },
        set reportFile(value) {
            _reportFile = value;
        }



    };

    return retrieve;
});