using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace FindMyFamilies.Web.Models {
    public class CustomHandleErrorAttribute : HandleErrorAttribute {
        //private readonly ILog _logger;

        public override void OnException(ExceptionContext filterContext) {
            base.OnException(filterContext);

            // signal ELMAH to log the exception
            if (filterContext.ExceptionHandled) {
                ErrorSignal.FromCurrentContext().Raise(filterContext.Exception);
            }
        }

        private static void RaiseErrorSignal(Exception e) {
            var context = HttpContext.Current;

            ErrorSignal.FromContext(context).Raise(e, context);
        }
    }
}