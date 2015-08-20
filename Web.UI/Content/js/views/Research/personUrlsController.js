define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');

    // models
    var personUrls = require('personUrls');
    var person = require('person');

    function row(id, firstName, middleName, lastName, fullName, gender, birthYear, deathYear, birthPlace) {
        this.id = id;
        this.firstName = firstName;
        this.middleName = middleName;
        this.lastName = lastName;
        this.fullName = fullName;
        this.gender = gender;
        this.birthYear = birthYear;
        this.deathYear = deathYear;
        this.birthPlace = birthPlace;
    }


//    public string id { get; set; }
//    public string firstName { get; set; }
//    public string middleName { get; set; }
//    public string lastName { get; set; }
//    public string fullName { get; set; }
//    public string gender { get; set; }
//    public string birthYear { get; set; }
//    public string deathYear { get; set; }
//    public string birthPlace { get; set; }
//    public string deathPlace { get; set; }
//    public string state { get; set; }
//    public string motherName { get; set; }
//    public string fatherName { get; set; }
//    public string spouseName { get; set; }
//    public string spouseGender { get; set; }


    function loadEvents() {

        $("#personUrlsMaidenName").change(function (e) {
            person.includeMaidenName = $("#personUrlsMaidenName").prop("checked");
            return false;
        });

        $("#personUrlsMiddleName").change(function (e) {
            person.includeMiddleName = $("#personUrlsMiddleName").prop("checked");
            return false;
        });

        $("#personUrlsPlace").change(function (e) {
            person.includePlace = $("#personUrlsPlace").prop("checked");
            return false;
        });

        $("#personUrlsYearRange").change(function (e) {
            person.yearRange = $("#personUrlsYearRange").val();
            return false;
        });

        $("#personUrlsOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, findPerson);
        });

        $(".personUrlsAction").unbind('click').bind('click', function(e) {
            var dropdown = $(this);
            var dropdownRow = new row(dropdown.data('id'), dropdown.data('firstname'), dropdown.data('middlename'), dropdown.data('lastname'), dropdown.data('fullname'), dropdown.data('gender'), dropdown.data('birthyear'), dropdown.data('deathyear'), dropdown.data('birthplace'));
            dropdown.append(findPersonHelper.getMenuOptions(dropdownRow));
        });


        $("#personUrlsCloseButton").unbind('click').bind('click', function(e) {
            personUrls.form.dialog(constants.CLOSE);
            return false;
        });


        personUrls.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            if (personUrls.callerSpinner) {
                system.spinnerArea = personUrls.callerSpinner; 
            } else {
                system.spinnerArea = constants.DEFAULT_SPINNER_AREA;
            }
            $(".personAction1").unbind("mouseenter").unbind("mouseleave");
            return false;
        });
    }

    function updateForm() {
        if (person.generation) {
            $("#personUrlsGeneration").val(person.generation);
        }
        if (person.includeMaidenName) {
            $('#personUrlsMaidenName').prop('checked', person.includeMaidenName);
        }
        if (person.includeMiddleName) {
            $('#personUrlsMiddleName').prop('checked', person.includeMiddleName);
        }
        if (person.includePlace) {
            $('#personUrlsPlace').prop('checked', person.includePlace);
        }
        if (person.yearRange) {
            $("#personUrlsYearRange").val(person.yearRange);
        }
    }

    function mouseOverTrigger() {
        $('#personUrlsDiv').empty();
        var mouseOver = $(this);
        var id = mouseOver.data('id');
        var row = $('#' + id);
        $('#personUrlsId').val(row.data('id'));
        $('#personUrlsFullName').val(row.data('fullname'));
        $('#personUrlsBirthDate').val(row.data('birthdate'));
        $('#personUrlsBirthPlace').val(row.data('birthplace'));
        $('#personUrlsDeathDate').val(row.data('deathdate'));
        $('#personUrlsDeathPlace').val(row.data('deathplace'));
        $('#personUrlsSpouse').val(row.data('spousename'));
        $('#personUrlsMother').val(row.data('mothername'));
        $('#personUrlsFather').val(row.data('fathername'));
    }

    function mouseOutTrigger() {
        //            var object = $(this);
    }

    function open() {
        if (system.target) {
            personUrls.callerSpinner = system.target.id;
        }

        personUrls.form = $("#personUrlsForm");
        loadEvents();
        updateForm();
        system.openForm(personUrls.form, personUrls.formTitleImage, personUrls.spinner);
        $(document).ready(function () {
            var hoverIntentConfig = {
                sensitivity: 1,
                interval: 100,
                timeout: 300,
                over: mouseOverTrigger,
                out: mouseOutTrigger
            }

            $(".personAction1").hoverIntent(hoverIntentConfig);
        });

    }

    var personUrlsController = {
        open: function() {
            open();
        }
    };

    researchHelper.personUrlsController = personUrlsController;
    open();


    return personUrlsController;
});

//# sourceURL=personUrlsController.js