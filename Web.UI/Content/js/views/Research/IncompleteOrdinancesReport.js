(function($) {
    $("[data-toggle='tooltip']").tooltip();
    if (IncompleteOrdinances.displayType === "start") {
        $.ajax({
            data: { "personId": IncompleteOrdinances.personId, "personName": IncompleteOrdinances.personName, "generation": IncompleteOrdinances.generation, "researchType": IncompleteOrdinances.researchType, "reportId": IncompleteOrdinances.reportId },
            url: '/Home/IncompleteOrdinancesReportData',
            success: function(data) {
                IncompleteOrdinances.previous = data;
                $('#ordinancesTable').bootstrapTable("load", data);
                openForm($("#ordinancesReportForm"), "fmf24-report", "getOrdinancesSpinner");
            }
        });
    } else {
        $('#ordinancesTable').bootstrapTable("append", IncompleteOrdinances.previous);
        openForm($("#ordinancesReportForm"), "fmf24-report", "getOrdinancesSpinner");
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

function ordinanceFormatter(value) {
    var result = "";
    if (value != null) {
        if (value.indexOf("~") > -1) {
            var status = value.substring(0, value.indexOf("~"));
            var reservable = value.substring(value.indexOf("~") + 1, value.size);
            result = "<p>" + status + "</p><p>" + reservable + "</p>";
        } else {
            result = value;
        }
    }
    return result;
}

//# sourceURL=GetOrdinances.js