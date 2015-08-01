define(function (require) {
    var constants = require('constants');
    var _formName = "possibleDuplicatesReportForm";
    var _formTitleImage = "fa fmf-duplicates24";
    var _form = $("#possibleDuplicatesReportForm");
    var _spinner = "possibleDuplicatesReportSpinner";
    var _callerSpinner;

    function PossibleDuplicatesReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var possibleDuplicatesReport = new PossibleDuplicatesReportDO();
            localStorage.setItem(constants.POSSIBLE_DUPLICATES_REPORT, JSON.stringify(possibleDuplicatesReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var possibleDuplicatesReport = JSON.parse(localStorage.getItem(constants.POSSIBLE_DUPLICATES_REPORT));
            if (!possibleDuplicatesReport) {
                possibleDuplicatesReport = new PossibleDuplicatesReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var possibleDuplicatesReport = {
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

    return possibleDuplicatesReport;
});
//# sourceURL=possibleDuplicatesReport.js