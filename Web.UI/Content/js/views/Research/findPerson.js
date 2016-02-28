define(function (require) {

    var constants = require('constants');
    var person = require('person');

    var _formName = "findPersonForm";
    var _formTitleImage = "fa fmf-search24";
    var _form = $("#findPersonForm");
    var _displayType = "start";
    var _spinner = "findPersonSpinner";
    var _callerSpinner;
    var _callback;

    var _personId;
    var _fullName;
    var _firstName;
    var _middleName;
    var _lastName;
    var _gender;
    var _birthYear;
    var _deathYear;
    var _birthPlace;
    var _selected = false;


    function FindPersonDO(personId, fullName, firstName, middleName, lastName, gender, birthYear, deathYear, birthPlace) {
        this.personId = personId;
        this.fullName = fullName;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
        this.birthPlace = birthPlace;
    }

    function save() {
        if (window.localStorage) {
            var findPerson = new FindPersonDO(_personId, _fullName, _firstName, _middleName, _lastName, _gender, _birthYear, _deathYear, _birthPlace);
            localStorage.setItem(constants.FIND_PERSON, JSON.stringify(findPerson));
        }
    }

    function load() {
        if (window.localStorage) {
            var findPerson = JSON.parse(localStorage.getItem(constants.FIND_PERSON));
            if (findPerson) {
                _personId = findPerson.personId;
                _fullName = findPerson.fullName;
                _firstName = findPerson.firstName;
                _middleName = findPerson.middleName;
                _lastName = findPerson.lastName;
                _gender = findPerson.gender;
                _birthYear = findPerson.birthYear;
                _deathYear = findPerson.deathYear;
                _birthPlace = findPerson.birthPlace;
            }

        }
    }

    function reset() {
        _selected = false;
        _callback = null;
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
        get fullName() {
            return _fullName;
        },
        set fullName(value) {
            _fullName = value;
        },
        get firstName() {
            return _firstName;
        },
        set firstName(value) {
            _firstName = value;
        },
        get middleName() {
            return _middleName;
        },
        set middleName(value) {
            _middleName = value;
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
        set deathYear(value) {
            _deathYear = value;
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

    return findPerson;
});

//# sourceURL=findPerson.js