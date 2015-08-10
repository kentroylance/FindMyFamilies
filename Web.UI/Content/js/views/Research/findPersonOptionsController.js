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
            person.findPersonOptions[5] = $("#findPersonOptions6").val();
            person.findPersonOptions[6] = $("#findPersonOptions7").val();
            person.save();
            findPersonOptions.form.dialog(constants.CLOSE);
        });

        $("#findPersonOptionCloseButton").unbind('click').bind('click', function (e) {
            findPersonOptions.form.dialog(constants.CLOSE);
        });

        findPersonOptions.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            if (findPersonOptions.callerSpinner) {
                system.spinnerArea = findPersonOptions.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
        });


    }

    function loadOptions() {
        var select1 = $("#findPersonOptions1");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select1.append(optionhtml);
        });

        var select2 = $("#findPersonOptions2");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select2.append(optionhtml);
        });

        var select3 = $("#findPersonOptions3");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select3.append(optionhtml);
        });

        var select4 = $("#findPersonOptions4");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select4.append(optionhtml);
        });

        var select5 = $("#findPersonOptions5");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select5.append(optionhtml);
        });

        var select6 = $("#findPersonOptions6");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select6.append(optionhtml);
        });

        var select7 = $("#findPersonOptions7");
        $.each(findPersonHelper.findUrls, function (i, value) {
            var optionhtml = '<option value="' + i + '">' + value + '</option>';
            select7.append(optionhtml);
        });

        select1.val(person.findPersonOptions[0]);
        select2.val(person.findPersonOptions[1]);
        select3.val(person.findPersonOptions[2]);
        select4.val(person.findPersonOptions[3]);
        select5.val(person.findPersonOptions[4]);
        select6.val(person.findPersonOptions[5]);
        select7.val(person.findPersonOptions[6]);
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
        if (findPersonOptions.callerSpinner) {
                system.spinnerArea = findPersonOptions.callerSpinner;
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
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