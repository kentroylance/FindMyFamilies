if (!Hints) {
    Hints = (function ($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation = "3";
        var _topScore = true;
        var _count = false;
        var _reportId = "0";
        var _formName = "hintsForm";
        var _formTitleImage = "fmf24-hint";
        var _form = $("#hintsForm");
        var _initialized = false;
        var _previous;
        var _displayType = "start";
        var _generationAncestors = "3";
        var _generationDescendants = "1";
        var _addChildren;

        function HintsDO(researchType, personId, personName, generation, topScore, count, reportId, addChildren) {
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
            this.topScore = topScore;
            this.count = count;
            this.reportId = reportId;
            this.addChildren = addChildren;
        }

        function save() {
            if (window.localStorage) {
                var hints = new HintsDO(_researchType, _personId, _personName, _generation, _topScore, _count, _reportId, _addChildren);
                localStorage.setItem("Hints", JSON.stringify(hints));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var hints = JSON.parse(localStorage.getItem("Hints"));
                if (!hints) {
                    hints = new HintsDO();
                }
                if (!hints.researchType) {
                    hints.researchType = "Ancestors";
                    hints.personId = login.personId;
                    hints.personName = login.personName;
                    hints.generation = "3";
                    hints.topScore = true;
                    hints.count = false;
                }
                _researchType = hints.researchType;
                _personId = hints.personId;
                _personName = hints.personName;
                _generation = hints.generation;
                _topScore = hints.topScore;
                _count = hints.count;
                _reportId = hints.reportId;
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
                $("#hintsPersonId").val(_personId);
            }
            if (_researchType) {
                $("#hintsResearchType").val(_researchType);
            }
            if (_generation) {
                $("#hintsGeneration").val(_generation);
            }
            if (_topScore) {
                $("#hintsTopScore").prop('checked', _topScore);
            }
            if (_count) {
                $("#hintsCount").prop('checked', _count);
            }
            if (_reportId) {
                $("#hintsReportId").val(_reportId);
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
            _topScore = true;
            _count = false;
            _addChildren = false;
            updateForm();
        }

        function savePrevious() {
            if (_previous) {
                if (window.localStorage) {
                    localStorage.setItem("hintsPrevious", JSON.stringify(_previous));
                }
            }
        }


        function updateResearchData() {
            $("#hintsReportId").val(_reportId);
            var reportText = $("#hintsReportId option:selected").text();
            if (reportText && reportText.length > 8) {
                var nameIndex = reportText.indexOf("Name: ") + 6;
                var dateIndex = reportText.indexOf(", Date:  ");
                var researchTypeIndex = reportText.indexOf(", Research Type: ");
                var generationoIndex = reportText.indexOf(",  Generations: ");
                _personId = reportText.substring(nameIndex, nameIndex + 8);
                _personName = reportText.substring(nameIndex + 11, dateIndex);
                _researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
                _generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
//                _reportId = $("#hintsReportId option:selected").val();
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
                                $("#hintsReportId").append(optionhtml);
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
                    $("#hintsReportId").append(optionhtml);
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
                                    $("#hintsReportId").append(optionhtml);
                                });
                            }
                        }
                    });
                } else if (RetrieveData.reports) {
                    var data = RetrieveData.reports;
                    $("#hintsReportId").empty();
                    var optionhtml = '<option value="0">Select</option>';
                    $("#hintsReportId").append(optionhtml);
                    var found = false;
                    $.each(data, function (i) {
                        if (data[i].ValueMember === _reportId) {
                            found = true;
                        } else if ((_reportId === "0") && (i === 0)) {
                            _reportId = data[i].ValueMember;
                            found = true;
                        }
                        var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                        $("#hintsReportId").append(optionhtml);
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
                            url: '/Home/GetHintHtml',
                            success: function (data) {
                                var $dialogContainer = $('#hintsReportForm');
                                var $detachedChildren = $dialogContainer.children().detach();
                                $('<div id="hintsReportForm"></div>').dialog({
                                    position: {
                                        my: "center top",
                                        at: ("center top+" + (window.innerHeight * .07)),
                                        collision: "none"
                                    },
                                    title: "Hints",
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
                                        progressInit('hintsSpinner', true);
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
                                $("#hintsReportForm").empty().append(data);
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
                    _previous = JSON.parse(localStorage.getItem("hintsPrevious"));
                }
            }
            if (_previous) {
                progressInit('hintsSpinner');
                $.ajax({
                    url: '/Home/GetHintHtml',
                    success: function (data) {
                        var $dialogContainer = $('#hintsReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="hintsReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Hints",
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
                        $("#hintsReportForm").empty().append(data);
                    }
                });
            } else {
                msgBox.message("Sorry, there is nothing previous to display.");
            }
            return false;
        }

        function resetReportId() {
            if (_reportId !== "0") {
                $("#hintsReportId").val("0");
            }
            _reportId = "0";
        }

        function setHiddenFields() {
            if (_researchType === "Ancestors") {
            } else {
                _generation = "2";
            }
            $("hintsGeneration").val(_generation);
        }

        function initialize() {
            if (!_initialized) {
                loadData();
            }
            loadReports();
            PersonInfo.loadPersons($("#hintsPersonId"), false);
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
            $("#hintsGeneration").val(_generation);
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
            $("#hintsGeneration").val(_generation);
        }

        function addGenerationOptions(options) {
            var select = $("<select class=\"form-control select1Digit\" id=\"hintsGeneration\"\>");
            $.each(options, function (a, b) {
                select.append($("<option/>").attr("value", b).text(b));
            });
            $('#hintsGenerationDiv').empty();
            $("#hintsGenerationDiv").append(select);
            $('#hintsGenerationDiv').nextAll().remove();
            if ((_researchType === "Ancestors") && (_reportId === "0")) {
                $("#hintsGenerationDiv").after("<span class=\"input-group-btn\"><input id=\"addChildren\" type=\"checkbox\" style=\"vertical-align: top; margin-top: -0.625em;\"/></span><label for=\"addChildren\" style=\"vertical-align: middle; margin-top: -0.965em\">&nbsp;<span style=\"font-weight: normal\">Add Children</span></label>");
            }
            $("#hintsGeneration").change(function (e) {
                var generation = $("#hintsGeneration").val();
                if (_researchType === "Descendants") {
                    if (generation > 1) {
                        msgBox.warning("Selecting two generations of Descendants will more than double the time to retrieve descendants.");
                    }
                } else {
                    if (generation > _generation) {
                        msgBox.warning("Increasing the number of generations will increase the time to retrieve ancestors.");
                    }
                }
                _generation = $("#hintsGeneration").val();
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
            get topScore() {
                return _topScore;
            },
            set topScore(value) {
                _topScore = value;
            },
            get count() {
                return _count;
            },
            set count(value) {
                _count = value;
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
    Hints.form = $("#hintsForm");
}


(function ($) {
    Hints.initialize();

    $('#hintsPerson').change(function (e) {
        debugger;
    });

    $("#hintsReportId").change(function (e) {
        Hints.reportId = $("#hintsReportId option:selected").val();
        if (Hints.reportId === "0") {
            msgBox.warning("Even though the \"Select\" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.");
        }
        Hints.updateResearchData();
    });

    $('#hintsResearchType').change(function (e) {
        Hints.researchType = $("#hintsResearchType").val();
        if (Hints.researchType === "Descendants") {
            Hints.generationAncestors = Hints.generation;
            Hints.generation = Hints.generationDescendants;
            Hints.addDecendantGenerationOptions();
        } else {
            Hints.generationDescendants = Hints.generation;
            Hints.generation = Hints.generationAncestors;
            Hints.addAncestorGenerationOptions();
        }

        Hints.setHiddenFields();
        Hints.resetReportId();
        Hints.updateResearchData();
    });

    $("#hintsFindPersonButton").unbind('click').bind('click', function (e) {
        findPerson(e, $(this)).then(function () {
            if (FindPerson.selected) {
                var changed = (FindPerson.personId === Hints.personId) ? false : true;
                PersonInfo.updateFromFindPerson();
                Hints.personId = FindPerson.personId;
                Hints.personName = FindPerson.personName;
                Hints.save();
                PersonInfo.loadPersons($("#hintsPersonId"), true);
                if (changed) {
                    Hints.resetReportId();
                    Hints.updateResearchData();
                }
            }
        });
        return false;
    });

    $("#hintsRetrieveButton").unbind('click').bind('click', function (e) {
        if (Hints.personId) {
            RetrieveData.personId = Hints.personId;
            RetrieveData.personName = Hints.personName;
            RetrieveData.researchType = Hints.researchType;
            RetrieveData.generation = Hints.generation;
            RetrieveData.caller = "Hints";
            RetrieveData.retrievedRecords = 0;
            RetrieveData.popup = true;
            Hints.updateResearchData();
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#addChildren").change(function (e) {
        Hints.addChildren = $("#addChildren").prop("checked");
        if (Hints.addChildren) {
            msgBox.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors.");
        }
        Hints.resetReportId();
        Hints.updateResearchData();
    });

    $("#hintsTopScore").change(function (e) {
        Hints.topScore = $("#hintsTopScore").is(':checked');
        if (Hints.topScore) {
            Hints.count = false;
        } else {
            Hints.count = true;
        }
    });

    $("#hintsCount").change(function (e) {
        Hints.count = $('#hintsCount').is(':checked');
        if (Hints.count) {
            Hints.topScore = false;
        } else {
            Hints.topScore = true;
        }
    });

    $("#hintsCancelButton").unbind('click').bind('click', function (e) {
        Hints.form.dialog("close");
    });

    Hints.form.unbind('dialogclose').bind('dialogclose', function (e) {
        Hints.save();
    });
    openForm(Hints.form, Hints.formTitleImage, "hintsSpinner");
})(jQuery);

//# sourceURL=Hints.js