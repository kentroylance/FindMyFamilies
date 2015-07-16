define(function (require) {

    var constants = require('constants');
//    var person = require('person');

    var _formName = "findPersonForm";
    var _formTitleImage = "fa fmf-search24";
    var _form = $("#findPersonForm");
    var _spinner = "findPersonSpinner";
    var _callerSpinner;
    var _findPersonOptionsController;

    var _personId;
    var _firstName;
    var _lastName;
    var _gender;
    var _birthYear;
    var _deathYear;

    function FindPersonDO(personId, firstName, lastName, gender, birthYear, deathYear) {
        this.personId = personId;
        this.firstName = firstName;
        this.lastName = lastName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
    }

    function save() {
        if (window.localStorage) {
            var findPerson = new FindPersonDO(_personId, _firstName, _lastName, _gender, _birthYear, _deathYear);
            localStorage.setItem(constants.FIND_PERSON, JSON.stringify(findPerson));
        }
    }

    function load() {
        if (window.localStorage) {
            var findPerson = JSON.parse(localStorage.getItem(constants.FIND_PERSON));
            if (findPerson) {
                _personId = findPerson.personId;
                _firstName = findPerson.firstName;
                _lastName = findPerson.lastName;
                _gender = findPerson.gender;
                _birthYear = findPerson.birthYear;
                _deathYear = findPerson.deathYear;
            }

        }
    }

    function clear() {
//        person.clear();
    }

    load();

    var findPerson = {
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
        },
        get findPersonOptionsController() {
            return _findPersonOptionsController;
        },
        set findPersonOptionsController(value) {
            _findPersonOptionsController = value;
        }
    };

    return findPerson;
});

//# sourceURL=findPerson.js