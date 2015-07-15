(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (FindClues.displayType === "start") {
        $.ajax({
            data: { "personId": FindClues.personId, "personName": FindClues.personName, "reportId": FindClues.reportId, "searchCriteria": FindClues.searchCriteria, "gapInChildren": FindClues.gapInChildren, "ageLimit": FindClues.ageLimit },
            url: "/Home/FindCluesReportData",
            success: function(data) {
                FindClues.previous = data;
                $("#findCluesReportTable").bootstrapTable("load", data);
                openForm($("#findCluesReportForm"), "fmf24-report", "findCluesReportSpinner");
            }
        });
    } else {
        $("#findCluesReportTable").bootstrapTable("append", FindClues.previous);
        openForm($("#findCluesReportForm"), "fmf24-report", "findCluesReportSpinner");
    }
})(jQuery);

function nameFormatter(value) {
    var result = "";
    if (value != null) {
        var idNumber = value.substring(0, value.indexOf("~"));
        var fullname = value.substring(value.indexOf("~") + 1, value.size);
        var idNumberUrl = "<p><a style=\"color: rgb(0,0,255)\" href=\"" + getFamilySearchSystem() + "/tree/#view=ancestor&person=" + idNumber + "\" target=\"_tab\">" + idNumber + "</a></p>";
        var fullnameUrl = "<p><a href= \"#\" onClick=\" displayPerson('" + idNumber + "'); \" style= \" color: rgb(0, 153, 0)\" value= \"" + idNumber + "\" data-toggle=\" tooltip\" data-placement= \"top \" title=\" Select to display more info about this person\" >" + fullname + "</a></p>";
        result = fullnameUrl + idNumberUrl;
    }
    return result;
}

function clueFormatter(value) {
    var result = "";
    if (value != null) {
        var clue = value.substring(0, value.indexOf("~"));
        var criteriaId = value.substring(value.indexOf("~") + 1, value.size);
        result = "<p><a href= \"#\" onClick=\" displayClue(" + criteriaId + "); \" style= \" color: rgb(0, 153, 0)\" value= \"" + criteriaId + "\" data-toggle=\" tooltip\" data-placement= \"top \" title=\" Select to display more info about this clue\" >" + clue + "</a></p>";
    }
    return result;
}


function helpersFormatter(value) {
    var result = "";
    result += "<ul class=\"list-inline table-buttons\">";
    result += getVideoInfo(value);
    result += getTutorialInfo(value);
    result += getWebInfo(value);
    result += "</ul>";
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


