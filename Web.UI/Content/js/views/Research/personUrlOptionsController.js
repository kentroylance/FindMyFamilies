define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var researchHelper = require("researchHelper");

    // models
    var personUrlOptions = require('personUrlOptions');
    var person = require('person');

    function loadEvents() {

        $("#personUrlOptionsMaidenName").change(function (e) {
            person.includeMaidenName = $("#personUrlOptionsMaidenName").prop("checked");
            return false;
        });

        $("#personUrlOptionsMiddleName").change(function (e) {
            person.includeMiddleName = $("#personUrlOptionsMiddleName").prop("checked");
            return false;
        });

        $("#personUrlOptionsPlace").change(function (e) {
            person.includePlace = $("#personUrlOptionsPlace").prop("checked");
            return false;
        });

        $("#personUrlOptionsYearRange").change(function (e) {
            person.yearRange = $("#personUrlOptionsYearRange").val();
            return false;
        });

        $("#personUrlOptionsOkButton").unbind('click').bind('click', function (e) {
            person.save();
            researchHelper.displayPersonUrls();
            return false;
        });

        $("#personUrlOptionsCloseButton").unbind('click').bind('click', function (e) {
            personUrlOptions.form.dialog(constants.CLOSE);
            return false;
        });

        personUrlOptions.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.initSpinner(personUrlOptions.callerSpinner, true);
            person.save();
            return false;
        });
    }

    function updateForm() {
        if (person.generation) {
            $("#personUrlOptionsGeneration").val(person.generation);
        }
        if (person.includeMaidenName) {
            $('#personUrlOptionsMaidenName').prop('checked', person.includeMaidenName);
        }
        if (person.includeMiddleName) {
            $('#personUrlOptionsMiddleName').prop('checked', person.includeMiddleName);
        }
        if (person.includePlace) {
            $('#personUrlOptionsPlace').prop('checked', person.includePlace);
        }
        if (person.yearRange) {
            $("#personUrlOptionsYearRange").val(person.yearRange);
        }
    }

    function open() {
        if (system.target) {
            personUrlOptions.callerSpinner = system.target.id;
        }

        personUrlOptions.form = $("#personUrlOptionsForm");
        loadEvents();
        updateForm();
        system.openForm(personUrlOptions.form, personUrlOptions.formTitleImage, personUrlOptions.spinner, $('#personUrlOptionsGeneration'));
    }

    var personUrlOptionsController = {
        open: function() {
            open();
        }
    };

    researchHelper.personUrlOptionsController = personUrlOptionsController;
    open();

    return personUrlOptionsController;
});

//# sourceURL=personUrlOptionsController.js