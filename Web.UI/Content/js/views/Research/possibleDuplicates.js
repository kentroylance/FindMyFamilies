define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "possibleDuplicatesForm";
    var _formTitleImage = "fa fmf-duplicates24";
    var _form = $("#possibleDuplicatesForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "possibleDuplicatesSpinner";

    var _includePossibleDuplicates = true;
    var _includePossibleMatches;

    function PossibleDuplicatesDO(includePossibleDuplicates, includePossibleMatches) {
        this.includePossibleDuplicates = includePossibleDuplicates;
        this.includePossibleMatches = includePossibleMatches;
    }

    function save() {
        if (window.localStorage) {
            var possibleDuplicatesDO = new PossibleDuplicatesDO(_includePossibleDuplicates, _includePossibleMatches);
            localStorage.setItem(constants.POSSIBLE_DUPLICATES, JSON.stringify(possibleDuplicatesDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
                localStorage.setItem("possibleDuplicatesPrevious", JSON.stringify(_previous));
            }
        }
    }
    if (window.localStorage) {
        var possibleDuplicatesDO = JSON.parse(localStorage.getItem(constants.POSSIBLE_DUPLICATES));
        if (!possibleDuplicatesDO) {
            possibleDuplicatesDO = new PossibleDuplicatesDO();
        }
        if (!possibleDuplicatesDO.researchType) {
            possibleDuplicatesDO.includePossibleDuplicates = true;
        }
        _includePossibleDuplicates = possibleDuplicatesDO.includePossibleDuplicates;
        _includePossibleMatches = possibleDuplicatesDO.includePossibleMatches;
    }

    var possibleDuplicates = {
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
        get includePossibleDuplicates() {
            return _includePossibleDuplicates;
        },
        set includePossibleDuplicates(value) {
            _includePossibleDuplicates = value;
        },
        get includePossibleMatches() {
            return _includePossibleMatches;
        },
        set includePossibleMatches(value) {
            _includePossibleMatches = value;
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

    return possibleDuplicates;
});

//# sourceURL=possibleDuplicates.js