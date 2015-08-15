define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var _id;
    var _name;
    var _firstName;
    var _middleName
    var _lastName;
    var _fullName;
    var _gender;
    var _birthYear;
    var _deathYear;
    var _birthPlace;
    var _deathPlace;
    var _motherName;
    var _fatherName;
    var _spouseName;
    var _spouseGender;

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

    function PersonDO(id, name, firstName, middleName, lastName, fullName, gender, birthYear, deathYear, birthPlace, deathPlace, motherName, fatherName, spouseName, spouseGender, researchType, generation, includeMaidenName, includeMiddleName, includePlace, yearRange, history, findPersonOptions, reportId, addChildren) {
        this.id = id;
        this.name = name;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.fullName = fullName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
        this.birthPlace = birthPlace;
        this.deathPlace = deathPlace;
        this.motherName = motherName;
        this.fatherName = fatherName;
        this.spouseName = spouseName;
        this.spouseGender = spouseGender;
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

    function shiftPersons() {
        var temp = {}
        temp[_id] = _name;
        $.each(_history, function(id, name) {
            temp[id] = name;
        });
        _history = temp;
    }

    function save() {
        if (window.localStorage) {
            var person = new PersonDO(_id, _name, _firstName, _middleName, _lastName, _fullName, _gender, _birthYear, _deathYear, _birthPlace, _deathPlace, _motherName, _fatherName, _spouseName, _spouseGender, _researchType, _generation, _includeMaidenName, _includeMiddleName, _includePlace, _yearRange, _history, _findPersonOptions, _reportId, _addChildren);
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
            $.each(_history, function(i, value) {
                var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
                id.append(optionhtml);
            });
        } else {
            var iterator = 0;
            $.each(_history, function(i, value) {
                iterator++;
                if (iterator == constants.HISTORY_MAX + 1) {
                    delete _history[i];
                    return false;
                }
            });

            $.each(_history, function(i, value) {
                var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
                id.append(optionhtml);
            });
        }
        id.val(_id);
    }

    function getPersonInfoHoverHtml(id, personIdType) {
        var html = "";
        if (id === personIdType) {
            html = "<label><span style=\"color: " + getPersonColor(_gender) + "\">" + _name + "</span></label><br>";
            html += "<b>ID:</b>  " + _id + "<br>";
            html += '<b>Birth Date:</b>  ' + ((_birthYear) ? (_birthYear) : "") + '<br>';
            html += '<b>Birth Place:</b>  ' + ((_birthPlace) ? (_birthPlace) : "") + '<br>';
            html += '<b>Death Date:</b>  ' + ((_deathYear) ? (_deathYear) : "") + '<br>';
            html += '<b>Death Place:</b>  ' + ((_deathPlace) ? (_deathPlace) : "") + '<br>';
            html += "<b>Spouse:</b>";
            if (_spouseName) {
                html += "  <span style=\"color: " + getPersonColor(_spouseGender) + "\">" + _spouseName + "</span><br>";
            } else {
                html += "<br>";
            }
            html += "<b>Mother:</b>";
            if (_motherName) {
                html += "  <span style=\"color: " + getPersonColor("Female") + "\">" + _motherName + "</span><br>";
            } else {
                html += "<br>";
            }
            html += "<b>Father:</b>";
            if (_fatherName) {
                html += "  <span style=\"color: " + getPersonColor("Male") + "\">" + _fatherName + "</span><br>";
            } else {
                html += "<br>";
            }
            $('#startingPointContent').empty();
            $('#startingPointContent').append(html);
            $('#startingPointPersonInfoDiv').show();
            $("#startingPointPersonInfoDiv").position({
                my: "center+200 center",
                at: "center",
                of: $("#startingPointForm")
            });
        } else {
//            html = "<label><span style=\"color: " + getPersonColor(_gender) + "\">" + _name + "</span></label><br>";
//            html += "<b>ID:</b>  " + _id + "<br>";
//            html += '<b>Date:</b>  ' + ((_birthYear) ? (_birthYear) : "") + '<br>';
//            html += '<b>Research Type:</b>  ' + ((_birthPlace) ? (_birthPlace) : "") + '<br>';
//            html += '<b>Generations:</b>  ' + ((_deathYear) ? (_deathYear) : "") + '<br>';
//            html += '<b>Records:</b>  ' + ((_deathPlace) ? (_deathPlace) : "") + '<br>';
//            $('#startingPointContent').empty();
//            $('#startingPointContent').append(html);
//            $('#startingPointPersonInfoDiv').show();
//            $("#startingPointPersonInfoDiv").position({
//                my: "center+200 center",
//                at: "center",
//                of: $("#startingPointForm")
//            });
        }
        return html;
    }


    function getPersonInfoHover(id, personIdType) {
        if (_id && (!_firstName || (_firstName && _name.indexOf(_fullName) < 0))) {
            $.ajax({
                data: { "id": _id },
                url: constants.GET_PERSON_INFO,
                success: function(data) {
                    if (data) {
                        _firstName = data.firstName;
                        _middleName = data.middleName;
                        _lastName = data.lastName;
                        _fullName = data.fullName;
                        _gender = data.gender;
                        _birthYear = data.birthYear;
                        _deathYear = data.deathYear;
                        _birthPlace = data.birthPlace;
                        _deathPlace = data.deathPlace;
                        _motherName = data.motherName;
                        _fatherName = data.fatherName;
                        _spouseName = data.spouseName;
                        _spouseGender = data.spouseGender;
                        save();
                        getPersonInfoHoverHtml(id, personIdType);
                    }
                }
            });
        } else {
            getPersonInfoHoverHtml(id, personIdType);
        }
    }

    function getPersonInfo() {
        if (_id && (!_firstName || (_firstName && _name.indexOf(_fullName) < 0))) {
            $.ajax({
                data: { "id": _id },
                url: constants.GET_PERSON_INFO,
                success: function(data) {
                    if (data) {
                        _firstName = data.firstName;
                        _middleName = data.middleName;
                        _lastName = data.lastName;
                        _fullName = data.fullName;
                        _gender = data.gender;
                        _birthYear = data.birthYear;
                        _deathYear = data.deathYear;
                        _birthPlace = data.birthPlace;
                        _deathPlace = data.deathPlace;
                        _motherName = data.motherName;
                        _fatherName = data.fatherName;
                        _spouseName = data.spouseName;
                        _spouseGender = data.spouseGender;
                        save();
                    }
                },
                'beforeSend': function() {
                },
                'complete': function() {
                }
            });
        }
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
            _firstName = person.firstName;
            _middleName = person.middleName;
            _lastName = person.lastName;
            _fullName = person.fullName;
            _gender = person.gender;
            _birthYear = person.birthYear;
            _deathYear = person.deathYear;
            _birthPlace = person.birthPlace;
            _deathPlace = person.deathPlace;
            _motherName = person.motherName;
            _fatherName = person.fatherName;
            _spouseName = person.spouseName;
            _spouseGender = person.spouseGender;
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

            //     _reportId = startingPoint.reportId;
            if (!_reportId) {
                _reportId = constants.REPORT_ID;
            }
            //      _addChildren = startingPoint.addChildren;
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
        get fullName() {
            return _fullName;
        },
        set fullName(value) {
            _fullName = value;
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
        get deathPlace() {
            return _deathPlace;
        },
        set deathPlace(value) {
            _deathPlace = value;
        },
        get motherName() {
            return _motherName;
        },
        set motherName(value) {
            _motherName = value;
        },
        get fatherName() {
            return _fatherName;
        },
        set fatherName(value) {
            _fatherName = value;
        },
        get spouseName() {
            return _spouseName;
        },
        set spouseName(value) {
            _spouseName = value;
        },
        get spouseGender() {
            return _spouseGender;
        },
        set spouseGender(value) {
            _spouseGender = value;
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
        shiftPersons: function() {
            shiftPersons();
        },
        save: function() {
            save();
        },
        getPersonInfo: function() {
            getPersonInfo();
        },
        loadPersons: function(id) {
            loadPersons(id);
        },
        updateFromFindPerson: function(id, name) {
            updateFromFindPerson(id, name);
        },
        isInList: function(id, name) {
            isInList(id, name);
        },
        getPersonImage: function(gender) {
            return getPersonImage(gender);
        },
        getPersonColor: function(gender) {
            return getPersonColor(gender);
        },
        getPersonInfoHover: function(id, personIdType) {
            return getPersonInfoHover(id, personIdType);
        },
        clear: function() {
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
        reset: function() {
            reset();
        },
        resetReportId: function(reportId) {
            resetReportId(reportId);
        }
    };

    return person;
});