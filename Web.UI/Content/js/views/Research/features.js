define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var _formName = "featuresForm";
    var _formTitleImage = "fa fmf-features24";
    var _form = $("#featuresForm");
    var _spinner = "featuresSpinner";

    var _bug = true;
    var _featureRequest = false;
    var _other = false;

    var _email;
    var _message;

    function FeaturesDO(bug, featureRequest, other) {
        this.bug = bug;
        this.featureRequest = featureRequest;
        this.other = other;
    }

    function save() {
        if (window.localStorage) {
            var featuresDO = new FeaturesDO(_bug, _featureRequest, _other);
            localStorage.setItem(constants.features, JSON.stringify(featuresDO));
        }
    }

    if (window.localStorage) {
        var featuresDO = JSON.parse(localStorage.getItem(constants.features));
        if (!featuresDO) {
            featuresDO = new FeaturesDO();
        }
        _bug = featuresDO.bug;
        _featureRequest = featuresDO.featureRequest;
        _other = featuresDO.other;
    }

    var features = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        get other() {
            return _other;
        },
        set other(value) {
            _other = value;
        },
        get bug() {
            return _bug;
        },
        set bug(value) {
            _bug = value;
        },
        get featureRequest() {
            return _featureRequest;
        },
        set featureRequest(value) {
            _featureRequest = value;
        },
        save: function() {
            save();
        },
        clear: function() {
            clear();
        },
        reset: function() {
            reset();
        }
    };

    return features;
});

//# sourceURL=features.js