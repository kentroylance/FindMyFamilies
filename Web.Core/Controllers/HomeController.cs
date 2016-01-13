using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FindMyFamiles.Services.Data;
using FindMyFamilies.Data;
using FindMyFamilies.Helper;
using FindMyFamilies.Services;
using FindMyFamilies.Util;
using log4net;

namespace FindMyFamilies.Web.Controllers {
    public class HomeController : ControllerBase {
        private const int ITEMSPERPAGE = 10;
        private readonly ILog Logger = LogManager.GetLogger(typeof (HomeController));

        [System.Web.Mvc.HttpGet]
        public ActionResult StartingPoint() {
            return PartialView("~/Views/Research/StartingPoint.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FAQ() {
            return PartialView("~/Views/Faq/Faq.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Help() {
            return PartialView("~/Views/Help/Help.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FindPersonOptions() {
            return PartialView("~/Views/Research/FindPersonOptions.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult StartingPointReportHtml() {
            return PartialView("~/Views/Research/StartingPointReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult StartingPointReportData(StartingPointInputDO startingPoint) {
            var result = new ResultDO();

            if ((startingPoint != null) && (startingPoint.Id != null)) {
                try {
                    session = GetSession();
                    result.list = Service.GetStartingPoints(startingPoint, ref session);
                    if (startingPoint.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    result.errorMessage = "Error retrieving starting point report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetPersonInfo(string id) {
            var personInfo = new FindListItemDO();

            if (id != null) {
                try {
                    session = GetSession();
                    var researchDO = new ResearchDO();
                    researchDO.CurrentPersonId = id;
                    researchDO.PersonId = id;
                    personInfo = Service.GetPersonWithSpouseParents(researchDO, ref session);
                    ResetTokenHourExpire();
                    personInfo.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    personInfo.errorMessage = "Error retrieving starting point report data";
                }
            }

            return Json(personInfo, JsonRequestBehavior.AllowGet);
        }


        [System.Web.Mvc.HttpGet]
        public ActionResult DateProblems() {
            return PartialView("~/Views/Research/DateProblems.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult DateProblemsReportHtml() {
            return PartialView("~/Views/Research/DateProblemsReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult DateProblemsReportData(DateInputDO dateInputDO) {
            var result = new ResultDO();

            if (dateInputDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.GetDates(dateInputDO, ref session);
                    if (dateInputDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    result.errorMessage = "Error retrieving date problems report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PlaceProblems() {
            return PartialView("~/Views/Research/PlaceProblems.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PlaceProblemsReportHtml() {
            return PartialView("~/Views/Research/PlaceProblemsReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult PlaceProblemsReportData(PlaceInputDO placeInputDO) {
            var result = new ResultDO();

            if (placeInputDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.GetPlaces(placeInputDO, ref session);
                    if (placeInputDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    result.errorMessage = "Error retrieving place problems report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FindClues() {
            return PartialView("~/Views/Research/FindClues.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FindCluesReportHtml() {
            return PartialView("~/Views/Research/FindCluesReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult FindCluesReportData(FindCluesInputDO findCluesInputDO) {
            var result = new ResultDO();

            if (findCluesInputDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.FindClues(findCluesInputDO, ref session);
                    if (findCluesInputDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    result.errorMessage = "Error retrieving find clues report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Ordinances() {
            return PartialView("~/Views/Research/Ordinances.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult OrdinancesReportHtml() {
            return PartialView("~/Views/Research/OrdinancesReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult OrdinancesReportData(OrdinancesDO ordinancesDO) {
            var result = new ResultDO();

            if (ordinancesDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.GetOrdinances(ordinancesDO, ref session);
                    if (ordinancesDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception) {
                    result.errorMessage = "Error retrieving ordinance report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Feedback() {
            return PartialView("~/Views/Research/Feedback.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Features() {
            return PartialView("~/Views/Research/Features.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Hints() {
            return PartialView("~/Views/Research/Hints.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult HintsReportHtml() {
            return PartialView("~/Views/Research/HintsReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult HintsReportData(HintInputDO hintInputDO) {
            var result = new ResultDO();

            if (hintInputDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.GetHints(hintInputDO, ref session);
                    if (hintInputDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception e) {
                    Logger.Error(e.Message, e);
                    result.errorMessage = "Error retrieving hint report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PossibleDuplicates() {
            return View("~/Views/Research/PossibleDuplicates.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult PossibleDuplicatesReportHtml() {
            return PartialView("~/Views/Research/PossibleDuplicatesReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult PossibleDuplicatesReportData(PossibleDuplicateInputDO possibleDuplicatesInputDO) {
            var result = new ResultDO();

            if (possibleDuplicatesInputDO != null) {
                try {
                    session = GetSession();
                    result.list = Service.GetPossibleDuplicates(possibleDuplicatesInputDO, ref session);
                    if (possibleDuplicatesInputDO.ReportId == 0) {
                        result.reportId = session.ReportID;
                        result.reportFile = session.ReportFilePath;
                    }
                    ResetTokenHourExpire();
                    result.errorMessage = session.ErrorMessage;
                } catch (Exception e) {
                    Logger.Error(e.Message, e);
                    result.errorMessage = "Error retrieving possible duplicates report data";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // need to work on this
        [System.Web.Mvc.HttpGet]
        public JsonResult IsAuthenticated() {
            Logger.Debug("IsAuthenticated " + DisplayName + " " + DateTime.Now + " TokenHourExpire:" + TokenHourExpire + "  Token24HourExpire:" + Token24HourExpire);
            var authenticated = "yes";
            if ((DateTime.Now > TokenHourExpire) || (DateTime.Now > Token24HourExpire)) {
                Logger.Debug("Token expired, resetting token");
                Token = null;
                authenticated = null;
            } else if ((DateTime.Now.AddMinutes(16) > TokenHourExpire) || (DateTime.Now.AddMinutes(16) > Token24HourExpire)) {
                authenticated = null;
                session = GetSession();
                if (session == null) {
                    session = new SessionDO();
                    session.DisplayName = DisplayName;
                    session.Username = PersonId;
                    session.AccessToken = Token;
                    session.Token24HourExpire = Token24HourExpire;
                    session.TokenHourExpire = TokenHourExpire;
                    Logger.Debug("KeepSessionAlive: " + DisplayName + " session = null");
                }
                Service.GetCurrentPerson(ref session);
                ResetTokenHourExpire();
            }
            return Json(authenticated, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult KeepSessionAlive() {
//            Logger.Error("KeepSessionAlive IsAuthenticated " + DisplayName + " " + DateTime.Now + " TokenHourExpire:" + TokenHourExpire + "  Token24HourExpire:" + Token24HourExpire);
            //if ((DateTime.Now.AddMinutes(16) > TokenHourExpire) || (DateTime.Now.AddMinutes(16) > Token24HourExpire)) {
            session = GetSession();
            try {
                Service.GetCurrentPerson(ref session);
                ResetTokenHourExpire();
            } catch (Exception e) {
                if (session.StatusCode.Equals(HttpStatusCode.Unauthorized.ToString())) {
                    ClearLoginInfo();
                }
                throw;
            }
            return null;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Retrieve() {
            return PartialView("~/Views/Research/Retrieve.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult RetrieveData(RetrieveFamilySearchDO retrieveFamilySearchDO) {
            var research = new ResearchDO();
            var retrieve = new RetrieveDO();
            try {
                session = GetSession();
                research.CurrentPersonId = PersonId;
                research.PersonId = retrieveFamilySearchDO.PersonId;
                research.PersonName = retrieveFamilySearchDO.Name;
                research.Generation = retrieveFamilySearchDO.Generation;
                research.ResearchType = retrieveFamilySearchDO.ResearchType;
                research.AddChildren = retrieveFamilySearchDO.AddChildren;
                research.SearchCriteria = 0;
                session.Action = Constants.ACTION_RETRIEVE;
                Service.GetPersonAncestryValidations(ref research, ref session);
                ResetTokenHourExpire();
                retrieve.errorMessage = session.ErrorMessage;
                if (!session.Error) {
                    retrieve.ReportId = research.ReportId;
                    retrieve.RetrievedRecords = research.RetrievedRecords;
                }
            } catch (Exception) {
                retrieve.errorMessage = "Error retrieving family search data";
            }

            return Json(retrieve, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult SendFeedback(FeedbackDO feedbackDO) {
            var result = new ResultDO();
            result.message = "We really appreciate your feedback.";
            try {
                session = GetSession();
                Service.SendFeedback(feedbackDO, ref session);
            } catch (Exception) {
                result.errorMessage = "Error sending feedback";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Index() {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Research() {
            Logger.Debug("Entered Research");

            var researchDO = new ResearchDO();
            session = GetSession();
            //           Logger.Debug("Entered Research" + session.AccessToken + " " + session.TokenHourExpire + " " + session.Token24HourExpire);
            //  getCurrentPerson(ref session);
            //            if (session.Authenticated) {
            //                getCurrentPerson(ref session);
            //                Logger.Debug("authenticated = " + session.Authenticated + "; token = " + session.AccessToken); //; isChurchMember = " + Service.IsChurchMember(ref session));
            //            }
            bool isAuthenticated = session.Authenticated;
            if (!isAuthenticated) {
                ClearLoginInfo();
            }

            try {
                if (!isLocal) {
                    //                   Logger.Debug("Entered Research" + session.AccessToken + " " + session.TokenHourExpire + " " + session.Token24HourExpire);
                    if (!isAuthenticated) {
                        var code = Request.QueryString["code"];
                        //                     Logger.Debug("!IsAuthenticated Code = " + code);
                        if (string.IsNullOrEmpty(code)) {
                            //                        Logger.Debug("Response.Redirect(GetRedirectUrl()) = " + GetRedirectUrl());
                            try {
                                Response.Redirect(GetRedirectUrl(), true);
                                Response.End();
                            } catch (Exception) {
                                RedirectToAction("Research", "Home");
                            }
                        }
                        return OAuthCallback(code);
                    }
                } else {
                    if (!isAuthenticated) {
                        if (isFamilySearchOnline) {
                            session = null;
                            if (Constants.FAMILY_SEARCH_SYSTEM.Equals(Constants.FAMILY_SEARCH_SYSTEM_BETA)) {
                                session = Service.Authenticate(Constants.USERNAME_DEV, Constants.PASSWORD_DEV, Constants.DEVELOPER_ID_DEV);
                            } else if (Constants.FAMILY_SEARCH_SYSTEM.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                                session = Service.Authenticate(Constants.USERNAME_DEV, Constants.PASSWORD_DEV, Constants.DEVELOPER_ID_DEV);
                            } else {
                                session = Service.Authenticate(Constants.USERNAME_PROD, Constants.PASSWORD_PROD, Constants.DEVELOPER_ID_PROD);
                            }
                            UpdateToken(session);
                            getCurrentPerson(ref session);
                        }
                    }
                }
                AddCookie("FamilySearchSystem", Constants.FAMILY_SEARCH_SYSTEM);
                if (session == null) {
                    Logger.Debug("session == null");
                    session = new SessionDO();
                } else {
                    if (!session.Error && session.Authenticated && !string.IsNullOrEmpty(session.AccessToken)) {
                        if (session.CurrentPerson == null) {
                            session.CurrentPerson = new PersonDO();
                            session.CurrentPerson.Id = PersonId;
                            session.CurrentPerson.Fullname = DisplayName;
                            researchDO.PersonId = PersonId;
                        } else {
                            session.CurrentPerson.Id = PersonId;
                            session.CurrentPerson.Fullname = DisplayName;
                            researchDO.PersonId = PersonId;
                        }
                    } else if (session.CurrentPerson != null) {
                        researchDO.PersonId = null; //session.CurrentPerson.Id;
                    }
                }
            } catch (Exception e) {
                Logger.Error("Error executing Research: " + e.ToString(), e);
                return Redirect("Home/Research");
            }

            return View("~/Views/Research/Research.cshtml", researchDO);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult SearchCriteriaList() {
            return Json(AncestryHelper.GetSearchCriteriaList(), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetReportList() {
            return Json(GetReports(), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult SubscribeEmail(string email) {
            var result = new ResultDO();
            try {
                if (IsEmailValid(email)) {
                    Service.Admin.SubscribeEmail(email, session);
                    result.errorMessage = session.ErrorMessage;
                } else {
                    result.text = "Email is invalid, please try again";
                }
            } catch (Exception) {
                result.errorMessage = "Error subscribing to email";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private ResultDO GetReports() {
            var result = new ResultDO();
            var report = new ReportDO();
            report.ResearchType = "Research";
            report.UserID = PersonId; //"KW71-97V";
            try {
                result.list = PersonServices.Instance.ReadReportsByUser(report);
            } catch (Exception e) {
                Logger.Error(e.Message, e);
                result.errorMessage = "Error retrieving reports: " + e.Message;
            }

            return result;
        }

        private ActionResult OAuthCallback(string code) {
            try {
                Logger.Debug("starting OAuthCallback");
                var error = Request.QueryString["error"];
                var errorDescription = Request.QueryString["error_description"];
                var state = Request.QueryString["state"];
                if (!string.IsNullOrEmpty(code)) {
                    var grantType = Constants.AUTH2_GRANT_TYPE;
                    var data = "code={0}&client_id={1}&grant_type={2}";
                    var request = WebRequest.Create(GetTokenUri()) as HttpWebRequest;
                    string result = null;
                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.ContentType = Constants.AUTH2_CONTENT_TYPE;
                    var param = string.Format(data, code, getClientID(), grantType);
                    var bs = Encoding.UTF8.GetBytes(param);
                    using (var reqStream = request.GetRequestStream()) {
                        reqStream.Write(bs, 0, bs.Length);
                    }

                    using (var response = request.GetResponse()) {
                        if (response != null) {
                            var sr = new StreamReader(response.GetResponseStream());
                            result = sr.ReadToEnd();
                            sr.Close();
                        }
                    }

                    if (!string.IsNullOrEmpty(result)) {
                        var jsonSerializer = new JavaScriptSerializer();
                        var tokenData = jsonSerializer.Deserialize<FamilySearchTokenData>(result);
                        Token = tokenData.Access_Token;
                        Logger.Debug("Token " + Token);
                        session = new SessionDO(tokenData.Access_Token, tokenData.Error + " " + tokenData.Error_Description);
                        getCurrentPerson(ref session);
                        if (session.Error) {
                            Logger.Debug("session.error = " + session.Error);
                            Logger.Debug("authenticated = " + session.Authenticated + "; token = " + session.AccessToken);
                        } else {
                            UpdateToken(session);
                        }
                    }
                }
                return RedirectToAction("Research");
            } catch (Exception e) {
                Logger.Error("Error occured on OAuthCallback: " + e.Message, e);
                return Redirect("Home/Research");
            }
        }

        public static List<SelectListItem> GetReports(ArrayList reports) {
            var reportSelectList = new List<SelectListItem>();
            foreach (ReportDO report in reports) {
                reportSelectList.Add(new SelectListItem {Text = report.ReportFile, Value = report.ReportID.ToString()});
            }

            return reportSelectList;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult DisplayPersonUrls(RetrieveFamilySearchDO retrieveFamilySearchDO) {
            var personInfo = new PersonInfoDO();
            try {
                session = GetSession();
                var researchDO = new ResearchDO();
                researchDO.CurrentPersonId = PersonId;
                researchDO.PersonId = retrieveFamilySearchDO.PersonId;
                researchDO.Generation = 1;
                personInfo.IncludeMaidenName = retrieveFamilySearchDO.IncludeMaidenName;
                personInfo.IncludeMiddleName = retrieveFamilySearchDO.IncludeMiddleName;
                personInfo.IncludePlace = retrieveFamilySearchDO.IncludePlace;
                personInfo.YearRange = retrieveFamilySearchDO.YearRange;
                personInfo.Person = Service.GetPersonInformation(researchDO, ref session);
                ResetTokenHourExpire();
                personInfo.errorMessage = session.ErrorMessage;
            } catch (Exception) {
                personInfo.errorMessage = "Error with displaying person urls";
            }

            return PartialView("~/Views/Research/PersonUrls.cshtml", personInfo);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult FindPerson() {
            return PartialView("~/Views/Research/FindPerson.cshtml");
        }

        public ActionResult PersonUrlOptions() {
            return PartialView("~/Views/Research/PersonUrlOptions.cshtml");
        }

        public ActionResult ResearchFamily() {
            return PartialView("~/Views/Research/ResearchFamily.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult DisplayPerson() {
            session = GetSession();
            var person = session.CurrentPerson;

            return PartialView("~/Views/Research/DisplayPerson.cshtml", person);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult FindPersons(string personId, string firstName, string lastName, string gender, string birthYear, string deathYear) {
            var result = new ResultDO();
            try {
                var session = GetSession();
                personId = personId.ToUpper();
                var personDO = new PersonDO();
                if (!string.IsNullOrEmpty(personId)) {
                    personDO = Service.GetPerson(personId, ref session);
                    ResetTokenHourExpire();
                } else {
                    personDO.Id = personId;
                    personDO.Firstname = firstName;
                    personDO.Lastname = lastName;
                    if (!string.IsNullOrEmpty(birthYear)) {
                        personDO.BirthYear = Convert.ToInt16(birthYear);
                    }
                    if (!string.IsNullOrEmpty(deathYear)) {
                        personDO.DeathYear = Convert.ToInt16(deathYear);
                    }
                    personDO.Gender = gender.ToUpper();
                }

                result.list = Service.FindPersons(personDO, ref session);
                ResetTokenHourExpire();
                result.errorMessage = session.ErrorMessage;
            } catch (Exception) {
                result.errorMessage = "Error with finding persons";
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public LogReader myLogReader {
            get {
                if (HttpContext.Cache["LogReader"] == null) {
                    HttpContext.Cache["LogReader"] = new LogReader();
                }
                return (LogReader) HttpContext.Cache["LogReader"];
            }
            set {
                HttpContext.Cache["LogReader"] = value;
            }
        }

        public ActionResult Logomatic() {
            var ddlLogFolders = ConfigurationReader.ReadLogFolders().Select(x => new SelectListItem {Text = x.Name, Value = x.Name}).ToList();
            var viewModel = new HomeModel {LogFolderChoices = ddlLogFolders};
            return View(viewModel);
        }

        [System.Web.Http.HttpGet]
        public ActionResult GetEntries(string currentLogFolder, string searchTerm, int page, bool reload) {
            if (string.IsNullOrEmpty(currentLogFolder)) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            if (reload) {
                myLogReader = new LogReader();
            }

            var logFolders = ConfigurationReader.ReadLogFolders();
            var folderEntry = logFolders.FirstOrDefault(x => x.Name == currentLogFolder);

            if (folderEntry == null) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            //Load Log Folder (if not already loaded)
            if (myLogReader.LogPathsLoaded.Contains(folderEntry.Path) != true) {
                myLogReader.LoadLogFolder(folderEntry.Name, folderEntry.Path);
            }

            var results = myLogReader.GroupedLogEntries.ToList();

            //Apply Search Critera if provided
            if (string.IsNullOrEmpty(searchTerm) != true) {
                results = results.Where(x => x.ErrorMessage.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || x.ErrorDetail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var pageCount = results.Count / ITEMSPERPAGE;
            if ((results.Count % ITEMSPERPAGE) > 0) {
                pageCount++;
            }

            //Apply Paging
            var skip = (page - 1) * ITEMSPERPAGE;
            results = results.Skip(skip).Take(ITEMSPERPAGE).ToList();

            return Json(new {Items = results, PageCount = pageCount}, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        public void RemoveEntries(string currentLogFolder) {
            myLogReader.RemoveEntries(currentLogFolder);
        }

        // GET api/logentries/5
        public string Get(int id) {
            return "value";
        }

        // POST api/logentries
        public void Post([FromBody] string value) {
        }

        // PUT api/logentries/5
        public void Put(int id, [FromBody] string value) {
        }

        // DELETE api/logentries/5
        public void Delete(string id) {
            myLogReader.ClearLogEntriesInGroup(id);
        }
    }
}