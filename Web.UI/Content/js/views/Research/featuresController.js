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
    var features = require('features');

    var retrieve = require('retrieve');

        function updateForm() {
            if (features.bug) {
                $("#featuresBug").prop('checked', features.bug);
            }
            if (features.featureRequest) {
                $("#featuresFeatureRequest").prop('checked', features.featureRequest);
            }
	    if (features.other) {
                $("#featuresOther").prop('checked', features.other);
            }
    }

    function open() {
        features.form = $("#featuresForm");
        loadEvents();
        updateForm();
        system.openForm(features.form, features.formTitleImage, features.spinner);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
    }

    function loadEvents() {

        $("#featuresCloseButton").unbind('click').bind('click', function (e) {
            features.form.dialog(constants.CLOSE);
        });

        $("#featuresResetButton").unbind('click').bind('click', function (e) {
            reset();
        });

        $("#featuresSubmitButton").unbind('click').bind('click', function (e) {
            return false;
        });

        $("#featuresEmail").change(function (e) {
            retrieve.email = $("#featuresEmail").val();
        });

        $("#featuresMessage").change(function (e) {
            retrieve.email = $("#featuresMessage").val();
        });

    $("#featuresBug").change(function (e) {
        features.bug = $("#featuresBug").prop("checked");
        if (features.bug) {
            features.count = false;
        } else {
            features.count = true;
        }
    });
    $("#featuresFeatureRequest").change(function (e) {
        features.featureRequest = $('#featuresFeatureRequest').prop("checked");
        if (features.count) {
            features.bug = false;
        } else {
            features.bug = true;
        }
    });
    $("#featuresOther").change(function (e) {
        features.other = $('#featuresOther').prop("checked");
        if (features.count) {
            features.bug = false;
        } else {
            features.bug = true;
        }
    });


        $("#featuresCancelButton").unbind('click').bind('click', function (e) {
            features.form.dialog(constants.CLOSE);
        });

        features.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            features.save();
        });
    }

    var featuresController = {
        open: function () {
            open();
        }
    };

    researchHelper.featuresController = featuresController;
    open();

    return featuresController;
});


//# sourceURL=featuresController.js