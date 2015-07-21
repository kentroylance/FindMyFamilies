define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');

    // models
    var findPersonOptions = require('findPersonOptions');
    var person = require('person');

    function loadEvents() {

        $("#findPersonOptionSaveButton").unbind('click').bind('click', function (e) {
            person.findPersonOptions[0] = $("#findPersonOptions1").val();
            person.findPersonOptions[1] = $("#findPersonOptions2").val();
            person.findPersonOptions[2] = $("#findPersonOptions3").val();
            person.findPersonOptions[3] = $("#findPersonOptions4").val();
            person.findPersonOptions[4] = $("#findPersonOptions5").val();
            person.save();
            findPersonOptions.form.dialog(constants.CLOSE);
        });

        $("#findPersonOptionCloseButton").unbind('click').bind('click', function (e) {
            findPersonOptions.form.dialog(constants.CLOSE);
        });
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

        select1.val(person.findPersonOptions[0]);
        select2.val(person.findPersonOptions[1]);
        select3.val(person.findPersonOptions[2]);
        select4.val(person.findPersonOptions[3]);
        select5.val(person.findPersonOptions[4]);
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

    findPersonHelper.findPersonOptionsController = findPersonOptionsController;
    open();

    return findPersonOptionsController;
});

//# sourceURL=findPersonOptionsController.js