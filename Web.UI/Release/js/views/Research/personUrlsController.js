define(["require","jquery","system","constants","findPersonHelper","personUrls"],function(e){function o(e,t,n,r,i,s,o,u,a){this.id=e,this.firstName=t,this.middleName=n,this.lastName=r,this.fullName=i,this.gender=s,this.birthYear=o,this.deathYear=u,this.birthPlace=a}function u(){t(".personUrlsAction").unbind("click").bind("click",function(e){var n=t(this),r=new o(n.data("id"),n.data("firstname"),n.data("middlename"),n.data("lastname"),n.data("fullname"),n.data("gender"),n.data("birthyear"),n.data("deathyear"),n.data("birthplace"));n.children().length<=1&&n.append(i.getMenuOptions(r))}),t("#personUrlsCloseButton").unbind("click").bind("click",function(e){return s.form.dialog(r.CLOSE),!1}),s.form.unbind(r.DIALOG_CLOSE).bind(r.DIALOG_CLOSE,function(e){return n.initSpinner(s.callerSpinner,!0),!1})}function a(){}function f(){n.target&&(s.callerSpinner=n.target.id),s.form=t("#personUrlsForm"),u(),a(),n.openForm(s.form,s.formTitleImage,s.spinner)}var t=e("jquery"),n=e("system"),r=e("constants"),i=e("findPersonHelper"),s=e("personUrls"),l={open:function(){f()}};return researchHelper.personUrlsController=l,f(),l});