﻿define(function (require) {

    var _formName = "personUrlsForm";
    var _formTitleImage = "fmf-family24";
    var _form = $("#personUrlsForm");
    var _spinner = "personUrlsSpinner";
    var _callerSpinner;

    var _id;
    var _name;


    var personUrls = {
        formName: _formName,
        formTitleImage: _formTitleImage,
        spinner: _spinner,
        get form() {
            return _form;
        },
        set form(value) {
            _form = value;
        },
        get callerSpinner() {
            return _callerSpinner;
        },
        set callerSpinner(value) {
            _callerSpinner = value;
        },
        get id() {
            return _id;
        },
        set id(value) {
            _id = value;
        },
        get name() {
            return _name;
        },
        set name(value) {
            _name = value;
        }


    };

    return personUrls;
});

//# sourceURL=personUrls.js