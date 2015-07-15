(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (PossibleDuplicates.displayType === "start") {
        $.ajax({
            data: { "personId": PossibleDuplicates.personId, "personName": PossibleDuplicates.personName, "generation": PossibleDuplicates.generation, "researchType": PossibleDuplicates.researchType, "reportId": PossibleDuplicates.reportId, "includePossibleDuplicates": PossibleDuplicates.includePossibleDuplicates, "includePossibleMatches": PossibleDuplicates.includePossibleMatches },
            url: '/Home/PossibleDuplicatesReportData',
            success: function(data) {
                PossibleDuplicates.previous = data;
                $('#possibleDuplicatesTable').bootstrapTable("load", data);
                openForm($("#possibleDuplicatesReportForm"), "fmf24-report", "possibleDuplicatesReportSpinner");
            }
        });
    } else {
        $('#possibleDuplicatesTable').bootstrapTable("append", PossibleDuplicates.previous);
        openForm($("#possibleDuplicatesReportForm"), "fmf24-report", "possibleDuplicatesReportSpinner");
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

function linkFormatter(value) {
    var result = "";
    if (value) {
        result = getFamilySearchSystem() + "/tree/#view=possibleDuplicates&person=" + value;
        result = "<a style=\"color: rgb(50,205,50)\" href=\"" + result + "\" target=\"_tab\">Duplicate</a>&nbsp;";
    }
    return result;
}

//# sourceURL=GetPossibleDuplicates.js