if (!PersonUrlOptions) {
    PersonUrlOptions = (function ($) {
        var _personId;
        var _personName;
        var _generation;
        var _includeMaidenName;
        var _includeMiddleName;
        var _includePlace;
        var _yearRange;
        var _formName = "personUrlOptionsForm";
        var _formTitleImage = "fmf24-family";
        var _form = $("#personUrlOptionsForm");
        var _initialized = false;
        var _selected = false;
        var _callerSpinner;


        function loadData() {
            if (PersonInfo) {
                _personId = PersonInfo.personId;
                _personName = PersonInfo.personName;
                _generation = PersonInfo.generation;
                _includeMaidenName = PersonInfo.includeMaidenName;
                _includeMiddleName = PersonInfo.includeMiddleName;
                _includePlace = PersonInfo.includePlace;
                _yearRange = PersonInfo.yearRange;
            }
        }

        function save() {
            if (PersonInfo) {
                PersonInfo.personId = _personId;
                PersonInfo.personName = _personName;
                PersonInfo.generation = _generation;
                PersonInfo.includeMaidenName = _includeMaidenName;
                PersonInfo.includeMiddleName = _includeMiddleName;
                PersonInfo.includePlace = _includePlace;
                PersonInfo.yearRange = _yearRange;
                PersonInfo.save();
            }
        }

        function reset() {
            _generation = 2;
            updateForm();
        }

        function updateForm() {
            if (_generation) {
                $("#personUrlOptionsGeneration").val(_generation);
            }
            if (_includeMaidenName) {
                $('#personUrlOptionsMaidenName').prop('checked', _includeMaidenName);
            }
            if (_includeMiddleName) {
                $('#personUrlOptionsMiddleName').prop('checked', _includeMiddleName);
            }
            if (_includePlace) {
                $('#personUrlOptionsPlace').prop('checked', _includePlace);
            }
            if (_yearRange) {
                $("#personUrlOptionsYearRange").val(_yearRange);
            }
        }

        function submit() {
            if (isAuthenticated()) {
                save();
                displayPersonUrls();
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
            updateForm: function () {
                updateForm();
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
    PersonUrlOptions.form = $("#personUrlOptionsForm");
}


(function ($) {
    PersonUrlOptions.initialize();

    $('#personUrlOptionsPerson').change(function (e) {
        debugger;
    });

    $("#personUrlOptionsMaidenName").change(function (e) {
        PersonUrlOptions.includeMaidenName = $("#personUrlOptionsMaidenName").prop("checked");
    });

    $("#personUrlOptionsMiddleName").change(function (e) {
        PersonUrlOptions.includeMiddleName = $("#personUrlOptionsMiddleName").prop("checked");
    });

    $("#personUrlOptionsPlace").change(function (e) {
        PersonUrlOptions.includePlace = $("#personUrlOptionsPlace").prop("checked");
    });

    $("#personUrlOptionsYearRange").change(function (e) {
        PersonUrlOptions.yearRange = $("#personUrlOptionsYearRange").val();
    });

    $("#personUrlOptionsCancelButton").unbind('click').bind('click', function (e) {
        PersonUrlOptions.form.dialog("close");
    });

    PersonUrlOptions.form.unbind('dialogclose').bind('dialogclose', function (e) {
        PersonUrlOptions.save();
    });

    if (spinnerTarget) {
        PersonUrlOptions.callerSpinner = spinnerTarget.id;
    }

    openForm(PersonUrlOptions.form, PersonUrlOptions.formTitleImage, "personUrlOptionsSpinner");

})(jQuery);

//# sourceURL=PersonUrlOptions.js