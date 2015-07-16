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

        select1.val('fmf-urls');
        select2.val('ancestry');
        select3.val('findagrave');
        select4.val('myheritage');
        select5.val('amerancest');

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