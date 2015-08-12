define(function (require) {

    var $ = require("jquery");
    var person = require("person");
    var constants = require("constants");

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
            $.each(_reports, function (i) {
                if (_reports[i].DisplayMember && _reports[i].DisplayMember.indexOf(person.id) > -1) {
                    person.reportId = _reports[i].ValueMember;
                    found = true;
                    return false;
                }
            });
        }
        return found;
    }

    function loadReportData() {
        $.ajax({
            'async': false,
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

            }
        });
        return false;
    }

    function loadReports(reportId, refreshReport) {
        var optionhtml;
        if (!_reports) {
            $.ajax({
                'async': false,
                url: constants.GET_REPORT_LIST_URL,
                success: function (data) {
                    if (data && data.errorMessage) {
                        msgBox.error(data.errorMessage);
                    } else {
                        if (data && data.list) {
                            _reports = data.list;
                            var found = false;
                            save();
                            $.each(_reports, function (i) {
                                if (_reports[i].ValueMember === person.reportId) {
                                    found = true;
                                }
                                optionhtml = '<option value="' + _reports[i].ValueMember + '">' + _reports[i].DisplayMember + '</option>';
                                reportId.append(optionhtml);
                            });
                            if (!found) {
                                person.reportId = constants.REPORT_ID;
                            }
                        }
                    }
                }
            });
        } else {
            if (refreshReport) {
                reportId.empty();
                if (_addSelect) {
                    optionhtml = '<option value="' + constants.REPORT_ID + '">' + constants.SELECT + '</option>';
                    reportId.append(optionhtml);
                }
                $.ajax({
                    'async': false,
                    url: constants.GET_REPORT_LIST_URL,
                    success: function (data) {
                        if (data && data.errorMessage) {
                            msgBox.error(data.errorMessage);
                        } else {
                            if (data) {
                                _reports = data.list;
                                $.each(_reports, function (i) {
                                    if (person.reportId && (parseInt(person.reportId) === 0)) {
                                        optionhtml = '<option value="' + _reports[i].ValueMember + '" selected>' + _reports[i].DisplayMember + '</option>';
                                        person.reportId = _reports[i].ValueMember;
                                    } else {
                                        optionhtml = '<option value="' + _reports[i].ValueMember + '">' + _reports[i].DisplayMember + '</option>';
                                    }
                                    reportId.append(optionhtml);
                                });
                            }
                        }
                    }
                });
            } else if (_reports) {
                var data = _reports;
                reportId.empty();
                if (_addSelect) {
                    optionhtml = '<option value="' + constants.REPORT_ID + '">Select</option>';
                    reportId.append(optionhtml);
                }
                var found = false;
                $.each(data, function (i) {
                    if (data[i].ValueMember === person.reportId) {
                        found = true;
                    }
                    optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                    reportId.append(optionhtml);
                });
                if (!found) {
                    person.reportId = constants.REPORT_ID;
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
        findReport: function () {
            return findReport();
        },
        loadReports: function (reportId, refreshReport) {
            loadReports(reportId, refreshReport);
        },
        clear: function () {
            clear();
        },
        reset: function () {
            reset();
        },
        save: function () {
            save();
        },
        loadReportData: function () {
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
        }

        
    };

    return retrieve;
});

