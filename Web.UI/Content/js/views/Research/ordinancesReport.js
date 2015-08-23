define(function (require) {
    var constants = require('constants');
    var _formName = "ordinancesReportForm";
    var _formTitleImage = "fa fmf-temple24";
    var _form = $("#ordinancesReportForm");
    var _spinner = "ordinancesReportSpinner";
    var _callerSpinner;

    function OrdinancesReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var ordinancesReport = new OrdinancesReportDO();
            localStorage.setItem(constants.ORDINANCES_REPORT, JSON.stringify(ordinancesReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var ordinancesReport = JSON.parse(localStorage.getItem(constants.ORDINANCES_REPORT));
            if (!ordinancesReport) {
                ordinancesReport = new OrdinancesReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var ordinancesReport = {
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

    return ordinancesReport;
});
//# sourceURL=ordinancesReport.js