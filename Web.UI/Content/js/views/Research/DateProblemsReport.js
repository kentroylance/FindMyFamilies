(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (DateProblems.displayType === "start") {
        $.ajax({
            data: { "personId": DateProblems.personId, "personName": DateProblems.personName, "generation": DateProblems.generation, "researchType": DateProblems.researchType, "reportId": DateProblems.reportId, "empty": DateProblems.empty, "invalid": DateProblems.invalid, "invalidFormat": DateProblems.invalidFormat, "incomplete": DateProblems.incomplete },
            url: '/Home/DateProblemsReport',
            success: function(data) {
                DateProblems.previous = data;
                $('#datesTable').bootstrapTable("load", data);
                openForm($("#datesReportForm"), "fmf24-report", "getDatesSpinner");
            }
        });
    } else {
        $('#datesTable').bootstrapTable("append", DateProblems.previous);
        openForm($("#datesReportForm"), "fmf24-report", "getDatesSpinner");
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

function dateFormatter(value) {
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

//# sourceURL=DateProblemsReport.js