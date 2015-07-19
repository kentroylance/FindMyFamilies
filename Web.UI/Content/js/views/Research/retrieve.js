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
    
    var _title;
    var _retrievedRecords = 0;
    var _reports;

    function save() {
        person.save();
    }

    function reset() {
        person.selected = false;
        _callback = null;
    }

    function clear() {
        _retrievedRecords = 0;
        _title = "";
    }

    function loadReports(reportId, refreshReport) {
        var optionhtml;
        if (!_reports) {
            $.ajax({
                'async': false,
                url: constants.GET_REPORT_LIST_URL,
                success: function (data) {
                    if (data) {
                        _reports = data;
                        var found = false;
                        $.each(data, function (i) {
                            if (data[i].ValueMember === person.reportId) {
                                found = true;
                            } else if ((person.reportId === constants.REPORT_ID) && (i === 0)) {
                                person.reportId = data[i].ValueMember;
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
            });
        } else {
            if (refreshReport) {
                reportId.empty();
                optionhtml = '<option value="' + constants.REPORT_ID + '">' + constants.SELECT + '</option>';
                reportId.append(optionhtml);
                $.ajax({
                    'async': false,
                    url: constants.GET_REPORT_LIST_URL,
                    success: function (data) {
                        if (data) {
                            _reports = data;
                            $.each(data, function (i) {
                                if (person.reportId && (parseInt(person.reportId) === 0)) {
                                    optionhtml = '<option value="' + data[i].ValueMember + '" selected>' + data[i].DisplayMember + '</option>';
                                    person.reportId = data[i].ValueMember;
                                } else {
                                    optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                                }
                                reportId.append(optionhtml);
                            });
                        }
                    }
                });
            } else if (_reports) {
                var data = _reports;
                reportId.empty();
                optionhtml = '<option value="' + constants.REPORT_ID + '">Select</option>';
                reportId.append(optionhtml);
                var found = false;
                $.each(data, function (i) {
                    if (data[i].ValueMember === person.reportId) {
                        found = true;
                    } else if ((person.reportId === constants.REPORT_ID) && (i === 0)) {
                        person.reportId = data[i].ValueMember;
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
    }

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
        get retrievedRecords() {
            return _retrievedRecords;
        },
        set selected(value) {
            _retrievedRecords = value;
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
        }
        
    };

    return retrieve;
});

