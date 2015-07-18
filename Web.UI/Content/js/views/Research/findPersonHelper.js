define(function(require) {

    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var string = require('string');
    var person = require('person');

    var _findUrls = {};
    _findUrls['fmf-urls'] = 'Family Research Urls';
    _findUrls['ancestry'] = 'Ancestry';
    _findUrls['puz-descend'] = 'Puzilla Descendants';
    _findUrls['puz-ancest'] = 'Puzilla Ancestors';
    _findUrls['findagrave'] = 'Find-A-Grave';
    _findUrls['billgrave'] = 'Billion Graves';
    _findUrls['findmypast'] = 'Find My Past';
    _findUrls['myheritage'] = 'My Heritage';
    _findUrls['amerancest'] = 'American Ancestors';
    _findUrls['fmf-retrieve'] = 'Retrieve';
    _findUrls['fmf-starting'] = 'Starting Point';
    _findUrls['fs-fan'] = 'Family Search - Fan Chart';
    _findUrls['fs-desc'] = 'Family Search - Descendancy';
    _findUrls['fs-tree'] = 'Family Search - Tree';
    _findUrls['fs-search'] = 'Family Search - Search';
    _findUrls['fs-person'] = 'Family Search - Person';
    _findUrls['google'] = 'Google Search';

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

                $.each(birthPlaceParts, function(key, value) {
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
                $.each(birthPlaceParts, function(key, value) {
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
                $.each(birthPlaceParts, function(key, value) {
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

    function getMenuOptions(row) {
        var menuOptions = "";
        person.includeMiddleName = true;
        person.includePlace = true;

        menuOptions += "<ul class=\"dropdown-menu\" role=\"menu\" aria-labelledby=\"dLabel\" >";
        $.each(person.findPersonOptions, function (key, value) {
            switch (value) {
            case 'fmf-urls':
                menuOptions += "<li><a onclick=\"researchController.personUrlOptions('" + row.id + "');\" href=\"javascript:void(0);\"><span class=\"fa fmf-family16\"></span> Family Research Urls</a></li>";
                break;
            case 'google':
                menuOptions += "<li><a href=\"" + constants.GOOGLE + row.firstName + getMiddleName(row.middleName, constants.GOOGLE) + "+" + getLastName(row.lastName) + "+" + getBirthYear(row.birthYear, constants.GOOGLE) + "+" + getDeathYear(row.deathYear, constants.GOOGLE) + "+" + getPlace(row.birthPlace, constants.GOOGLE) + "\" target=\" _tab\" ><span class=\"fa fmf-google16\"></span> Google</a></li>";
                break;
            case 'findmypast':
                menuOptions += "<li><a href=\"" + constants.FIND_MY_PAST + "firstname=" + row.firstName + getMiddleName(row.middleName, constants.FIND_MY_PAST) + "&lastname=" + getLastName(row.lastName) + getBirthYear(row.birthYear, constants.FIND_MY_PAST) + "\" target=\" _tab\" ><span class=\"fa fmf-findmypast16\"></span> Find My Past</a></li>";
                break;
            case 'myheritage':
                menuOptions += "<li><a href=\"" + constants.MY_HERITAGE + "&qname=Name+fn." + row.firstName + getMiddleName(row.middleName, constants.MY_HERITAGE) + "+ln." + getLastName(row.lastName) + getBirthYear(row.birthYear, constants.MY_HERITAGE) + getPlace(row.birthPlace, constants.MY_HERITAGE) + "\" target=\" _tab\" ><span class=\"fa fmf-myheritage16\"></span> My Heritage</a></li>";
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
//            case 'fs-person':
                //                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=ancestor&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Person</a></li>";
//                break;
            case 'fs-tree':
                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=pedigree&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Tree</a></li>";
                break;
            case 'fs-desc':
                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=descendancy&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Descendancy</a></li>";
                break;
            case 'fs-fan':
                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=fan&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Fan</a></li>";
                break;
            case 'fs-person':
                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/tree/#view=tree&section=person&person=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Person</a></li>";
                break;
            case 'fs-search':
                menuOptions += "<li><a href=\"" + system.familySearchSystem() + "/search/record/results?count=20&query=%2Bgivenname%3A" + getMiddleNameQuote(row.middleName) + row.firstName + getMiddleName(row.middleName, system.familySearchSystem()) + getMiddleNameQuote(row.middleName) + "~%20%2Bsurname%3A" + getLastName(row.lastName) + getPlace(row.birthPlace, system.familySearchSystem()) + getBirthYear(row.birthYear, system.familySearchSystem()) + getDeathYear(row.deathYear, system.familySearchSystem()) + "~&treeref=" + row.id + "\" target=\" _tab\" ><span class=\"fa fmf-familysearch16\"></span> Family Search - Search</a></li>";
                break;
            default:
                break;
            }
        });
        menuOptions += "</ul>";
        return menuOptions;
    }

    var findPersonHelper = {
        getMenuOptions: function (row) {
            return getMenuOptions(row);
        },
        get findUrls() {
            return _findUrls;
        }

    };


    return findPersonHelper;
});


//# sourceURL=findPersonHelper.js