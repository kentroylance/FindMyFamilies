(function ($) {
    $("[data-toggle='tooltip']").tooltip();
    if (StartingPoint.displayType === "start") {
        $.ajax({
            data: { "personId": StartingPoint.personId, "personName": StartingPoint.personName, "generation": StartingPoint.generation, "researchType": StartingPoint.researchType, "nonMormon": StartingPoint.nonMormon, "born18101850": StartingPoint.born18101850, "livedInUSA": StartingPoint.livedInUSA, "needOrdinances": StartingPoint.ordinances, "hint": StartingPoint.hints, "duplicate": StartingPoint.duplicates, "reportId": StartingPoint.reportId },
            url: "/Home/StartingPointReportData",
            success: function (data) {
                StartingPoint.previous = data;
                $("#startingPointTable").bootstrapTable("append", data);
                openForm($("#startingPointReportForm"), "fmf24-report", "startingPointSpinner");
            }
        });
    } else {
        $("#startingPointTable").bootstrapTable("append", StartingPoint.previous);
        openForm($("#startingPointReportForm"), "fmf24-report", "startingPointSpinner");
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

function reasonsFormatter(value) {
    var result = "";
    if (value) {
        var reasons = value.split("~");
        for (var i = 0; i < reasons.length - 1; i++) {
            var reason = reasons[i];
            if (reason.indexOf("BornBetween1810and1850") > -1) {
                var birthDate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Born between 1810 and 1850 - <b>" + birthDate + "</b></p>";
            } else if (reason.indexOf("DiedInUSA") > -1) {
                var deathPlace = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Died in the United States - <b>" + deathPlace + "</b></p>";
            } else if (reason.indexOf("BornInUSA") > -1) {
                var birthPlace = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Born in United States - <b>" + birthPlace + "</b></p>";
            } else if (reason.indexOf("NoBirthDate") > -1) {
                var noBirthDate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Invalid birth date - <b>" + noBirthDate + "</b></p>";
            } else if (reason.indexOf("NonMormon") > -1) {
                result += "<p>Non-Mormon</p>";
            } else if (reason.indexOf("Hint") > -1) {
                var hint = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Hint - <b>" + hint + "</b></p>";
            } else if (reason.indexOf("PossibleDuplicate") > -1) {
                var possibleDuplicate = reason.substring(reason.indexOf("[") + 1, reason.length - 1);
                result += "<p>Possible Duplicate - <b>" + possibleDuplicate + "</b></p>";
            } else if (reason.indexOf("NoBirthPlace") > -1) {
                result += "<p>No birth place</p>";
            } else if (reason.indexOf("IncompleteOrdinances") > -1) {
                var ordinances = reason.substring(reason.indexOf("[") + 1, reason.length - 2);
                result += "<p>IncompleteOrdinances - <b>" + ordinances + "</b></p>";
            } else {
                result = value;
            }

        }
    }
    return result;
}

//# sourceURL=StartingPointReport.js