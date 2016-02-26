define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var _msgBox;
    var _retrieve;

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
    var _reportFile;
    var _addChildren;
    var _generationAncestors = constants.GENERATION;
    var _generationDescendants = "1";

    function PersonDO(id, name, firstName, middleName, lastName, fullName, gender, birthYear, deathYear, birthPlace, deathPlace, motherName, fatherName, spouseName, spouseGender, researchType, generation, includeMaidenName, includeMiddleName, includePlace, yearRange, history, findPersonOptions, reportId, addChildren, reportFile) {
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
        this.reportFile = reportFile;
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
            var person = new PersonDO(_id, _name, _firstName, _middleName, _lastName, _fullName, _gender, _birthYear, _deathYear, _birthPlace, _deathPlace, _motherName, _fatherName, _spouseName, _spouseGender, _researchType, _generation, _includeMaidenName, _includeMiddleName, _includePlace, _yearRange, _history, _findPersonOptions, _reportId, _addChildren, _reportFile);
            localStorage.setItem(constants.PERSON, JSON.stringify(person));
        }
    }

    function isInList(id, name) {
        if (!_history || _history[id] === undefined) {
            if ($.isEmptyObject(_history)) {
                _history = {};
            }
            _history[id] = name;
            shiftPersons();
        }
    }

    function setHiddenFields(generationInput) {
        if (_researchType === constants.ANCESTORS) {
        } else {
            _generation = "2";
        }
        $('#' + generationInput).val(_generation);
    }

    function mouseOver(control, model) {
//        _id = $('option:selected', $(control)).val();
//        _name = $('option:selected', $(control)).text();
//        var id = $(control).attr("id");
        getPersonInfoHover(model);
    }

    function mouseOut(model) {
        $('#' + model.personInfoDiv).hide();
    }

    function addGenerationOptions(options, generationInput, reportInput, personInput, generationInputDiv) {
        var generationDiv = $('#' + generationInputDiv);
        var select = $("<select class=\"form-control select1Digit\" id=\"" + generationInput + "\"\>");
        $.each(options, function (a, b) {
            select.append($("<option/>").attr("value", b).text(b));
        });
        generationDiv.empty();
        generationDiv.append(select);
        generationDiv.nextAll().remove();
        if (_researchType === "Ancestors") {
            generationDiv.after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: middle; margin-top: -0.625em; margin-left: .7em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -1.4em;\">&nbsp;<span style=\"font-weight: normal;\">Add Children</span></label>");
        }
        var generation = $('#' + generationInput);
        generation.change(function (e) {
            var generationVal = generation.val();
            if (_researchType === constants.DESCENDANTS) {
                if (generationVal > 1) {
                    _msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                }
            } else {
                if (generationVal > _generation) {
                    _msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                }
            }
            _generation = generationVal;
            _retrieve.checkReports(reportInput, personInput);
        });
    }

    function addDecendantGenerationOptions(generationInput, reportInput, personInput, generationInputDiv) {
        var options = [];
        options[0] = "1";
        options[1] = "2";
//        options[2] = "3";
        addGenerationOptions(options, generationInput, reportInput, personInput, generationInputDiv);
        $("#" + generationInput).val(_generation);
    }

    function addAncestorGenerationOptions(generationInput, reportInput, personInput, generationInputDiv) {
        var options = [];
        options[0] = "2";
        options[1] = "3";
        options[2] = "4";
        options[3] = "5";
        options[4] = "6";
//        options[5] = "7";
//        options[6] = "8";
        addGenerationOptions(options, generationInput, reportInput, personInput, generationInputDiv);
        $("#" + generationInput).val(_generation);
    }

    function loadPersons(id) {
        var optionalhtml = '';
        isInList(_id, _name);
        id.empty();
        if (Object.keys(_history).length <= constants.HISTORY_MAX) {
            $.each(_history, function(i, value) {
                optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
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
                optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
                id.append(optionhtml);
            });
        }
        id.val(_id);
    }

    function getPersonInfoHoverHtml(model) {
        var html = "";
        var personId = $('#' + model.personId).val();

        if (_id === personId) {
            var personContent = $('#' + model.personContent);
            var personInfoDiv = $('#' + model.personInfoDiv);
//            html = "<label><span style=\"color: " + getPersonColor(_gender) + "\">" + _name + "</span></label><br>";
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
            personContent.empty();
            personContent.append(html);
            personInfoDiv.show();
            personInfoDiv.position({
                my: "center+200 center",
                at: "center",
                of: model.form
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
//        return false;
    }


    function getPersonInfoHover(model) {
        if (_id && (!_firstName || (_firstName && _name.indexOf(_fullName) < 0))) {
            $.ajax({
                data: { "id": _id },
                url: constants.GET_PERSON_INFO,
                success: function(data) {
                    if (data && data.firstName) {
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
                        getPersonInfoHoverHtml(model);
                    }
                }
            });
        } else {
            getPersonInfoHoverHtml(model);
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
        _reportFile = "";
    }

    function load() {
        if (window.localStorage) {
            var person = JSON.parse(localStorage.getItem(constants.PERSON));
            if (!person) {
                person = new PersonDO();
                person.researchType = constants.RESEARCH_TYPE;
                person.generation = constants.GENERATION;
            }

            if (!person.id || !person.name || (person && person.id === "0")) {
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
            _reportId = person.reportId;
            _reportFile = person.reportFile;
            if (!_yearRange) {
                _yearRange = constants.YEAR_RANGE;
            }
            _history = person.history;
            _findPersonOptions = person.findPersonOptions;

            if (!_findPersonOptions) {
                _findPersonOptions = [];
                _findPersonOptions.push('fmf-urls');
                _findPersonOptions.push('ancestry');
                _findPersonOptions.push('fmf-google');
                _findPersonOptions.push('findagrave');
                _findPersonOptions.push('puz-descend');
                _findPersonOptions.push('myheritage');
                _findPersonOptions.push('findmypast');
            }

            //     _reportId = startingPoint.reportId;
            if (!_reportId) {
                _reportId = constants.REPORT_ID;
                _reportFile = "";
            }
            _addChildren = person.addChildren;
            _generation = person.generation;
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
            if (value && value !== "null") {
                _id = value;
            } else {
                _id = "";
            }
        },
        get name() {
            return _name;
        },
        set name(value) {
            _name = value;
        },
        get fullName() {
            return _fullName;
        },
        set fullName(value) {
            if (value && value !== "null") {
                _fullName = value;
            } else {
                _fullName = "";
            }
        },
        get firstName() {
            return _firstName;
        },
        set firstName(value) {
            if (value && value !== "null") {
                _firstName = value;
            } else {
                _firstName = "";
            }
        },
        get lastName() {
            return _lastName;
        },
        set lastName(value) {
            if (value && value !== "null") {
                _lastName = value;
            } else {
                _lastName = "";
            }
        },
        get middleName() {
            return _middleName;
        },
        set middleName(value) {
            if (value && value !== "null") {
                _middleName = value;
            } else {
                _middleName = "";
            }
        },
        get gender() {
            return _gender;
        },
        set gender(value) {
            if (value && value !== "null") {
                _gender = value;
            } else {
                _gender = "";
            }
        },
        get birthYear() {
            return _birthYear;
        },
        set birthYear(value) {
            if (value && value !== "null") {
                _birthYear = value;
            } else {
                _birthYear = "";
            }
        },
        get deathYear() {
            return _deathYear;
        },
        set deathYear(value) {
            if (value && value !== "null") {
                _deathYear = value;
            } else {
                _deathYear = "";
            }
        },
        get birthPlace() {
            return _birthPlace;
        },
        set birthPlace(value) {
            if (value && value !== "null") {
                _birthPlace = value;
            } else {
                _birthPlace = "";
            }
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
        addDecendantGenerationOptions: function (generationInput, reportInput, personInput, generationInputDiv) {
            addDecendantGenerationOptions(generationInput, reportInput, personInput, generationInputDiv);
        },
        addAncestorGenerationOptions: function (generationInput, reportInput, personInput, generationInputDiv) {
            addAncestorGenerationOptions(generationInput, reportInput, personInput, generationInputDiv);
        },
        mouseOver: function (control, personInput) {
            mouseOver(control, personInput);
        },
        mouseOut: function (personInfoDiv) {
            mouseOut(personInfoDiv);
        },
        setHiddenFields: function (generationInput) {
            setHiddenFields(generationInput);
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
        get msgBox() {
            return _msgBox;
        },
        set msgBox(value) {
            _msgBox = value;
        },
        get retrieve() {
            return _retrieve;
        },
        set retrieve(value) {
            _retrieve = value;
        },
        get reportFile() {
            return _reportFile;
        },
        set reportFile(value) {
            _reportFile = value;
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