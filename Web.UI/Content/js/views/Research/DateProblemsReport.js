define(function (require) {
    var constants = require('constants');
    var _formName = "dateProblemsReportForm";
    var _formTitleImage = "fa fmf-search24";
    var _form = $("#dateProblemsReportForm");
    var _spinner = "dateProblemsReportSpinner";

    var _callerSpinner;

    function DateProblemsReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var dateProblemsReport = new DateProblemsReportDO();
            localStorage.setItem(constants.DATE_PROBLEMS_REPORT, JSON.stringify(dateProblemsReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var dateProblemsReport = JSON.parse(localStorage.getItem(constants.DATE_PROBLEMS_REPORT));
            if (!dateProblemsReport) {
                dateProblemsReport = new DateProblemsReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var dateProblemsReport = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        save: function () {
            save();
        }
    };

    return dateProblemsReport;
});
//# sourceURL=DateProblemsReport.js