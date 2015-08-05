using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using FindMyFamiles.Services.Data;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Helper;
using FindMyFamilies.Services;
using FindMyFamilies.Util;
using Gx;
using log4net;
using ProtoBuf;

namespace FindMyFamilies.Web.Controllers {

    public class HomeController : ControllerBase {
        private readonly ILog Logger = LogManager.GetLogger(typeof (HomeController));

        public HomeController() {
        }

        public ActionResult Logomatic() {
            List<SelectListItem> ddlLogFolders = ConfigurationReader.ReadLogFolders().Select(x => new SelectListItem {Text = x.Name, Value = x.Name}).ToList();

            var viewModel = new HomeModel {LogFolderChoices = ddlLogFolders,};

            return View(viewModel);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult StartingPoint() {
            return PartialView("~/Views/Research/StartingPoint.cshtml");
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
            List<StartingPointListItemDO> startingPoints = new List<StartingPointListItemDO>();
            if (Request.IsAjaxRequest() && (startingPoint != null) && (startingPoint.Id != null)) {
                session = GetSession();
                startingPoints = Service.GetStartingPoints(startingPoint, ref session);
                checkAuthentication();
            }

            return Json(startingPoints, JsonRequestBehavior.AllowGet);
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
        public JsonResult DateProblemsReportData(DateInputDO dateProblem) {
            List<DateListItemDO> dateProblems = new List<DateListItemDO>();
            if (Request.IsAjaxRequest() && (dateProblem != null)) {
                session = GetSession();
                dateProblems = Service.GetDates(dateProblem, ref session);
                checkAuthentication();
            }

            return Json(dateProblems, JsonRequestBehavior.AllowGet);
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
        public JsonResult PlaceProblemsReportData(PlaceInputDO place) {
            List<PlaceListItemDO> placeProblems = new List<PlaceListItemDO>();
            if (Request.IsAjaxRequest() && (place != null)) {
                session = GetSession();
                placeProblems = Service.GetPlaces(place, ref session);
                checkAuthentication();
            }

            return Json(placeProblems, JsonRequestBehavior.AllowGet);
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
        public JsonResult FindCluesReportData(FindCluesInputDO findClue) {
            List<AnalyzeListItemDO> findClues = new List<AnalyzeListItemDO>();
            if (Request.IsAjaxRequest() && (findClues != null)) {
                session = GetSession();
                findClues = Service.GetAnalyzeData(findClue, ref session);
                checkAuthentication();
            }

            return Json(findClues, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult IncompleteOrdinances() {
            return PartialView("~/Views/Research/IncompleteOrdinances.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult IncompleteOrdinancesReportHtml() {
            return PartialView("~/Views/Research/IncompleteOrdinancesReport.cshtml");
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult IncompleteOrdinancesReportData(IncompleteOrdinanceDO incompleteOrdinance) {
            List<OrdinanceListItemDO> incompleteOrdinances = new List<OrdinanceListItemDO>();
            if (Request.IsAjaxRequest() && (incompleteOrdinance != null)) {
                session = GetSession();
                incompleteOrdinances = Service.GetOrdinances(incompleteOrdinance, ref session);
                checkAuthentication();
            }

            return Json(incompleteOrdinances, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Feedback() {
            return PartialView("~/Views/Research/Feedback.cshtml");
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
        public JsonResult HintsReportData(HintInputDO hintInput) {
            List<HintListItemDO> hints = new List<HintListItemDO>();
            if (Request.IsAjaxRequest() && (hintInput != null)) {
                session = GetSession();
                hints = Service.GetHints(hintInput, ref session);
                checkAuthentication();
            }

            return Json(hints, JsonRequestBehavior.AllowGet);
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
            List<PossibleDuplicateListItemDO> possibleDuplicates = new List<PossibleDuplicateListItemDO>();
            if (Request.IsAjaxRequest() && (possibleDuplicatesInputDO != null)) {
                session = GetSession();
                possibleDuplicates = Service.GetPossibleDuplicates(possibleDuplicatesInputDO, ref session);
                checkAuthentication();
            }

            return Json(possibleDuplicates, JsonRequestBehavior.AllowGet);
        }

        private string GetIncludeInfo(ResearchDO researchDO) {
            string includeInfo = "ancestors";
            if (researchDO.ResearchType.Equals(Constants.RESEARCH_TYPE_DESCENDANTS)) {
                includeInfo = "descendants";
            } else if (researchDO.ResearchType.Equals(Constants.RESEARCH_TYPE_DESCENDANTS)) {
                includeInfo = "both ancestors and descendants";
            }
            return includeInfo;
        }

        private string GetStartingAt(ResearchDO researchDO) {
            string startAt = "";
            switch (researchDO.StartAt) {
                case 1:
                    startAt = "FamilySearch ID: " + researchDO.PersonId;
                    break;
                case 2:
                    startAt = "2nd Generation";
                    break;
                case 3:
                    startAt = "3rd Generation";
                    break;
                case 4:
                    startAt = "4th Generation";
                    break;
                case 5:
                    startAt = "5th Generation";
                    break;
            }

            return startAt;
        }

        [System.Web.Mvc.HttpPost]
        public JsonResult Startup(LoginDO login) {
            session = new SessionDO();
            session.DisplayName = login.DisplayName;
            session.Username = login.PersonId;
            session.AccessToken = login.Token;
            if (!string.IsNullOrEmpty(login.Token24HourExpire)) {
                _Token24HourExpire = Convert.ToDateTime(login.Token24HourExpire);
            }
            if (!string.IsNullOrEmpty(login.TokenHourExpire)) {
                _TokenHourExpire = Convert.ToDateTime(login.TokenHourExpire);
            }
            return null;
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult IsAuthenticated() {
            Logger.Debug("IsAuthenticated " + DisplayName + " " + DateTime.Now + " TokenHourExpire:" + TokenHourExpire + "  Token24HourExpire:" + Token24HourExpire);
            string authenticated = "yes";
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
            Logger.Error("KeepSessionAlive IsAuthenticated " + DisplayName + " " + DateTime.Now + " TokenHourExpire:" + TokenHourExpire + "  Token24HourExpire:" + Token24HourExpire);
            //if ((DateTime.Now.AddMinutes(16) > TokenHourExpire) || (DateTime.Now.AddMinutes(16) > Token24HourExpire)) {
                session = GetSession();
                if (session == null) {
                    session = new SessionDO();
                    session.DisplayName = DisplayName;
                    session.Username = PersonId;
                    session.AccessToken = Token;
                    session.Token24HourExpire = Token24HourExpire;
                    session.TokenHourExpire = TokenHourExpire;
                    Logger.Error("KeepSessionAlive: " + DisplayName + " session = null");
                }
                Service.GetCurrentPerson(ref session);
                ResetTokenHourExpire();
            //}
//            DateTime? tokenHourDataTime = TokenHourExpire;
//            DateTime? token24HourDataTime = Token24HourExpire;
//            bool expired = TokenExpired;
//            if (session != null) {
//                Service.GetCurrentPerson(ref session);
//                session.ResetExpiration();
//                Logger.Error("KeepSessionAlive: " + DateTime.Now);
//            }
            return null;
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult Retrieve() {
            return PartialView("~/Views/Research/Retrieve.cshtml");
        }

    [System.Web.Mvc.HttpGet]
        public JsonResult RetrieveData(RetrieveFamilySearchDO retrieveFamilySearchDO) {
            session = GetSession();
            var research = new ResearchDO();
            var retrieve = new RetrieveDO();
            if (Request.IsAjaxRequest()) {
                try {
                    research.CurrentPersonId = PersonId;
                    research.PersonId = retrieveFamilySearchDO.PersonId;
                    research.PersonName = retrieveFamilySearchDO.Name;
                    research.Generation = retrieveFamilySearchDO.Generation;
                    research.ResearchType = retrieveFamilySearchDO.ResearchType;
                    research.AddChildren = retrieveFamilySearchDO.AddChildren;
                    research.SearchCriteria = 0;
                    session.Action = Constants.ACTION_RETRIEVE;
                    Service.GetPersonAncestryValidations(ref research, ref session);
                    retrieve.ReportId = research.ReportId;
                    retrieve.RetrievedRecords = research.RetrievedRecords;
                }
                catch (Exception e) {
                    Logger.Error("Error trying to retrieve data. " + e.Message, e);
                    throw;
                }
            }
            return Json(retrieve, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult ValidatePersonId(string personId) {
            var personInfo = new PersonInfoDO();
            session = GetSession();
            if (session.Authenticated) {
                PersonDO person = Service.GetPerson(personId.ToUpper(), ref session);
                checkAuthentication();
                if (person == null) {
                    personInfo.Message = personId + " does not exist on familysearch.org";
                } else {
                    personInfo.Name = person.Fullname;
                }
            }

            return Json(personInfo, JsonRequestBehavior.AllowGet);
        }

        private string getFilePath(ResearchDO research) {
            string filePath = "";

            foreach (ListItemDO listItem in GetReports().Cast<ListItemDO>().ToList()) {
                if (listItem.ValueMember.Equals(research.ReportId.ToString())) {
                    var report = new ReportDO();
                    report.ReportId = Convert.ToInt32(listItem.ValueMember);
                    report = PersonServices.Instance.ReadReport(report);
                    checkAuthentication();
                    filePath = report.ReportFile;
                    break;
                }
            }
            return filePath;
        }

        public ActionResult Index() {
            return View("~/Views/Home/Index.cshtml");
        }

        public ActionResult Research() {
            Logger.Debug("Entered Research");

            var researchDO = new ResearchDO();
            session = GetSession();
            Logger.Debug("Entered Research" + session.AccessToken + " " + session.TokenHourExpire + " " + session.Token24HourExpire);
          //  getCurrentPerson(ref session);
            if (session.Authenticated) {
                getCurrentPerson(ref session);
                Logger.Debug("authenticated = " + session.Authenticated + "; token = " + session.AccessToken); //; isChurchMember = " + Service.IsChurchMember(ref session));
            }
            try {
                if (!isLocal) {
                    Logger.Debug("Entered Research" + session.AccessToken + " " + session.TokenHourExpire + " " + session.Token24HourExpire);
                    if (!session.Authenticated) {
                        string code = Request.QueryString["code"];
                        Logger.Debug("!IsAuthenticated Code = " + code);
                        if (string.IsNullOrEmpty(code)) {
                            Logger.Debug("Response.Redirect(GetRedirectUrl()) = " + GetRedirectUrl());
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
                    if (!session.Authenticated) {
                        if (isFamilySearchOnline) {
                            session = null;
                            if (Constants.FAMILY_SEARCH_SYSTEM.Equals(Constants.FAMILY_SEARCH_SYSTEM_BETA)) {
                                session = Service.Authenticate(Constants.USERNAME_DEV, Constants.PASSWORD_DEV, Constants.DEVELOPER_ID_DEV);
                            } else if (Constants.FAMILY_SEARCH_SYSTEM.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                                session = Service.Authenticate(Constants.USERNAME_DEV, Constants.PASSWORD_DEV, Constants.DEVELOPER_ID_DEV);
                            } else {
                                session = Service.Authenticate(Constants.USERNAME_PROD, Constants.PASSWORD_PROD, Constants.DEVELOPER_ID_PROD);
                            }
                            if (session.Authenticated) {
                                Token = session.AccessToken;
                                getCurrentPerson(ref session);
                                if (session.Error) {
                                    Logger.Debug("session.error = " + session.Error);
                                    Logger.Debug("authenticated = " + session.Authenticated + "; token = " + session.AccessToken);
                                }
                                researchDO.PersonId = PersonId;
                            } else {
                                ClearLoginInfo();
                            }
                        }
                    }
                }
                AddCookie("FamilySearchSystem", Constants.FAMILY_SEARCH_SYSTEM);
                if (session == null) {
                    Logger.Debug("session == null");
                    session = new SessionDO();
                } else {
                    if (session.Authenticated && session.CurrentPerson == null) {
                        Logger.Debug("session.CurrentPerson == null");
                        getCurrentPerson(ref session);
                        researchDO.PersonId = null; //session.CurrentPerson.Id;
                    } else if (session.CurrentPerson != null) {
                        researchDO.PersonId = PersonId;
                        Logger.Debug("person id = " + PersonId + " name: " + DisplayName);
                    }
                }
            }
            catch (Exception e) {
                Logger.Error("Error executing Research: " + e.Message, e);
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
            string message = "";
            if (IsEmailValid(email)) {
                Service.Admin.SubscribeEmail(email, session);
            } else {
                message = "Email is invalid, please try again";
            }

            return Json(message, JsonRequestBehavior.AllowGet);
        }

        private ArrayList GetReports() {
            var report = new ReportDO();
            report.ReportType = "Research";
            report.ReportBy = PersonId; //"KW71-97V";
            var reports = (ArrayList) PersonServices.Instance.ReadReportsByReport(report);

            return reports;
        }

        private ActionResult OAuthCallback(string code) {
            try {
                Logger.Debug("starting OAuthCallback");
                string error = Request.QueryString["error"];
                string errorDescription = Request.QueryString["error_description"];
                string state = Request.QueryString["state"];
                if (!string.IsNullOrEmpty(code)) {
                    string grantType = Constants.AUTH2_GRANT_TYPE;
                    string data = "code={0}&client_id={1}&grant_type={2}";
                    var request = WebRequest.Create(GetTokenUri()) as HttpWebRequest;
                    string result = null;
                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.ContentType = Constants.AUTH2_CONTENT_TYPE;
                    string param = string.Format(data, code, getClientID(), grantType);
                    byte[] bs = Encoding.UTF8.GetBytes(param);
                    using (Stream reqStream = request.GetRequestStream()) {
                        reqStream.Write(bs, 0, bs.Length);
                    }

                    using (WebResponse response = request.GetResponse()) {
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
                            Logger.Error("session.error = " + session.Error);
                            Logger.Error("authenticated = " + session.Authenticated + "; token = " + session.AccessToken);
                        }
                    }
                }
                return RedirectToAction("Research");

            } catch (Exception e) {
                Logger.Error("Error occured on OAuthCallback: " + e.Message, e);
                return Redirect("Home/Research");
            }
        }

        public static List<SelectListItem> GetGapsInChildren() {
            var gapsInChildren = new List<SelectListItem>();
            gapsInChildren.Add(new SelectListItem {Text = "3", Value = "3"});
            gapsInChildren.Add(new SelectListItem {Text = "4", Value = "4"});
            gapsInChildren.Add(new SelectListItem {Text = "5", Value = "5"});
            gapsInChildren.Add(new SelectListItem {Text = "6", Value = "6"});
            gapsInChildren.Add(new SelectListItem {Text = "7", Value = "7"});
            gapsInChildren.Add(new SelectListItem {Text = "8", Value = "8"});
            gapsInChildren.Add(new SelectListItem {Text = "9", Value = "9"});

            return gapsInChildren;
        }

        public static List<SelectListItem> GetReports(ArrayList reports) {
            var reportSelectList = new List<SelectListItem>();
            foreach (ReportDO report in reports) {
                reportSelectList.Add(new SelectListItem {Text = report.ReportFile, Value = report.ReportId.ToString()});
            }

            return reportSelectList;
        }

        public static List<SelectListItem> GetActions() {
            var actions = new List<SelectListItem>();
            actions.Add(new SelectListItem {Text = "Search for couples without children", Value = "1"});
            return actions;
        }

        #region Nested type: FamilySearchTokenData

        private class FamilySearchTokenData {
            public string Access_Token {
                get;
                set;
            }

            public string Error {
                get;
                set;
            }

            public string Error_Description {
                get;
                set;
            }
        }

        #endregion


        private const int ITEMSPERPAGE = 10;

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
  
        [System.Web.Http.HttpGet]
        public ActionResult GetEntries(string currentLogFolder, string searchTerm, int page, bool reload) {

            if (string.IsNullOrEmpty(currentLogFolder)) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            if (reload) {
                myLogReader = new LogReader();
            }

            List<ConfigurationReader.LogPathEntry> logFolders = ConfigurationReader.ReadLogFolders();
            ConfigurationReader.LogPathEntry folderEntry = logFolders.FirstOrDefault(x => x.Name == currentLogFolder);

            if (folderEntry == null) {
                return Json(new {}, JsonRequestBehavior.AllowGet);
            }

            //Load Log Folder (if not already loaded)
            if (myLogReader.LogPathsLoaded.Contains(folderEntry.Path) != true) {
                myLogReader.LoadLogFolder(folderEntry.Name, folderEntry.Path);
            }

            List<LogGrouping> results = myLogReader.GroupedLogEntries.ToList();

            //Apply Search Critera if provided
            if (string.IsNullOrEmpty(searchTerm) != true) {
                results = results.Where(x => x.ErrorMessage.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || x.ErrorDetail.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            int pageCount = results.Count / ITEMSPERPAGE;
            if ((results.Count % ITEMSPERPAGE) > 0) {
                pageCount++;
            }

            //Apply Paging
            int skip = (page - 1) * ITEMSPERPAGE;
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

        [System.Web.Mvc.HttpGet]
        public ActionResult DisplayPersonUrls(RetrieveFamilySearchDO retrieveFamilySearchDO) {
            session = GetSession();
            var personInfo = new PersonInfoDO();
            var researchDO = new ResearchDO();
            if (Request.IsAjaxRequest()) {
                try {
                    researchDO.CurrentPersonId = PersonId;
                    researchDO.PersonId = retrieveFamilySearchDO.PersonId;
                    researchDO.Generation = 1;
                    personInfo.IncludeMaidenName = retrieveFamilySearchDO.IncludeMaidenName;
                    personInfo.IncludeMiddleName = retrieveFamilySearchDO.IncludeMiddleName;
                    personInfo.IncludePlace = retrieveFamilySearchDO.IncludePlace;
                    personInfo.YearRange = retrieveFamilySearchDO.YearRange;
                    personInfo.Person = Service.GetPersonInformation(researchDO, ref session);
                    checkAuthentication();
                } catch (Exception e) {
                    return new HttpStatusCodeResult(401, e.Message);
                }
            }

            if (personInfo.Person == null) {
                Logger.Debug("RetrievePersonInfo personInfo == null. ");
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
        public ActionResult ResearchFamily(string personId) {
            PersonDO person = GetPersonInCache(personId);

            var personInfoDO = new PersonInfoDO();

            return PartialView("~/Views/Research/ResearchFamily.cshtml", personInfoDO);
        }

        [System.Web.Mvc.HttpGet]
        public ActionResult DisplayPerson() {
            session = GetSession();
            PersonDO person = session.CurrentPerson;

            return PartialView("~/Views/Research/DisplayPerson.cshtml", person);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetAncestorList(string personId) {
            var researchDO = new ResearchDO();
            researchDO.PersonId = personId;
            return Json(GetAncestors(researchDO), JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult getAncestorsBornBetween18101850(ResearchDO researchDO) {
            SessionDO session = GetSession();
            if (string.IsNullOrEmpty(researchDO.PersonId)) {
                researchDO.PersonId = PersonId;
            }
            List<SelectListItemDO> ancestorList = Service.getAncestorsBornBetween18101850(researchDO, ref session);
            var ancestors = new AncestorsDO();
            ancestors.Ancestors = ancestorList;

            if (ancestorList.Count > 0) {
                int count = ancestorList.Count;
                var random = new Random();
                int randomNumber = random.Next(0, count);
                SelectListItemDO item = ancestorList[randomNumber];
                ancestors.Id = item.id;
                ancestors.Text = item.text;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetAncestorByPersonId(string personId) {
            personId = personId.ToUpper();
            var ancestors = new AncestorsDO();
            SessionDO session = GetSession();
            PersonDO person = Service.GetPerson(personId, ref session);
            checkAuthentication();
            if ((person != null) && !person.IsEmpty) {
                ancestors.Ancestors.Add(new SelectListItemDO(person.Id, person.Id + " - " + person.Fullname));
                ancestors.Id = person.Id;
                ancestors.Text = person.Id + " - " + person.Fullname;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult FindPersons(string personId, string firstName, string lastName, string gender, string birthYear, string deathYear) {
            SessionDO session = GetSession();
            personId = personId.ToUpper();
            var personDO = new PersonDO();
            if (!string.IsNullOrEmpty(personId)) {
                personDO = Service.GetPerson(personId, ref session);
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
            List<FindListItemDO> persons = Service.FindPersons(personDO, ref session);

            //            checkAuthentication();
            //            if ((person != null) && !person.IsEmpty) {
            //                ancestors.Ancestors.Add(new SelectListItemDO(person.Id, person.Id + " - " + person.Fullname));
            //                ancestors.Id = person.Id;
            //                ancestors.Text = person.Id + " - " + person.Fullname;
            //            }

            return Json(persons, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetAncestors(ResearchDO researchDO) {
            SessionDO session = GetSession();
            if (string.IsNullOrEmpty(researchDO.PersonId)) {
                researchDO.PersonId = PersonId;
            }
            List<SelectListItemDO> ancestorList = PersonServices.Instance.GetAncestorsForPersonInfo(researchDO, ref session);
            checkAuthentication();
            var ancestors = new AncestorsDO();
            ancestors.Ancestors = ancestorList;

            if ((ancestorList != null) && ancestorList.Count > 0) {
                SelectListItemDO item = ancestorList[0];
                ancestors.Id = item.id;
                ancestors.Text = item.text;
            }

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult GetDescendants(string personId) {
            SessionDO session = GetSession();
            var researchDO = new ResearchDO();
            if (string.IsNullOrEmpty(personId)) {
                personId = PersonId;
            }
            researchDO.PersonId = personId;
            researchDO.Generation = 2;
            List<SelectListItemDO> descendantList = PersonServices.Instance.GetDescendantsForPersonInfo(researchDO, ref session);
            checkAuthentication();

            var descendants = new DescendantsDO();
            descendants.Descendants = descendantList;

            if (descendantList.Count > 0) {
                SelectListItemDO item = descendantList[0];
                descendants.Id = item.id;
                descendants.Text = item.text;
            }

            return Json(descendants, JsonRequestBehavior.AllowGet);
        }

        [System.Web.Mvc.HttpGet]
        public JsonResult RefreshDescendantsForPersonInfo(string personId) {
            SessionDO session = GetSession();
            var researchDO = new ResearchDO();
            if (string.IsNullOrEmpty(personId)) {
                personId = PersonId;
            }
            researchDO.PersonId = personId;
            researchDO.Generation = 7;
            researchDO.Refresh = true;
            List<SelectListItemDO> ancestors = PersonServices.Instance.GetAncestorsForPersonInfo(researchDO, ref session);
            checkAuthentication();

            return Json(ancestors, JsonRequestBehavior.AllowGet);
        }
    }
}