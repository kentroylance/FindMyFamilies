function nameFormatter(e,t,n){var r="";return t.id&&(r='<div class="btn-group"><button type="button" class="btn btn-default"><span style="color: '+_possibleDuplicatesPerson.getPersonColor(t.gender)+'">'+_possibleDuplicatesPerson.getPersonImage(t.gender)+t.fullName+'</span></button><a class="personAction" href="javascript:void(0)" title="Select button for options to research other websites"><button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown"><span class="caret"></span><span class="sr-only">Toggle Dropdown</span></button></a></div>'),r}function linkFormatter(e){var t="";return e&&(t=_possibleDuplicatesSystem.familySearchSystem()+"/tree/#view=possibleDuplicates&person="+e,t='<a style="color: rgb(50,205,50)" href="'+t+'" target="_tab">Duplicate</a>&nbsp;'),t}define(["require","jquery","system","constants","findPersonHelper","researchHelper","person","possibleDuplicates","possibleDuplicatesReport"],function(e){function f(){t("#possibleDuplicatesReportOptionsButton").unbind("click").bind("click",function(e){i.findOptions(e,a)}),t("#possibleDuplicatesReportSaveButton").unbind("click").bind("click",function(e){u.savePrevious(),a.form.dialog(r.CLOSE)}),t("#possibleDuplicatesReportCancelButton").unbind("click").bind("click",function(e){a.form.dialog(r.CLOSE)}),t("#possibleDuplicatesReportCloseButton").unbind("click").bind("click",function(e){a.form.dialog(r.CLOSE)}),a.form.unbind(r.DIALOG_CLOSE).bind(r.DIALOG_CLOSE,function(e){n.initSpinner(a.callerSpinner,!0),o.save(),a.callback&&typeof a.callback=="function"&&a.callback(o.selected)}),window.nameEvents={"click .personAction":function(e,n,r,s){t(this).children().length<=1&&t(this).append(i.getMenuOptions(r))}};var e=t("#eventsResult");t("#possibleDuplicatesTable").on("all.bs.table",function(e,t,n){console.log("Event:",t,", data:",n)}).on("click-row.bs.table",function(t,n,r){e.text("Event: click-row.bs.table")}).on("dbl-click-row.bs.table",function(t,n,r){e.text("Event: dbl-click-row.bs.table")}).on("sort.bs.table",function(t,n,r){e.text("Event: sort.bs.table")}).on("check.bs.table",function(t,n){e.text("Event: check.bs.table")}).on("uncheck.bs.table",function(t,n){e.text("Event: uncheck.bs.table")}).on("check-all.bs.table",function(t){e.text("Event: check-all.bs.table")}).on("uncheck-all.bs.table",function(t){e.text("Event: uncheck-all.bs.table")}).on("load-success.bs.table",function(t,n){e.text("Event: load-success.bs.table")}).on("load-error.bs.table",function(t,n){e.text("Event: load-error.bs.table")}).on("column-switch.bs.table",function(t,n,r){e.text("Event: column-switch.bs.table")}).on("page-change.bs.table",function(t,n,r){e.text("Event: page-change.bs.table")}).on("search.bs.table",function(t,n){e.text("Event: search.bs.table")})}function l(){a.form=t("#possibleDuplicatesReportForm"),f(),u.displayType==="start"?t.ajax({data:{id:o.id,fullName:o.name,generation:o.generation,researchType:o.researchType,possibleDuplicates:u.possibleDuplicates,possibleMatches:u.possibleMatches,reportId:o.reportId},url:r.POSSIBLE_DUPLICATES_REPORT_DATA_URL,success:function(e){u.previous=e,t("#possibleDuplicatesReportTable").bootstrapTable("append",e),n.openForm(a.form,a.formTitleImage,a.spinner)}}):(t("#possibleDuplicatesReportTable").bootstrapTable("append",u.previous),n.openForm(a.form,a.formTitleImage,a.spinner))}var t=e("jquery"),n=e("system"),r=e("constants"),i=e("findPersonHelper"),s=e("researchHelper"),o=e("person"),u=e("possibleDuplicates"),a=e("possibleDuplicatesReport"),c={open:function(){l()},close:function(){close()}};return s.possibleDuplicatesReportController=c,l(),c});var _possibleDuplicatesPerson=require("person"),_possibleDuplicatesSystem=require("system");