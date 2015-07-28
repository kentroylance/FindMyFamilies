define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "placeProblemsForm";
    var _formTitleImage = "fa fmf-place24";
    var _form = $("#placeProblemsForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "placeProblemsSpinner";
    var _callerSpinner;

    function PlaceProblemsDO() {

    }

    function save() {
        if (window.localStorage) {
            var placeProblemsDO = new PlaceProblemsDO();
            localStorage.setItem(constants.PLACE_PROBLEMS, JSON.stringify(placeProblemsDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
	        localStorage.setItem("placeProblemsPrevious", JSON.stringify(_previous));
            }
        }
    }
    
    if (window.localStorage) {
        var placeProblemsDO = JSON.parse(localStorage.getItem(constants.PLACE_PROBLEMS));
        if (!placeProblemsDO) {
            placeProblemsDO = new PlaceProblemsDO();
        }
	if (!placeProblemsDO.researchType) {
           
        }
     
    }

    var placeProblems = {
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

    return placeProblems;
});

//# sourceURL=placeProblems.js