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
            if (feedback.topScore) {
                $("#feedbackTopScore").prop('checked', feedback.topScore);
            }
            if (feedback.count) {
                $("#feedbackCount").prop('checked', feedback.count);
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

        $("#feedbackCloseButton").unbind('click').bind('click', function (e) {
            feedback.form.dialog(constants.CLOSE);
        });

        $("#feedbackResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#feedbackSubmitButton").unbind('click').bind('click', function (e) {
            return false;
        });

    $("#feedbackTopScore").change(function (e) {
        feedback.topScore = $("#feedbackTopScore").prop("checked");
        if (feedback.topScore) {
            feedback.count = false;
        } else {
            feedback.count = true;
        }
    });
    $("#feedbackCount").change(function (e) {
        feedback.count = $('#feedbackCount').prop("checked");
        if (feedback.count) {
            feedback.topScore = false;
        } else {
            feedback.topScore = true;
        }
    });


        $("#feedbackCancelButton").unbind('click').bind('click', function (e) {
            feedback.form.dialog(constants.CLOSE);
        });

        feedback.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            feedback.save();
        });
    }

    var feedbackController = {
        open: function () {
            open();
        }
    };

    researchHelper.feedbackController = feedbackController;
    open();

    return feedbackController;
});


//# sourceURL=feedbackController.js