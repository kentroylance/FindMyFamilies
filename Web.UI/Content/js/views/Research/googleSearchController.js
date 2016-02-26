define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var msgBox = require('msgBox');

    // models
    var googleSearch = require('googleSearch');
    var person = require('person');
    var researchHelper = require('researchHelper');

    $("#googleFirstName").val("");
    $("#googleMiddleName").val("");
    $("#googleLastName").val("");
    $("#googleBirthYear").val("");
    $("#googleBirthPlace").val("");
    $("#googleDeathYear").val("");
    $("#googleYearRange").val("10");
    $('#googleFirstName').focus();

    $("#clearButton").unbind('click').bind('click', function (e) {
        clear();
    });


    function updateForm() {
        if (googleSearch.firstName) {
            $("#googleFirstName").val(googleSearch.firstName);
        }
        if (googleSearch.middleName) {
            $("#googleMiddleName").val(googleSearch.middleName);
        }
        if (googleSearch.lastName) {
            $("#googleLastName").val(googleSearch.lastName);
        }
        if (googleSearch.birthYear) {
            $('#googleBirthYear').val(googleSearch.birthYear);
        }
        if (googleSearch.deathYear) {
            $('#googleDeathYear').val(googleSearch.deathYear);
        }
        if (googleSearch.birthPlace) {
            $('#googleBirthPlace').val(googleSearch.birthPlace);
        }
        if (googleSearch.yearRange) {
            $('#googleYearRange').val(googleSearch.yearRange);
        }
    }


    function loadEvents() {
        $("#clearButton").unbind('click').bind('click', function (e) {
            clear();
        });
        $("#searchButton").unbind('click').bind('click', function (e) {
            submit();
        });

        $("#googleSearchCancelButton").unbind('click').bind('click', function (e) {
            googleSearch.selected = false;
            googleSearch.form.dialog(constants.CLOSE);
        });

        $("#googleSearchCloseButton").unbind('click').bind('click', function(e) {
            googleSearch.form.dialog(constants.CLOSE);
        });

        $("#googleOptions").change(function (e) {
            buildSearchString(" ");
        });

        googleSearch.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (googleSearch.callerSpinner) {
                system.spinnerArea = googleSearch.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            googleSearch.save();
            if (googleSearch.callback) {
                if (typeof (googleSearch.callback) === "function") {
                    googleSearch.callback(googleSearch.selected);
                }
            }
            googleSearch.reset();
        });


        googleSearch.form.formValidation({
                framework: 'bootstrap',
                icon: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    googleFirstName: {
                        row: '.col-xs-4',
                        validators: {
                        }
                    },
                    googleMiddleName: {
                        row: '.col-xs-4',
                        validators: {
                        }
                    },
                    googleLastName: {
                        row: '.col-xs-4',
                        validators: {
                        }
                    },
                    googleBirthYear: {
                        validators: {
                            numeric: {
                                message: 'Must be a number'
                            },
                            stringLength: {
                                min: 4,
                                max: 4,
                                message: 'Must be 4 digits'
                            }
                        }
                    },
                    googleDeathYear: {
                        validators: {
                            numeric: {
                                message: 'Must be a number'
                            },
                            stringLength: {
                                min: 4,
                                max: 4,
                                message: 'Must be 4 digits'
                            }
                        }
                    }
                }
            })
            .on('keyup', '', function(e) {
                var fv = googleSearch.form.data('formValidation');
                $("#googleFeedbackMessage").val(buildSearchString('"'));


//              switch (e.target.id) {
//                  case 'firstName': {
//
//                      buildSearchString();
//
//                      break;
//                  }
//
//                  case 'lastName': {
//                      buildSearchString();
//                      break;
//                  }
//
//                  case 'middleName': {
//                      buildSearchString();
//
//                      break;
//                  }
//
//                  case 'birthYear': {
//                      buildSearchString();
//
//                      break;
//                  }
//
//                  case 'deathYear': {
//                      buildSearchString();
//
//                      break;
//                  }
//
//                  default:
//                    break;
//                }
            })
            .on('success.form.fv', function(e) {
                e.preventDefault();

                //                // Some instances you can use are
                //                var $form = $(e.target); // The form instance
                //                var fv = $(e.target).data('formValidation'); // FormValidation instance
                submit();

            });
    };

    function clear() {
        $("#googleFirstName").val("");
        $("#googleMiddleName").val("");
        $("#googleLastName").val("");
        $("#googleBirthYear").val("");
        $("#googleBirthPlace").val("");
        $("#googleDeathYear").val("");
        $("#googleYearRange").val("10");
        // Revalidate the fields
        validateRow1(googleSearch.form.data('formValidation'));
        googleSearch.form
            .formValidation('revalidateField', 'googleBirthYear')
            .formValidation('revalidateField', 'googleDeathYear');
        $("#googleFeedbackMessage").val(buildSearchString('"'));
        $('#googleFirstName').focus();

    }

    //  “William Clarkson” OR “William * Clarkson” OR “Clarkson, William” genealogy “Utah OR UT” 1910..1920
    function buildSearchString(quoteChar) {
        var searchString = "";
        googleSearch.firstName = $("#googleFirstName").val();
        googleSearch.middleName = $("#googleMiddleName").val();
        googleSearch.lastName = $("#googleLastName").val();
        googleSearch.birthYear = $("#googleBirthYear").val();
        googleSearch.birthPlace = $("#googleBirthPlace").val();
        googleSearch.deathYear = $("#googleDeathYear").val();
        googleSearch.yearRange = $("#googleYearRange").val();
        var googleOption = $("#googleOptions option:selected").val();
        if (googleSearch.firstName && !googleSearch.middleName && !googleSearch.lastName) {
            //            buildSearchString = "%22" + googleSearch.firstName + "%22" + googleSearch.lastName + "%22";
            searchString = googleSearch.firstName;
        } else if (googleSearch.firstName && googleSearch.middleName && !googleSearch.lastName) {
            //            buildSearchString = "%22" + googleSearch.firstName + "%22" + googleSearch.lastName + "%22";
            searchString = quoteChar + googleSearch.firstName + " " + googleSearch.middleName + quoteChar + " OR " + quoteChar + '*, ' + googleSearch.firstName + " " + googleSearch.middleName + quoteChar;
        } else if (googleSearch.firstName && googleSearch.middleName && googleSearch.lastName) {
            //            buildSearchString = "%22" + googleSearch.firstName + "%22" + googleSearch.lastName + "%22";
            searchString = quoteChar + googleSearch.firstName + " " + googleSearch.middleName + " " + googleSearch.lastName + quoteChar + " OR " + quoteChar + googleSearch.lastName + ", " + googleSearch.firstName + " " + googleSearch.middleName + quoteChar;
        } else if (googleSearch.firstName && googleSearch.lastName) {
            //            buildSearchString = "%22" + googleSearch.firstName + "%22" + googleSearch.lastName + "%22";
            searchString = quoteChar + googleSearch.firstName + " " + googleSearch.lastName + quoteChar + " OR " + quoteChar + googleSearch.firstName + " * " + googleSearch.lastName + quoteChar + " OR " + quoteChar + googleSearch.lastName + ", " + googleSearch.firstName + '"';
        }

        if (searchString) {
            if (googleOption) {
                searchString += " " + googleOption;
            }

            if (googleSearch.birthPlace) {
                if (googleSearch.birthPlace.indexOf(' ') >= 0) {
                    searchString += " " + quoteChar + googleSearch.birthPlace + quoteChar;
                } else {
                    searchString += " " + googleSearch.birthPlace;
                }
            }

            if (googleSearch.birthYear && googleSearch.birthYear.length > 3) {
                var halfYearRange = Math.round(Math.abs(googleSearch.yearRange/2));
                var part1 = parseInt(googleSearch.birthYear) - halfYearRange;
                var part2 = parseInt(googleSearch.birthYear) + halfYearRange;
                searchString += " " + part1 + ".." + part2;
            }

            if (googleSearch.deathYear && googleSearch.deathYear.length > 3) {
                var halfYearRange = Math.round(Math.abs(googleSearch.yearRange / 2));
                var part1 = parseInt(googleSearch.deathYear) - halfYearRange;
                var part2 = parseInt(googleSearch.deathYear) + halfYearRange;
                searchString += " " + part1 + ".." + part2;
            }
        }
        return searchString;
    }

    function validateRow1(fv) {
        test = "";
    }

    function open() {
        googleSearch.form = $("#googleSearchForm");
        loadEvents();
        updateForm();
        system.openForm(googleSearch.form, googleSearch.formTitleImage, googleSearch.spinner);
        if (!googleSearch.yearRange) {
            $("#googleYearRange").val("10");
        }
        $("#googleFeedbackMessage").val(buildSearchString('"'));
        $('#googleFirstName').focus();
    }

    function submit() {
//        if (system.isAuthenticated()) {
////            googleSearch.firstName = $("#firstName").val();
////            googleSearch.middleName = $("#middleName").val();
////            googleSearch.lastName = $("#lastName").val();
////            googleSearch.birthYear = $("#birthYear").val();
////            googleSearch.deathYear = $("#deathYear").val();
////            googleSearch.birthPlace = $("#birthPlace").val();
//
//        } else {
//            $(this).dialog("close");
//            system.relogin();
        //        }
        //  “William Clarkson” OR “William * Clarkson” OR “Clarkson, William” genealogy “Utah OR UT” 1910..1920
        googleSearch.save();
        var url = "https://www.google.com/webhp?sourceid=chrome-instant&ion=1&espv=2&ie=UTF-8#q=" + buildSearchString("%22"); // "%22is%22+OR+%22that%22";
        window.open(url, '_blank');
    }

    var googleSearchController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        },
        searchKeyPress: function(e) {
            searchKeyPress(e);
        }
    };

    researchHelper.googleSearchController = googleSearchController;
    open();

    $(function() {
    });


    return googleSearchController;
});




//# sourceURL=googleSearchController.js