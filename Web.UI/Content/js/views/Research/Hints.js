define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "hintsForm";
    var _formTitleImage = "fa fmf-hint24";
    var _form = $("#hintsForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "hintsSpinner";

    var _topScore = true;
    var _count = false;

    function HintsDO(topScore, count) {
        this.topScore = topScore;
        this.count = count;
    }

    function save() {
        if (window.localStorage) {
            var hintsDO = new HintsDO(_topScore, _count);
            localStorage.setItem(constants.HINTS, JSON.stringify(hintsDO));
        }
        person.save();
    }

    function savePrevious() {
        if (_previous) {
	    if (window.localStorage) {
                localStorage.setItem("hintsPrevious", JSON.stringify(_previous));
            }
        }
    }
    
    if (window.localStorage) {
        var hintsDO = JSON.parse(localStorage.getItem(constants.HINTS));
        if (!hintsDO) {
            hintsDO = new HintsDO();
        }
        if (!hintsDO.researchType) {
            hintsDO.topScore = true;
        }
        _topScore = hintsDO.topScore;
        _count = hintsDO.count;
    }

    var hints = {
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
        get topScore() {
            return _topScore;
        },
        set topScore(value) {
            _topScore = value;
        },
        get count() {
            return _count;
        },
        set count(value) {
            _count = value;
        },
        save: function() {
            save();
        },
        savePrevious: function () {
            savePrevious();
        },
        clear: function() {
            clear();
        },
        reset: function() {
            reset();
        }
    };

    return hints;
});

//# sourceURL=hints.js