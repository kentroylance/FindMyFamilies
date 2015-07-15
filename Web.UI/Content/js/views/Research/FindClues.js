if (!FindClues) {
    FindClues = (function($) {
        var _researchType;
        var _personId;
        var _personName;
        var _generation = "3";
        var _reportId = "0";
        var _searchCriteria = "0";
        var _gapInChildren = "3";
        var _ageLimit = "18";
        var _formName = "findCluesForm";
        var _formTitleImage = "fmf24-clue";
        var _form = $("#findCluesForm");
        var _initialized = false;
        var _previous;
        var _displayType = "start";
        var _searchCriteriaList;
        var _addChildren;

        function FindCluesDO(reportId, researchType, personId, personName, generation, searchCriteria, gapInChildren, ageLimit, addChildren) {
            this.reportId = reportId;
            this.researchType = researchType;
            this.personId = personId;
            this.personName = personName;
            this.generation = generation;
            this.searchCriteria = searchCriteria;
            this.gapInChildren = gapInChildren;
            this.ageLimit = ageLimit;
            this.addChildren = addChildren;
        }

        function FindCluesDO1(personId, reportId, searchCriteria, gapInChildren, ageLimit, rowsReturned) {
            this.personId = personId;
            this.reportId = reportId;
            this.searchCriteria = searchCriteria;
            this.gapInChildren = gapInChildren;
            this.ageLimit = ageLimit;
            this.rowsReturned = rowsReturned;
        }


        function save() {
            if (window.localStorage) {
                var findClues = new FindCluesDO(_reportId, _researchType, _personId, _personName, _generation, _searchCriteria, _gapInChildren, _ageLimit);
                localStorage.setItem("FindClues", JSON.stringify(findClues));
            }
        }

        function loadData() {
            if (window.localStorage) {
                var findClues = JSON.parse(localStorage.getItem("FindClues"));
                if (!findClues) {
                    findClues = new FindCluesDO();
                }
                if (!findClues.gapInChildren) {
                    findClues.searchCriteria = "0";
                    findClues.gapInChildren = "3";
                    findClues.ageLimit = "18";
                    findClues.generation = "3";
                }

                _reportId = findClues.reportId;
                _researchType = findClues.researchType;
                _personId = findClues.personId;
                _personName = findClues.personName;
                _generation = findClues.generation;
                _searchCriteria = findClues.searchCriteria;
                if (!_reportId) {
                    _reportId = "0";
                }
                _gapInChildren = findClues.gapInChildren;
                _ageLimit = findClues.ageLimit;
                _generation = findClues.generation;
            }
        }

        function updateForm() {
            if (_personId) {
                $("#findCluesPersonId").val(_personId + " - " + _personName);
            }
            if (_researchType) {
                $("#findCluesResearchType").val(_researchType);
            }
            if (_generation) {
                $("#findCluesGeneration").val(_generation);
            }
            if (_searchCriteria) {
                $("#findCluesSearchCriteria").val(_searchCriteria);
            }
            if (_gapInChildren) {
                $("#findCluesGapInChildren").val(_gapInChildren);
            }
            if (_ageLimit) {
                $("#findCluesAgeLimit").val(_ageLimit);
            }
            if (_reportId) {
                $("#findCluesReportId").val(_reportId);
            }
        }

        function reset() {
            _researchType = "Ancestors";
            _personId = login.personId;
            _personName = login.personName;
            _generation = "2";
            _reportId = "0";
            _searchCriteria = "0";
            _gapInChildren = "3";
            _ageLimit = "18";
            _addChildren = false;
            updateForm();
        }

        function savePrevious() {
            if (_previous) {
                if (window.localStorage) {
                    localStorage.setItem("findCluesPrevious", JSON.stringify(_previous));
                }
            }
        }


        function updateResearchData() {
            var reportText = $("#findCluesReportId option:selected").text();
            if (reportText && reportText.length > 8) {
                var nameIndex = reportText.indexOf("Name: ") + 6;
                var dateIndex = reportText.indexOf(", Date:  ");
                var researchTypeIndex = reportText.indexOf(", Research Type: ");
                var generationoIndex = reportText.indexOf(",  Generations: ");
                _personId = reportText.substring(nameIndex, nameIndex + 8);
                _personName = reportText.substring(nameIndex + 11, dateIndex);
                _researchType = reportText.substring(researchTypeIndex + 17, generationoIndex);
                _generation = reportText.substring(generationoIndex + 16, generationoIndex + 17);
                _reportId = $("#findCluesReportId option:selected").val();
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
                            _reportId = "0";
                            $.each(data, function (i) {
                                if (i === 0) {
                                    _reportId = data[i].ValueMember;
                                }
                                var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                                $("#findCluesReportId").append(optionhtml);
                            });
                        }
                    }
                });
            } else {
                if (refreshReport) {
                    $("#findCluesReportId").empty();
                    var optionhtml = '';
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
                                    $("#findCluesReportId").append(optionhtml);
                                });
                            }
                        }
                    });
                } else if (RetrieveData.reports) {
                    var data = RetrieveData.reports;
                    $("#findCluesReportId").empty();
                    _reportId = "0";
                    $.each(data, function (i) {
                        if (i === 0) {
                            _reportId = data[i].ValueMember;
                        }
                        var optionhtml = '<option value="' + data[i].ValueMember + '">' + data[i].DisplayMember + '</option>';
                        $("#findCluesReportId").append(optionhtml);
                    });
                }
            }
            updateResearchData();
        }

        function submit() {
            if (isAuthenticated()) {
                if (!_reportId || _reportId < 1) {
                    msgBox.message("You must first select family search data to findClues");
                } else {
                    save();
                }

                $.ajax({
                    url: '/Home/GetFindCluesHtml',
                    success: function(data) {
                        var $dialogContainer = $('#findCluesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="findCluesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Clues for " + _personName + "'s " + _researchType,
                            width: 1100,
                            open: function() {
                                $detachedChildren.appendTo($dialogContainer);
                                $(this).css("maxHeight", 700);
                            },
                            close: function(event, ui) {
                                event.preventDefault();
                                if (_reportId === "0") {
                                    loadReports(true);
                                }
                                progressInit('findCluesSpinner', true);
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
                        $("#findCluesReportForm").empty().append(data);
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
                    _previous = JSON.parse(localStorage.getItem("findCluesPrevious"));
                }
            }
            if (_previous) {
                progressInit('findCluesSpinner');
                $.ajax({
                    url: '/Home/GetFindCluesHtml',
                    success: function (data) {
                        var $dialogContainer = $('#findCluesReportForm');
                        var $detachedChildren = $dialogContainer.children().detach();
                        $('<div id="findCluesReportForm"></div>').dialog({
                            position: {
                                my: "center top",
                                at: ("center top+" + (window.innerHeight * .07)),
                                collision: "none"
                            },
                            title: "Analysis for " + _personName + "'s " + + _researchType,
                            width: 1100,
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
                        $("#findCluesReportForm").empty().append(data);
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

        function loadSearchCriteria() {
            if (!_searchCriteria) {
                _searchCriteria = "0";
            }
            if (!_searchCriteriaList) {
                $.ajax({
                    'async': false,
                    url: "/Home/GetSearchCriteriaList",
                    success: function (data) {
                        if (data) {
                            _searchCriteriaList = data;
                        }
                    }
                });
            }
            $.each(_searchCriteriaList, function (i) {
                var optionhtml = '<option value="' + _searchCriteriaList[i].ValueMember + '">' + _searchCriteriaList[i].DisplayMember + '</option>';
                $("#findCluesSearchCriteria").append(optionhtml);
            });
        }

        function initialize() {
            if (!_initialized) {
                loadData();
            }
            loadSearchCriteria();

            loadReports();
            updateForm();
            if (!_initialized) {
                _initialized = true;
            }
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
            get searchCriteria() {
                return _searchCriteria;
            },
            set searchCriteria(value) {
                _searchCriteria = value;
            },
            get gapInChildren() {
                return _gapInChildren;
            },
            set gapInChildren(value) {
                _gapInChildren = value;
            },
            get ageLimit() {
                return _ageLimit;
            },
            set ageLimit(value) {
                _ageLimit = value;
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
            }

        }
    }(jQuery));
} else {
    FindClues.form = $("#findCluesForm");
}


(function ($) {
    FindClues.initialize();

    $("#findCluesReportId").change(function (e) {
        FindClues.updateResearchData();

    });

    $("#findCluesRetrieveButton").unbind('click').bind('click', function (e) {
        if (FindClues.gapInChildren) {
            RetrieveData.gapInChildren = FindClues.gapInChildren;
            RetrieveData.ageLimit = FindClues.ageLimit;
            RetrieveData.searchCriteria = FindClues.searchCriteria;
            RetrieveData.generation = FindClues.generation;
            RetrieveData.caller = "FindClues";
            RetrieveData.retrievedRecords = 0;
            RetrieveData.popup = true;
        }
        retrieveData(e, $(this)).then(function () {
        });
        return false;
    });

    $("#findCluesSearchCriteria").change(function (e) {
        FindClues.searchCriteria = $("#findCluesSearchCriteria").val();
    });

    $("#findCluesGapInChildren").change(function (e) {
        FindClues.gapInChildren = $("#findCluesGapInChildren").val();
    });

    $("#findCluesAgeLimit").change(function (e) {
        FindClues.ageLimit = $("#findCluesAgeLimit").val();
    });


    $("#findCluesCancelButton").unbind('click').bind('click', function (e) {
        FindClues.form.dialog("close");
    });

    FindClues.form.unbind('dialogclose').bind('dialogclose', function (e) {
        FindClues.save();
    });
    openForm(FindClues.form, FindClues.formTitleImage, "findCluesSpinner");

})(jQuery);

//# sourceURL=FindClues.js