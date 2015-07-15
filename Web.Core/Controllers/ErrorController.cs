using System.Net;
using System.Web.Mvc;
using Elmah;
using FindMyFamiles.Web.Helpers;
using log4net;
using Newtonsoft.Json;
using ControllerBase = FindMyFamilies.Web.Controllers.ControllerBase;

namespace FindMyFamiles.Web.Controllers {
    public class ErrorController : ControllerBase {
        private readonly ILog Logger = LogManager.GetLogger(typeof (ErrorController));

        public ActionResult Index() {
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            if (Request.Url != null) {
                string path = Request.Url.AbsoluteUri;
                Logger.Error("Path: " + path);
            }
            return View("Error");
        }

        public ActionResult NotFound() {
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            if (Request.Url != null) {
                string path = Request.Url.AbsoluteUri;
                Logger.Error("404: Path: " + path);
            }
            return View("Error");
        }

        public ActionResult NotAuthorized() {
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            if (Request.Url != null) {
                string path = Request.Url.AbsoluteUri;
                Logger.Error("NotAuthorized: Path: " + path);
            }
            return View("Error");
        }

        public ActionResult AccessDenied() {
            Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            if (Request.Url != null) {
                string path = Request.Url.AbsoluteUri;
                Logger.Error("Access Denied: Path: " + path);
            }
            return View("Error");
        }
		
        [HttpPost]
        public JsonResult Record(ErrorInputModel model) {
            var ex = new ClientErrorException(model.ToString());

            ErrorSignal signal = ErrorSignal.FromCurrentContext();
            signal.Raise(ex);

            var result = new JsonResult {Data = JsonConvert.SerializeObject(new {Success = true})};
            return result;
        }

        //        /// <summary>
        //        /// Displays a generic error message
        //        /// </summary>
        //        /// <param name="title"></param>
        //        /// <param name="message"></param>
        //        /// <param name="redirectTo"></param>
        //        /// <returns></returns>
        //        public ActionResult ShowError(string title, string message, string redirectTo)
        //        {
        //            if (string.IsNullOrEmpty(message))
        //                message = "We are sorry, but an unspecified error occurred in the application. The error has been logged and forwarded to be checked out as soon as possible.";
        //
        //            ErrorViewModel model = new ErrorViewModel
        //            {
        //                Message = message,
        //                Title = title,
        //                RedirectTo = redirectTo,
        //                
        //                MessageIsHtml = true
        //            };
        //
        //            // Explicitly point at Error View regardless of action
        //            return View("Error", model);
        //        }
        //
        //        /// <summary>
        //        /// Displays a generic error message but allows passing a view model directly for 
        //        /// additional flexibility
        //        /// </summary>
        //        /// <param name="errorModel"></param>
        //        /// <returns></returns>
        //        public ActionResult ShowErrorFromModel(ErrorViewModel errorModel)
        //        {
        //            return View("Error", errorModel);
        //        }
        //
        //        public ActionResult ShowMessage(string title, string message, string redirectTo)
        //        {
        //            return this.ShowError(title, message, redirectTo);
        //        }
        //
        //        public ActionResult CauseError()
        //        {
        //            ErrorController controller = null;
        //            controller.CauseError();  // cause exception
        //            // never called
        //            return View("Error");
        //        }
        //
        //        /// <summary>
        //        /// Static method that can be called from outside of MVC requests
        //        /// (like in Application_Error) to display an error View.
        //        /// </summary>
        //        /// <param name="title"></param>
        //        /// <param name="message"></param>
        //        /// <param name="redirectTo"></param>
        //        /// <param name="messageIsHtml"></param>
        //        public static void ShowErrorPage(string title, string message, string redirectTo)
        //        {
        //            ErrorController controller = new ErrorController();
        //
        //            RouteData routeData = new RouteData();
        //            routeData.Values.Add("controller", "Error");
        //            routeData.Values.Add("action", "ShowError");
        //            routeData.Values.Add("title", title);
        //            routeData.Values.Add("message", message);
        //            routeData.Values.Add("redirectTo", redirectTo);
        //
        //            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), routeData));
        //        }
        //
        //        /// <summary>
        //        /// Static method that can be called from outside of MVC requests
        //        /// (like in Application_Error) to display an error View.
        //        /// </summary>
        //        public static void ShowErrorPage(ErrorViewModel errorModel)
        //        {
        //            ErrorController controller = new ErrorController();
        //
        //            RouteData routeData = new RouteData();
        //            routeData.Values.Add("controller", "Error");
        //            routeData.Values.Add("action", "ShowErrorFromModel");
        //            routeData.Values.Add("errorModel", errorModel);
        //
        //            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), routeData));
        //        }
        //
        //        /// <summary>
        //        /// Static method that can be called from outside of MVC requests
        //        /// (like in Application_Error) to display an error View.
        //        /// </summary>
        //        public static void ShowErrorPage(string title, string message)
        //        {
        //            ShowErrorPage(title, message, null);
        //        }
    }
}