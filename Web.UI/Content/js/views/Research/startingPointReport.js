define(function (require) {
    var constants = require('constants');
    var _formName = "startingPointReportForm";
    var _formTitleImage = "fa fmf-search24";
    var _form = $("#startingPointReportForm");
    var _spinner = "startingPointReportSpinner";

    var _callerSpinner;

    function StartingPointReportDO(previous) {
        this.previous = previous;
    }

    function save() {
        if (window.localStorage) {
            var startingPointReport = new StartingPointReportDO();
            localStorage.setItem(constants.STARTING_POINT_REPORT, JSON.stringify(startingPointReport));
        }
    }

    function load() {
        if (window.localStorage) {
            var startingPointReport = JSON.parse(localStorage.getItem(constants.STARTING_POINT_REPORT));
            if (!startingPointReport) {
                startingPointReport = new StartingPointReportDO();
            }
        }
    }

    function clear() {
    }

    load();


    var startingPointReport = {
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

    return startingPointReport;
});
//# sourceURL=StartingPointReport.js