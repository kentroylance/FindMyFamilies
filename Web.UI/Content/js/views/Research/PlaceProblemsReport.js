(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (StartingPoint.displayType === "start") {
        $.ajax({
            data: { "personId": PlaceProblems.personId, "personName": PlaceProblems.personName, "generation": PlaceProblems.generation, "researchType": PlaceProblems.researchType, "reportId": PlaceProblems.reportId },
            url: '/Home/PlaceProblemsReportData',
            success: function(data) {
                PlaceProblems.previous = data;
                $('#placesTable').bootstrapTable("load", data);
                openForm($("#placesReportForm"), "fmf24-report", "getPlacesSpinner");
            }
        });
    } else {
        $('#placesTable').bootstrapTable("append", PlaceProblems.previous);
        openForm($("#placesReportForm"), "fmf24-report", "getPlacesSpinner");
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

function placeFormatter(value) {
    var result = "";
    if (value) {
        var problems = value.split("~");
        if (problems.length > 1) {
            result += "<p>" + problems[0] + "</p>";
            result += "<p>" + problems[1] + "</p>";
        } else {
            result += "<p>" + problems[0] + "</p>";
        }
    }
    return result;
}

//# sourceURL=GetPlaces.js