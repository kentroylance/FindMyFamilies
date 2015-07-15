using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FindMyFamilies.Util;

namespace FindMyFamiles.Web.Helpers
{
    public class ErrorHelper {
        private static Logger logger = new Logger(MethodBase.GetCurrentMethod().DeclaringType);

        private static ErrorHelper instance;
        private static readonly object syncLock = new object();
        private ArrayList searchCriteriaList;


        private ErrorHelper() {
        }

        public static ErrorHelper Instance {
            get {
                // Support multithreaded applications through
                // 'Double checked locking' pattern which (once
                // the instance exists) avoids locking each
                // time the method is invoked
                lock (syncLock) {
                    if (instance == null) {
                        instance = new ErrorHelper();
                    }

                    return instance;
                }
            }
        }

        public static string GetStatusCode(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string statusCode = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("statusCode")) {
                    statusCode = (string) viewContext.RouteData.Values["statusCode"].ToString();
                }
            }
            if (string.IsNullOrEmpty(statusCode) && (handleErrorInfo.Exception != null)) {
                statusCode = handleErrorInfo.Exception.Message;
            }

            return statusCode;
        }

        public static string GetExceptionMessage(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string exceptionMessage = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("exception")) {
                    exceptionMessage = ((Exception) viewContext.RouteData.Values["exception"]).Message;
                }
            }
            if (string.IsNullOrEmpty(exceptionMessage) && (handleErrorInfo.Exception != null)) {
                exceptionMessage = handleErrorInfo.Exception.Message;
            }

            return exceptionMessage;
        }

        public static string GetStackTrace(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string stackTrace = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("exception")) {
                    stackTrace = ((Exception) viewContext.RouteData.Values["exception"]).StackTrace;
                }
            } 
            if (string.IsNullOrEmpty(stackTrace) && (handleErrorInfo.Exception != null)) {
                stackTrace = handleErrorInfo.Exception.StackTrace;
            }

            return stackTrace;
        }

        public static string GetActionName(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string actionName = "Error";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("action")) {
                    actionName = (string)viewContext.RouteData.Values["action"];
                }
            } 
            if (string.IsNullOrEmpty(actionName) && (handleErrorInfo != null)) {
                actionName = handleErrorInfo.ActionName;
            }

            return actionName;
        }

        public static string GetTitle(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string title = "Error";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("title")) {
                    title = (string)viewContext.RouteData.Values["title"];
                }
            } 

            return title;
        }


        public static string GetControllerName(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string controllerName = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("controller")) {
                    controllerName = (string)viewContext.RouteData.Values["controller"];
                }
            }
            if (string.IsNullOrEmpty(controllerName) && (handleErrorInfo != null)) {
                controllerName = handleErrorInfo.ControllerName;
            }

            return controllerName;
        }


        public static string GetPath(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string path = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("path")) {
                    path = (string)viewContext.RouteData.Values["path"];
                }
            }
            if (string.IsNullOrEmpty(path) && (handleErrorInfo != null)) {
                path = "/" + handleErrorInfo.ControllerName + "/" + handleErrorInfo.ActionName;
            }

            return path;
        }

        public static string GetUri(ViewContext viewContext, HandleErrorInfo handleErrorInfo) {
            string uri = "";
            if ((viewContext != null) && (viewContext.RouteData != null)) {
                if (viewContext.RouteData.Values.ContainsKey("uri")) {
                    uri = (string)viewContext.RouteData.Values["uri"];
                }
            }
            if (string.IsNullOrEmpty(uri) && (handleErrorInfo != null)) {
                uri = "/" + handleErrorInfo.ControllerName + "/" + handleErrorInfo.ActionName;
            }

            return uri;
        }

        //@Model.Exception.StackTrace

