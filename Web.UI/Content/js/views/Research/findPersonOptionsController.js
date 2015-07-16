define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var findPersonHelper = require('findPersonHelper');

    // models
    var findPersonOptions = require('findPersonOptions');
    var person = require('person');
    var findPerson = require('findPerson');
    var findPersonController = require('findPersonController');

    function loadEvents() {

        $("#findPersonOptionSaveButton").unbind('click').bind('click', function (e) {
            person.save();
            findPersonOptions.form.dialog(constants.CLOSE);
        });

        $("#findPersonOptionCloseButton").unbind('click').bind('click', function (e) {
            findPersonOptions.form.dialog(constants.CLOSE);
        });
    }

    function open() {
        if (system.target) {
            findPersonOptions.callerSpinner = system.target.id;
        }

        findPersonOptions.form = $("#findPersonOptionsForm");
        loadEvents();
        loadOptions();
        system.openForm(findPersonOptions.form, findPersonOptions.formTitleImage, findPersonOptions.spinner, $('#findPersonOptions1'));
    }

    function loadOptions() {
        var select1 = $("#findPersonOptions1");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            select1.append(optionhtml);
        });

        var select2 = $("#findPersonOptions2");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            select2.append(optionhtml);
        });

        var select3 = $("#findPersonOptions3");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            select3.append(optionhtml);
        });

        var select4 = $("#findPersonOptions4");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            select4.append(optionhtml);
        });

        var select5 = $("#findPersonOptions5");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '" selected>' + value + '</option>';
            select5.append(optionhtml);
        });

        select1.val(Object.keys(findPersonHelper.findUrls)[0]);
        select2.val(Object.keys(findPersonHelper.findUrls)[1]);
        select3.val(Object.keys(findPersonHelper.findUrls)[2]);
        select4.val(Object.keys(findPersonHelper.findUrls)[3]);
        select5.val(Object.keys(findPersonHelper.findUrls)[4]);


    }

    function close() {
        system.initSpinner(findPersonOptions.callerSpinner, true);
    }

    var findPersonOptionsController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    findPerson.findPersonOptionsController = findPersonOptionsController;
    open();

    return findPersonOptionsController;
});

//# sourceURL=findPersonOptionsController.js