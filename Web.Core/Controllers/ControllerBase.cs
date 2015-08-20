using System;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using FindMyFamiles.Services.Data;
using FindMyFamilies.Data;
using FindMyFamilies.DataAccess;
using FindMyFamilies.Services;
using FindMyFamilies.Util;
using log4net;
using log4net.Config;
using ProtoBuf;
using HttpException = System.Web.HttpException;

namespace FindMyFamilies.Web.Controllers {
    public class ControllerBase : Controller {
        protected ServiceManager _Service;
        protected bool isFamilySearchOnline;
        protected bool isLocal;
        protected PersonInfoDO personInfoDo;
        protected PersonsDO persons;
        protected SessionDO session;
        protected HttpCookie userInfoCookies;
        private readonly ILog Logger = LogManager.GetLogger(typeof (ControllerBase));

        public string ReportFilePath {
            get;
            set;
        }

        public string DisplayName {
            get;
            set;
        }

        public string Token {
            get;
            set;
        }

        public DateTime? TokenHourExpire {
            get;
            set;
        }

        public DateTime? Token24HourExpire {
            get;
            set;
        }

        public string PersonId {
            get;
            set;
        }

        public ControllerBase() {
            XmlConfigurator.Configure();

            //            DateTime timeNow = DateTime.Now;
            //           TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            //           var easternTimeNow = TimeZoneInfo.ConvertTime(timeNow, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString();
            //
            //            var datestring = easternTimeNow.ToString();

            isLocal = System.Web.HttpContext.Current.Request.IsLocal;
            isFamilySearchOnline = true;
            personInfoDo = new PersonInfoDO();
            session = GetSession();
            var cfg = (CompilationSection) ConfigurationManager.GetSection("system.web/compilation");
            AddCookie("debugging", cfg.Debug.ToString());

            if (session.Authenticated) {
                personInfoDo.Name = DisplayName;
                personInfoDo.PersonId = PersonId;
            } else {
                personInfoDo.Name = "";
                personInfoDo.PersonId = "";
            }
            Logger.Debug("layout.Name = " + personInfoDo.Name);
            Logger.Debug("layout.PersonId = " + personInfoDo.PersonId);
            FilePathHelper.exceedsFileCount();
            UpdateLayout();
        }

        public ServiceManager Service {
            get {
                return ServiceManager.Instance;
            }
        }

        public bool TokenExpired {
            get {
                var expired = false;
                var now = DateTime.Now;
                if ((now > TokenHourExpire) || (now > Token24HourExpire)) {
                    expired = true;
                }
                return expired;
            }
        }

        public void ResetTokenHourExpire() {
            TokenHourExpire = DateTime.Now.AddMinutes(120);
            AddCookie("TokenHourExpire", TokenHourExpire.Value.ToString("O"));

            //            if (Token24HourExpire == null) {
            //                Token24HourExpire = DateTime.Now.AddHours(24);
            //            }
            //            Logger.Debug("ResetTokenHourExpire " + DisplayName + ": " + TokenHourExpire.Value);
        }

        public void ResetToken24HourExpire() {
            Logger.Debug("ResetToken24HourExpire");
            Token24HourExpire = DateTime.Now.AddHours(24);
            Logger.Debug("TokenHourExpire: " + TokenHourExpire);
            if (TokenHourExpire == null) {
                TokenHourExpire = DateTime.Now.AddMinutes(120);
            }
            //            Logger.Debug("Token24HourExpire1 " + DisplayName + ": 1" + Token24HourExpire.Value);
        }

        protected SessionDO GetSession() {
            if (session == null) {
                session = new SessionDO();

                session.ServerPath = HttpRuntime.AppDomainAppPath;
                session.DisplayName = GetCookie("DisplayName");
                session.Username = GetCookie("PersonId");
                session.AccessToken = GetCookie("Token");
                Token = session.AccessToken;
                DisplayName = session.DisplayName;
                PersonId = session.Username;

                var _tokenHourExpire = GetCookie("TokenHourExpire");
                if (!string.IsNullOrEmpty(_tokenHourExpire)) {
                    session.TokenHourExpire = DateTime.Parse(_tokenHourExpire);
                    TokenHourExpire = session.TokenHourExpire;
                } else {
                    TokenHourExpire = null;
                }

                var _token24HourExpire = GetCookie("Token24HourExpire");
                if (!string.IsNullOrEmpty(_token24HourExpire)) {
                    session.Token24HourExpire = DateTime.Parse(_token24HourExpire);
                    Token24HourExpire = session.Token24HourExpire;
                } else {
                    Token24HourExpire = null;
                }
            } else {
                session.DisplayName = DisplayName;
                session.Username = PersonId;
                session.AccessToken = Token;
                session.Token24HourExpire = Token24HourExpire;
                session.TokenHourExpire = TokenHourExpire;
            }
            return session;
        }

