using System;
using System.Collections;
using System.Configuration;
using System.IO;

namespace FindMyFamilies.Util {

    public class Constants {

        private static Configuration appConfig;

        private Constants()
            : base() {
        }

        public static Constants Instance() {
            return Nested.instance;
        }

        private class Nested {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() {
            }

            internal static readonly Constants instance = new Constants();

        }

        private static Configuration GetConfig() {
            if (appConfig != null) {
                return appConfig;
            }

            Uri uri = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase));
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(uri.LocalPath, "App.config")
            };
            appConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            return appConfig;
        }

        private static string GetKeyStringValue(string key) {
            return GetConfig().AppSettings.Settings[key].Value;
        }

        private static bool GetKeyBoolValue(string key) {
            bool getKeyBoolValue = false;
            string value = GetConfig().AppSettings.Settings[key].Value;
            if (value.Equals("true")) {
                getKeyBoolValue = true;
            }
            return getKeyBoolValue;
        }

        public static string SESSION = "userSession";
        public static string CLIENT_ID_PROD = "71WM-S8P4-W22Z-S6SQ-Z6Q3-N68M-XJVZ-1W8Q";
        public static string CLIENT_ID_DEV = "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK";
        public static string CLIENT_SECRET = "wiRCccXnq1uyKcXnq1uyK";
        public static string GRANT_TYPE = "authorization_code";
        public static string REDIRECT_URI_RESEARCH = "http://www.findmyfamilies.com/home/research";

        public static string FAMILY_SEARCH_SYSTEM = GetKeyStringValue("FAMILY_SEARCH_SYSTEM");
        public static string FAMILY_SEARCH_SYSTEM_SANDBOX = "https://sandbox.familysearch.org";
        public static string FAMILY_SEARCH_SYSTEM_BETA = "https://beta.familysearch.org";
        public static string FAMILY_SEARCH_SYSTEM_PRODUCTION = "https://familysearch.org";

        public static string CONNECTION_STRING = GetKeyStringValue("CONNECTION_STRING");
        public static string DISCOVERY_PATH = GetKeyStringValue("DISCOVERY_PATH");
        public static string FIND_A_GRAVE = "http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr";
        public static string GOOGLE = "https://www.google.com/#q=";
        public static string ANCESTRY = "http://search.ancestry.com/cgi-bin/sse.dll?gl=allgs&gss=sfs28_ms_r_f-2_s&new=1&rank=1";

// fmf - http://search.ancestry.com/cgi-bin/sse.dll?gl=ROOT_CATEGORY&rank=1        &new=1&so=3&MSAV=1&gss=ms_r_f-2_s&gsfn=Kent&gsln=Roylance&cpxt=0&catBucket=rstp&uidh=6k4&msbdp=0&msddp=0&_83004003-n_xcl=f&cp=0&gl=42&so=2
// anc - http://search.ancestry.com/cgi-bin/sse.dll?gl=allgs&gss=sfs28_ms_f-2_s    &new=1&rank=1&msT=1&gsfn=Kent&gsln=Roylance&MSAV=1&_83004003-n_xcl=f&cp=0&catbucket=rstp&uidh=6k4

        public static string BILLION_GRAVES = "http://billiongraves.com/pages/search/#";
        public static string MY_HERITAGE = "http://www.myheritage.com/research?formId=master&formMode=&action=query&exactSearch=0&useTranslation=0&catId=1";
        public static string FIND_MY_PAST = "http://search.findmypast.com/search/united-states-records?";
        public static bool FAMILY_SEARCH_SYSTEM_ONLINE = GetKeyBoolValue("FAMILY_SEARCH_SYSTEM_ONLINE");

        public static string RESEARCH_TYPE_ANCESTORS = "Ancestors";
        public static string RESEARCH_TYPE_DESCENDANTS = "Descendants";
        public static string RESEARCH_TYPE_BOTH = "Both";

    //    public const string AUTH2_AUTHORIZATION = @"http://oauth.net/core/2.0/endpoint/authorize";
        public const string AUTH2_AUTHORIZE = @"http://oauth.net/core/2.0/endpoint/authorize";

        public const string AUTH2_TOKEN = @"http://oauth.net/core/2.0/endpoint/token";
        public const string AUTH2_AUTHORIZATION_URI = "/cis-web/oauth2/v3/authorization";
        public const string AUTH2_TOKEN_URI = "/cis-web/oauth2/v3/token";
        public const string AUTH2_CONTENT_TYPE = "application/x-www-form-urlencoded";
        public const string AUTH2_GRANT_TYPE = "authorization_code";
        public const string TOKEN_PATH = "/OAuth/Token";


