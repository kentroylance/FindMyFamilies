define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var findPersonHelper = require('findPersonHelper');
    var msgBox = require('msgBox');

    // models
    var findPerson = require('findPerson');
    var person = require('person');
    var researchHelper = require('researchHelper');

    var _previousType = "find";

    function loadEvents() {
        $("#findPersonOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, findPerson);
        });

        $("#clearButton").unbind('click').bind('click', function (e) {
            clear();
        });

        $("#previousFind").unbind('click').bind('click', function(e) {
            _previousType = "find";
            $("#personId").val(findPerson.personId);
            $("#firstName").val(findPerson.firstName);
            $("#lastName").val(findPerson.lastName);
            $("#gender").val(findPerson.gender);
            $("#birthYear").val(findPerson.birthYear);
            $("#deathYear").val(findPerson.deathYear);

            // Revalidate the fields
            validateRow1(findPerson.form.data('formValidation'));
            findPerson.form
                .formValidation('revalidateField', 'gender')
                .formValidation('revalidateField', 'birthYear')
                .formValidation('revalidateField', 'deathYear');

            $('#submit').focus();
            submit();
        });

        $("#previousDialog").unbind('click').bind('click', function (e) {
            _previousType = "dialog";
            if (person.id) {
                $("#personId").val(person.id);

                // Revalidate the fields
                validateRow1(findPerson.form.data('formValidation'));
                findPerson.form
                    .formValidation('revalidateField', 'gender')
                    .formValidation('revalidateField', 'birthYear')
                    .formValidation('revalidateField', 'deathYear');

                $('#submit').focus();
                submit();
            }
        });

        $("#findPersonSelectButton").unbind('click').bind('click', function (e) {
            if (!findPerson.selected) {
                msgBox.message("You must first select a person by checking the checkbox before proceeding");
                $('#eventsTable').focus();
            } else {
                findPerson.form.dialog(constants.CLOSE);
            }
        });

        $("#findPersonCancelButton").unbind('click').bind('click', function (e) {
            findPerson.selected = false;
            findPerson.form.dialog(constants.CLOSE);
        });

        $("#findPersonCloseButton").unbind('click').bind('click', function(e) {
            findPerson.form.dialog(constants.CLOSE);
        });

        findPerson.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (findPerson.callerSpinner) {
                system.spinnerArea = findPerson.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            person.save();
            if (findPerson.callback) {
                if (typeof (findPerson.callback) === "function") {
                    findPerson.callback(findPerson.selected);
                }
            }
            findPerson.reset();
        });

        $("#findPersonActionButton").unbind('click').bind('click', function (e) {
            findPerson.personId = $("#personId").val();
            findPerson.fullName = $("#firstName").val() + " " + $("#lastName").val();
            findPerson.firstName = $("#firstName").val();
            findPerson.middleName = "";
            findPerson.lastName = $("#lastName").val();
            findPerson.gender = $("#gender").val();
            findPerson.birthYear = $("#birthYear").val();
            findPerson.deathYear = $("#deathYear").val();
            findPerson.birthPlace = "";

            var findPersonOptionsBak = person.findPersonOptions;
            person.findPersonOptions = [];
            person.findPersonOptions.push('fmf-google');
            person.findPersonOptions.push('findagrave');
            person.findPersonOptions.push('ancestry');
            person.findPersonOptions.push('myheritage');
            person.findPersonOptions.push('findmypast');
            person.findPersonOptions.push('billgrave');
            person.findPersonOptions.push('fs-search');
            person.findPersonOptions.push('amerancest');

            $("#findPersonAction").append(findPersonHelper.getMenuOptions(findPerson));
            person.findPersonOptions = findPersonOptionsBak;
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                $(this).append(findPersonHelper.getMenuOptions(row));
            },
            'mouseout .personAction1': function (e, value, row, index) {
                $('#personInfoDiv').hide();
            },
            'mouseover .personAction1': function (e, value, row, index) {

                $('#content').empty();

                var html = "<label><span style=\"color: " + _findPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _findPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _findPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _findPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#content').append(html);
                $('#personInfoDiv').show();
                $("#personInfoDiv").position({
                    my: "center center+70",
                    at: "center",
                    of: $("#findPersonForm")
                });


            }

            
    };

        var $result = $('#eventsResult');

        $('#eventsTable').on('all.bs.table', function(e, name, args) {
                console.log('Event:', name, ', data:', args);
            })
            .on('click-row.bs.table', function(e, row, $element) {
                $result.text('Event: click-row.bs.table');
            })
            .on('dbl-click-row.bs.table', function(e, row, $element) {
                $result.text('Event: dbl-click-row.bs.table');
            })
            .on('sort.bs.table', function(e, name, order) {
                $result.text('Event: sort.bs.table');
            })
            .on('check.bs.table', function(e, row) {
                $result.text('Event: check.bs.table');
                if (row.state && row.id) {
                    findPerson.id = row.id;
                    findPerson.name = row.fullName;
                    findPerson.selected = true;
                }
            })
            .on('uncheck.bs.table', function(e, row) {
                $result.text('Event: uncheck.bs.table');
                //        person.id = row.id;
                //        person.name = row.fullName;
                findPerson.selected = false;
            })
            .on('check-all.bs.table', function(e) {
                $result.text('Event: check-all.bs.table');
            })
            .on('uncheck-all.bs.table', function(e) {
                $result.text('Event: uncheck-all.bs.table');
            })
            .on('load-success.bs.table', function(e, data) {
                $result.text('Event: load-success.bs.table');
            })
            .on('load-error.bs.table', function(e, status) {
                $result.text('Event: load-error.bs.table');
            })
            .on('column-switch.bs.table', function(e, field, checked) {
                $result.text('Event: column-switch.bs.table');
            })
            .on('page-change.bs.table', function(e, size, number) {
                $result.text('Event: page-change.bs.table');
            })
            .on('search.bs.table', function(e, text) {
                $result.text('Event: search.bs.table');
            });


        findPerson.form.formValidation({
                framework: 'bootstrap',
                icon: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    personId: {
                        row: '.col-xs-2',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: 'Please enter id number'
                            },
                            stringLength: {
                                min: 8,
                                message: 'Must be 8 characters long'
                            },
                            regexp: {
                                regexp: /^[a-zA-Z0-9]{4}-[a-zA-Z0-9]{3}$/,
                                message: 'ID must be in this format: ####-###',
                            }
                        },
                        onSuccess: function (e, data) {
                            data.fv.enableFieldValidators('gender', false).revalidateField('gender');
                        }
                    },
                    firstName: {
                        row: '.col-xs-5',
                        validators: {
                            notEmpty: {
                                message: 'Or the first name'
                            }
                        }
                    },
                    lastName: {
                        row: '.col-xs-5',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: 'Or the last name'
                            }
                        }
                    },
                    gender: {
                        validators: {
                            notEmpty: {
                                message: 'Gender is required'
                            }
                        }
                    },
                    birthYear: {
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
                    deathYear: {
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
            .on('keyup', '[name="personId"], [name="firstName"], [name="lastName"]', function(e) {
                var fv = findPerson.form.data('formValidation');

                switch ($(this).attr('name')) {
                case 'firstName':
                    validateRow1(fv);
                    break;

                case 'lastName':
                    validateRow1(fv);
                    break;

                case 'personId':
                    validateRow1(fv);
                    break;

                default:
                    break;
                }
            })
            .on('success.form.fv', function(e) {
                e.preventDefault();

                //                // Some instances you can use are
                //                var $form = $(e.target); // The form instance
                //                var fv = $(e.target).data('formValidation'); // FormValidation instance
                submit();

            });


    }

    function clear() {
        $("#personId").val("");
        $("#firstName").val("");
        $("#lastName").val("");
        $("#gender").val("");
        $("#birthYear").val("");
        $("#deathYear").val("");
        $('#firstName').focus();
        // Revalidate the fields
        validateRow1(findPerson.form.data('formValidation'));
        findPerson.form
            .formValidation('revalidateField', 'gender')
            .formValidation('revalidateField', 'birthYear')
            .formValidation('revalidateField', 'deathYear');
    }

    function validateRow1(fv) {

        if (string($("#personId").val()).isEmpty() && string($("#firstName").val()).isEmpty() && string($("#lastName").val()).isEmpty()) {
            fv.enableFieldValidators('firstName', true).revalidateField('firstName');
            fv.enableFieldValidators('lastName', true).revalidateField('lastName');
            fv.enableFieldValidators('personId', true).revalidateField('personId');
        } else {
            if (!string($("#firstName").val()).isEmpty()) {
                fv.enableFieldValidators('firstName', true).revalidateField('firstName');
            } else {
                fv.enableFieldValidators('firstName', false).revalidateField('firstName');
            }
            if (!string($("#lastName").val()).isEmpty()) {
                fv.enableFieldValidators('lastName', true).revalidateField('lastName');
            } else {
                fv.enableFieldValidators('lastName', false).revalidateField('lastName');
            }
            if (!string($("#personId").val()).isEmpty()) {
                fv.enableFieldValidators('personId', true).revalidateField('personId');
            } else {
                fv.enableFieldValidators('personId', false).revalidateField('personId');
                fv.enableFieldValidators('gender', true).revalidateField('gender');
            }
        }
    }

    function open() {
        $("#popover").popover({ trigger: "hover" });
        findPerson.form = $("#findPersonForm");
        loadEvents();
        system.openForm(findPerson.form, findPerson.formTitleImage, findPerson.spinner);
        $('#firstName').focus();
        $('.no-records-found').hide();
        var selectButton = document.getElementById("findPersonSelectButton");
        if (findPerson.callerSpinner === constants.DEFAULT_SPINNER_AREA) {
            $('#findPersonSelectButton').hide();
        } else {
            $('#findPersonSelectButton').show();
        }

    }

    function submit() {
        if (system.isAuthenticated()) {
            findPerson.personId = $("#personId").val();
            findPerson.firstName = $("#firstName").val();
            findPerson.lastName = $("#lastName").val();
            findPerson.gender = $("#gender").val();
            findPerson.birthYear = $("#birthYear").val();
            findPerson.deathYear = $("#deathYear").val();

            if (_previousType === "find") {
                findPerson.save();
            }

            $.ajax({
                url: constants.FIND_PERSONS_URL,
                data: {
                    "personId": findPerson.personId,
                    "firstName": findPerson.firstName,
                    "lastName": findPerson.lastName,
                    "gender": findPerson.gender,
                    "birthYear": findPerson.birthYear,
                    "deathYear": findPerson.deathYear
                },
                success: function (data) {
                    if (data && data.errorMessage) {
                        msgBox.error(data.errorMessage);
                    } else {
                        $('#eventsTable').bootstrapTable("load", data.list);
                    }
                }
            });
        } else {
            $(this).dialog("close");
            system.relogin();
        }
    }

    function findPersonsStartingPoint() {
        startingPoint();
    }

    var findPersonController = {
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

    researchHelper.findPersonController = findPersonController;
    open();

    $(function() {
    });


    return findPersonController;
});

