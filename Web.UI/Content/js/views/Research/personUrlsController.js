define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var findPersonHelper = require('findPersonHelper');

    // models
    var personUrls = require('personUrls');

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

        $(".personUrlsAction").unbind('click').bind('click', function(e) {
            var dropdown = $(this);
            var dropdownRow = new row(dropdown.data('id'), dropdown.data('firstname'), dropdown.data('middlename'), dropdown.data('lastname'), dropdown.data('fullname'), dropdown.data('gender'), dropdown.data('birthyear'), dropdown.data('deathyear'), dropdown.data('birthplace'));
            if (dropdown.children().length <= 1) {
                dropdown.append(findPersonHelper.getMenuOptions(dropdownRow));
            }
        });


        $("#personUrlsCloseButton").unbind('click').bind('click', function(e) {
            personUrls.form.dialog(constants.CLOSE);
            return false;
        });


        personUrls.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function(e) {
            system.initSpinner(personUrls.callerSpinner, true);
            $(".personAction1").unbind("mouseenter").unbind("mouseleave");
            return false;
        });
    }

    function updateForm() {
    }

    function open() {
        if (system.target) {
            personUrls.callerSpinner = system.target.id;
        }

        personUrls.form = $("#personUrlsForm");
        loadEvents();
        updateForm();
        system.openForm(personUrls.form, personUrls.formTitleImage, personUrls.spinner);
    }

    var personUrlsController = {
        open: function() {
            open();
        }
    };

    function mouseOverTrigger() {
        $('#personInfoDiv').empty();
        var mouseOver = $(this);
        var id = mouseOver.data('id');
        var row = $('#' + id);
        $('#personUrlsId').val(row.data('id'));
        $('#personUrlsFullName').val(row.data('fullname'));
        $('#personUrlsBirthDate').val(row.data('birthyear'));
        $('#personUrlsBirthPlace').val(row.data('birthplace'));
        $('#personUrlsDeathDate').val(row.data('deathyear'));
        $('#personUrlsDeathPlace').val(row.data('deathplace'));
        $('#personUrlsSpouse').val(row.data('spousename'));
        $('#personUrlsMother').val(row.data('mothername'));
        $('#personUrlsFather').val(row.data('fathername'));
    }


    function mouseOutTrigger() {
        //            var object = $(this);
    }


    researchHelper.personUrlsController = personUrlsController;
    open();

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

    return personUrlsController;
});

//# sourceURL=personUrlsController.js