define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');

    // models
    var findPerson = require('findPerson');
    var person = require('person');
    var research = require('research');
    var researchController = require('researchController');

//        system.requireQueue([
//                'css!/Content/css/vendor/formValidation.min.css',
//                'formValidation',
//                'bootstrapValidation'
//        ], function () {
//           
//        });

//        require("css!/Content/css/vendor/formValidation.min.css");
//        var formValidation = require("formValidation");
//        var bootstrapValidation = require("bootstrapValidation");


    function loadEvents() {

        $("#clearButton").unbind('click').bind('click', function(e) {
            $("#personId").val("");
            $("#firstName").val("");
            $("#lastName").val("");
            $("#gender").val("");
            $("#birthYear").val("");
            $("#deathYear").val("");
            $('#firstName').focus();
        });

        $("#previousButton").unbind('click').bind('click', function (e) {
            $("#personId").val(findPerson.personId);
            $("#firstName").val(findPerson.firstName);
            $("#lastName").val(findPerson.lastName);
            $("#gender").val(findPerson.gender);
            $("#birthYear").val(findPerson.birthYear);
            $("#deathYear").val(findPerson.deathYear);

            // Revalidate the fields
            validateRow1(findPerson.form.data('formValidation'));
            findPerson.form
                .formValidation('revalidateField', 'gender')
                .formValidation('revalidateField', 'birthYear')
                .formValidation('revalidateField', 'deathYear');

            $('#submit').focus();
        });


        $("#findPersonCloseButton").unbind('click').bind('click', function(e) {
            findPerson.form.dialog(constants.CLOSE);
        });


        function getMiddleNameQuote(middleName) {
            var result = "";
            if (!string(middleName).isEmpty() && person.includeMiddleName) {
                result = "%22";
            }
            return result;
        }
        
        function getMiddleName(middleName, website) {
            var result = "";
            if (!string(middleName).isEmpty() && !string(website).isEmpty() && person.includeMiddleName) {
                if (website === constants.ANCESTRY) {
                    result = "%20" + middleName;
                } else if (website === constants.FIND_A_GRAVE) {
                    result = "&GSmn=" + middleName;
                } else if (website === constants.BILLION_GRAVES) {
                    result = "+" + middleName;
                } else if (website === constants.MY_HERITAGE) {
                    result = "%2F3" + middleName;
                } else if (website === constants.FIND_MY_PAST) {
                    result = "%20" + middleName;
                } else if (website === system.familySearchSystem()) {
                    result = "%20" + middleName;
                } else if (website === constants.GOOGLE) {
                    result = "+" + middleName;
                }
            }

            return result;
        }


        function getPlace(birthPlace, webSite) {
            var result = "";
            var birthPlaceParts;

            if (!string(birthPlace).isEmpty() && !string(webSite).isEmpty() && person.includePlace) {
                var isUnitedStates = false;
                    if (webSite === constants.ANCESTRY) {
                        result = "&msbpn__ftp="; //&msbpn__ftp=Othello%2C+Adams%2C+Washington%2C+USA
                        birthPlaceParts = birthPlace.split(',');

                        $.each(birthPlaceParts, function (key, value) {
                            if (value === " United States") {
                                result += "United%20States";
                                isUnitedStates = true;
                            } else {
                                result += string(value).trim() + "%2C";
                            }
                        });
                        if (isUnitedStates === false) {
                            result = result.substring(0, result.length - 3);
                        }
                    } else if (webSite === constants.FIND_A_GRAVE) {
                        result = birthPlace;
                    } else if (webSite === constants.BILLION_GRAVES) {
                        result = birthPlace;
                    } else if (webSite === constants.MY_HERITAGE) {
                        result = "&qany%2F1event=Event+et.any+ep.";
                        birthPlaceParts = birthPlace.split(',');
                        $.each(birthPlaceParts, function (key, value) {
                            if (value === " United States") {
                                result += "United%2F3States";
                                isUnitedStates = true;
                            } else {
                                result += string(value).trim() + "%2C%2F3";
                            }
                        });
                        if (isUnitedStates === false) {
                            result = result.substring(0, result.length - 7);
                        }
//                        int unitedStatesPos = place.ToLower().IndexOf("united");
//                        if ((unitedStatesPos > -1) && (place.ToLower().IndexOf("states") > -1)) {
//                            place = place.Substring(0, unitedStatesPos);
//                            place += "United%2F3States";
//                        } else {
//                            place = place.Substring(0, place.Length - 7);
//                        }
                    } else if (webSite === constants.FIND_MY_PAST) {
                        result = birthPlace;
                    } else if (webSite === system.familySearchSystem()) {
                        birthPlaceParts = birthPlace.split(',');
                        result = "~%20%2Bbirth_place%3A%22";
                        $.each(birthPlaceParts, function (key, value) {
                            result += string(value).trim() + "%2C%20";
                        });
                        result = result.substring(0, result.length - 6) + "%22";
                    } else if (webSite === constants.GOOGLE) {
                        result = birthPlace;
                    }
            }
            return result;
        }

        function getBirthYear(birthYear, webSite) {
            var result = "";
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=in&GSby=1874&GSdyrel=in&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n

            if (!string(birthYear).isEmpty() && string(birthYear).toInt() > 100) {
                if (webSite === constants.ANCESTRY) {
                    result = "&MSAV=1&msbdy=" + birthYear;
                } else if (webSite === constants.FIND_A_GRAVE) {
                    if (person.yearRange === 0) {
                        result = "&GSbyrel=in&GSby=" + birthYear;
                    } else {
                        result = "&GSbyrel=after&GSby=" + (string(birthYear).toInt() - person.yearRange - 1);
                    }
                } else if (webSite === constants.BILLION_GRAVES) {
                    result = "&birth_year=" + birthYear;
                } else if (webSite === constants.MY_HERITAGE) {
                    result = "&qbirth=Event+et.birth+ey." + birthYear;
                } else if (webSite === constants.FIND_MY_PAST) {
                    result = "&yearofbirth=" + birthYear + "&yearofbirth_offset=" + person.yearRange;
                } else if (webSite === system.familySearchSystem()) {
                    result = "~%20%2Bbirth_year%3A" + (string(birthYear).toInt() - person.yearRange) + "-" + (string(birthYear).toInt() + person.yearRange);
                } else if (webSite === constants.GOOGLE) {
                    result = birthYear + "";
                }
            }
            return result;
        }


        function getDeathYear(deathYear, webSite) {
            var result = "";

            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=in&GSby=1874&GSdyrel=in&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
            //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
 
            if (!string(deathYear).isEmpty() && string(deathYear).toInt() > 100) {
                if (webSite === constants.ANCESTRY) {
                    result = "&msddy=" + deathYear;
                } else if (webSite === constants.FIND_A_GRAVE) {
                    if (person.yearRange == 0) {
                        result = "&GSdyrel=in&GSdy=" + deathYear;
                    } else {
                        result = "&GSdyrel=before&GSdy=" + (string(deathYear).toInt() + person.yearRange + 1);
                    }
                } else if (webSite === constants.BILLION_GRAVES) {
                    result = "&death_year=" + (deathYear);
                } else if (webSite === constants.MY_HERITAGE) {
                } else if (webSite === constants.FIND_MY_PAST) {
                } else if (webSite === system.familySearchSystem()) {
                    result = "~%20%2Bdeath_year%3A" + +(string(deathYear).toInt() - person.yearRange) + "-" + (string(deathYear).toInt() + person.yearRange);
                } else if (webSite === constants.GOOGLE) {
                    result = "" + deathYear;
                }
            }

            return result;

        }

        function getLastName(lastName) {
//            if (lastname && !person.IsEmpty && person.IsFemale && personInfo.IncludeMaidenName) {
//                if (!person.Father.IsEmpty && !string.IsNullOrEmpty(person.Father.Lastname)) {
//                    lastname = person.Father.Lastname;
//                } if (!person.Mother.IsEmpty && !string.IsNullOrEmpty(person.Mother.Lastname)) {
//                    lastname = person.Mother.Lastname;
//                }
//            }
            return lastName;
        }


        var findUrls = {};
        findUrls['fmf-urls'] = 'Family Research Urls';
        findUrls['ancestry'] = 'Ancestry';
        findUrls['puz-descend'] = 'Puzilla Descendants';
        findUrls['puz-ancest'] = 'Puzilla Ancestors';
        findUrls['findagrave'] = 'Find-A-Grave';
        findUrls['billgrave'] = 'Billion Graves';
        findUrls['findmypast'] = 'Find My Past';
        findUrls['myheritage'] = 'My Heritage';
        findUrls['amerancest'] = 'American Ancestors';
        findUrls['fmf-retrieve'] = 'Retrieve';
        findUrls['fmf-starting'] = 'Starting Point';
        findUrls['fs-fan'] = 'Family Search - Fan Chart';
        findUrls['fs-desc'] = 'Family Search - Descendancy';
        findUrls['fs-tree'] = 'Family Search - Tree';
        findUrls['fs-search'] = 'Family Search - Search';
        findUrls['fs-person'] = 'Family Search - Person';
        findUrls['google'] = 'Google Search';

//        public string id { get; set; }
//        public string firstName { get; set; }
//        public string middleName { get; set; }
//        public string lastName { get; set; }
//        public string fullName { get; set; }
//        public string gender { get; set; }
//        public string birthYear { get; set; }
//        public string deathYear { get; set; }
//        public string birthPlace { get; set; }
//        public string deathPlace { get; set; }
//        public string state { get; set; }
//        public string motherName { get; set; }
//        public string fatherName { get; set; }
//        public string spouseName { get; set; }
//        public string spouseGender { get; set; }

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                if ($( this).children().length <= 1) {
                    var menuOptions = "";
                    person.includeMiddleName = true;
                    person.includePlace = true;

                    menuOptions = menuOptions + "<ul class=\"dropdown-menu\" role=\"menu\" aria-labelledby=\"dLabel\" >";
                    $.each(findUrls, function (key, value) {
                        switch (key) {
                            case 'fmf-urls':
                                menuOptions += "<li><a onclick=\"researchController.personUrlOptions('" + row.id + "');\" href=\"javascript:void(0);\"><span class=\"fa fmf-family16\"></span> Family Research Urls</a></li>";
                                break;
                            case 'ancestry':
                                menuOptions += "<li><a href=\"" + constants.ANCESTRY + "&gsfn=" + row.firstName + getMiddleName(row.middleName, constants.ANCESTRY) + "&gsln=" + getLastName(row.lastName) + ((person.yearRange === 0) ? "&msbdy_x=1" : "&msbdy_x=1&msbdp=" + person.yearRange) + getBirthYear(row.birthYear, constants.ANCESTRY) + getPlace(row.birthPlace, constants.ANCESTRY) + getDeathYear(row.deathYear, constants.ANCESTRY) + ((person.yearRange === 0) ? "&msddy_x=1" : "&msddy_x=1&msddp=" + person.yearRange) + "&_83004003-n_xcl=" + ((row.gender === "Male") ? "f" : "m") + "&cp=0&catbucket=rstp&uidh=000\" target=\" _tab\" ><span class=\"fa fmf-ancestry16\"></span> Ancestry</a></li>";
                                break;
                            case 'findagrave':
                                menuOptions += "<li><a href=\"" + constants.FIND_A_GRAVE + "&GSfn=" + row.firstName + getMiddleName(row.middleName, constants.FIND_A_GRAVE) + "&GSln=" + getLastName(row.lastName) + getBirthYear(row.birthYear, constants.FIND_A_GRAVE) + getDeathYear(row.deathYear, constants.FIND_A_GRAVE) + "&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n\" target=\" _tab\" ><span class=\"fa fmf-findagrave16\"></span> Find-A-Grave</a></li>";
                                break;
                            case 'puz-descend':
                                menuOptions += "<li><a href=\"https://puzzilla.org/descendants?id=" + row.id + "&changeToId=" + row.id + "&depth=6&ancestorsView=false\" target=\" _tab\" ><span class=\"fa fmf-puzilla16\"></span> Puzilla - Descendants</a></li>";
                                break;
                            case 'puz-ancest':
                                menuOptions += "<li><a href=\"https://puzzilla.org/descendants?id=" + row.id + "&changeToId=" + row.id + "&depth=6&ancestorsView=true\" target=\" _tab\" ><span class=\"fa fmf-puzilla16\"></span> Puzilla - Ancestors</a></li>";
                                break;
                            case 'fs-person':
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=ancestor&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Person</a></li>";
                                break;
                            case 'fs-tree':
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=pedigree&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Tree</a></li>";
                                break;
                            case 'fs-desc':
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=descendancy&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Descendancy</a></li>";
                                break;
                            case 'fs-fan':
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=fan&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Fan</a></li>";
                                break;
                            case 'fs-person':
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=person&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Person</a></li>";
                                break;
                            case 'fs-search':
                                // familySearchUrl = Constants.FAMILY_SEARCH_SYSTEM + "/search/record/results?count=20&query=%2Bgivenname%3A" + MiddleNameQuote(person, personInfo) + person.Firstname + getMiddlename(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + MiddleNameQuote(person, personInfo) + "~%20%2Bsurname%3A" + getLastname(person, personInfo) + getPlace(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + getBirthYear(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + getDeathYear(person, Constants.FAMILY_SEARCH_SYSTEM, personInfo) + "~&treeref=" + person.Id;

//                                var url = "<li><a href=\"" + system.familySearchSystem() + "/search/record/results?count=20&query=%2Bgivenname%3A" + getMiddleNameQuote(row.middleName) + row.firstName + getMiddlename(row.middleName, system.familySearchSystem()) + getMiddleNameQuote(row.middleName) + "~%20%2Bsurname%3A" + getLastName(row.lastName) + getPlace(row.birthPlace, system.familySearchSystem()) + getBirthYear(row.birthYear, system.familySearchSystem()) + getDeathYear(row.deathYear, system.familySearchSystem()) + "~&treeref=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Person</a></li>";
                                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/search/record/results?count=20&query=%2Bgivenname%3A" + getMiddleNameQuote(row.middleName) + row.firstName + getMiddleName(row.middleName, system.familySearchSystem()) + getMiddleNameQuote(row.middleName) + "~%20%2Bsurname%3A" + getLastName(row.lastName) + getPlace(row.birthPlace, system.familySearchSystem()) + getBirthYear(row.birthYear, system.familySearchSystem()) + getDeathYear(row.deathYear, system.familySearchSystem()) + "~&treeref=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-FamilySearch16\"></span> Family Search - Search</a></li>";
                                break;
                            default:
                                break;
                        }
                    });
                    $(this).append(menuOptions);
                }
            }
        };

        var $result = $('#eventsResult');

            $('#eventsTable').on('all.bs.table', function(e, name, args) {
                    console.log('Event:', name, ', data:', args);
            })
            .on('click-row.bs.table', function(e, row, $element) {
                $result.text('Event: click-row.bs.table');
            })
            .on('dbl-click-row.bs.table', function (e, row, $element) {
                $result.text('Event: dbl-click-row.bs.table');
            })
            .on('sort.bs.table', function(e, name, order) {
                $result.text('Event: sort.bs.table');
            })
            .on('check.bs.table', function(e, row) {
                $result.text('Event: check.bs.table');
                if (row.state && row.id) {
                    person.id = row.id;
                    person.name = row.fullName;
                    person.selected = true;
                }
            })
            .on('uncheck.bs.table', function(e, row) {
                $result.text('Event: uncheck.bs.table');
                //        person.id = row.id;
                //        person.name = row.fullName;
                person.selected = false;
            })
            .on('check-all.bs.table', function(e) {
                $result.text('Event: check-all.bs.table');
            })
            .on('uncheck-all.bs.table', function(e) {
                $result.text('Event: uncheck-all.bs.table');
            })
            .on('load-success.bs.table', function(e, data) {
                $result.text('Event: load-success.bs.table');
            })
            .on('load-error.bs.table', function(e, status) {
                $result.text('Event: load-error.bs.table');
            })
            .on('column-switch.bs.table', function(e, field, checked) {
                $result.text('Event: column-switch.bs.table');
            })
            .on('page-change.bs.table', function(e, size, number) {
                $result.text('Event: page-change.bs.table');
            })
            .on('search.bs.table', function(e, text) {
                $result.text('Event: search.bs.table');
            });


        findPerson.form.formValidation({
                framework: 'bootstrap',
                icon: {
                    valid: 'glyphicon glyphicon-ok',
                    invalid: 'glyphicon glyphicon-remove',
                    validating: 'glyphicon glyphicon-refresh'
                },
                fields: {
                    personId: {
                        row: '.col-xs-2',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: 'Please enter id number'
                            },
                            stringLength: {
                                min: 8,
                                message: 'Must be 8 characters long'
                            },
                            regexp: {
                                regexp: /^[a-zA-Z0-9]{4}-[a-zA-Z0-9]{3}$/,
                                message: 'ID must be in this format: ####-###'
                            }
                        }
                    },
                    firstName: {
                        row: '.col-xs-5',
                        validators: {
                            notEmpty: {
                                message: 'Or the first name'
                            }
                        }
                    },
                    lastName: {
                        row: '.col-xs-5',
                        enabled: false,
                        validators: {
                            notEmpty: {
                                message: 'Or the last name'
                            }
                        }
                    },
                    gender: {
                        row: '.col-xs-3',
                        validators: {
                            notEmpty: {
                                message: 'Gender is required'
                            }
                        }
                    },
                    birthYear: {
                        validators: {
                            numeric: {
                                message: 'Must be a number'
                            },
                            stringLength: {
                                min: 4,
                                max: 4,
                                message: 'Must be 4 digits'
                            }
                        }
                    },
                    deathYear: {
                        validators: {
                            numeric: {
                                message: 'Must be a number'
                            },
                            stringLength: {
                                min: 4,
                                max: 4,
                                message: 'Must be 4 digits'
                            }
                        }
                    }
                }
            })
            .on('keyup', '[name="personId"], [name="firstName"], [name="lastName"]', function (e) {
                var fv = findPerson.form.data('formValidation');

                switch ($(this).attr('name')) {
                    case 'firstName':
                        validateRow1(fv);
                        break;

                    case 'lastName':
                        validateRow1(fv);
                        break;

                    case 'personId':
                        validateRow1(fv);
                        break;

                    default:
                        break;
                    }
                })
            .on('success.form.fv', function(e) {
                e.preventDefault();

                //                // Some instances you can use are
                //                var $form = $(e.target); // The form instance
                //                var fv = $(e.target).data('formValidation'); // FormValidation instance
                submit();

            });

    }

    function validateRow1(fv) {

        if (string($("#personId").val()).isEmpty() && string($("#firstName").val()).isEmpty() && string($("#lastName").val()).isEmpty()) {
           fv.enableFieldValidators('firstName', true).revalidateField('firstName');
            fv.enableFieldValidators('lastName', true).revalidateField('lastName');
            fv.enableFieldValidators('personId', true).revalidateField('personId');
        } else {
            if (!string($("#firstName").val()).isEmpty()) {
                fv.enableFieldValidators('firstName', true).revalidateField('firstName');
            } else {
                fv.enableFieldValidators('firstName', false).revalidateField('firstName');
            }
            if (!string($("#lastName").val()).isEmpty()) {
                fv.enableFieldValidators('lastName', true).revalidateField('lastName');
            } else {
                fv.enableFieldValidators('lastName', false).revalidateField('lastName');
            }
            if (!string($("#personId").val()).isEmpty()) {
                fv.enableFieldValidators('personId', true).revalidateField('personId');
            } else {
                fv.enableFieldValidators('personId', false).revalidateField('personId');
            }
        }
    }
 
    function open() {
        if (system.target) {
            findPerson.callerSpinner = system.target.id;
        }

        findPerson.form = $("#findPersonForm");
        loadEvents();
        system.openForm(findPerson.form, findPerson.formTitleImage, findPerson.spinner);
        $('#firstName').focus();
    }

    function submit() {
        if (system.isAuthenticated()) {
            findPerson.personId = $("#personId").val();
            findPerson.firstName = $("#firstName").val();
            findPerson.lastName = $("#lastName").val();
            findPerson.gender = $("#gender").val();
            findPerson.birthYear = $("#birthYear").val();
            findPerson.deathYear = $("#deathYear").val();
            
            findPerson.save();

            $.ajax({
                url: constants.FIND_PERSONS_URL,
                data: {
                    "personId": findPerson.personId,
                    "firstName": findPerson.firstName,
                    "lastName": findPerson.lastName,
                    "gender": findPerson.gender,
                    "birthYear": findPerson.birthYear,
                    "deathYear": findPerson.deathYear
                },
                success: function(data) {
                    $('#eventsTable').bootstrapTable("load", data);
                }
            });
        } else {
            $(this).dialog("close");
            system.relogin();
        }
    }

    function close() {
        system.initSpinner(findPerson.callerSpinner, true);
    }

    function findPersonsStartingPoint() {
        startingPoint();
    }

    var findPersonController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        },
        searchKeyPress: function(e) {
            searchKeyPress(e);
        }

    };

    research.findPersonController = findPersonController;
    open();

    return findPersonController;
});

