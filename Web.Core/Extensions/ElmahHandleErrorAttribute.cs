using System.Web.Mvc;
using Elmah;

namespace FindMyFamiles.Web.Extensions {
    public class ElmahHandleErrorAttribute : HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            bool exceptionHandled = filterContext.ExceptionHandled;

            base.OnException(filterContext);

            // signal ELMAH to log the exception
//            if (!exceptionHandled && filterContext.ExceptionHandled) {
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
//            }
        }
    }
}