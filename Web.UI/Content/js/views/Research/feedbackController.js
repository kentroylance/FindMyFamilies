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

        $("#feedbackResetButton").unbind('click').bind('click', function(e) {
            reset();
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
                success: function(data) {
                    msgBox.message(data);
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