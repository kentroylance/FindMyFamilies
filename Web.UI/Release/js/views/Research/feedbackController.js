define(["require","jquery","system","msgBox","constants","researchHelper","lazyRequire","person","hints","hintsReport","retrieve","findPerson","retrieve"],function(e){function h(){a.id&&t("#hintsPersonId").val(a.id),a.researchType&&t("#hintsResearchType").val(a.researchType),a.generation&&t("#hintsGeneration").val(a.generation),f.topScore&&t("#hintsTopScore").prop("checked",f.topScore),f.count&&t("#hintsCount").prop("checked",f.count),a.reportId&&t("#hintsReportId").val(a.reportId),a.addChildren&&t("#addChildren").prop("checked",a.addChildren)}function p(){t("#hintsReportId").val(a.reportId);var e=t("#hintsReportId option:selected").text();if(e&&e.length>8&&e!=="Select"){var n=e.indexOf("Name: ")+6,r=e.indexOf(", Date:  "),s=e.indexOf(", Research Type: "),o=e.indexOf(",  Generations: ");a.id=e.substring(n,n+8),a.name=e.substring(n+11,r),a.researchType=e.substring(s+17,o),a.generation=e.substring(o+16,o+17),a.loadPersons(t("#hintsPersonId"))}a.researchType===i.DESCENDANTS?addDecendantGenerationOptions():addAncestorGenerationOptions(),h()}function d(e){c.loadReports(t("#hintsReportId"),e),p()}function v(){a.researchType!==i.ANCESTORS&&(a.generation="2"),t("hintsGeneration").val(a.generation)}function m(){f.form=t("#hintsForm"),w(),d(),a.loadPersons(t("#hintsPersonId")),h(),n.openForm(f.form,f.formTitleImage,f.spinner)}function g(){a.clear()}function y(){a.reset(),p()}function b(){a.resetReportId(t("#hintsReportId")),p()}function w(){t("#hintsPerson").change(function(e){debugger}),t("#hintsReportId").change(function(e){a.reportId=t("#hintsReportId option:selected").val(),a.reportId===i.REPORT_ID&&r.warning('Even though the "Select" option is availiable to retrieve family search data, to avoid performance problems it is best practice to first retrieve the data before analyzing.'),p()}),t("#hintsPersonId").change(function(e){a.id=t("option:selected",t(this)).val(),a.name=t("option:selected",t(this)).text(),b()}),t("#hintsResearchType").change(function(e){a.researchType=t("#hintsResearchType").val(),a.researchType===i.DESCENDANTS?(f.generationAncestors=a.generation,a.generation=f.generationDescendants,addDecendantGenerationOptions()):(f.generationDescendants=a.generation,a.generation=f.generationAncestors,addAncestorGenerationOptions()),v(),b()}),t("#hintsFindPersonButton").unbind("click").bind("click",function(n){return s.findPerson(n,function(n){var r=e("findPerson");if(n){var i=r.id===t("#hintsPersonId").val()?!1:!0;i&&(a.id=r.id,a.name=r.name),f.save(),a.loadPersons(t("#hintsPersonId")),i&&b()}r.reset()}),!1}),t("#hintsRetrieveButton").unbind("click").bind("click",function(t){return s.retrieve(function(t){var n=e("retrieve");t&&(a.reportId=n.reportId,f.save(),d(!0)),n.reset()}),!1}),t("#hintsHelpButton").unbind("click").bind("click",function(e){}),t("#hintsCloseButton").unbind("click").bind("click",function(e){f.form.dialog(i.CLOSE)}),t("#hintsResetButton").unbind("click").bind("click",function(e){y()}),t("#hintsPreviousButton").unbind("click").bind("click",function(e){f.previous||window.localStorage&&(f.previous=JSON.parse(localStorage.getItem(i.HINTS_PREVIOUS))),f.previous?(n.initSpinner("hints.spinner"),f.callerSpinner=f.spinner,t.ajax({url:i.HINTS_REPORT_HTML_URL,success:function(e){var n=t("#hintsReportForm"),r=n.children().detach();t('<div id="hintsReportForm"></div>').dialog({title:"Hints",width:975,open:function(){r.appendTo(n),t(this).css("maxHeight",700)}}),f.displayType="previous",t("#hintsReportForm").empty().append(e),s&&s.hintsReportController&&s.hintsReportController.open()}})):r.message("Sorry, there is nothing previous to display.")}),t("#hintsSubmitButton").unbind("click").bind("click",function(e){return n.isAuthenticated()?(a.id||r.message("You must first select a person from Family Search"),r.question("Depending on the number of generations you selected, this could take a minute or two.  Select Yes if you want to contine.","Question",function(e){e&&(n.initSpinner(f.spinner),u(["css!/Content/css/lib/research/bootstrap-table.min.css"],function(){},function(){t.ajax({url:i.HINTS_REPORT_HTML_URL,success:function(e){var n=t("#hintsReportForm"),r=n.children().detach();t('<div id="hintsReportForm"></div>').dialog({title:"Hints",width:975,height:515,open:function(){r.appendTo(n)}}),l.displayType="start",f.save(),t("#hintsReportForm").empty().append(e),s&&s.hintsReportController&&s.hintsReportController.open()}})}))})):(l.form.dialog("close"),n.relogin()),!1}),t("#addChildren").change(function(e){a.addChildren=t("#addChildren").prop("checked"),a.addChildren&&r.warning("Selecting <b>Add Children</b> check box will probably double the time to retrieve ancestors."),a.resetReportId(t("#hintsReportId")),p()}),t("#hintsTopScore").change(function(e){f.topScore=t("#hintsTopScore").prop("checked"),f.topScore?f.count=!1:f.count=!0}),t("#hintsCount").change(function(e){f.count=t("#hintsCount").prop("checked"),f.count?f.topScore=!1:f.topScore=!0}),t("#hintsCancelButton").unbind("click").bind("click",function(e){f.form.dialog(i.CLOSE)}),f.form.unbind(i.DIALOG_CLOSE).bind(i.DIALOG_CLOSE,function(e){f.save()})}var t=e("jquery"),n=e("system"),r=e("msgBox"),i=e("constants"),s=e("researchHelper"),o=e("lazyRequire"),u=o.once(),a=e("person"),f=e("hints"),l=e("hintsReport"),c=e("retrieve"),E={open:function(){m()}};return s.hintsController=E,m(),E});