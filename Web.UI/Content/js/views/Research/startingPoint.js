﻿define(function(require) {

    var $ = require('jquery');
    var person = require('person');
    var constants = require('constants');

    var _formName = "startingPointForm";
    var _formTitleImage = "fmf24-compass";
    var _form = $("#startingPointForm");
    var _previous;
    var _displayType = "start";
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";
    var _spinner = "startingPointSpinner";

    var _nonMormon = false;
    var _born18101850 = false;
    var _livedInUSA = false;
    var _ordinances = false;
    var _hints = false;
    var _duplicates = false;

    function StartingPointDO(nonMormon, livedInUSA, born18101850, ordinances, hints, duplicates) {
        this.nonMormon = nonMormon;
        this.livedInUSA = livedInUSA;
        this.born18101850 = born18101850;
        this.ordinances = ordinances;
        this.hints = hints;
        this.duplicates = duplicates;
    }

    function save() {
        if (window.localStorage) {
            var startingPointDO = new StartingPointDO(_nonMormon, _livedInUSA, _born18101850, _ordinances, _hints, _duplicates);
            localStorage.setItem(constants.STARTING_POINT, JSON.stringify(startingPointDO));
        }
        person.save();
    }

    if (window.localStorage) {
        var startingPointDO = JSON.parse(localStorage.getItem(constants.STARTING_POINT));
        if (!startingPointDO) {
            startingPointDO = new StartingPointDO();
        }
        if (!startingPointDO.researchType) {
            startingPointDO.livedInUSA = true;
            startingPointDO.born18101850 = true;
        }
        _nonMormon = startingPointDO.nonMormon;
        _born18101850 = startingPointDO.born18101850;
        _livedInUSA = startingPointDO.livedInUSA;
        _ordinances = startingPointDO.ordinances;
        _hints = startingPointDO.hints;
        _duplicates = startingPointDO.duplicates;
    }

    var startingPoint = {
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
        get nonMormon() {
            return _nonMormon;
        },
        set nonMormon(value) {
            _nonMormon = value;
        },
        get born18101850() {
            return _born18101850;
        },
        set born18101850(value) {
            _born18101850 = value;
        },
        get livedInUSA() {
            return _livedInUSA;
        },
        set livedInUSA(value) {
            _livedInUSA = value;
        },
        get ordinances() {
            return _ordinances;
        },
        set ordinances(value) {
            _ordinances = value;
        },
        get hints() {
            return _hints;
        },
        set hints(value) {
            _hints = value;
        },
        get duplicates() {
            return _duplicates;
        },
        set duplicates(value) {
            _duplicates = value;
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

    return startingPoint;
});

//# sourceURL=startingPoint.js