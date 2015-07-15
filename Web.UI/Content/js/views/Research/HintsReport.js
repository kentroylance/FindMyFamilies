(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (Hints.displayType === "start") {
        $.ajax({
            data: { "personId": Hints.personId, "personName": Hints.personName, "generation": Hints.generation, "researchType": Hints.researchType, "topScore": Hints.topScore, "count": Hints.count, "reportId": Hints.reportId },
            url: '/Home/HintsReportData',
            success: function(data) {
                Hints.previous = data;
                $('#hintsTable').bootstrapTable("append", data);
                openForm($("#hintsReportForm"), "fmf24-report", "getHintsSpinner");
            }
        });
    } else {
        $('#hintsTable').bootstrapTable("append", Hints.previous);
        openForm($("#hintsReportForm"), "fmf24-report", "getHintsSpinner");
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
        result = getFamilySearchSystem() + "/tree/#view=hints&person=" + value;
        result = "<a style=\"color: rgb(50,205,50)\" href=\"" + result + "\" target=\"_tab\">Duplicate</a>&nbsp;";
    }
    return result;
}

//# sourceURL=HintsReport.js