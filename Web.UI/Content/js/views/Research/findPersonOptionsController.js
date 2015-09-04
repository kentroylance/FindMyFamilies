define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');
    var msgBox = require('msgBox');

    // models
    var findPersonOptions = require('findPersonOptions');
    var person = require('person');

    function loadEvents() {

        $("#findPersonOptionSaveButton").unbind('click').bind('click', function (e) {
            person.findPersonOptions = [];

            var selected = $("#selected li");
            selected.each(function (li) {
                person.findPersonOptions.push($(this).attr('id'));
            });
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
        var available = $("#available");
        var selected = $("#selected");
        var findUrls = findPersonHelper.findUrls;
        var findPersonOptions = person.findPersonOptions;
        $.each(findPersonOptions, function (id, value) {
            selected.append("<li class=\"list-group-item\" id=\"" + value + "\"><span class=\"" + findPersonHelper.getIconForMenuOptions(value) + "\"></span> " + findUrls[value] + "</li>");
        });
        $.each(findUrls, function (id, value) {
            if ($.inArray(id, findPersonOptions) === -1) {
                available.append("<li class=\"list-group-item\" id=\"" + id + "\"><span class=\"" + findPersonHelper.getIconForMenuOptions(id) + "\"></span> " + value + "</li>");
            }
        });

        $("ul.list-group").sortable({
            connectWith: "ul"
        });

        $("#selected").on("sortreceive", function (event, ui) {
            if ($("#selected li").length > 7) {
                msgBox.warning("Sorry, no more than seven options can be selected.  To add an available option to selected options, you must first remove a selected option.");
                $(ui.sender).sortable('cancel');
            }
        });

        $("#available").sortable({
            stop: function(e, ui) {
                var sortedIDs = $("#available").sortable("toArray");
            }
        });

        $("#available, #selected").disableSelection();
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