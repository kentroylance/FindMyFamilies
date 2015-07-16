define(function (require) {

    var constants = require('constants');
//    var person = require('person');

    var _formName = "findPersonOptionsForm";
    var _formTitleImage = "fa fmf-options24";
    var _form = $("#findPersonOptionsForm");
    var _spinner = "findPersonOptionsSpinner";
    var _callerSpinner;

    var _personId;
    var _firstName;
    var _lastName;
    var _gender;
    var _birthYear;
    var _deathYear;

    function FindPersonOptionsDO(personId, firstName, lastName, gender, birthYear, deathYear) {
        this.personId = personId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
    }

    function save() {
        if (window.localStorage) {
            var findPersonOptions = new FindPersonOptionsDO(_personId, _firstName, _lastName, _gender, _birthYear, _deathYear);
            localStorage.setItem(constants.FIND_PERSON_OPTIONS, JSON.stringify(findPersonOptions));
        }
    }

    function load() {
        if (window.localStorage) {
            var findPersonOptions = JSON.parse(localStorage.getItem(constants.FIND_PERSON_OPTIONS));
            if (findPersonOptions) {
                _personId = findPersonOptions.personId;
                _firstName = findPersonOptions.firstName;
                _lastName = findPersonOptions.lastName;
                _gender = findPersonOptions.gender;
                _birthYear = findPersonOptions.birthYear;
                _deathYear = findPersonOptions.deathYear;
            }

        }
    }

    function clear() {
//        person.clear();
    }

    load();

    var findPersonOptions = {
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
        set _deathYear(value) {
            _deathYear = value;
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
        save: function () {
            save();
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        }
    };

    return findPersonOptions;
});

//# sourceURL=findPersonOptions.js