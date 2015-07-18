define(function(require) {
    var USER = "user";
    var USER_ID = "PersonId";
    var USER_NAME = "DisplayName";

    var DOMAIN_NAME = "findmyfamilies.com"; // we replace this by our production domain.
    var PERSON = "person";
    var FIND_PERSON = "findPerson";
    var STARTING_POINT_PREVIOUS = "startingPointPrevious";
    var POSSIBLE_DUPLICATES_PREVIOUS = "possibleDuplicatesPrevious";
    var CLOSE = "close";
    var DIALOG_CLOSE = "dialogclose";


    // Default values
    var ANCESTORS = "Ancestors";
    var DESCENDANTS = "Descendants";
    var GENERATION = "3";
    var REPORT_ID = "0";
    var HISTORY_MAX = 5;
    var YEAR_RANGE = 2;
    var MALE = "Male";
    var FEMALE = "Female";
    var SELECT = "Select";
    var BUTTON_ATTR = "btn-u btn-brd btn-brd-hover rounded btn-u-blue";

    // Forms
    var STARTING_POINT = "startingPoint";
    var STARTING_POINT_REPORT = "startingPointReport";
    var POSSIBLE_DUPLICATES = "possibleDuplicates";

    // Urls
    var GET_REPORT_LIST_URL = '/Home/GetReportList';
    var STARTING_POINT_REPORT_HTML_URL = '/Home/StartingPointReportHtml';
    var KEEP_SESSION_ALIVE_URL = '/Home/KeepSessionAlive';
    var STARTING_POINT_URL = '/Home/StartingPoint';
    var STARTING_POINT_REPORT_DATA_URL = '/Home/StartingPointReportData';
    var STARTING_POINT_HTML = '/Home/StartingPointReportHtml';
    var POSSIBLE_DUPLICATES_REPORT_HTML_URL = '/Home/PossibleDuplicatesReportHtml';
    var FIND_PERSON_URL = '/Home/FindPerson';
    var RETRIEVE_DATA_URL = '/Home/Retrieve';
    var DISPLAY_PERSON_URLS_URL = "/Home/DisplayPersonUrls";
    var PERSON_URL_OPTIONS_URL = "/Home/PersonUrlOptions";
    var INCOMPLETE_ORDINANCES_URL = "/Home/IncompleteOrdinances";
    var FIND_CLUES_URL = "/Home/FindClues";
    var POSSIBLE_DUPLICATES_URL = "/Home/PossibleDuplicates";
    var DATE_PROBLEMS_URL = "/Home/DateProblems";
    var PLACE_PROBLEMS_URL = "/Home/PlaceProblems";
    var HINTS_URL = "/Home/Hints";
    var DISPLAY_PERSON_URL = "/Home/DisplayPerson";
    var FIND_PERSONS_URL = "/Home/FindPersons";
    var FIND_PERSON_OPTIONS_URL = "/Home/FindPersonOptions";

    var FIND_A_GRAVE = "http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr";
    var GOOGLE = "https://www.google.com/#q=";
    var ANCESTRY = "http://search.ancestry.com/cgi-bin/sse.dll?gl=allgs&gss=sfs28_ms_r_f-2_s&new=1&rank=1";
    var BILLION_GRAVES = "http://billiongraves.com/pages/search/#";
    var MY_HERITAGE = "http://www.myheritage.com/research?formId=master&formMode=&action=query&exactSearch=0&useTranslation=0&catId=1";
    var FIND_MY_PAST = "http://search.findmypast.com/search/united-states-records?";
    var AMERICAN_ANCESTOR = "";

    // Spinner
    var DEFAULT_SPINNER_AREA = 'spinnerArea';
    var SPINNER_OPTIONS = {
        lines: 13, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        rotate: 0, // The rotation offset
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false, // Whether to use hardware acceleration
        className: 'fmf-spinner', // The CSS class to assign to the spinner
        zIndex: 2e9, // The z-index (defaults to 2000000000)
        top: 'auto', // Top position relative to parent in px
        left: 'auto' // Left position relative to parent in px
    };

    var constants = {
        get USER() {
            return USER;
        },
        get USER_ID() {
            return USER_ID;
        },
        get USER_NAME() {
            return USER_NAME;
        },
        get DOMAIN_NAME() {
            return DOMAIN_NAME;
        },
        get PERSON() {
            return PERSON;
        },
        get FIND_PERSON() {
            return FIND_PERSON;
        },
        get RESEARCH_TYPE() {
            return ANCESTORS;
        },
        get GENERATION() {
            return GENERATION;
        },
        get ANCESTORS() {
            return ANCESTORS;
        },
        get DESCENDANTS() {
            return DESCENDANTS;
        },
        get REPORT_ID() {
            return REPORT_ID;
        },
        get HISTORY_MAX() {
            return HISTORY_MAX;
        },
        get YEAR_RANGE() {
            return YEAR_RANGE;
        },
        get MALE() {
            return MALE;
        },
        get FEMALE() {
            return FEMALE;
        },
        get STARTING_POINT() {
            return STARTING_POINT;
        },
        get STARTING_POINT_REPORT() {
            return STARTING_POINT_REPORT;
        },
        get STARTING_POINT_URL() {
            return STARTING_POINT_URL;
        },
        get STARTING_POINT_PREVIOUS() {
            return STARTING_POINT_PREVIOUS;
        },
        get GET_REPORT_LIST_URL() {
            return GET_REPORT_LIST_URL;
        },
        get STARTING_POINT_REPORT_HTML_URL() {
            return STARTING_POINT_REPORT_HTML_URL;
        },
        get CLOSE() {
            return CLOSE;
        },
        get DIALOG_CLOSE() {
            return DIALOG_CLOSE;
        },
        get SELECT() {
            return SELECT;
        },
        get DEFAULT_SPINNER_AREA() {
            return DEFAULT_SPINNER_AREA;
        },
        get SPINNER_OPTIONS() {
            return SPINNER_OPTIONS;
        },
        get KEEP_SESSION_ALIVE_URL() {
            return KEEP_SESSION_ALIVE_URL;
        },
        get POSSIBLE_DUPLICATES_URL() {
            return POSSIBLE_DUPLICATES_URL;
        },
        get FIND_CLUES_URL() {
            return FIND_CLUES_URL;
        },
        get INCOMPLETE_ORDINANCES_URL() {
            return INCOMPLETE_ORDINANCES_URL;
        },
        get PERSON_URL_OPTIONS_URL() {
            return PERSON_URL_OPTIONS_URL;
        },
        get DISPLAY_PERSON_URLS_URL() {
            return DISPLAY_PERSON_URLS_URL;
        },
        get RETRIEVE_DATA_URL() {
            return RETRIEVE_DATA_URL;
        },
        get FIND_PERSON_URL() {
            return FIND_PERSON_URL;
        },
        get FIND_PERSONS_URL() {
            return FIND_PERSONS_URL;
        },
        get FIND_PERSON_OPTIONS_URL() {
            return FIND_PERSON_OPTIONS_URL;
        },
        get DISPLAY_PERSON_URL() {
            return DISPLAY_PERSON_URL;
        },
        get HINTS_URL() {
            return HINTS_URL;
        },
        get PLACE_PROBLEMS_URL() {
            return PLACE_PROBLEMS_URL;
        },
        get DATE_PROBLEMS_URL() {
            return DATE_PROBLEMS_URL;
        },
        get BUTTON_ATTR() {
            return BUTTON_ATTR;
        },
        get POSSIBLE_DUPLICATES_PREVIOUS() {
            return POSSIBLE_DUPLICATES_PREVIOUS;
        },
        get POSSIBLE_DUPLICATES() {
            return POSSIBLE_DUPLICATES;
        },
        get FIND_A_GRAVE() {
            return FIND_A_GRAVE;
        },
        get GOOGLE() {
            return GOOGLE;
        },
        get ANCESTRY() {
            return ANCESTRY;
        },
        get BILLION_GRAVES() {
            return BILLION_GRAVES;
        },
        get MY_HERITAGE() {
            return MY_HERITAGE;
        },
        get FIND_MY_PAST() {
            return FIND_MY_PAST;
        },
        get AMERICAN_ANCESTOR() {
            return AMERICAN_ANCESTOR;
        },
        get STARTING_POINT_REPORT_DATA_URL() {
            return STARTING_POINT_REPORT_DATA_URL;
        },
        get STARTING_POINT_HTML() {
            return STARTING_POINT_HTML;
        },
        get POSSIBLE_DUPLICATES_REPORT_HTML_URL() {
            return POSSIBLE_DUPLICATES_REPORT_HTML_URL;
}

        
    }
    return constants;
});