//        public static string GetWebInfo(ValidationDO validation) {
//            string webId = "";
//            StringBuilder webInfo = new StringBuilder();
//            if (!string.IsNullOrEmpty(webId)) {
////                webInfo.Append("<li>");
//                webInfo.Append("<a class=\"fancybox\" data-fancybox-type=\"iframe\" href=\"" + webId + "\">");
//                webInfo.Append("<button type=\"button\" class=\"btn-u btn-u-sm btn-u-dark\" data-toggle=\"tooltip\" data-placement=\"top\" title=\"More Info\"><i class=\"fa fa-file-text-o\"></i></button>");
//                webInfo.Append("</a>");
////                webInfo.Append("<li>");
//            }
//            return webInfo.ToString();
//        }


//        public static string FindAGraveUrl(PersonDO person, PersonInfoDO personInfo) {
//            string url = "";
//            if (person != null && !person.IsEmpty) {
//                try {
//                    url = Constants.FIND_A_GRAVE + "&GSfn=" + person.Firstname + getMiddlename(person, Constants.FIND_A_GRAVE, personInfo) + "&GSln=" + getLastname(person, personInfo) + getBirthYear(person, Constants.FIND_A_GRAVE, personInfo) + getDeathYear(person, Constants.FIND_A_GRAVE, personInfo) + "&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n";
//                    //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
//                    //   http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr&GSfn=Bertha&GSmn=&GSln=Vevers&GSbyrel=after&GSby=1872&GSdyrel=before&GSdy=1913&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n
//                    //                return Constants.FIND_A_GRAVE + "&GSfn=" + person.Firstname + "&GSmn=" + person.Middlename + "&GSln=" + person.Lastname + "&GSbyrel=after&GSby=" + (person.BirthYear - 5) + "&GSdyrel=after&GSdy=" + (person.DeathYear - 5) + "&GScntry=0&GSst=0&GSgrid=&df=all&GSob=n";
//                } catch (Exception e) {
//                    logger.Error(e.Message, e);
//                    throw e;
//                }
//            }
//            return url;
//        }


//        private static string getLastname(PersonDO person, PersonInfoDO personInfo) {
//            string lastname = person.Lastname;
//
//            if ((person != null) && !person.IsEmpty && person.IsFemale && personInfo.IncludeMaidenName) {
//                if (!person.Father.IsEmpty && !string.IsNullOrEmpty(person.Father.Lastname)) {
//                    lastname = person.Father.Lastname;
//                } if (!person.Mother.IsEmpty && !string.IsNullOrEmpty(person.Mother.Lastname)) {
//                    lastname = person.Mother.Lastname;
//                }
//            }
//
//            return lastname;
//        }


//        public ArrayList GetSearchCriteriaList() {
//            if (searchCriteriaList == null) {
//                searchCriteriaList = new ArrayList();
//                searchCriteriaList.Add(new ListItemDO("0", "All"));
//                searchCriteriaList.Add(new ListItemDO("1", "No death date, or \"deceased\", and lived between 1850 to 1940."));
//                searchCriteriaList.Add(new ListItemDO("2", "A female child with no spouse and no death date, and lived between 1850 and 1940."));
//                searchCriteriaList.Add(new ListItemDO("3", "Gap between children"));
//                searchCriteriaList.Add(new ListItemDO("4", "Couples with no children, might have missing children"));
//                searchCriteriaList.Add(new ListItemDO("5", "Couples with only one child and lived longer than 20 years"));
//                searchCriteriaList.Add(new ListItemDO("6", "Person has no spouse and lived longer than 20 years"));
//                searchCriteriaList.Add(new ListItemDO("7", "Person has no spouse and no children"));
//                searchCriteriaList.Add(new ListItemDO("8", "Person has no spouse and only one child"));
////                searchCriteriaList.Add(new ListItemDO("9", "Last name is different than parents last name"));
//                searchCriteriaList.Add(new ListItemDO("10", "Child's birth year is after mother's death year"));
//                searchCriteriaList.Add(new ListItemDO("11", "Death year is before the marriage year"));
//                searchCriteriaList.Add(new ListItemDO("12", "Last child was born 4 or more years before the mother turned 40"));
////                searchCriteriaList.Add(new ListItemDO("13", "Married too early"));
//            }
//
//            return searchCriteriaList;
//        }
    }



}