//Sandbox	https://sandbox.familysearch.org/cis-web/oauth2/v3/authorization
//Staging	https://identbeta.familysearch.org/cis-web/oauth2/v3/authorization
//Production	https://ident.familysearch.org/cis-web/oauth2/v3/authorization
        

        public const string REPORT_TYPE_RESEARCH = "Research";

        public const string TEMPLATE_ID_PERSON = "pid";
        public const string TEMPLATE_ID_PERSON_SPOUSE = "spid";
        public const string TEMPLATE_ID_CONCLUSION = "cid";
        public const string TEMPLATE_ID_COUPLE_RELATIONSHIP = "crid";
        public const string TEMPLATE_ID_SOURCE_REFERENCE = "srid";
        public const string TEMPLATE_ID_NOTE = "nid";
        public const string TEMPLATE_ID_COLLECTION = "clid";
        public const string TEMPLATE_ID_SOURCE_DESCRIPTION = "sdid";
        public const string TEMPLATE_ID_ARTIFACT = "aid";
        public const string TEMPLATE_ID_PERSONAL_REFERENCE = "prid";
        public const string TEMPLATE_ID_ACCESS_TOKEN = "access_token";
        public const string TEMPLATE_ID_GENERATIONS = "generations";
        public const string TEMPLATE_ID_QUERY = "query";

        public const string MEDIA_TYPE_GEDCOM = "application/x-gedcomx-v1+xml";
        public const string MEDIA_TYPE_ATOM = "application/atom+xml";
        public const string MEDIA_TYPE_JSON = "application/x-fs-v1+json";
        public const string MEDIA_TYPE_XML = "application/x-fs-v1+xml";
        public const string MEDIA_TYPE_GEDCOM_JSON = "application/x-gedcomx-atom+json";

        public const string TEMPLATE_PERSON = "/platform/tree/persons/{pid}";
        public const string TEMPLATE_PERSON_WITH_DETAILS = "/platform/tree/ancestry?person={pid}&generations={generations}&personDetails&marriageDetails{?access_token}";
        public const string TEMPLATE_PERSON_WITH_RELATIONSHIPS = "/platform/tree/persons-with-relationships?person={pid}{?access_token}";
        public const string TEMPLATE_PERSON_ANCESTRY = " /platform/tree/ancestry?person={pid}&generations={generations}&personDetails&marriageDetails{?access_token}";
        public const string TEMPLATE_PERSON_DESCENDANTS = " /platform/tree/descendancy?person={pid}&generations={generations}&personDetails&marriageDetails{?access_token}";
        public const string TEMPLATE_PERSON_CURRENT = " /platform/tree/current-person?access_token={access_token}";
        public const string TEMPLATE_PERSON_SPOUSES = " /platform/tree/persons/{pid}/spouses{?access_token}";
        public const string TEMPLATE_PERSON_DUPLICATES = "/platform/tree/persons/{pid}/matches{?access_token}";
        public const string TEMPLATE_FIND_BY = "/platform/tree/matches{?access_token}";
        public const string TEMPLATE_PERSON_SEARCH = "/platform/tree/search?";



        public const string TEMPLATE_PERSON_HINTS = "/platform/tree/persons/{pid}/matches";
        public const string TEMPLATE_ACCESS_TO_ORDINANCES = "/platform/ordinances/ordinances{?access_token}";

        public const string TEMPLATE_HINTS = "/platform/tree/persons/{pid}/matches?collection={clid}{?access_token}";
        public const string TEMPLATE_PERSON_CHILDREN = " /platform/tree/persons/{pid}/children{?access_token}";
        public const string TEMPLATE_PERSON_PARENTS = " /platform/tree/persons/{pid}/parents{?access_token}";
        public const string TEMPLATE_CHILD_PARENT_RELATIONSHIPS = " /platform/tree/persons/{pid}/fs-child-relationships{?access_token}";
        public const string TEMPLATE_THROTTLE = "/platform/throttled{?access_token}";
        public const string TEMPLATE_PERSON_TEMPLE = "/reservation/v1/person/{pid}?access_token={access_token}";    //{?access_token}";   https://api.familysearch.org
        

        public const string TEMPLATE_PERSON_ANCESTRY_WITH_SPOUSE =
            "/platform/tree/ancestry?person={pid}&spouse={spid}&generations={generations}&personDetails&marriageDetails{?access_token}";

        public const string TEMPLATE_PERSON_ANCESTRY_WITH_SPOUSE_MARRIAGE_DETAILS =
            "/platform/tree/ancestry?person={pid}&spouse={spid}&generations={generations}&personDetails&marriageDetails{?access_token}";

        public const string TEMPLATE_PERSON_DESCENDANCY_WITH_SPOUSE_MARRIAGE_DETAILS =
            "/platform/tree/descendancy?person={pid}&spouse={spid}&generations={generations}&personDetails&marriageDetails{?access_token}";

        public const string TEMPLATE_INVALIDATE_TOKEN = "/cis-web/oauth2/v3/token?access_token={access_token}";
 //       public const string TEMPLATE_INVALIDATE_TOKEN = AUTH2_TOKEN_URI + "?access_token={access_token}";

        public const string TEMPLATE_PARENT_RELATIONSHIPS = "parent-relationships-template";

        public const string TEST_USERNAME = "tuf000204920";
        public const string TEST_PASSWORD = "1234pass";
        public const string TEST_DEVELOPER_ID = "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK";
        public const string USERNAME_DEV = "tuf000204920";
        public const string PASSWORD_DEV = "1234pass";
        public const string DEVELOPER_ID_DEV = "WCQY-7J1Q-GKVV-7DNM-SQ5M-9Q5H-JX3H-CMJK";  // for development


        public const string USERNAME_PROD = "kentroylance";
        public const string PASSWORD_PROD = "disney01";
        public const string DEVELOPER_ID_PROD = "71WM-S8P4-W22Z-S6SQ-Z6Q3-N68M-XJVZ-1W8Q";  // for Staging and Production

        public const string EMAIL_PATTERN =  @"^\s*[\w\-\+_']+(\.[\w\-\+_']+)*\@[A-Za-z0-9]([\w\.-]*[A-Za-z0-9])?\.[A-Za-z][A-Za-z\.]*[A-Za-z]$";

        public const int EXPIRATION = 30;
        public const string MALE = "MALE";
        public const string FEMALE = "FEMALE";
		public const string DATASOURCE_SBF = "SBF";
		public const string DATASOURCE_CERTITAX = "CertiTAX";
		public const string PRIMARAY_KEY_PREFIX = "CLQ";
		private static Hashtable m_ClassTable;
		public const string DEFAULT_PAGE = "Default";
		public const string DEBUG = "Debug";
		//public const string ACTION = "action";
		public const string HOME_PAGE = "Default";
		public const string LANGUAGE_DEFAULT = "en-us";
		public const string GLOBAL = "global";
		public const string LOGIN_PAGE = "Login";
		public const string LOGOUT_PAGE = "Logout";
		public const string USER_PAGE = "UserPage";
		public const string ACTIVITY_PAGE = "ActivityPage";
		public const string CORE_TEAM = "CoreTeam";
		public const string MY_PROJECTS = "myprojects";
		public const string MAIN_ACTION = "mainaction.asp";
		public const string VERIFY_LOGIN = "verifylogin.asp";
		public const string FORGOT_PASSWORD = "ForgotPassword";
		public const string VOLUNTEER = "volunteer";
		public const string APPLY = "apply";
		public const string PAGE_SIZE = "pageSize";
		public const string CALENDAR = "calendar";
		public const int STARFISH_ORGANIZATION = 5;
		public const string TRANSACTION_KEY = "ZID3DaHfQGiNmKl3";

		public const string ERROR_MESSAGES = "ErrorMessages";

		public const string ASSEMBLY_SERVICES = "FindMyFamilies.Services";

		public const string TASK_DAILY_BUILD = "DailyBuild";
		public const string TASK_BUILD_INSTALL = "BuildInstall";
		public const string TASK_UPDATE_DATABASE = "UpdateDatabase";
		public const string TASK_CREATE_RADIXWARE_WEB = "CreateFindMyFamiliesWeb";
		public const string TASK_CREATE_DAILYBUILD_WEB = "CreateDailyBuildWeb";
		public const string TASK_CREATE_RADIXWARE_WEB_DEV = "CreateFindMyFamiliesWebDev";
		public const string TASK_CREATE_DAILYBUILD_WEB_DEV = "CreateDailyBuildWebDev";
		public const string TASK_UPDATE_STARTUP_CONFIG = "UpdateStartupConfig";

		public const string DATA_OBJECT_LIST_ITEM = "ListItemDO";
		public const string DATA_OBJECT_ISSUE = "IssueDO";

		public const string RANK_BASIC = "Basic";
		public const string AUTOSHIP_REGULAR = "Regular";
		public const string PAYMENT_CREDIT_CARD = "Credit Card";

		public const string VALIDATION_CREATE = "Create";
		public const string VALIDATION_UPDATE = "Update";
		public const string VALIDATION_DELETE = "Delete";

		public const string MEMBERSHIP_STATUS = "MembershipStatus";
		public const string AUTOSHIP_TYPE = "AutoshipType";
		public const string PAYMENT_TYPE = "PaymentType";

		public const string MENU_TYPE_MENU = "Menu";
		public const string MENU_TYPE_POPUP = "Popup";

		public const string CUSTOM_TYPE_STRING = "String";
		public const string CUSTOM_TYPE_INTEGER = "Integer";
		public const string CUSTOM_TYPE_NUMBER = "Number";
		public const string CUSTOM_TYPE_DATE = "Date";
		public const string CUSTOM_TYPE_BOOLEAN = "Boolean";

		public const string MESSAGE_TYPE_ERROR = "Error";
		public const string MESSAGE_TYPE_WARNING = "Warning";
		public const string MESSAGE_TYPE_INFO = "Info";

		public const string MODE_NEW = "New";
		public const string MODE_VIEW = "View";
		public const string MODE_UPDATE = "Update";
		public const string MODE_DELETE = "Delete";

		public const string STATUS_NEW = "New";
		public const string STATUS_VIEW = "View";
		public const string STATUS_EDIT = "Edit";
		public const string STATUS_DELETE = "Delete";
		public const string STATUS_DEFAULT = "Default";

		public const string TYPE_CUSTOMER = "Customer";

		public const string CATEGORY_MEMBER = "Member";
		public const string CATEGORY_ISSUE = "Issue";

		public const int INDEX_OF_CUSTOM_FIELD_ID = 11;

		public const string ACTION_CREATE = "Create";
		public const string ACTION_UPDATE = "Update";
		public const string ACTION_CLOSING = "Close";
		public const string ACTION_DELETE = "Delete";
		public const string ACTION_CLEAR = "Clear";
		public const string ACTION_DISABLE = "Disable";
		public const string ACTION_VALIDATE = "Validate";
		public const string ACTION_ENABLE = "Enable";
		public const string ACTION_UNDO = "Undo";
		public const string ACTION_FORCE_CLEAR = "ForceClear";
		public const string ACTION_VERIFY_CHANGES = "VerifyChanges";
		public const string ACTION_COLLECT_VALUES = "CollectValues";
		public const string ACTION_POPULATE = "Populate";
		public const string ACTION_POPULATE_CONTROLS = "PopulateControls";
		public const string ACTION_POPULATE_DATA_OBJECTS = "PopulateDataObjects";
		public const string ACTION_ADD_KEY_DOWN_EVENT = "Keydownevent";
		public const string ACTION_SETUP = "Setup";
		public const string ACTION_SET_DATA_VALUES = "SetDataValues";
		public const string ACTION_ADD_TO_FORM = "AddToForm";

		public const string ADDRESS_TYPE_MAILING = "Mailing";
		public const string ADDRESS_TYPE_BILLING = "Billing";
		public const string ADDRESS_TYPE_SHIPPING = "Shipping";
		public const string ADDRESS_STATUS_DEFAULT = "Default";
		public const string ADDRESS_STATUS_ACTIVE = "Active";
		public const string ADDRESS_STATUS_INACTIVE = "InActive";
		public const string ADDRESS_STATE_UTAH = "Utah";
		public const string ADDRESS_COUNTRY_UNITED_STATES = "United States";
		public const string ADDRESS_ASSOSIATE_TYPE_CONTACT = "Contact";
		public const string ADDRESS_NAME_MAILING = "Mailing";
		
		public const string MEMBER_STATUS_ACTIVE = "Active";
		public const string MEMBER_STATUS_INACTIVE = "InActive";
		public const string MEMBER_TYPE_CONTACT = "Contact";
		
		public const string ASSOCIATED_TYPE_EVENT = "Event";
		public const string ASSOCIATED_TYPE_MEMBER = "Member";

		public const string OPERATION_CREATE = "create";
		public const string OPERATION_READ = "read";
		public const string OPERATION_UPDATE = "update";
		public const string OPERATION_DELETE = "delete";

		public const int ORGANIZATION_RTC = 21;
		public const int ORGANIZATION_STARFISH = 22;

		public const string REGISTER = "register";
		public const string PROJECT_REQUEST = "projectRequest";
		public const string PROFILE = "profile";
		public const string JOIN_MAILER = "joinmailer";
		public const string DONATE = "donate";
		public const string DONATION = "donation";

		// List of actions
		public const string LIST = "list";
		public const string ADD = "add";
		public const string VERIFY = "verify";
		public const string EDIT = "edit";
		public const string SEARCH = "search";
		public const string FIND = "find";
		public const string DELETE = "delete";
		public const string UPDATE = "update";
		public const string VIEW = "view";
		public const string ADMIN = "admin";
		public const string ADD_ADMIN = "addadmin";
		public const string UPDATE_ADMIN = "updateadmin";

		public const string ADD_CHECK = "addcheck";
		public const string EDIT_CHECK = "editcheck";
		public const string RECEIPT = "receipt";

		// constants for lists
		public const string LIST_STATES = "states";
		public const string LIST_PAYMENT_TYPES = "paymentTypes";
		public const string LIST_AMOUNT_TYPES = "amountTypes";
		public const string LIST_MONTH = "month";
		public const string LIST_MONTHS = "months";
		public const string LIST_YEARS = "years";
		public const string LIST_STATUS = "status";
		public const string LIST_PROJECT_STATUS = "projectStatus";

		public const string LIST_EVENT_TYPE = "eventType";
		public const string LIST_TIME = "time";
		public const string LIST_TIME_ZONE = "timeZone";
		public const string LIST_DURATION_TYPE = "durationType";
		public const string LIST_IMPORTANCE = "importance";

		// constants for stored procedures
		public const string SP_PROJECT_LIST = "sf_GetProjectList";
		public const string SP_USER_GROUPS = "sf_GetUserGroups";
		public const string SP_USER_GROUPS_AVAIL = "sf_GetUserGroupsAvailable";
		public const string SP_USER_GROUPS_AVAIL_ALL = "sf_GetUserGroupsAvailableAll";
		public const string EMAIL_STARFISH = "info@starfishsociety.org";
		public const string HEADER_BG = "#397CB1";
		public const string HEADER_FG = "#FFFFFF";
		//public const string COLOR_LINE = "#FFFF9F";
		//public const string COLOR_LINE = "#FEF2C2";
		public const string COLOR_LINE = "#EAF1F7";
		public const string WHITE_LINE = "#FFFFFF";
		public const Boolean DEVELOPMENT = true;

		public const string SESSION_STATE = "session";
		public const string FORM_STATE = "form";
		public const string QUERYSTRING_STATE = "querystring";

		public const string IMAGE_ACTION = "imageaction.asp";
		public const string IMAGE_LIST = "imagelist.asp";
		public const string IMAGE_ADD = "imageadd.asp";
		public const string IMAGE_EDIT = "imageedit.asp";

		public const string USER_ACTION = "/user/useraction.asp";
		public const string USER_LIST = "UserList";
		public const string PROJECT_LIST = "ProjectList";

		public const string USER_ADD = "/user/useradd.asp";
		public const string USER_EDIT = "/user/useredit.asp";
		public const string USER_VERIFY = "/user/userverify.asp";
		public const string USER_CHECK_ADD = "/user/checkuseradd.asp";

		public const string PROFILE_EDIT = "/user/profileedit.asp";
		public const string PROFILE_CONFIRM = "/user/profileconfirm.asp";

		public const string USER_SEARCH_INPUT = "/user/usersearcher.asp";
		public const string MAILER_SEARCH_INPUT = "/user/mailersearcher.asp";

		public const string MAILER_ADD = "/user/maileradd.asp";
		public const string MAILER_LIST = "/user/mailerlist.asp";
		public const string MAILER_EDIT = "/user/maileredit.asp";

		public const string REGISTER_ADD = "registerAdd";
		public const string REGISTER_EDIT = "/user/registeredit.asp";
		public const string REGISTER_COMPLETE = "registercomplete.asp";
		public const string COMPLETE = "complete";

		public const string ORGANIZATION_ACTION = "organizationaction.asp";
		public const string ORGANIZATION_LIST = "organizationlist.asp";
		public const string ORGANIZATION_ADD = "organizationadd.asp";
		public const string ORGANIZATION_EDIT = "organizationedit.asp";
		public const string ORGANIZATION_VERIFY = "organizationverify.asp";

		public const string GROUP_ACTION = "groupaction.asp";
		public const string GROUP_LIST = "grouplist.asp";
		public const string GROUP_ADD = "groupadd.asp";
		public const string GROUP_EDIT = "groupedit.asp";

		public const string DONATE_ACTION = "/donate/donateaction.asp";
		public const string DONATE_LIST = "/donate/donatelist.asp";
		public const string DONATE_ADD = "/donate/donateadd.asp";
		public const string DONATE_VERIFY = "/donate/donateverify.asp";
		public const string DONATE_EDIT = "/donate/donateedit.asp";
		public const string DONATE_ADD_CHECK = "/donate/checkadd.asp";
		public const string DONATE_EDIT_CHECK = "/donate/checkedit.asp";
		public const string DONATE_RECEIPT = "/donate/donatereceipt.asp";

		public const string USERNAME_EXISTS = "Username is already used, try another";
		public const string USER_NOT_FOUND = "Username does not exist.";
		public const string INVALID_PASSWORD = "Password is invalid.";
		public const string INACTIVE_LOGIN = "Your login is inactive.";

		public const int STRING_TYPE = 200;
		public const int NUMBER_TYPE = 3;
		public const int DATE_TYPE = 135;
		public const int CHECKBOX_TYPE = 2;

		public const int GROUP_ADMIN = 1;
		public const int GROUP_EXECUITE_DIRECTOR = 8;
		public const int GROUP_DONOR = 3;
		public const int GROUP_ONSITE_MANAGER = 4;
		public const int GROUP_BOOKKEEPER = 6;
		public const int GROUP_ENRICHMENT_MANAGER = 7;
		public const int GROUP_BOARD_MEMBER = 9;
		public const int GROUP_PROJECT_COORDINATOR = 5;
		public const int GROUP_PROJECT_REQUESTOR = 11;
		public const int GROUP_STARFISH_VOLUNTEER = 12;
		public const int GROUP_PROJECT_REQUEST_INTEREST = 14;
		public const int GROUP_MAILER = 15;
		public const int GROUP_EMAIL_UPDATES = 20;
		public const int GROUP_EMAIL_ENDOW_FUND = 21;
		public const int GROUP_EMAIL_TRUST = 22;
		public const int GROUP_EMAIL_PROJECT_FUNDING = 27;
		public const int GROUP_CORE_TEAM = 23;
		public const int GROUP_RTC_ADMIN = 24;
		public const int GROUP_RTC_VOLUNTEER = 25;
		public const int GROUP_RTC_EMPLOYEE = 26;
		public const int GROUP_STARFISH_ADMIN = 17;

		public const string GROUP_TYPE_SYSTEM = "System";
		public const string GROUP_TYPE_HIDDEN = "Hidden";

		public const string STATUS_ACTIVE = "Active";
		public const string STATUS_INACTIVE = "InActive";

		public const string STATUS_REQUEST = "Request";
		public const string STATUS_PROPOSED = "Proposed";
		public const string STATUS_DONATIONS_NEEDED = "Donations Needed";
		public const string STATUS_IN_PROGRESS = "In Progress";
		public const string STATUS_COMPLETED = "Completed";
		public const string REQUESTS = "requests";

		public const string ACTIVITY_TYPE = "ActivityType";
		public const string ACTIVITY_REPORT = "Report";
		public const string ACTIVITY_APPROVAL = "Approval";
		public const string ACTIVITY_NOT_APPROVAL = "NotApproval";
		public const string ACTIVITY_GET_APPROVALS = "GetApprovals";

		public const string DATE_DEFAULT = "";
		public const string STRING_DEFAULT = "";
		public const int NUMBER_DEFAULT = 0;
		public const int CHECKBOX_DEFAULT = 0;
		public const string DB_EMPTY_DATE = "1/1/1900";
		public const string EMPTY_DATE = "01/01/0001";
		public const string DATE_FORMAT = "MM/dd/yyyy";
		public const string DATETIME_FORMAT = "MM/dd/yyyy HH:mm:ss";
		public const string DATETIME_FORMAT_HM = "MM/dd/yyyy HH:mm";
		//public const string EMPTY_DATE = "";
		public const string TIME_FORMAT="HH:mm:ss";

		public const int UPDATE_INTERVAL = 300;

		// synchronization constants
		public const string TRANSACTION_TYPE_NORMAL = "normal";
		public const string TRANSACTION_TYPE_SYNCHRONIZATION = "synchronization";

		// Calendar constants

		public const string CALENDARTITLE = "Project Events"; //Change this to reflect the title of you Events Calendar.

		public const bool ENABLEAUTODELETE = true; //If true, auto delete expired events according to following period.
		public const int EVENTEXPIRATIONPERIOD = -7; //Number of days after which past events are deleted from database.
		//This MUST be a negative number to specify days in the past.

		public const int MAXYEARSSELECTIONS = 10; //Maximum number of years into the future that may be selected in a pull-down box.
		public const int MAXLOOKAHEADMONTHS = 12; //Maximum number of months to include in the look ahead calendar view.

		// The EzEvents App utilizes a main table to format the app for all pages that are displayed.
		// I call this the Main App Table.  This table consists of two rows.  The first row contains the 
		// a single cell.  That cell contains a table with 1 row and 1 cell that is the app//s header.
		// The background color of the app//s header is controlled by the constant APPHEADERCOLOR.
		// The second row of the Main App Table is used to contain all output of the App.
		// The cell spacing and cell padding of the App Main Table 
		// are controllable through the APPMAINTABLECELLSPACING and APPMAINTABLECELLPADDING                             
		// constants.  The constant APPMAINTABLEBORDER controls the border of the Main App Table ONLY.

		public const string APPHEADERCOLOR = "#eaf1f7"; //"#6CB6FF";
		public const string APPMAINTABLEBORDER = "0";
		public const string APPMAINTABLECELLPADDING = "0";
		public const string APPMAINTABLECELLSPACING = "0";

		// As stated above, the App Header is itself a table of 1 row and 1 cell within the first row
		// of the App Main Table.  The border, cell padding and cell spacing of the header may be
		// controlled through the following three constants.

		public const string APPMAINHEADERBORDER = "0";
		public const string APPMAINHEADERCELLPADDING = "4";
		public const string APPMAINHEADERCELLSPACING = "4";

		public const string CALENDARHEADERCOLOR = "#eaf1f7"; //9DCfFF";   //Color for calendar table header rows

		public const string MONTHNAMETEXTCOLOR = "#000000"; //Color for Month/Year text in calendar table header row

		//Colors to be used for the text displaying the days of week column headings.

		public const string SUNDAYTEXTCOLOR = "#000000";
		public const string MONDAYTEXTCOLOR = "#000000";
		public const string TUESDAYTEXTCOLOR = "#000000";
		public const string WEDNESDAYTEXTCOLOR = "#000000";
		public const string THURSDAYTEXTCOLOR = "#000000";
		public const string FRIDAYTEXTCOLOR = "#000000";
		public const string SATURDAYTEXTCOLOR = "#000000";

		public const string TODAYHIGHLIGHTCOLOR = "#008000"; //Color used to highlight today on the calendar

		public const bool ALLOWEDITDELETE = true; //If true, the user has access to edit/delete rightality.
		//If you wanted to modify the app to provide another page to
		//enter/edit/delete events that was restricted from everyone else
		//(that is, everyone else can only read events) then you set this
		//to false.  Of course, YOU have to create the new page that has
		//restricted access. 

		public const bool ALLOWADD = true; //If true, allow users to add an event.  Again, this would 
		//typically be set the same as ALLOWEDITDELETE according to what
		//access you wish to provide.

		// The following three constants point to the icons used by the app.
		public const string BACKUPONEMONTHIMG = "../images/13_arrow_left_md_wht.gif"; //Backup 1 month icon
		public const string FORWARDONEMONTHIMG = "../images/13_arrow_right_md_wht.gif"; //Advance 1 month icon
		//      public const string BACKUPONEMONTHIMG="/images/16_arrow_left_md_wht.gif";           //Backup 1 month icon
		//      public const string FORWARDONEMONTHIMG="/images/16_arrow_right_md_wht.gif";     //Advance 1 month icon
		//      public const string BACKUPONEMONTHIMG="/images/BackArrow.gif";          //Backup 1 month icon
		//      public const string FORWARDONEMONTHIMG="/images/ForwardArrow.gif";      //Advance 1 month icon

		public static Hashtable ClassTable {
			get {
				if (m_ClassTable == null) {
					m_ClassTable = new Hashtable();
//					m_ClassTable.Add("MemberModel", new ClassInfoDO("FindMyFamilies.Windows.Member", "FindMyFamilies.Windows.Member"));
				}
				return m_ClassTable;
			}
			set {
				m_ClassTable = value;
			}
		}

        public const string ACTION_RETRIEVE = "retrieve";
        public const string ACTION_ANALYZE = "findClues";
        public const string ACTION_RETRIEVE_ANALYZE = "retrieveAnalyze";
    }

}