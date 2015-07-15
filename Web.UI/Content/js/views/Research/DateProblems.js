if (!DateProblems) {
    DateProblems = (function ($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation = "3";
        var _empty = true;
        var _invalid = true;
        var _invalidFormat = true;
        var _incomplete = true;
        var _reportId = "0";
        var _formName = "datesForm";
        var _formTitleImage = "fmf24-dates";
        var _form = $("#datesForm");
        var _initialized = false;
        var _previous;
        var _displayType = "start";
        var _generationAncestors = "3";
        var _generationDescendants = "1";
        var _addChildren;

        function DatesDO(researchType, personId, personName, generation, reportId, addChildren, empty, invalid, invalidFormat, incomplete) {
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
            this.reportId = reportId;
            this.addChildren = addChildren;
            this.empty = empty;
            this.invalid = invalid;
            this.invalidFormat = invalidFormat;
            this.incomplete = incomplete;
        }

        function save() {
            if (window.localStorage) {
                var dates = new DatesDO(_researchType, _personId, _personName, _generation, _reportId, _addChildren, _empty, _invalid, _invalidFormat, _incomplete);
                localStorage.setItem("DateProblems", JSON.stringify(dates));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var dates = JSON.parse(localStorage.getItem("DateProblems"));
                if (!dates) {
                    dates = new DatesDO();
                }
                if (!startingPoint.researchType) {
                    dates.researchType = "Ancestors";
                    dates.personId = login.personId;
                    dates.personName = login.personName;
                    dates.generation = "3";
                    dates.empty = true;
                    dates.invalid = true;
                    dates.invalidFormat = true;
                    dates.incomplete = true;
                }
                _researchType = dates.researchType;
                _personId = dates.personId;
                _personName = dates.personName;
                _generation = dates.generation;
                _empty = dates.empty;
                _invalid = dates.invalid;
                _invalidFormat = dates.invalidFormat;
                _incomplete = dates.incomplete;
                _reportId = dates.reportId;
                if (!_reportId) {
                    _reportId = "0";
                }
                _addChildren = startingPoint.addChildren;
                if (_generation) {
                    if (_researchType === "Ancestors") {
                        _generationAncestors = _generation;
                    } else {
                        _generationDescendants = _generation;
                    }
                } else {
                    if (_researchType === "Ancestors") {
                        _generation = "3";
                        _generationAncestors = _generation;
                    } else {
                        _generation = "2";
                        _generationDescendants = _generation;
                    }
                }
            }
        }

        function updateForm() {
            if (_personId) {
                $("#datesPersonId").val(_personId + " - " + _personName);
            }
            if (_researchType) {
                $("#datesResearchType").val(_researchType);
            }
            if (_generation) {
                $("#datesGeneration").val(_generation);
            }
            if (_reportId) {
                $("#datesReportId").val(_reportId);
            }
            if (_addChildren) {
                $('#addChildren').prop('checked', _addChildren);
            }
            if (_empty) {
                $('#datesEmpty').prop('checked', _empty);
            }
            if (_invalid) {
                $('#datesInvalid').prop('checked', _invalid);
            }
            if (_invalidFormat) {
                $('#datesInvalidFormat').prop('checked', _invalidFormat);
            }
            if (_incomplete) {
                $('#datesIncomplete').prop('checked', _incomplete);
            }
        }

        function reset() {
            _researchType = "Ancestors";
            _personId = login.personId;
            _personName = login.personName;
            _generation = "2";
            _reportId = "0";
            _addChildren = false;
            _empty = true;
            _invalid = true;
            _invalidFormat = true;
            _incomplete = true;
            updateForm();
        }

        function savePrevious() {
            if (_previous) {
                if (window.localStorage) {
                    localStorage.setItem("datesPrevious", JSON.stringify(_previous));
                }
            }
        }

        function updateResearchData() {
            $("#datesReportId").val(_reportId);
            var reportText = $("#datesReportId option:selected").text();
            if (reportText && reportText.length > 8) {
                var nameIndex = reportText.indexOf("Name: ") + 6;
                var dateIndex = reportText.indexOf(", Date:  ");
                var researchTypeIndex = reportText.indexOf(", Research Type: ");
                var generationoIndex = reportText.indexOf(",  Generations: ");
                _personId = reportText.substring(nameIndex, nameIndex + 8);
                _personName = reportText.substring(nameIndex + 11, dateIndex);
                _researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
                _generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
//                _reportId = $("#datesReportId option:selected").val();
            }
            if (_researchType === "Descendants") {
                addDecendantGenerationOptions();
            } else {
                addAncestorGenerationOptions();
            }
            updateForm();
        }

        function loadReports(refreshReport) {
            if (!RetrieveData.reports) {
                $.ajax({
                    'async': false,
                    url: "/Home/GetReportList",
                    success: function (data) {
                        if (data) {
                            RetrieveData.reports = data;
                            var found = false;
                            $.each(data, function (i) {
                                if (data[i].ValueMember === _reportId) {
                                    found = true;
                                } else if ((_reportId === "0") && (i === 0)) {
                                    _reportId = data[i].ValueMember;
                                    found = true;
                                }
                                var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                                $("#datesReportId").append(optionhtml);
                            });
                            if (!found) {
                                _reportId = "0";
                            }
                        }
                    }
                });
            } else {
                if (refreshReport) {
                    $("#datesReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#datesReportId").append(optionhtml);
                    $.ajax({
                        'async': false,
                        url: "/Home/GetReportList",
                        success: function(data) {
                            if (data) {
                                RetrieveData.reports = data;
                                $.each(data, function (i) {
                                    if (_reportId && (parseInt(_reportId) === 0)) {
                                        optionhtml = '<option value="' + data[i].ValueMember + '" selected>' + data[i].DisplayMember + '</option>';
                                        _reportId = data[i].ValueMember;
                                    } else {
                                        optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                                    }
                                    $("#datesReportId").append(optionhtml);
                                });
                            }
                        }
                    });
                } else if (RetrieveData.reports) {
                    var data = RetrieveData.reports;
                    $("#datesReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#datesReportId").append(optionhtml);
                    var found = false;
                    $.each(data, function (i) {
                        if (data[i].ValueMember === _reportId) {
                            found = true;
                        } else if ((_reportId === "0") && (i === 0)) {
                            _reportId = data[i].ValueMember;
                            found = true;
                        }
                        var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                        $("#datesReportId").append(optionhtml);
                    });
                    if (!found) {
                        _reportId = "0";
                    }
                }
            }
            updateResearchData();
        }

        function submit() {
            if (isAuthenticated()) {
                if (_personId) {
                    save();
                } else {
                    msgBox.message("You must first select a person from Family Search");
                }

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine. ", "Question", function (result) {
                    if (result) {
                        $.ajax({
                            url: '/Home/GetDatesHtml',
                            success: function (data) {
                                var $dialogContainer = $('#datesReportForm');
                                var $detachedChildren = $dialogContainer.children().detach();
                                $('<div id="datesReportForm"></div>').dialog({
                                    position: {
                                        my: "center top",
                                        at: ("center top+" + (window.innerHeight * .07)),
                                        collision: "none"
                                    },
                                    title: "Date Problems",
                                    width: 975,
                                    open: function () {
                                        $detachedChildren.appendTo($dialogContainer);
                                        $(this).css("maxHeight", 700);
                                    },
                                    close: function (event, ui) {
                                        event.preventDefault();
                                        if (_reportId === "0") {
                                            loadReports(true);
                                        }
                                        progressInit('datesSpinner', true);
                                        $(this).dialog('destroy').remove();
                                    },
                                    buttons: {
                                        "0": {
                                            id: 'save',
                                            text: 'Save',
                                            icons: { primary: "saveIcon" },
                                            click: function(event) {
                                                event.preventDefault();
                                                savePrevious();
                                                $(this).dialog("close");
                                            },

                                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                                        },
                                        "1": {
                                            id: 'close',
                                            text: 'Close',
                                            icons: { primary: "closeIcon" },
                                            click: function(event) {
                                                event.preventDefault();
                                                _displayType = "start";
                                                $(this).dialog("close");
                                            },
                                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                                        }
                                    }
                                });
                                _displayType = "start";
                                $("#datesReportForm").empty().append(data);
                            }
                        });
                    }
                });
            } else {
                _form.dialog("close");
                relogin();
            }
            return false;
        }

        function displayPrevious() {
            if (!_previous) {
                if (window.localStorage) {
                    _previous = JSON.parse(localStorage.getItem("datesPrevious"));
                }
            }
            if (_previous) {
                progressInit('datesSpinner');
                $.ajax({
                    url: '/Home/GetDatesHtml',
                    success: function (data) {
                        var $dialogContainer = $('#datesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="datesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Date Problems",
                            width: 975,
                            open: function () {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            },
                            close: function (event, ui) {
                                event.preventDefault();
                                $(this).dialog('destroy').remove();
                            },
                            buttons: {
                                "0": {
                                    id: 'close',
                                    text: 'Close',
                                    icons: { primary: "closeIcon" },
                                    click: function (event) {
                                        event.preventDefault();
                                        $(this).dialog("close");
                                    },
                                    "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                                }
                            }
                        });
                        _displayType = "previous";
                        $("#datesReportForm").empty().append(data);
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            return false;
        }

        function resetReportId() {
            if (_reportId !== "0") {
                $("#datesReportId").val("0");
            }
            _reportId = "0";
        }

        function setHiddenFields() {
            if (_researchType === "Ancestors") {
            } else {
                _generation = "2";
            }
            $("datesGeneration").val(_generation);
        }

        function initialize() {
            if (!_initialized) {
                loadData();
            }
            loadReports();
            updateForm();
            if (!_initialized) {
                _initialized = true;
            }
        }

        function addDecendantGenerationOptions() {
            var options = [];
            options[0] = "1";
            options[1] = "2";
            options[2] = "3";
            addGenerationOptions(options);
            $("#datesGeneration").val(_generation);
        }

        function addAncestorGenerationOptions() {
            var options = [];
            options[0] = "2";
            options[1] = "3";
            options[2] = "4";
            options[3] = "5";
            options[4] = "6";
            options[5] = "7";
            options[6] = "8";
            addGenerationOptions(options);
            $("#datesGeneration").val(_generation);
        }

        function addGenerationOptions(options) {
            var select = $("<select class=\"form-control select1Digit\" id=\"datesGeneration\"\>");
            $.each(options, function (a, b) {
                select.append($("<option/>").attr("value", b).text(b));
            });
            $('#datesGenerationDiv').empty();
            $("#datesGenerationDiv").append(select);
            $('#datesGenerationDiv').nextAll().remove();
            if ((_researchType === "Ancestors") && (_reportId === "0")) {
                $("#datesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: top; margin-top: -0.625em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -0.965em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
            }
            $("#datesGeneration").change(function (e) {
                var generation = $("#datesGeneration").val();
                if (_researchType === "Descendants") {
                    if (generation > 1) {
                        msgBox.warning("Selecting two or more generations of Descendants will more than double the time to retrieve descendants, but the results will be much better.");
                    }
                } else {
                    if (generation > _generation) {
                        msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                    }
                }
                _generation = $("#datesGeneration").val();
                resetReportId();
            });

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
            get addChildren() {
                return _addChildren;
            },
            set addChildren(value) {
                _addChildren = value;
            },
            get empty() {
                return _empty;
            },
            set empty(value) {
                _empty = value;
            },
            get invalid() {
                return _invalid;
            },
            set invalid(value) {
                _invalid = value;
            },
            get invalidFormat() {
                return _invalidFormat;
            },
            set invalidFormat(value) {
                _invalidFormat = value;
            },
            get incomplete() {
                return _incomplete;
            },
            set incomplete(value) {
                _incomplete = value;
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
            loadReports: function (refreshReport) {
                loadReports(refreshReport);
            },
            save: function () {
                save();
            },
            savePrevious: function () {
                savePrevious();
            },
            displayPrevious: function () {
                displayPrevious();
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
            addDecendantGenerationOptions: function () {
                addDecendantGenerationOptions();
            },
            addAncestorGenerationOptions: function () {
                addAncestorGenerationOptions();
            }

        }
    }(jQuery));
} else {
    DateProblems.form = $("#datesForm");
}


(function ($) {
    DateProblems.initialize();

    $('#ordinancesPerson').change(function (e) {
        debugger;
    });

    $("#datesReportId").change(function (e) {
        DateProblems.reportId = $("#datesReportId option:selected").val();
        if (DateProblems.reportId === "0") {
            msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
        }
        DateProblems.updateResearchData();
    });

    $('#datesResearchType').change(function (e) {
        DateProblems.researchType = $("#datesResearchType").val();
        if (DateProblems.researchType === "Descendants") {
            DateProblems.generationAncestors = DateProblems.generation;
            DateProblems.generation = DateProblems.generationDescendants;
            DateProblems.addDecendantGenerationOptions();
        } else {
            DateProblems.generationDescendants = DateProblems.generation;
            DateProblems.generation = DateProblems.generationAncestors;
            DateProblems.addAncestorGenerationOptions();
        }

        DateProblems.setHiddenFields();
        DateProblems.resetReportId();
        DateProblems.updateResearchData();
    });

    $("#datesFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (FindPerson.selected) {
                $("#datesPersonId").val(FindPerson.personId + " - " + FindPerson.personName);
                var changed = (FindPerson.personId === DateProblems.personId) ? false : true;
                DateProblems.personId = FindPerson.personId;
                DateProblems.personName = FindPerson.personName;
                if (changed) {
                    DateProblems.resetReportId();
                    DateProblems.updateResearchData();
                }
            }
        });
        return false;
    });

    $("#datesRetrieveButton").unbind('click').bind('click', function (e) {
        if (DateProblems.personId) {
            RetrieveData.personId = DateProblems.personId;
            RetrieveData.personName = DateProblems.personName;
            RetrieveData.researchType = DateProblems.researchType;
            RetrieveData.generation = DateProblems.generation;
            RetrieveData.caller = "DateProblems";
            RetrieveData.retrievedRecords = 0;
            RetrieveData.popup = true;
            DateProblems.updateResearchData();
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#addChildren").change(function (e) {
        DateProblems.addChildren = $("#addChildren").prop("checked");
        if (DateProblems.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
        }
        DateProblems.resetReportId();
        DateProblems.updateResearchData();
    });

    $("#datesEmpty").change(function (e) {
        DateProblems.empty = $("#datesEmpty").is(':checked');
    });

    $("#datesInvalid").change(function (e) {
        DateProblems.invalid = $("#datesInvalid").is(':checked');
    });

    $("#datesInvalidFormat").change(function (e) {
        DateProblems.invalidFormat = $("#datesInvalidFormat").is(':checked');
    });

    $("#datesIncomplete").change(function (e) {
        DateProblems.incomplete = $("#datesIncomplete").is(':checked');
    });

    $("#datesCancelButton").unbind('click').bind('click', function (e) {
        DateProblems.form.dialog("close");
    });

    DateProblems.form.unbind('dialogclose').bind('dialogclose', function (e) {
        DateProblems.save();
    });
    openForm(DateProblems.form, DateProblems.formTitleImage, "datesSpinner");


})(jQuery);

//# sourceURL=DateProblems.js