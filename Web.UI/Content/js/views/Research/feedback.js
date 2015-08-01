define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');

    var _formName = "feedbackForm";
    var _formTitleImage = "fa fmf-feedback24";
    var _form = $("#feedbackForm");
    var _spinner = "feedbackSpinner";

    var _bug = true;
    var _featureRequest = false;
    var _other = false;

    function FeedbackDO(bug, featureRequest, other) {
        this.bug = bug;
        this.featureRequest = featureRequest;
        this.other = other;
    }

    function save() {
        if (window.localStorage) {
            var feedbackDO = new FeedbackDO(_bug, _featureRequest, _other);
            localStorage.setItem(constants.feedback, JSON.stringify(feedbackDO));
        }
    }

    if (window.localStorage) {
        var feedbackDO = JSON.parse(localStorage.getItem(constants.feedback));
        if (!feedbackDO) {
            feedbackDO = new FeedbackDO();
        }
        _bug = feedbackDO.bug;
        _featureRequest = feedbackDO.featureRequest;
        _other = feedbackDO.other;
    }

    var feedback = {
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

    return feedback;
});

//# sourceURL=feedback.js