using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace FindMyFamiles.Web.Extensions {
    public class ElmahMVCErrorFilter : IExceptionFilter {
        private static ErrorFilterConfiguration _config;

        public void OnException(ExceptionContext context) {
            if (context.ExceptionHandled) //The unhandled ones will be picked by the elmah module
            {
                Exception e = context.Exception;
                HttpContext context2 = context.HttpContext.ApplicationInstance.Context;
                //TODO: Add additional variables to context.HttpContext.Request.ServerVariables for both handled and unhandled exceptions
                if ((context2 == null) || (!_RaiseErrorSignal(e, context2) && !_IsFiltered(e, context2))) {
                    _LogException(e, context2);
                }
            }
        }

        private static bool _IsFiltered(Exception e, HttpContext context) {
            if (_config == null) {
                _config = (context.GetSection("elmah/errorFilter") as ErrorFilterConfiguration) ?? new ErrorFilterConfiguration();
            }
            var context2 = new ErrorFilterModule.AssertionHelperContext(e, context);
            return _config.Assertion.Test(context2);
        }

        private static void _LogException(Exception e, HttpContext context) {
            ErrorLog.GetDefault(context).Log(new Error(e, context));
        }

        private static bool _RaiseErrorSignal(Exception e, HttpContext context) {
            ErrorSignal signal = ErrorSignal.FromContext(context);
            if (signal == null) {
                return false;
            }
            signal.Raise(e, context);
            return true;
        }
    }
}