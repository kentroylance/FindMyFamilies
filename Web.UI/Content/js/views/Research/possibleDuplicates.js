define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "possibleDuplicatesForm";
    var _formTitleImage = "fmf-duplicates24";
    var _form = $("#possibleDuplicatesForm");
    var _previous;
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
            var possibleDuplicates = new PossibleDuplicatesDO(_includePossibleDuplicates, _includePossibleMatches);
            localStorage.setItem("PossibleDuplicates", JSON.stringify(possibleDuplicates));
        }
        person.save();
    }

    if (window.localStorage) {
        var possibleDuplicatesDO = JSON.parse(localStorage.getItem(constants.POSSIBLE_DUPLICATES));
        if (!possibleDuplicatesDO) {
            possibleDuplicatesDO = new PossibleDuplicatesDO();
        }
        if (!possibleDuplicatesDO.researchType) {
            _includePossibleDuplicates = true;
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
        clear: function() {
            clear();
        },
        reset: function() {
            reset();
        }
    };

    return possibleDuplicates;
});

//# sourceURL=possibleDuplicates.js