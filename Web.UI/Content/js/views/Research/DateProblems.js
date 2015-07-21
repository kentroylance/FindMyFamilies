define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "dateProblemsForm";
    var _formTitleImage = "fa fmf-dates24";
    var _form = $("#dateProblemsForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "dateProblemsSpinner";

    var _empty = true;
    var _invalid = true;
    var _invalidFormat = true;
    var _incomplete = true;

    function DateProblemsDO(empty, invalid, invalidFormat, incomplete) {
        this.empty = empty;
        this.invalid = invalid;
        this.invalidFormat = invalidFormat;
        this.incomplete = incomplete;
    }

    function save() {
        if (window.localStorage) {
            var dateProblemsDO = new DateProblemsDO(_empty, _invalid, _invalidFormat, _incomplete);
            localStorage.setItem(constants.DATE_PROBLEMS, JSON.stringify(dateProblemsDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
                localStorage.setItem("dateProblemsPrevious", JSON.stringify(_previous));
            }
        }
    }
    
    if (window.localStorage) {
        var dateProblemsDO = JSON.parse(localStorage.getItem(constants.DATE_PROBLEMS));
        if (!dateProblemsDO) {
            dateProblemsDO = new DateProblemsDO();
        }
        if (!dateProblemsDO.researchType) {
            _empty = true;
        }
        _empty = dateProblemsDO.empty;
        _invalid = dateProblemsDO.invalid;
        _invalidFormat = dateProblemsDO.invalidFormat;
        _incomplete = dateProblemsDO.incomplete;
    }

    var dateProblems = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        get generationAncestors() {
            return _generationAncestors;
        },
        set generationAncestors(value) {
            _generationAncestors = value;
        },
        get generationDescendants() {
            return _generationDescendants;
        },
        set generationDescendants(value) {
            _generationDescendants = value;
        },
        get empty() {
            return _empty;
        },
        set empty(value) {
            _empty = value;
        },
        get invalid() {
            return _invalid;
        },
        set invalid(value) {
            _invalid = value;
        },
        get invalidFormat() {
            return _invalidFormat;
        },
        set invalidFormat(value) {
            _invalidFormat = value;
        },
        get incomplete() {
            return _incomplete;
        },
        set incomplete(value) {
            _incomplete = value;
        },
        get previous() {
            return _previous;
        },
        set previous(value) {
            _previous = value;
        },
        get displayType() {
            return _displayType;
        },
        set displayType(value) {
            _displayType = value;
        },
        save: function() {
            save();
        },
        savePrevious: function () {
            savePrevious();
        },
        clear: function () {
            clear();
        },
        reset: function() {
            reset();
        }
    };

    return dateProblems;
});

//# sourceURL=dateProblems.js