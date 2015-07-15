if (!ResearchFamily) {
    ResearchFamily = (function ($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation;
        var _includeMaidenName;
        var _includeMiddleName;
        var _includePlace;
        var _yearRange;
        var _formName = "researchFamilyForm";
        var _formTitleImage = "fmf24-family";
        var _form = $("#researchFamilyForm");
        var _initialized = false;
        var _selected = false;
        var _callerSpinner;

        function ResearchFamilyDO(researchType, personId, personName, generation, includeMaidenName, includeMiddleName, includePlace, yearRange) {
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
            this.includeMaidenName = includeMaidenName;
            this.includeMiddleName = includeMiddleName;
            this.includePlace = includePlace;
            this.yearRange = yearRange;
        }

        function save() {
            if (window.localStorage) {
                var researchFamily = new ResearchFamilyDO(_researchType, _personId, _personName, _generation, _includeMaidenName, _includeMiddleName, _includePlace, _yearRange);
                localStorage.setItem("ResearchFamily", JSON.stringify(researchFamily));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var researchFamily = JSON.parse(localStorage.getItem("ResearchFamily"));
                if (!researchFamily) {
                    researchFamily = new ResearchFamilyDO();
                }
                if (!researchFamily.researchType) {
                    researchFamily.personId = login.personId;
                    researchFamily.personName = login.personName;
                }
                _researchType = researchFamily.researchType;
                _personId = researchFamily.personId;
                _personName = researchFamily.personName;
                _generation = researchFamily.generation;
                _includeMaidenName = researchFamily.includeMaidenName;
                _includeMiddleName = researchFamily.includeMiddleName;
                _includePlace = researchFamily.includePlace;
                _yearRange = researchFamily.yearRange;
            }
        }

        function reset() {
            _researchType = "Ancestors";
            _personId = login.personId;
            _personName = login.personName;
            _generation = 2;
            updateForm();
        }

        function updateForm() {
            if (_personId) {
                $("#researchFamilyPersonId").val(_personId + " - " + _personName);
            }
            if (_researchType) {
                $("#researchFamilyResearchType").val(_researchType);
            }
            if (_generation) {
                $("#researchFamilyGeneration").val(_generation);
            }
            if (_includeMaidenName) {
                $('#researchFamilyMaidenName').prop('checked', _includeMaidenName);
            }
            if (_includeMiddleName) {
                $('#researchFamilyMiddleName').prop('checked', _includeMiddleName);
            }
            if (_includePlace) {
                $('#researchFamilyPlace').prop('checked', _includePlace);
            }
            if (_yearRange) {
                $("#researchFamilyYearRange").val(_yearRange);
            }
        }

        function submit() {
            if (isAuthenticated()) {
                if (_personId) {
                    save();
                    displayPerson(_personId);
                } else {
                    msgBox.message("You must first select a person from Family Search");
                }
            } else {
                _form.dialog("close");
                relogin();
            }
            return false;
        }

        function initialize() {
            if (!_initialized) {
                loadData();
                _initialized = true;
            }
            updateForm();
        }

        return {
            formName: _formName,
            formTitleImage: _formTitleImage,
            get form() {
                return _form;
            },
            set form(value) {
                _form = value;
            },
            get researchType() {
                return _researchType;
            },
            set researchType(value) {
                _researchType = value;
            },
            get personId() {
                return _personId;
            },
            set personId(value) {
                _personId = value;
            },
            get personName() {
                return _personName;
            },
            set personName(value) {
                _personName = value;
            },
            get generation() {
                return _generation;
            },
            set generation(value) {
                _generation = value;
            },
            get selected() {
                return _selected;
            },
            set selected(value) {
                _selected = value;
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
            initialize: function () {
                initialize();
            },
            setHiddenFields: function () {
                setHiddenFields();
            },
            resetReportId: function () {
                resetReportId();
            },
            updateForm: function () {
                updateForm();
            },
            loadReports: function (force) {
                loadReports(force);
            },
            save: function () {
                save();
            },
            loadData: function () {
                loadData();
            },
            submit: function () {
                submit();
            },
            reset: function () {
                reset();
            },
            updateResearchData: function () {
                updateResearchData();
            },
            get callerSpinner() {
                return _callerSpinner;
            },
            set callerSpinner(value) {
                _callerSpinner = value;
            }
        }
    }(jQuery));
} else {
    ResearchFamily.form = $("#researchFamilyForm");
}


(function ($) {
    ResearchFamily.initialize();

    $('#researchFamilyPerson').change(function (e) {
        debugger;
    });

    $("#researchFamilyFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (FindPerson.selected) {
                $("#researchFamilyPersonId").val(FindPerson.personId + " - " + FindPerson.personName);
                ResearchFamily.personId = FindPerson.personId;
                ResearchFamily.personName = FindPerson.personName;
            }
        });
        return false;
    });

    $("#researchFamilyMaidenName").change(function (e) {
        ResearchFamily.includeMaidenName = $("#researchFamilyMaidenName").prop("checked");
    });

    $("#researchFamilyMiddleName").change(function (e) {
        ResearchFamily.includeMiddleName = $("#researchFamilyMiddleName").prop("checked");
    });

    $("#researchFamilyPlace").change(function (e) {
        ResearchFamily.includePlace = $("#researchFamilyPlace").prop("checked");
    });

    $("#researchFamilyYearRange").change(function (e) {
        ResearchFamily.yearRange = $("#researchFamilyYearRange").val();
    });

    $("#researchFamilyCancelButton").unbind('click').bind('click', function (e) {
        ResearchFamily.form.dialog("close");
    });

    ResearchFamily.form.unbind('dialogclose').bind('dialogclose', function (e) {
        ResearchFamily.save();
    });

    if (spinnerTarget) {
        ResearchFamily.callerSpinner = spinnerTarget.id;
    }

    openForm(ResearchFamily.form, ResearchFamily.formTitleImage, "researchFamilySpinner");

})(jQuery);

//# sourceURL=ResearchFamily.js