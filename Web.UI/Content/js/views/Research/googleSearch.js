define(function (require) {

    var constants = require('constants');
    var person = require('person');

    var _formName = "googleSearchForm";
    var _formTitleImage = "fa fmf-google24";
    var _form = $("#googleSearchForm");
    var _displayType = "start";
    var _spinner = "googleSearchSpinner";
    var _callerSpinner;
    var _callback;

    var _personId;
    var _firstName;
    var _middleName;
    var _lastName;
    var _gender;
    var _birthYear;
    var _deathYear;
    var _birthPlace;
    var _yearRange;
    var _selected = false;


    function googleSearchDO(personId, firstName, middleName, lastName, gender, birthYear, deathYear, birthPlace, yearRange) {
        this.personId = personId;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
        this.birthPlace = birthPlace;
        this.yearRange = yearRange;
    }

    function save() {
        if (window.localStorage) {
            var googleSearch = new googleSearchDO(_personId, _firstName, _middleName, _lastName, _gender, _birthYear, _deathYear, _birthPlace, _yearRange);
            localStorage.setItem(constants.GOOGLE_SEARCH, JSON.stringify(googleSearch));
        }
    }

    function load() {
        if (window.localStorage) {
            var googleSearch = JSON.parse(localStorage.getItem(constants.GOOGLE_SEARCH));
            if (googleSearch) {
                _personId = googleSearch.personId;
                _firstName = googleSearch.firstName;
                _middleName = googleSearch.middleName;
                _lastName = googleSearch.lastName;
                _gender = googleSearch.gender;
                _birthYear = googleSearch.birthYear;
                _deathYear = googleSearch.deathYear;
                _birthPlace = googleSearch.birthPlace;
                _yearRange = googleSearch.yearRange;
            }

        }
    }

    function reset() {
        _selected = false;
        _callback = null;
    }

    function copyFromPerson() {
        _personId = person.personId;
        _firstName = person.firstName;
        _middleName = person.middleName;
        _lastName = person.lastName;
        _gender = person.gender;
        _birthYear = person.birthYear;
        _deathYear = person.deathYear;
        _birthPlace = person.birthPlace;
        _yearRange = person.yearRange;
        if (person.birthPlace && person.birthPlace.indexOf(",") > 0) {
            var birthPlace = person.birthPlace.split("United").join("");
            birthPlace = birthPlace.split("States").join("");
            birthPlace = birthPlace.trim();
            birthPlace = birthPlace.split(", ").join("~");
            if (birthPlace.indexOf(",") > -1) {
                birthPlace = birthPlace.split(",").join("~");
            }
            birthSplit = birthPlace.split("~");
            _birthPlace = '"';
            $.each(birthSplit, function (i, value) {
                if (value) {
                    _birthPlace += value + " OR ";
                }
            });
            if (_birthPlace && _birthPlace.lastIndexOf(" OR ") === (_birthPlace.length - 4)) {
                _birthPlace = _birthPlace.substr(1, _birthPlace.length - 5);
            }
        }

    }

    load();

    var googleSearch = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get personId() {
            return _personId;
        },
        set personId(value) {
            _personId = value;
        },
        get firstName() {
            return _firstName;
        },
        set firstName(value) {
            _firstName = value;
        },
        get lastName() {
            return _lastName;
        },
        set lastName(value) {
            _lastName = value;
        },
        get middleName() {
            return _middleName;
        },
        set middleName(value) {
            _middleName = value;
        },
        get gender() {
            return _gender;
        },
        set gender(value) {
            _gender = value;
        },
        get birthYear() {
            return _birthYear;
        },
        set birthYear(value) {
            _birthYear = value;
        },
        get deathYear() {
            return _deathYear;
        },
        set deathYear(value) {
            _deathYear = value;
        },
        get yearRange() {
            return _yearRange;
        },
        set yearRange(value) {
            _yearRange = value;
        },
        get birthPlace() {
            return _birthPlace;
        },
        set birthPlace(value) {
            _birthPlace = value;
        },
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        clear: function () {
            clear();
        },
        reset: function () {
            reset();
        },
        copyFromPerson: function () {
            copyFromPerson();
        },
        save: function () {
            save();
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        get callback() {
            return _callback;
        },
        set callback(value) {
            _callback = value;
        },
        get selected() {
            return _selected;
        },
        set selected(value) {
            _selected = value;
        }

    };

    return googleSearch;
});

//# sourceURL=googleSearch.js