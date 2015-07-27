define(function (require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var _id;
    var _name;
    var _researchType;
    var _generation;
    var _includeMaidenName = false;
    var _includeMiddleName = false;
    var _includePlace = false;
    var _yearRange = constants.YEAR_RANGE;
    var _history = {};
    var _findPersonOptions = {};
    var _callerSpinner;
    var _reportId = constants.REPORT_ID;
    var _addChildren;
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";

    function PersonDO(id, name, researchType, generation, includeMaidenName, includeMiddleName, includePlace, yearRange, history, findPersonOptions, reportId, addChildren) {
        this.id = id;
        this.name = name;
        this.researchType = researchType;
        this.generation = generation;
        this.includeMaidenName = includeMaidenName;
        this.includeMiddleName = includeMiddleName;
        this.includePlace = includePlace;
        this.yearRange = yearRange;
        this.history = history;
        this.findPersonOptions = findPersonOptions;
        this.reportId = reportId;
        this.addChildren = addChildren;
    }

    function shiftPersons() {
        var temp = {}
        temp[_id] = _name;
        $.each(_history, function (id, name) {
            temp[id] = name;
        });
        _history = temp;
    }

    function save() {
        if (window.localStorage) {
            var person = new PersonDO(_id, _name, _researchType, _generation, _includeMaidenName, _includeMiddleName, _includePlace, _yearRange, _history, _findPersonOptions, _reportId, _addChildren);
            localStorage.setItem(constants.PERSON, JSON.stringify(person));
        }
    }

    function isInList(id, name) {
        if (_history[id] === undefined) {
            if ($.isEmptyObject(_history)) {
                _history = {};
            }
            _history[id] = name;
            shiftPersons();
        }
    }

    function loadPersons(id) {
        var optionalhtml = '';
        isInList(_id, _name);
        id.empty();
        if (Object.keys(_history).length <= constants.HISTORY_MAX) {
            $.each(_history, function (i, value) {
                var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
                id.append(optionhtml);
            });
        } else {
            var iterator = 0;
            $.each(_history, function (i, value) {
                iterator++;
                if (iterator == constants.HISTORY_MAX + 1) {
                    delete _history[i];
                    return false;
                }
            });

            $.each(_history, function (i, value) {
                var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
                id.append(optionhtml);
            });
        }
        id.val(_id);
    }

    function resetReportId(reportId) {
        if (_reportId !== constants.REPORT_ID) {
            reportId.val(constants.REPORT_ID);
        }
        _reportId = constants.REPORT_ID;
    }

    function load() {
        if (window.localStorage) {
            var person = JSON.parse(localStorage.getItem(constants.PERSON));
            if (!person) {
                person = new PersonDO();
                person.researchType = constants.RESEARCH_TYPE;
                person.generation = constants.GENERATION;
            }

            if (!person.id) {
                person.id = system.userId;
                person.name = system.userName;
            }
            if (!person.name) {
                person.id = system.userId;
                person.name = system.userName;
            }
            if (person.id && $.isEmptyObject(person.history)) {
                person.history = {};
                person.history[person.id] = person.name;
            }

            _id = person.id;
            _name = person.name;
            _researchType = person.researchType;
            _includeMaidenName = person.includeMaidenName;
            _includeMiddleName = person.includeMiddleName;
            _includePlace = person.includePlace;
            _yearRange = person.yearRange;
            if (!_yearRange) {
                _yearRange = constants.YEAR_RANGE;
            }
            _history = person.history;
            _findPersonOptions = person.findPersonOptions;

            if (!_findPersonOptions) {
                _findPersonOptions = [];
                _findPersonOptions.push('fmf-urls');
                _findPersonOptions.push('ancestry');
                _findPersonOptions.push('findagrave');
                _findPersonOptions.push('puz-descend');
                _findPersonOptions.push('myheritage');
                _findPersonOptions.push('findmypast');
                _findPersonOptions.push('amerancest');
            }

            _reportId = startingPoint.reportId;
            if (!_reportId) {
                _reportId = constants.REPORT_ID;
            }
            _addChildren = startingPoint.addChildren;
            if (_generation) {
                if (_researchType === constants.ANCESTORS) {
                    _generationAncestors = _generation;
                } else {
                    _generationDescendants = _generation;
                }
            } else {
                if (_researchType === constants.ANCESTORS) {
                    _generation = constants.GENERATION;
                    _generationAncestors = _generation;
                } else {
                    _generation = "2";
                    _generationDescendants = _generation;
                }
            }
        }
    }

    function updateFromFindPerson(id, name) {
        _id = id;
        _name = name;
        shiftPersons();
        save();
    }

    function clear() {
        _id = "";
        _name = "";
        _generation = 2;
        _includeMaidenName = false;
        _includeMiddleName = false;
        _includePlace = false;
        _yearRange = constants.YEAR_RANGE;
        _history = {};
        _findPersonOptions = {};
        _callerSpinner = "";
    }

    function reset() {
        _researchType = constants.ANCESTORS;
        _id = system.userId;
        _name = system.userName;
        _generation = constants.GENERATION;
        _reportId = constants.REPORT_ID;
        _addChildren = false;
    }

    function getPersonImage(gender) {
        if (gender === constants.MALE) {
            return "<i class=\"fa fa-user color-dark-blue\"></i>&nbsp;";
        } else {
            return "<i class=\"fa fa-user color-red\"></i>&nbsp;";
        }
    }

    function getPersonColor(gender) {
        if (gender === constants.MALE) {
            return "rgb(0,0,255)";
        } else {
            return "rgb(255,0,0)";
        }
    }


    load();

    var person = {
        get id() {
            return _id;
        },
        set id(value) {
            _id = value;
        },
        get name() {
            return _name;
        },
        set name(value) {
            _name = value;
        },
        get researchType() {
            return _researchType;
        },
        set researchType(value) {
            _researchType = value;
        },
        get generation() {
            return _generation;
        },
        set generation(value) {
            _generation = value;
        },
        get includeMaidenName() {
            return _includeMaidenName;
        },
        set includeMaidenName(value) {
            _includeMaidenName = value;
        },
        get includeMiddleName() {
            return _includeMiddleName;
        },
        set includeMiddleName(value) {
            _includeMiddleName = value;
        },
        get includePlace() {
            return _includePlace;
        },
        set includePlace(value) {
            _includePlace = value;
        },
        get yearRange() {
            return _yearRange;
        },
        set yearRange(value) {
            _yearRange = value;
        },
        get findPersonOptions() {
            return _findPersonOptions;
        },
        set findPersonOptions(value) {
            _findPersonOptions = value;
        },
        get history() {
            return _history;
        },
        set personHisory(value) {
            _history = value;
        },
        shiftPersons: function () {
            shiftPersons();
        },
        save: function () {
            save();
        },
        loadPersons: function (id) {
            loadPersons(id);
        },
        updateFromFindPerson: function (id, name) {
            updateFromFindPerson(id, name);
        },
        isInList: function (id, name) {
            isInList(id, name);
        },
        getPersonImage: function (gender) {
            return getPersonImage(gender);
        },
        getPersonColor: function (gender) {
            return getPersonColor(gender);
        },
        clear: function () {
            clear();
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        get addChildren() {
            return _addChildren;
        },
        set addChildren(value) {
            _addChildren = value;
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
        get reportId() {
            return _reportId;
        },
        set reportId(value) {
            _reportId = value;
        },
        reset: function () {
            reset();
        },
        resetReportId: function (reportId) {
            resetReportId(reportId);
        }
    };

    return person;


});

