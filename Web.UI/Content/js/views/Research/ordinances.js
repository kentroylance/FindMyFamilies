define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "ordinancesForm";
    var _formTitleImage = "fa fmf-temple24";
    var _form = $("#ordinancesForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "ordinancesSpinner";
    var _callerSpinner;

    var _personId = "ordinancesPersonId";
    var _personContent = "ordinancesContent";
    var _personInfoDiv = "ordinancesPersonInfoDiv";
    
    function OrdinancesDO() {

    }

    function save() {
        if (window.localStorage) {
            var ordinancesDO = new OrdinancesDO();
            localStorage.setItem(constants.ORDINANCES, JSON.stringify(ordinancesDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
	        localStorage.setItem(constants.ORDINANCES_PREVIOUS, JSON.stringify(_previous));
            }
        }
    }

    if (window.localStorage) {
        var ordinancesDO = JSON.parse(localStorage.getItem(constants.ORDINANCES));
        if (!ordinancesDO) {
            ordinancesDO = new OrdinancesDO();
        }
	if (!ordinancesDO.researchType) {

        }

    }

    var ordinances = {
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
        get personId() {
            return _personId;
        },
        get personContent() {
            return _personContent;
        },
        get personInfoDiv() {
            return _personInfoDiv;
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

    return ordinances;
});

//# sourceURL=ordinances.js