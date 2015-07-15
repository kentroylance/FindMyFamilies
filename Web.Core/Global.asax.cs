using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Elmah;
using FindMyFamiles.Web.Controllers;
using FindMyFamilies.Util;
using log4net;
using HttpException = System.Web.HttpException;

namespace FindMyFamilies.Web {
    public class MvcApplication : HttpApplication {
/// <summary>
        /// The current web client version.
        /// </summary>
        public static readonly Version Version = typeof(MediaTypeNames.Application).Assembly.GetName().Version;
        private static readonly ILog log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

		/// <summary>
        /// Registers the controller action routes.
        /// </summary>
        /// <param name="routes">The global route configuration.</param>
        private static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
		
        // ELMAH Filtering
        protected void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e) {
            Filter(e);
        }

        protected void ErrorMail_Filtering(object sender, ExceptionFilterEventArgs e) {
            Filter(e);
        }

        // Dismiss 404 errors for ELMAH
        private void Filter(ExceptionFilterEventArgs e) {
            Exception exception = e.Exception.GetBaseException();
            var httpException = exception as HttpException;

            if ((httpException != null) && httpException.GetHttpCode() == 404) {
                e.Dismiss();
            }

            if (exception is FileNotFoundException || exception is HttpRequestValidationException || exception is HttpException) {
                e.Dismiss();
            }
        }

        protected void Application_Error(object sender, EventArgs e) {
            FilePathHelper.exceedsFileCount();
            HttpContext httpContext = ((MvcApplication) sender).Context;

            Exception ex = Server.GetLastError().GetBaseException();
            log.Error("Application_Error", ex);

            string currentController = " ";
            string currentAction = " ";
            RouteData currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null) {
                if (currentRouteData.Values["controller"] != null && !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString())) {
                    currentController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null && !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString())) {
                    currentAction = currentRouteData.Values["action"].ToString();
                }
            }
            

            Exception exception = Server.GetLastError();
            var controller = new ErrorController();
            var routeData = new RouteData();
            var action = "Index";
            var title = "Exception Notice";

            if (exception is HttpException) {
                var httpEx = exception as HttpException;
                routeData.Values["statusCode"] = httpEx.GetHttpCode();
                routeData.Values["path"] = httpContext.Request.Url.AbsolutePath;
                routeData.Values["uri"] = httpContext.Request.Url.AbsoluteUri;

                if (httpEx.GetHttpCode() == 404) {
                    action = "NotFound";
                    title = "Page Not Found";
                } else if (httpEx.GetHttpCode() == (int) HttpStatusCode.Unauthorized) {
                    action = "NotAuthorized";
                    title = "Not Authorized";
                } else if (httpEx.GetHttpCode() == 401) {
                    action = "AccessDenied";;
                    title = "Access Denied";;
                } else if (httpEx.GetHttpCode() == 500) {
                    action = "NotFound";
                    title = "Server Error";
                }
            } else {
                routeData.Values["statusCode"] = 500;
            }

            httpContext.ClearError();
            httpContext.Response.Clear();
            httpContext.Response.ContentType = "text/html";
            // Avoid IIS7 getting in the middle
            httpContext.Response.TrySkipIisCustomErrors = true;
            httpContext.Response.StatusCode = exception is HttpException ? ((HttpException) exception).GetHttpCode() : 500;
            httpContext.Response.TrySkipIisCustomErrors = true;

            routeData.Values["controller"] = "Error";
            routeData.Values["action"] = action;
            routeData.Values["exception"] = exception;
            routeData.Values["title"] = title;
            controller.ViewData.Model = new HandleErrorInfo(exception, currentController, currentAction);
            ((IController) controller).Execute(new RequestContext(new HttpContextWrapper(httpContext), routeData));
        }
    }
}