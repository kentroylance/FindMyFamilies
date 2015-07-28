define(function (require) {
    var constants = require('constants');
    var _formName = "findCluesReportForm";
    var _formTitleImage = "fa fmf-clue24";
    var _form = $("#findCluesReportForm");
    var _spinner = "findCluesReportSpinner";
    var _callerSpinner;

    function FindCluesReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var findCluesReport = new FindCluesReportDO();
            localStorage.setItem(constants.FIND_CLUES_REPORT, JSON.stringify(findCluesReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var findCluesReport = JSON.parse(localStorage.getItem(constants.FIND_CLUES_REPORT));
            if (!findCluesReport) {
                findCluesReport = new FindCluesReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var findCluesReport = {
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

    return findCluesReport;
});
//# sourceURL=findCluesReport.js