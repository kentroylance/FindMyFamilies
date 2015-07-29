define(function (require) {
    var constants = require('constants');
    var _formName = "placeProblemsReportForm";
    var _formTitleImage = "fa fmf-place24";
    var _form = $("#placeProblemsReportForm");
    var _spinner = "placeProblemsReportSpinner";
    var _callerSpinner;

    function PlaceProblemsReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var placeProblemsReport = new PlaceProblemsReportDO();
            localStorage.setItem(constants.PLACE_PROBLEMS_REPORT, JSON.stringify(placeProblemsReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var placeProblemsReport = JSON.parse(localStorage.getItem(constants.PLACE_PROBLEMS_REPORT));
            if (!placeProblemsReport) {
                placeProblemsReport = new PlaceProblemsReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var placeProblemsReport = {
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

    return placeProblemsReport;
});
//# sourceURL=placeProblemsReport.js