        protected void ClearLoginInfo() {
            session = new SessionDO();
            personInfoDo.Name = "";
            personInfoDo.PersonId = "";
            DisplayName = "";
            PersonId = "";
            RemoveCookie("Token");
            Token = null;
            RemoveCookie("TokenHourExpire");
            TokenHourExpire = null;
            RemoveCookie("Token24HourExpire");
            Token24HourExpire = null;

            UpdateLayout();
        }

        public ActionResult Relogin() {
            Logger.Debug("starting Relogin ...");
            ClearLoginInfo();
            Logger.Debug("RedirectToAction(\"Research\", \"Home\")");
            return RedirectToAction("Research", "Home");
        }

        public ActionResult Logout() {
            try {
                if (session != null) {
                    new PersonDAO().Logout(ref session);
                }
            } catch (Exception) {
                Logger.Error("Logout error trying to logout and invalidate token");
            }
            ClearLoginInfo();

            return View("~/Views/Home/Index.cshtml");
        }

        // “https://ident.familysearch.org/cis-web/oauth2/v3/authorization?client_id=71WM-S8P4-W22Z-S6SQ-Z6Q3-N68M-XJVZ-1W8Q&redirect_uri=http%3A%2F%2Fwww.findmyfamilies.com%2Fhome%2Fsearchancestry&response_type=code”;
        // https://ident.familysearch.org/cis-web/oauth2/v3/authorization?client_id=71WM-S8P4-W22Z-S6SQ-Z6Q3-N68M-XJVZ-1W8Q&redirect_uri=http://findmyfamilies.com/Home/SearchAncestry&response_type=code
        protected string GetRedirectUrl() {
            Logger.Debug("startomg GetRedirectUrl ...");
            var redirectUrl = HttpContext.Request.Url.ToString();
            Logger.Debug("redirectUrl1 = " + redirectUrl);
            redirectUrl = redirectUrl.ToLower();
            if (redirectUrl.IndexOf("://www.") < 0) {
                redirectUrl = redirectUrl.Replace("://www.", "://");
            }
            redirectUrl = redirectUrl.Replace(":", "%3A");
            redirectUrl = redirectUrl.Replace("/", "%2F");
            redirectUrl = GetAuthorizationUri() + "?client_id=" + getClientID() + "&redirect_uri=" + redirectUrl + "&response_type=code";
            Logger.Debug("redirectUrl2 = " + redirectUrl);
            return redirectUrl;
        }

