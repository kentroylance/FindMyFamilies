define(function (require) {
    var constants = require('constants');
    var _formName = "incompleteOrdinancesReportForm";
    var _formTitleImage = "fa fmf-search24";
    var _form = $("#incompleteOrdinancesReportForm");
    var _spinner = "incompleteOrdinancesReportSpinner";

    var _callerSpinner;

    function IncompleteOrdinancesReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var incompleteOrdinancesReport = new IncompleteOrdinancesReportDO();
            localStorage.setItem(constants.INCOMPLETE_ORDINANCES_REPORT, JSON.stringify(incompleteOrdinancesReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var incompleteOrdinancesReport = JSON.parse(localStorage.getItem(constants.INCOMPLETE_ORDINANCES_REPORT));
            if (!incompleteOrdinancesReport) {
                incompleteOrdinancesReport = new IncompleteOrdinancesReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var incompleteOrdinancesReport = {
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

    return incompleteOrdinancesReport;
});
//# sourceURL=incompleteOrdinancesReport.js