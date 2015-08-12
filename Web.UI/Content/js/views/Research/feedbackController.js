define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var msgBox = require('msgBox');
    var constants = require('constants');
    var researchHelper = require("researchHelper");

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();


    // models
    var person = require('person');
    var feedback = require('feedback');

    var retrieve = require('retrieve');

    function updateForm() {
        if (system.userName) {
            $("#feedbackName").val(system.userName);
        }
        if (system.userEmail) {
            $("#feedbackEmail").val(system.userEmail);
        }
        if (feedback.bug) {
            $("#feedbackBug").prop('checked', feedback.bug);
        }
        if (feedback.featureRequest) {
            $("#feedbackFeatureRequest").prop('checked', feedback.featureRequest);
        }
        if (feedback.other) {
            $("#feedbackOther").prop('checked', feedback.other);
        }
    }

    function open() {
        feedback.form = $("#feedbackForm");
        loadEvents();
        updateForm();
        system.openForm(feedback.form, feedback.formTitleImage, feedback.spinner);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
    }


    function loadEvents() {

        $("#feedbackCloseButton").unbind('click').bind('click', function(e) {
            feedback.form.dialog(constants.CLOSE);
        });

        $("#feedbackClearButton").unbind('click').bind('click', function(e) {
            $("#feedbackName").val("");
            $("#feedbackMessage").val("");
            $("#feedbackEmail").val("");
            // Revalidate the fields
            feedback.form
                .formValidation('revalidateField', 'feedbackName')
                .formValidation('revalidateField', 'feedbackMessage')
                .formValidation('revalidateField', 'feedbackEmail');
        });

        $("#feedbackSendButton").unbind('click').bind('click', function(e) {
            var name = $("#feedbackName").val();
            if (!name) {
                name = system.userName;
            }
            var bug = $("#feedbackBug").prop("checked");
            var other = $("#feedbackOther").prop("checked");
            var featureRequest = $("#feedbackFeatureRequest").prop("checked");
            var message = $("#feedbackMessage").val();
            var email = $("#feedbackEmail").val();
            if (email) {
                system.setCookie(constants.USER_EMAIL, email, 365);
            }

            $.ajax({
                url: constants.SEND_FEEDBACK_URL,
                data: {
                    "personId": system.userId,
                    "name": name,
                    "bug": bug,
                    "other": other,
                    "featureRequest": featureRequest,
                    "message": message,
                    "email": email
                },
                success: function (data) {
                    if (data.errorMessage) {
                        msgBox.message(data.errorMessage);
                    } else {
                        msgBox.message(data.message);
                        feedback.form.dialog(constants.CLOSE);
                    }
                }
            });
            return false;
        });

        $("#feedbackEmail").change(function(e) {
            retrieve.email = $("#feedbackEmail").val();
        });

        $("#feedbackMessage").change(function(e) {
            retrieve.email = $("#feedbackMessage").val();
        });

        $("#feedbackBug").change(function(e) {
            feedback.bug = $("#feedbackBug").prop("checked");
            if (feedback.bug) {
                feedback.count = false;
            } else {
                feedback.count = true;
            }
        });
        $("#feedbackFeatureRequest").change(function(e) {
            feedback.featureRequest = $('#feedbackFeatureRequest').prop("checked");
            if (feedback.count) {
                feedback.bug = false;
            } else {
                feedback.bug = true;
            }
        });
        $("#feedbackOther").change(function(e) {
            feedback.other = $('#feedbackOther').prop("checked");
            if (feedback.count) {
                feedback.bug = false;
            } else {
                feedback.bug = true;
            }
        });


        $("#feedbackCancelButton").unbind('click').bind('click', function(e) {
            feedback.form.dialog(constants.CLOSE);
        });

        feedback.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (feedback.callerSpinner) {
                system.spinnerArea = feedback.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            feedback.save();
        });

        feedback.form.formValidation({
            framework: 'bootstrap',
            icon: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: {
                email: {
                    validators: {
                        emailAddress: {
                            message: 'The value is not a valid email address'
                        }
                    }
                },
                name: {
                    validators: {
                        regexp: {
                            regexp: /^[A-Za-z\s]{1,}[\.]{0,1}[A-Za-z\s]{0,}$/,
                            message: 'Not a valid name'
                       }
                    }
                },
                message: {
                    validators: {
                            regexp: {
                                regexp: /^[A-Za-z\s\.\d,!?"]{1,}$/,
                                message: 'Special characters are not allowed'
                            }
                    }
               }
            }
        });
    }

    var feedbackController = {
        open: function() {
            open();
        }
    };

    researchHelper.feedbackController = feedbackController;
    open();

    return feedbackController;
});


//# sourceURL=feedbackController.js