var _findPerson = require('person');
var _findSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row && row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default dropdown-btn personAction1\"><span style=\"color: " + _findPerson.getPersonColor(row.gender) + "\">" + _findPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return [result].join('');
}

function spouseFormatter(value, row, index) {
    var result = "";
    if (row && row.id && row.spouseName) {
        result = "<p style=\"color: " + _findPerson.getPersonColor(row.spouseGender) + "\">" + _findPerson.getPersonImage(row.spouseGender) + row.spouseName + "</p>";
    }
    return result;
}

function eventsFormatter(value, row, index) {
    var result = "";
    if (row && row.id) {
        result = "<p><b>Birth:</b> " + row.birthYear + "</p><p><b>Death:</b> " + row.deathYear + "</p>";
    }
    return result;
}

function parentsFormatter(value, row, index) {
    var result = "";
    if (row && row.id && row.fatherName) {
        result = "<p style=\"color: " + _findPerson.getPersonColor("Male") + "\">" + _findPerson.getPersonImage("Male") + row.fatherName + "</p>";
    }
    if (row && row.id && row.motherName) {
        result = result + "<p style=\"color: " + _findPerson.getPersonColor("Female") + "\">" + _findPerson.getPersonImage("Female") + row.motherName + "</p>";
    }
    return result;
}


//# sourceURL=findPersonController.js