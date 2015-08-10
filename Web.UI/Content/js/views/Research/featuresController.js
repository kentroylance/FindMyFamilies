define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var lazyRequire = require("lazyRequire");
    var requireOnce = lazyRequire.once();


    // models
    var person = require('person');
    var features = require('features');

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
        
        $('#featuresSubmitButton').find('span').text(features.tryItNowButton);
    }

    function clear() {
        person.clear();
    }

    function reset() {
        person.reset();
    }

    function loadEvents() {

        $("#featuresCloseButton").unbind('click').bind('click', function(e) {
            features.form.dialog(constants.CLOSE);
        });

        $("#featuresResetButton").unbind('click').bind('click', function(e) {
            reset();
        });

        $("#featuresSubmitButton").unbind('click').bind('click', function (e) {
            var lastCalled = features.featureName;
//            switch (features.featureName) {
//                case constants.STARTING_POINT:
//                    lastCalled = constants.STARTING_POINT;
//                    break;
//                case constants.FIND_PERSON:
//                    lastCalled = constants.FIND_PERSON;
//                    break;
//                case constants.HINTS:
//                    lastCalled = constants.HINTS;
//                    break;
//                case constants.INCOMPLETE_ORDINANCES:
//                    lastCalled = constants.INCOMPLETE_ORDINANCES;
//                    break;
//                default:
//                    break;
//            }

            if (lastCalled) {
                system.setCookie(constants.LAST_CALLED, lastCalled, 1);
            }
            window.location.href = constants.RESEARCH_URL;

            return false;
        });

        $("#featuresEmail").change(function(e) {
            retrieve.email = $("#featuresEmail").val();
        });

        $("#featuresMessage").change(function(e) {
            retrieve.email = $("#featuresMessage").val();
        });

        $("#featuresBug").change(function(e) {
            features.bug = $("#featuresBug").prop("checked");
            if (features.bug) {
                features.count = false;
            } else {
                features.count = true;
            }
        });
        $("#featuresFeatureRequest").change(function(e) {
            features.featureRequest = $('#featuresFeatureRequest').prop("checked");
            if (features.count) {
                features.bug = false;
            } else {
                features.bug = true;
            }
        });
        $("#featuresOther").change(function(e) {
            features.other = $('#featuresOther').prop("checked");
            if (features.count) {
                features.bug = false;
            } else {
                features.bug = true;
            }
        });


        $("#featuresCancelButton").unbind('click').bind('click', function(e) {
            features.form.dialog(constants.CLOSE);
        });

        features.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (features.callerSpinner) {
                system.spinnerArea = features.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            features.save();
        });
    }

    var featuresController = {
        open: function() {
            open();
        }
    };

    features.featuresController = featuresController;
    open();

    return featuresController;
});


//# sourceURL=featuresController.js