var _findPerson = require('person');
var _findSystem = require('system');

function nameFormatter(value, row, index) {
    var result = "";
    if (row && row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default\"><span style=\"color: " + _findPerson.getPersonColor(row.gender) + "\">" + _findPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return [result].join('');
}

function spouseFormatter(value, row, index) {
    var result = "";
    if (row && row.id && row.spouseName) {
        result = "<p style=\"color: " + _findPerson.getPersonColor(row.spouseGender) + "\">" + _findPerson.getPersonImage(row.spouseGender) + row.spouseName + "</p>";
    }
    return result;
}

function eventsFormatter(value, row, index) {
    var result = "";
    if (row && row.id) {
        result = "<p><b>Birth:</b> " + row.birthYear + "</p><p><b>Death:</b> " + row.deathYear + "</p>";
    }
    return result;
}

function parentsFormatter(value, row, index) {
    var result = "";
    if (row && row.id && row.fatherName) {
        result = "<p style=\"color: " + _findPerson.getPersonColor("Male") + "\">" + _findPerson.getPersonImage("Male") + row.fatherName + "</p>";
    }
    if (row && row.id && row.motherName) {
        result = result + "<p style=\"color: " + _findPerson.getPersonColor("Female") + "\">" + _findPerson.getPersonImage("Female") + row.motherName + "</p>";
    }
    return result;
}


//# sourceURL=findPersonController.js