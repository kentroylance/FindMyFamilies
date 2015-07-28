define(function (require) {
    var constants = require('constants');
    var _formName = "hintsReportForm";
    var _formTitleImage = "fa fmf-hint24";
    var _form = $("#hintsReportForm");
    var _spinner = "hintsReportSpinner";
    var _callerSpinner;

    function HintsReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var hintsReport = new HintsReportDO();
            localStorage.setItem(constants.HINTS_REPORT, JSON.stringify(hintsReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var hintsReport = JSON.parse(localStorage.getItem(constants.HINTS_REPORT));
            if (!hintsReport) {
                hintsReport = new HintsReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var hintsReport = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        save: function () {
            save();
        }
    };

    return hintsReport;
});
//# sourceURL=hintsReport.js