define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "findCluesForm";
    var _formTitleImage = "fa fmf-clue24";
    var _form = $("#findCluesForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "findCluesSpinner";
    var _callerSpinner;

    var _personId = "findCluesPersonId";
    var _personContent = "findCluesContent";
    var _personInfoDiv = "findCluesPersonInfoDiv";
    var _searchCriteria = "0";
    var _gapInChildren = "3";
    var _ageLimit = "18";
    var _searchCriteriaList = {};

    _searchCriteriaList['0'] = 'All';
    _searchCriteriaList['1'] = 'No death date, or \"deceased\", and lived between 1850 to 1940.';
    _searchCriteriaList['2'] = 'A female child with no spouse and no death date, and lived between 1850 and 1940.';
    _searchCriteriaList['3'] = 'Gap between children';
    _searchCriteriaList['4'] = 'Couples with no children, might have missing children';
    _searchCriteriaList['5'] = 'Couples with only one child and lived longer than 20 years';
    _searchCriteriaList['6'] = 'Person has no spouse and lived longer than 20 years';
    _searchCriteriaList['7'] = 'Person has no spouse and no children';
    _searchCriteriaList['8'] = 'Person has no spouse and only one child';
    _searchCriteriaList['9'] = 'Last name is different than parents last name';
    _searchCriteriaList['10'] = "Child's birth year is after mother's death year";
    _searchCriteriaList['11'] = 'Death year is before the marriage year';
    _searchCriteriaList['12'] = 'Last child was born 4 or more years before the mother turned 40';

    function FindCluesDO(searchCriteria, gapInChildren, ageLimit) {
        this.searchCriteria = searchCriteria;
        this.gapInChildren = gapInChildren;
        this.ageLimit = ageLimit;
    }


    function save() {
        if (window.localStorage) {
            var findCluesDO = new FindCluesDO(_searchCriteria, _gapInChildren, _ageLimit);
            localStorage.setItem(constants.FIND_CLUES, JSON.stringify(findCluesDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
                localStorage.setItem(constants.FIND_CLUES_PREVIOUS, JSON.stringify(_previous));
            }
        }
    }

    if (window.localStorage) {
        var findCluesDO = JSON.parse(localStorage.getItem(constants.FIND_CLUES));
        if (!findCluesDO) {
            findCluesDO = new FindCluesDO();
        }

        if (!findCluesDO.gapInChildren) {
            findCluesDO.searchCriteria = "0";
            findCluesDO.gapInChildren = "3";
            findCluesDO.ageLimit = "18";
            person.generation = "3";
        }

        _searchCriteria = findCluesDO.searchCriteria;
        if (!person.reportId) {
            _reportId = "0";
        }
        _gapInChildren = findCluesDO.gapInChildren;
        _ageLimit = findCluesDO.ageLimit;
    }

    var findClues = {
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
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        get searchCriteria() {
            return _searchCriteria;
        },
        set searchCriteria(value) {
            _searchCriteria = value;
        },
        get gapInChildren() {
            return _gapInChildren;
        },
        set gapInChildren(value) {
            _gapInChildren = value;
        },
        get ageLimit() {
            return _ageLimit;
        },
        set ageLimit(value) {
            _ageLimit = value;
        },
        get displayType() {
            return _displayType;
        },
        set displayType(value) {
            _displayType = value;
        },
        get searchCriteriaList() {
            return _searchCriteriaList;
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

    return findClues;
});

//# sourceURL=findClues.js