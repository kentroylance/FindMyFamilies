if (!PossibleDuplicates) {
    PossibleDuplicates = (function($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation = "3";
	    var _includePossibleDuplicates = true;
	    var _includePossibleMatches;
        var _reportId = 0;
        var _formName = "possibleDuplicatesForm";
        var _formTitleImage = "fmf24-duplicates";
        var _form = $("#possibleDuplicatesForm");
        var _initialized = false;
        var _previous;
        var _displayType = "start";
        var _generationAncestors = "3";
        var _generationDescendants = "1";
        var _addChildren;

        function PossibleDuplicatesDO(researchType, personId, personName, generation, reportId, includePossibleDuplicates, includePossibleMatches, addChildren) {
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
	        this.includePossibleDuplicates = includePossibleDuplicates;
	        this.includePossibleMatches = includePossibleMatches;
            this.reportId = reportId;
            this.addChildren = addChildren;
        }

        function save() {
            if (window.localStorage) {
                var possibleDuplicates = new PossibleDuplicatesDO(_researchType, _personId, _personName, _generation, _reportId, _includePossibleDuplicates, _includePossibleMatches, _addChildren);
                localStorage.setItem("PossibleDuplicates", JSON.stringify(possibleDuplicates));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var possibleDuplicates = JSON.parse(localStorage.getItem("PossibleDuplicates"));
                if (!possibleDuplicates) {
                    possibleDuplicates = new PossibleDuplicatesDO();
                }
                if (!possibleDuplicates.researchType) {
                    possibleDuplicates.researchType = "Ancestors";
                    possibleDuplicates.personId = login.personId;
                    possibleDuplicates.personName = login.personName;
                    possibleDuplicates.generation = "3";
                    possibleDuplicates.includePossibleDuplicates = true;
                }
                _researchType = possibleDuplicates.researchType;
                _personId = possibleDuplicates.personId;
                _personName = possibleDuplicates.personName;
                _generation = possibleDuplicates.generation;
	    	    _includePossibleDuplicates = possibleDuplicates.includePossibleDuplicates;
	    	    _includePossibleMatches = possibleDuplicates.includePossibleMatches;
                _reportId = possibleDuplicates.reportId;
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
                $("#possibleDuplicatesPersonId").val(_personId + " - " + _personName);
            }
            if (_researchType) {
                $("#possibleDuplicatesResearchType").val(_researchType);
            }
            if (_generation) {
                $("#possibleDuplicatesGeneration").val(_generation);
            }
            if (_includePossibleDuplicates) {
                $("#includePossibleDuplicates").prop('checked', _includePossibleDuplicates);
            }
            if (_includePossibleMatches) {
                $("#includePossibleMatches").prop('checked', _includePossibleMatches);
            }
            if (_reportId) {
                $("#possibleDuplicatesReportId").val(_reportId);
            }
            if (_addChildren) {
                $('#addChildren').prop('checked', _addChildren);
            }
        }

        function reset() {
            _researchType = "Ancestors";
            _personId = login.personId;
            _personName = login.personName;
            _generation = "2";
            _reportId = "0";
            _addChildren = false;
            _includePossibleDuplicates = true;
            updateForm();
        }

        function savePrevious() {
            if (_previous) {
                if (window.localStorage) {
                    localStorage.setItem("possibleDuplicatesPrevious", JSON.stringify(_previous));
                }
            }
        }


        function updateResearchData() {
            $("#possibleDuplicatesReportId").val(_reportId);
            var reportText = $("#possibleDuplicatesReportId option:selected").text();
            if (reportText && reportText.length > 8) {
                var nameIndex = reportText.indexOf("Name: ") + 6;
                var dateIndex = reportText.indexOf(", Date:  ");
                var researchTypeIndex = reportText.indexOf(", Research Type: ");
                var generationoIndex = reportText.indexOf(",  Generations: ");
                _personId = reportText.substring(nameIndex, nameIndex + 8);
                _personName = reportText.substring(nameIndex + 11, dateIndex);
                _researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
                _generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
//                _reportId = $("#possibleDuplicatesReportId option:selected").val();
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
                                $("#possibleDuplicatesReportId").append(optionhtml);
                            });
                            if (!found) {
                                _reportId = "0";
                            }
                        }
                    }
                });
            } else {
                if (refreshReport) {
                    $("#hintsReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#possibleDuplicatesReportId").append(optionhtml);
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
                                    $("#possibleDuplicatesReportId").append(optionhtml);
                                });
                            }
                        }
                    });
                } else if (RetrieveData.reports) {
                    var data = RetrieveData.reports;
                    $("#possibleDuplicatesReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#possibleDuplicatesReportId").append(optionhtml);
                    var found = false;
                    $.each(data, function (i) {
                        if (data[i].ValueMember === _reportId) {
                            found = true;
                        } else if ((_reportId === "0") && (i === 0)) {
                            _reportId = data[i].ValueMember;
                            found = true;
                        }
                        var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                        $("#possibleDuplicatesReportId").append(optionhtml);
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

                msgBox.question("Depending on the number of generations you selected, this could take a minute or two. Select Yes if you want to contine. ", "Question", function (result) {
                    if (result) {
                        $.ajax({
                            url: '/Home/GetPossibleDuplicateHtml',
                            success: function(data) {
                                var $dialogContainer = $('#possibleDuplicatesReportForm');
                                var $detachedChildren = $dialogContainer.children().detach();
                                $('<div id="possibleDuplicatesReportForm"></div>').dialog({
                                    position: {
                                        my: "center top",
                                        at: ("center top+" + (window.innerHeight * .07)),
                                        collision: "none"
                                    },
                                    title: "Possible Duplicates",
                                    width: 975,
                                    open: function() {
                                        $detachedChildren.appendTo($dialogContainer);
                                        $(this).css("maxHeight", 700);
                                    },
                                    close: function(event, ui) {
                                        event.preventDefault();
                                        if (_reportId === "0") {
                                            loadReports(true);
                                        }
                                        progressInit('possibleDuplicatesSpinner', true);
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
                                $("#possibleDuplicatesReportForm").empty().append(data);
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
                    _previous = JSON.parse(localStorage.getItem("possibleDuplicatesPrevious"));
                }
            }
            if (_previous) {
                progressInit('possibleDuplicatesSpinner');
                $.ajax({
                    url: '/Home/GetPossibleDuplicateHtml',
                    success: function (data) {
                        var $dialogContainer = $('#possibleDuplicatesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="possibleDuplicatesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Possible Duplicates",
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
                        $("#possibleDuplicatesReportForm").empty().append(data);
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            return false;
        }

        function resetReportId() {
            if (_reportId !== "0") {
                $("#possibleDuplicatesReportId").val("0");
            }
            _reportId = "0";
        }

        function setHiddenFields() {
            if (_researchType === "Ancestors") {
            } else {
                _generation = "2";
            }
            $("possibleDuplicatesGeneration").val(_generation);
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
            $("#possibleDuplicatesGeneration").val(_generation);
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
            $("#possibleDuplicatesGeneration").val(_generation);
        }

        function addGenerationOptions(options) {
            var select = $("<select class=\"form-control select1Digit\" id=\"possibleDuplicatesGeneration\"\>");
            $.each(options, function (a, b) {
                select.append($("<option/>").attr("value", b).text(b));
            });
            $('#possibleDuplicatesGenerationDiv').empty();
            $("#possibleDuplicatesGenerationDiv").append(select);
            $('#possibleDuplicatesGenerationDiv').nextAll().remove();
            if ((_researchType === "Ancestors") && (_reportId === "0")) {
                $("#possibleDuplicatesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: top; margin-top: -0.625em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -0.965em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
            }
            $("#possibleDuplicatesGeneration").change(function (e) {
                var generation = $("#possibleDuplicatesGeneration").val();
                if (_researchType === "Descendants") {
                    if (generation > 1) {
                        msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                    }
                } else {
                    if (generation > _generation) {
                        msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                    }
                }
                _generation = $("#possibleDuplicatesGeneration").val();
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
            get includePossibleDuplicates() {
                return _includePossibleDuplicates;
            },
            set includePossibleDuplicates(value) {
                _includePossibleDuplicates = value;
            },
            get includePossibleMatches() {
                return _includePossibleMatches;
            },
            set includePossibleMatches(value) {
                _includePossibleMatches = value;
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
    PossibleDuplicates.form = $("#possibleDuplicatesForm");
}


(function ($) {
    PossibleDuplicates.initialize();

    $('#possibleDuplicatesPerson').change(function (e) {
        debugger;
    });

    $("#possibleDuplicatesReportId").change(function (e) {
        PossibleDuplicates.reportId = $("#possibleDuplicatesReportId option:selected").val();
        if (PossibleDuplicates.reportId === "0") {
            msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
        }
        PossibleDuplicates.updateResearchData();
    });

    $('#possibleDuplicatesResearchType').change(function (e) {
        PossibleDuplicates.researchType = $("#possibleDuplicatesResearchType").val();
        if (PossibleDuplicates.researchType === "Descendants") {
            PossibleDuplicates.generationAncestors = PossibleDuplicates.generation;
            PossibleDuplicates.generation = PossibleDuplicates.generationDescendants;
            PossibleDuplicates.addDecendantGenerationOptions();
        } else {
            PossibleDuplicates.generationDescendants = PossibleDuplicates.generation;
            PossibleDuplicates.generation = PossibleDuplicates.generationAncestors;
            PossibleDuplicates.addAncestorGenerationOptions();
        }

        PossibleDuplicates.setHiddenFields();
        PossibleDuplicates.resetReportId();
        PossibleDuplicates.updateResearchData();
    });

    $("#possibleDuplicatesFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (FindPerson.selected) {
                $("#possibleDuplicatesPersonId").val(FindPerson.personId + " - " + FindPerson.personName);
                var changed = (FindPerson.personId === PossibleDuplicates.personId) ? false : true;
                PossibleDuplicates.personId = FindPerson.personId;
                PossibleDuplicates.personName = FindPerson.personName;
                if (changed) {
                    PossibleDuplicates.resetReportId();
                    PossibleDuplicates.updateResearchData();
                }
            }
        });
        return false;
    });

    $("#possibleDuplicatesRetrieveButton").unbind('click').bind('click', function (e) {
        if (PossibleDuplicates.personId) {
            RetrieveData.personId = PossibleDuplicates.personId;
            RetrieveData.personName = PossibleDuplicates.personName;
            RetrieveData.researchType = PossibleDuplicates.researchType;
            RetrieveData.generation = PossibleDuplicates.generation;
            RetrieveData.caller = "PossibleDuplicates";
            RetrieveData.retrievedRecords = 0;
            RetrieveData.popup = true;
            PossibleDuplicates.updateResearchData();
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#addChildren").change(function (e) {
        PossibleDuplicates.addChildren = $("#addChildren").prop("checked");
        if (PossibleDuplicates.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
        }
        PossibleDuplicates.resetReportId();
        PossibleDuplicates.updateResearchData();
    });

    $("#includePossibleDuplicates").change(function (e) {
        PossibleDuplicates.includePossibleDuplicates = $("#includePossibleDuplicates").prop("checked");
    });

    $("#includePossibleMatches").change(function (e) {
        PossibleDuplicates.includePossibleMatches = $("#includePossibleMatches").prop("checked");
    });

    $("#possibleDuplicatesCancelButton").unbind('click').bind('click', function (e) {
        PossibleDuplicates.form.dialog("close");
    });

    PossibleDuplicates.form.unbind('dialogclose').bind('dialogclose', function (e) {
        PossibleDuplicates.save();
    });
    openForm(PossibleDuplicates.form, PossibleDuplicates.formTitleImage, "possibleDuplicatesSpinner");

})(jQuery);

//# sourceURL=PossibleDuplicates.js