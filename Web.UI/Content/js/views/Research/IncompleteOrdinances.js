define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "incompleteOrdinancesForm";
    var _formTitleImage = "fa fmf-temple24";
    var _form = $("#incompleteOrdinancesForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "incompleteOrdinancesSpinner";
    var _callerSpinner;

    function IncompleteOrdinancesDO() {

    }

    function save() {
        if (window.localStorage) {
            var incompleteOrdinancesDO = new IncompleteOrdinancesDO();
            localStorage.setItem(constants.INCOMPLETE_ORDINANCES, JSON.stringify(incompleteOrdinancesDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
	        localStorage.setItem("incompleteOrdinancesPrevious", JSON.stringify(_previous));
            }
        }
    }
    
    if (window.localStorage) {
        var incompleteOrdinancesDO = JSON.parse(localStorage.getItem(constants.INCOMPLETE_ORDINANCES));
        if (!incompleteOrdinancesDO) {
            incompleteOrdinancesDO = new IncompleteOrdinancesDO();
        }
	if (!incompleteOrdinancesDO.researchType) {
           
        }
     
    }

    var incompleteOrdinances = {
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
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
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

    return incompleteOrdinances;
});

//# sourceURL=incompleteOrdinances.js