        protected string GetAuthorizationUri() {
            string authorizationUri = null;
            var familySearchSystem = Constants.FAMILY_SEARCH_SYSTEM;
            if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                authorizationUri = "https://sandbox.familysearch.org" + Constants.AUTH2_AUTHORIZATION_URI;
            } else if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_BETA)) {
                authorizationUri = "https://identbeta.familysearch.org" + Constants.AUTH2_AUTHORIZATION_URI;
            } else if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_PRODUCTION)) {
                authorizationUri = "https://ident.familysearch.org/" + Constants.AUTH2_AUTHORIZATION_URI;
            }
            Logger.Debug("authorizationUri = " + authorizationUri);
            return authorizationUri;
        }

        protected string GetTokenUri() {
            string tokenUri = null;
            var familySearchSystem = Constants.FAMILY_SEARCH_SYSTEM;
            if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                tokenUri = "https://sandbox.familysearch.org" + Constants.AUTH2_TOKEN_URI;
            } else if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_BETA)) {
                tokenUri = "https://identbeta.familysearch.org" + Constants.AUTH2_TOKEN_URI;
            } else if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_PRODUCTION)) {
                tokenUri = "https://ident.familysearch.org/" + Constants.AUTH2_TOKEN_URI;
            }
            Logger.Debug("tokenUri = " + tokenUri);
            return tokenUri;
        }

        protected string getClientID() {
            var clientId = Constants.CLIENT_ID_PROD;

            var familySearchSystem = Constants.FAMILY_SEARCH_SYSTEM;
            if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                clientId = Constants.CLIENT_ID_DEV;
            }

            Logger.Debug("client id = " + clientId);
            return clientId;
        }

        protected string getClientSecret() {
            var clientSecret = Constants.CLIENT_SECRET;

            var familySearchSystem = Constants.FAMILY_SEARCH_SYSTEM;
            if (familySearchSystem.Equals(Constants.FAMILY_SEARCH_SYSTEM_SANDBOX)) {
                clientSecret = Constants.CLIENT_SECRET;
            }
            Logger.Debug("clientSecret = " + clientSecret);
            return clientSecret;
        }

        protected override void HandleUnknownAction(string actionName) {
            try {
                View(actionName).ExecuteResult(ControllerContext);
            } catch (Exception ex) {
                Logger.Error("Not Found: " + ex.Message, ex);
                throw new HttpException(404, "Not Found: " + ex.Message, ex);
            }
        }

        protected void UpdateLayout() {
            personInfoDo.Name = DisplayName;
            personInfoDo.PersonId = PersonId;
            ViewData["PersonInfo"] = personInfoDo;
        }

        protected void getCurrentPerson(ref SessionDO session) {
            try {
                if (!session.Error && !string.IsNullOrEmpty(session.AccessToken)) {
                    var currentPersonGx = Service.GetCurrentPerson(ref session);
                    if (!session.Error && session.Authenticated && (currentPersonGx != null) && ((currentPersonGx.Persons != null) && (currentPersonGx.Persons.Count > 0))) {
                        foreach (var person in currentPersonGx.Persons) {
                            session.CurrentPerson = Service.GetPerson(person);
                            PersonId = session.CurrentPerson.Id;
                            DisplayName = session.CurrentPerson.Firstname + " " + session.CurrentPerson.Lastname;
                            AddCookie("DisplayName", DisplayName);
                            AddCookie("PersonId", PersonId);
                            UpdateLayout();
                            break;
                        }
                    } else {
                        ClearLoginInfo();
                    }
                }
            } catch (Exception) {
                ClearLoginInfo();
                throw;
            }
        }

        protected PersonsDO GetPersonsInCache() {
            if ((persons == null) || ((persons != null) && (!ReportFilePath.Equals(session.ReportFilePath)))) {
                if (session.ReportFilePath != null) {
                    using (var file = System.IO.File.OpenRead(session.ReportFilePath)) {
                        persons = Serializer.Deserialize<PersonsDO>(file);
                        ReportFilePath = session.ReportFilePath;
                    }
                }
            }
            return persons;
        }

        protected PersonDO GetPersonInCache(string personID) {
            session = GetSession();
            var personDO = new PersonDO();
            var personsDO = GetPersonsInCache();
            if ((personsDO != null) && personsDO.Persons.ContainsKey(personID)) {
                personDO = personsDO.Persons[personID];
            } else {
                personDO = Service.GetPerson(personID, ref session);
                checkAuthentication();
            }
            return personDO;
        }

        protected void checkAuthentication() {
            if (!session.Authenticated) {
                ClearLoginInfo();
            } else {
                ResetTokenHourExpire();
            }
        }

        public bool IsEmailValid(string email) {
            if (Regex.IsMatch(email, Constants.EMAIL_PATTERN)) {
                if (Regex.Replace(email, Constants.EMAIL_PATTERN, String.Empty).Length == 0) {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void UpdateToken(SessionDO session) {
            Token = session.AccessToken;
            TokenHourExpire = session.TokenHourExpire;
            Token24HourExpire = session.Token24HourExpire;
            AddCookie("Token", Token);
            AddCookie("Token24HourExpire", session.Token24HourExpire.Value.ToString("O"));
            AddCookie("TokenHourExpire", session.TokenHourExpire.Value.ToString("O"));
        }

        public void AddCookie(string key, string value) {
            RemoveCookie(key);
            var httpCookie = new HttpCookie(key, value);
            httpCookie.Expires = DateTime.Now.AddYears(1);
            Logger.Debug("AddCookie key: " + key + " value: " + value);
            System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
            //            session.ResetExpiration();
        }

        public void RemoveCookie(string key) {
            var httpCookie = System.Web.HttpContext.Current.Response.Cookies[key];
            if (httpCookie != null) {
                System.Web.HttpContext.Current.Response.Cookies.Remove(key);
                httpCookie.Expires = DateTime.Now.AddYears(-30);
                httpCookie.Value = null;
                System.Web.HttpContext.Current.Response.SetCookie(httpCookie);
            }
            httpCookie = System.Web.HttpContext.Current.Response.Cookies[key];
            //            session.ResetExpiration();
        }

        private string GetCookie(string key) {
            var cookieValue = "";
            var request = System.Web.HttpContext.Current.Request.Cookies[key];
            if (request != null) {
                cookieValue = request.Value;
            } else {
                cookieValue = "";
            }
            return cookieValue;
        }
    }
}