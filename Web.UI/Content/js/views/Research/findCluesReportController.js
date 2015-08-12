define(function (require) {
    var $ = require('jquery');
    var system = require('system');
    var constants = require('constants');
    var msgBox = require('msgBox');
    var findPersonHelper = require('findPersonHelper');
    var researchHelper = require('researchHelper');

    // models
    var person = require('person');
    var findClues = require('findClues');
    var findCluesReport = require('findCluesReport');

    function loadEvents() {

        $("#findCluesReportOptionsButton").unbind('click').bind('click', function (e) {
            findPersonHelper.findOptions(e, findCluesReport);
        });

        $("#findCluesReportSaveButton").unbind('click').bind('click', function (e) {
            findClues.savePrevious();
            findCluesReport.form.dialog(constants.CLOSE);
        });


        $("#findCluesReportCancelButton").unbind('click').bind('click', function (e) {
            findCluesReport.form.dialog(constants.CLOSE);
        });

        $("#findCluesReportCloseButton").unbind('click').bind('click', function(e) {
            findCluesReport.form.dialog(constants.CLOSE);
        });

        findCluesReport.form.unbind(constants.DIALOG_CLOSE).bind(constants.DIALOG_CLOSE, function (e) {
            system.spinnerArea = findClues.spinner;
            person.save();
            if (findCluesReport.callback) {
                if (typeof (findCluesReport.callback) === "function") {
                    findCluesReport.callback(person.selected);
                }
            }
//            findCluesReport.reset();
        });

        window.nameEvents = {
            'click .personAction': function(e, value, row, index) {
                    $(this).append(findPersonHelper.getMenuOptions(row));
            },
            'mouseout .personAction1': function (e, value, row, index) {
                $('#personInfoDiv').hide();
            },
            'mouseover .personAction1': function (e, value, row, index) {

                $('#content').empty();

                var html = "<label><span style=\"color: " + _findCluesPerson.getPersonColor(row.gender) + "\">" + row.fullName + "</span></label><br>";
                html += "<b>ID:</b>  " + row.id + "<br>";
                html += '<b>Birth Date:</b>  ' + ((row.birthYear) ? (row.birthYear) : "") + '<br>';
                html += '<b>Birth Place:</b>  ' + ((row.birthPlace) ? (row.birthPlace) : "") + '<br>';
                html += '<b>Death Date:</b>  ' + ((row.deathYear) ? (row.deathYear) : "") + '<br>';
                html += '<b>Death Place:</b>  ' + ((row.deathPlace) ? (row.deathPlace) : "") + '<br>';
                html += "<b>Spouse:</b>";
                if (row.spouseName) {
                    html += "  <span style=\"color: " + _findCluesPerson.getPersonColor(row.spouseGender) + "\">" + row.spouseName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Mother:</b>";
                if (row.motherName) {
                    html += "  <span style=\"color: " + _findCluesPerson.getPersonColor("Female") + "\">" + row.motherName + "</span><br>";
                } else {
                    html += "<br>";
                }
                html += "<b>Father:</b>";
                if (row.fatherName) {
                    html += "  <span style=\"color: " + _findCluesPerson.getPersonColor("Male") + "\">" + row.fatherName + "</span><br>";
                } else {
                    html += "<br>";
                }

                $('#content').append(html);
                $('#personInfoDiv').show();
                $("#personInfoDiv").position({
                    my: "center+33 center-45",
                    at: "center",
                    of: $("#findCluesReportForm")
                });


            }
        };

        var $result = $('#eventsResult');

        $('#findCluessTable').on('all.bs.table', function (e, name, args) {
                console.log('Event:', name, ', data:', args);
            })
            .on('click-row.bs.table', function(e, row, $element) {
                $result.text('Event: click-row.bs.table');
            })
            .on('dbl-click-row.bs.table', function(e, row, $element) {
                $result.text('Event: dbl-click-row.bs.table');
            })
            .on('sort.bs.table', function(e, name, order) {
                $result.text('Event: sort.bs.table');
            })
            .on('check.bs.table', function(e, row) {
                $result.text('Event: check.bs.table');
            })
            .on('uncheck.bs.table', function(e, row) {
                $result.text('Event: uncheck.bs.table');
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
    }

    function open() {
        findCluesReport.form = $("#findCluesReportForm");
        loadEvents();

        if (findClues.displayType === "start") {
            $.ajax({
                data: { "PersonId": person.id, "PersonName": person.name, "ReportId": person.reportId, "SearchCriteria": findClues.searchCriteria, "GapInChildren": findClues.gapInChildren, "AgeLimit": findClues.ageLimit },
                url: constants.FIND_CLUES_REPORT_DATA_URL,
                success: function (data) {
                    if (data && data.errorMessage) {
                        system.spinnerArea = findClues.spinner;
                        system.stopSpinner(true);
                        msgBox.error(data.errorMessage);
                    } else {
                        findClues.previous = data.list;
                        $("#findCluesReportTable").bootstrapTable("append", data.list);
                        system.openForm(findCluesReport.form, findCluesReport.formTitleImage, findCluesReport.spinner);
                        if (person.reportId === constants.REPORT_ID) {
                            findCluesController.loadReports(true);
                        }
                    }
                }
            });
        } else {
            $("#findCluesReportTable").bootstrapTable("append", findClues.previous);
            system.openForm(findCluesReport.form, findCluesReport.formTitleImage, findCluesReport.spinner);
        }

    }

    var findCluesReportController = {
        open: function() {
            open();
        },
        close: function() {
            close();
        }
    };

    researchHelper.findCluesReportController = findCluesReportController;
    open();

    return findCluesReportController;
});

var _findCluesPerson = require('person');
var _findCluesSystem = require('system');
var msgBox = require('msgBox');

function nameFormatter(value, row, index) {
    var result = "";
    if (row.id) {
        result = "<div class=\"btn-group\"><button type=\"button\" class=\"btn btn-default dropdown-btn personAction1\"><span style=\"color: " + _findCluesPerson.getPersonColor(row.gender) + "\">" + _findCluesPerson.getPersonImage(row.gender) + row.fullName + "</span></button><a class=\"personAction\" href=\"javascript:void(0)\" title=\"Select button for options to research other websites\"><button type=\"button\" class=\"btn btn-success dropdown-toggle\" data-toggle=\"dropdown\"><span class=\"caret\"></span><span class=\"sr-only\">Toggle Dropdown</span></button></a></div>";
    }
    return result;
}

function helpersFormatter(value, row, index) {
    var result = "";
    result += "<ul class=\"list-inline table-buttons\">";
    result += getVideoInfo(row.helpers);
    result += getTutorialInfo(row.helpers);
    result += getWebInfo(row.helpers);
    result += "</ul>";
    return result;
}

function clueFormatter(value, row, index) {
    var result = "";
    if (row.helpers) {
        result = "<p><a href= \"#\" onClick=\" displayClue(" + row.helpers + "); \" style= \" color: rgb(0, 153, 0)\" value= \"" + row.helpers + "\" data-toggle=\" tooltip\" data-placement= \"top \" title=\" Select to display more info about this clue\" >" + row.clue + "</a></p>";
    }
    return result;
}

function displayClue(criteriaId) {
    var criteriaInfo = "";
    switch (criteriaId) {
    case 1:
        criteriaInfo = "When there is no death date, or the death date is only “deceased”, it indicates that the person submitting the information did not know much about the person. Census records are available from 1850 to 1940 and could reveal new information, including spouses and children.";
        break;
    case 2:
        criteriaInfo = "Searching marriage, census or cemetery records, between 1850 and 1940, might find a spouse and/or children.";
        break;
    case 3:
        criteriaInfo = "When the mother is healthy, a child might be born every two years.  Checking census records every ten years might find additional children.  Sometimes the children died, and would not show up on a census record, but might be found in a cemetery record.";
        break;
    case 4:
        criteriaInfo = "Most married couples had children, in the 1800s.  With the name of the married couple, the census records and cemetery records can be checked, to see if children are listed.";
        break;
    case 5:
        criteriaInfo = "Most married mothers had a child every two years, in the 1800s.  If only one child is listed, the census and cemetery records should be checked to look for other children.";
        //                    criteriaInfo = "The family search record shows that " + person.Fullname + " has a spouse, but only one child. " +
        //                                   "This person lived to be # years old, and it shows that they lived with their spouse for # years. " +
        //                                   "They were a couple long enough that most likely they had more than one child. " +
        //                                   "What usually happens is that someone will add a new parent to a person, and so that parent will be left with showing only one child.  " +
        //                                   "They will not continue on to include the rest of the children for that parent, so that only leaves one child. " +
        //                                   "Even though it is possible there is only one child, in most cases, if they lived long enough, and there is one child, " +
        //                                   "then most likely there are more missing children. <p></p>" +
        //                                   "Next step is to find if there are any census records for <persons name> by clicking on the Family Search button below.  " +
        //                                   "There are several other good web sites that can be researched by selecting any of the buttons below.";
        break;
    case 6:
        criteriaInfo = "Most people married in the 1800s.  Do a search of marriage, census and cemetery records to look for a possible spouse and children.";
        break;
    case 7:
        criteriaInfo = "Most people married in the 1800s.  Do a search of marriage, census and cemetery records to look for a possible spouse and children.";
        break;
    case 8:
        criteriaInfo = "If one child is listed, the person probably married, and might have other children.  Do a search of marriage, census and cemetery records to look for a possible spouse and more children.";
        break;
    case 9:
        criteriaInfo = "";
        break;
    case 10:
        criteriaInfo = "Check records to determine whether the child’s birth year or the mother’s death year is accurate.  Obviously, one of them is wrong.  The child might have a different mother, or belong to a different family.";
        break;
    case 11:
        criteriaInfo = "This is an obvious error.  If the person died before the marriage date, the marriage is wrong.  This requires further searching to determine which is correct, and if the couple was married.";
        break;
    default:
        criteriaInfo = "";
        break;
    }
    msgBox.message(criteriaInfo);
}

function getVideoInfo(criteria) {
    var videoId = "";
    switch (criteria) {
    case 1:
        videoId = "";
        break;
    case 2:
        videoId = "";
        break;
    case 3:
        //  Gap in children
        videoId = "96324885";
        break;
    case 4:
        videoId = "";
        break;
    case 5:
        videoId = "";
        break;
    case 6:
        videoId = "";
        break;
    case 7:
        videoId = "";
        break;
    case 8:
        videoId = "";
        break;
    case 9:
        videoId = "";
        break;
    case 10:
        videoId = "";
        break;
    case 11:
        videoId = "";
        break;
    default:
        videoId = "";
        break;
    }
    var videoInfo = "";
    if (videoId) {
        //                videoInfo.Append("<li>");
        videoInfo += "<a class=\"fancyboxvideo fitVideo\" data-width=\"800\" data-height=\"450\" caption=\"\" href=\"http://player.vimeo.com/video/" + videoId + "?title=0&amp;byline=0&amp;portrait=0\">";
        videoInfo += "<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Video\"><i class=\"fa fa-film\"></i></button>";
        videoInfo += "</a>";
        //                videoInfo.Append("<li>");
    }
    return videoInfo;
}

function getTutorialInfo(criteria) {
    var tutorialId = "";
    switch (criteria) {
    case 1:
        tutorialId = "";
        break;
    case 2:
        tutorialId = "";
        break;
    case 3:
        //  Gap in children
        //                    tutorialId = "/Content/person.pdf"; //"http://broadcast2.lds.org/elearning/fhd/Community/en/FamilySearch/Descendancy/Easy%20Steps%20to%20Descendancy%20Research.pdf";
        tutorialId = "/Content/Easy Steps to Descendancy Research.pdf";
        break;
    case 4:
        tutorialId = "";
        break;
    case 5:
        tutorialId = "";
        break;
    case 6:
        tutorialId = "";
        break;
    case 7:
        tutorialId = "";
        break;
    case 8:
        tutorialId = "";
        break;
    case 9:
        tutorialId = "";
        break;
    case 10:
        tutorialId = "";
        break;
    case 11:
        tutorialId = "";
        break;
    default:
        tutorialId = "";
        break;
    }
    var tutorialInfo = "";
    if (tutorialId) {
        //                tutorialInfo.Append("<li>");
        tutorialInfo += "<a class=\"fancybox\" data-fancybox-type=\"iframe\" href=\"" + tutorialId + "\">";
        tutorialInfo += "<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"Tutorial\"><i class=\"fa fa-book\"></i></button>";
        tutorialInfo += "</a>";
        //                tutorialInfo.Append("<li>");
    }
    return tutorialInfo;
}

function getWebInfo(criteria) {
    var webId = "";
    switch (criteria) {
    case 1:
        webId = "";
        break;
    case 2:
        webId = "";
        break;
    case 3:
        //  Gap in children
        webId = "https://www.lds.org/callings/temple-and-family-history/family-history-consultants/easy-steps-to-descendancy?lang=eng";
        break;
    case 4:
        webId = "";
        break;
    case 5:
        webId = "";
        break;
    case 6:
        webId = "";
        break;
    case 7:
        webId = "";
        break;
    case 8:
        webId = "";
        break;
    case 9:
        webId = "";
        break;
    case 10:
        webId = "";
        break;
    case 11:
        webId = "";
        break;
    default:
        webId = "";
        break;
    }
    var webInfo = "";
    if (webId) {
        //                webInfo.Append("<li>");
        webInfo += "<a class=\"fancybox\" data-fancybox-type=\"iframe\" href=\"" + webId + "\">";
        webInfo += "<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"More Info\"><i class=\"fa fa-file-text-o\"></i></button>";
        webInfo += "</a>";
        //                webInfo.Append("<li>");
    }
    return webInfo;
}


//# sourceURL=findCluesReportController.js