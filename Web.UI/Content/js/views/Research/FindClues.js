define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "findCluesForm";
    var _formTitleImage = "fa fmf-compass24";
    var _form = $("#findCluesForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "findCluesSpinner";

    var _searchCriteria = "0";
    var _gapInChildren = "3";
    var _ageLimit = "18";
    var _searchCriteriaList;

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
                localStorage.setItem("findCluesPrevious", JSON.stringify(_previous));
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
        set searchCriteriaList(value) {
            _searchCriteriaList = value;
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