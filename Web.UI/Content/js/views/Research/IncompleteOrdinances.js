if (!IncompleteOrdinances) {
    IncompleteOrdinances = (function($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation = "3";
        var _reportId = "0";
        var _formName = "ordinancesForm";
        var _formTitleImage = "fmf24-temple";
        var _form = $("#ordinancesForm");
        var _initialized = false;
        var _previous;
        var _displayType = "start";
        var _generationAncestors = "3";
        var _generationDescendants = "1";
        var _addChildren;

        function OrdinancesDO(researchType, personId, personName, generation, reportId, addChildren) {
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
            this.reportId = reportId;
            this.addChildren = addChildren;
        }

        function save() {
            if (window.localStorage) {
                var ordinances = new OrdinancesDO(_researchType, _personId, _personName, _generation, _reportId, _addChildren);
                localStorage.setItem("IncompleteOrdinances", JSON.stringify(ordinances));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var ordinances = JSON.parse(localStorage.getItem("IncompleteOrdinances"));
                if (!ordinances) {
                    ordinances = new OrdinancesDO();
                }
                if (!ordinances.researchType) {
                    ordinances.researchType = "Ancestors";
                    ordinances.personId = login.personId;
                    ordinances.personName = login.personName;
                    ordinances.generation = "3";
                }

                _researchType = ordinances.researchType;
                _personId = ordinances.personId;
                _personName = ordinances.personName;
                _generation = ordinances.generation;
                _reportId = ordinances.reportId;
                if (!_reportId) {
                    _reportId = "0";
                }
                _addChildren = ordinances.addChildren;
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
                $("#ordinancesPersonId").val(_personId + " - " + _personName);
            }
            if (_researchType) {
                $("#ordinancesResearchType").val(_researchType);
            }
            if (_generation) {
                $("#ordinancesGeneration").val(_generation);
            }
            if (_reportId) {
                $("#ordinancesReportId").val(_reportId);
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
            updateForm();
        }

        function savePrevious() {
            if (_previous) {
                if (window.localStorage) {
                    localStorage.setItem("ordinancesPrevious", JSON.stringify(_previous));
                }
            }
        }


        function updateResearchData() {
            $("#ordinancesReportId").val(_reportId);
            var reportText = $("#ordinancesReportId option:selected").text();
            if (reportText && reportText.length > 8) {
                var nameIndex = reportText.indexOf("Name: ") + 6;
                var dateIndex = reportText.indexOf(", Date:  ");
                var researchTypeIndex = reportText.indexOf(", Research Type: ");
                var generationoIndex = reportText.indexOf(",  Generations: ");
                _personId = reportText.substring(nameIndex, nameIndex + 8);
                _personName = reportText.substring(nameIndex + 11, dateIndex);
                _researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
                _generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
//                _reportId = $("#ordinancesReportId option:selected").val();
            } else {
                _reportId = "0";
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
                                $("#ordinancesReportId").append(optionhtml);
                            });
                            if (!found) {
                                _reportId = "0";
                            }
                        }
                    }
                });
            } else {
                if (refreshReport) {
                    $("#ordinancesReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#ordinancesReportId").append(optionhtml);
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
                                    $("#ordinancesReportId").append(optionhtml);
                                });
                            }
                        }
                    });
                } else if (RetrieveData.reports) {
                    var data = RetrieveData.reports;
                    $("#ordinancesReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#ordinancesReportId").append(optionhtml);
                    var found = false;
                    $.each(data, function (i) {
                        if (data[i].ValueMember === _reportId) {
                            found = true;
                        } else if ((_reportId === "0") && (i === 0)) {
                            _reportId = data[i].ValueMember;
                            found = true;
                        }
                        var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                        $("#ordinancesReportId").append(optionhtml);
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
                            url: '/Home/GetOrdinanceHtml',
                            success: function (data) {
                                var $dialogContainer = $('#ordinancesReportForm');
                                var $detachedChildren = $dialogContainer.children().detach();
                                $('<div id="ordinancesReportForm"></div>').dialog({
                                    position: {
                                        my: "center top",
                                        at: ("center top+" + (window.innerHeight * .07)),
                                        collision: "none"
                                    },
                                    title: "Incomplete IncompleteOrdinances",
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
                                        progressInit('ordinancesSpinner', true);
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
                                $("#ordinancesReportForm").empty().append(data);
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
                    _previous = JSON.parse(localStorage.getItem("ordinancesPrevious"));
                }
            }
            if (_previous) {
                progressInit('ordinancesSpinner');
                $.ajax({
                    url: '/Home/GetOrdinanceHtml',
                    success: function (data) {
                        var $dialogContainer = $('#ordinancesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="ordinancesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Incomplete IncompleteOrdinances",
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
                        $("#ordinancesReportForm").empty().append(data);
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            return false;
        }
	
        function resetReportId() {
            if (_reportId !== "0") {
                $("#ordinancesReportId").val("0");
            }
            _reportId = "0";
        }

        function setHiddenFields() {
            if (_researchType === "Ancestors") {
            } else {
                _generation = "2";
            }
            $("ordinancesGeneration").val(_generation);
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
            $("#ordinancesGeneration").val(_generation);
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
            $("#ordinancesGeneration").val(_generation);
        }

        function addGenerationOptions(options) {
            var select = $("<select class=\"form-control select1Digit\" id=\"ordinancesGeneration\"\>");
            $.each(options, function (a, b) {
                select.append($("<option/>").attr("value", b).text(b));
            });
            $('#ordinancesGenerationDiv').empty();
            $("#ordinancesGenerationDiv").append(select);
            $('#ordinancesGenerationDiv').nextAll().remove();
            if ((_researchType === "Ancestors") && (_reportId === "0")) {
                $("#ordinancesGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: top; margin-top: -0.625em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -0.965em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
            }
            $("#ordinancesGeneration").change(function (e) {
                var generation = $("#ordinancesGeneration").val();
                if (_researchType === "Descendants") {
                    if (generation > 1) {
                        msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                    }
                } else {
                    if (generation > _generation) {
                        msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                    }
                }
                _generation = $("#ordinancesGeneration").val();
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
    IncompleteOrdinances.form = $("#ordinancesForm");
}


(function ($) {
    IncompleteOrdinances.initialize();

    $('#ordinancesPerson').change(function (e) {
        debugger;
    });

    $("#ordinancesReportId").change(function (e) {
        IncompleteOrdinances.reportId = $("#ordinancesReportId option:selected").val();
        if (IncompleteOrdinances.reportId === "0") {
            msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
        }
        IncompleteOrdinances.updateResearchData();
    });

    $('#ordinancesResearchType').change(function (e) {
        IncompleteOrdinances.researchType = $("#ordinancesResearchType").val();
        if (IncompleteOrdinances.researchType === "Descendants") {
            IncompleteOrdinances.generationAncestors = IncompleteOrdinances.generation;
            IncompleteOrdinances.generation = IncompleteOrdinances.generationDescendants;
            IncompleteOrdinances.addDecendantGenerationOptions();
        } else {
            IncompleteOrdinances.generationDescendants = IncompleteOrdinances.generation;
            IncompleteOrdinances.generation = IncompleteOrdinances.generationAncestors;
            IncompleteOrdinances.addAncestorGenerationOptions();
        }

        IncompleteOrdinances.setHiddenFields();
        IncompleteOrdinances.resetReportId();
        IncompleteOrdinances.updateResearchData();
    });

    $("#ordinancesFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (FindPerson.selected) {
                $("#ordinancesPersonId").val(FindPerson.personId + " - " + FindPerson.personName);
                var changed = (FindPerson.personId === IncompleteOrdinances.personId) ? false : true;
                IncompleteOrdinances.personId = FindPerson.personId;
                IncompleteOrdinances.personName = FindPerson.personName;
                if (changed) {
                    IncompleteOrdinances.resetReportId();
                    IncompleteOrdinances.updateResearchData();
                }
            }
        });
        return false;
    });

    $("#ordinancesRetrieveButton").unbind('click').bind('click', function (e) {
        if (IncompleteOrdinances.personId) {
            RetrieveData.personId = IncompleteOrdinances.personId;
            RetrieveData.personName = IncompleteOrdinances.personName;
            RetrieveData.researchType = IncompleteOrdinances.researchType;
            RetrieveData.generation = IncompleteOrdinances.generation;
            RetrieveData.caller = "IncompleteOrdinances";
            RetrieveData.retrievedRecords = 0;
            RetrieveData.popup = true;
            IncompleteOrdinances.updateResearchData();
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#addChildren").change(function (e) {
        IncompleteOrdinances.addChildren = $("#addChildren").prop("checked");
        if (IncompleteOrdinances.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
        }
        IncompleteOrdinances.resetReportId();
        IncompleteOrdinances.updateResearchData();
    });

    $("#ordinancesCancelButton").unbind('click').bind('click', function (e) {
        IncompleteOrdinances.form.dialog("close");
    });

    IncompleteOrdinances.form.unbind('dialogclose').bind('dialogclose', function (e) {
        IncompleteOrdinances.save();
    });
    openForm(IncompleteOrdinances.form, IncompleteOrdinances.formTitleImage, "ordinancesSpinner");
})(jQuery);

//# sourceURL=IncompleteOrdinances.js