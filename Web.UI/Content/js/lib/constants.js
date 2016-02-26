define(function(require) {
    var USER = "user";
    var USER_ID = "PersonId";
    var USER_NAME = "DisplayName";
    var USER_EMAIL = "Email";

    var DOMAIN_NAME = "findmyfamilies.com"; // we replace this by our production domain.
    var PERSON = "person";
    var RETRIEVE = "Retrieve";
    var FIND_PERSON = "findPerson";
    var GOOGLE_SEARCH = "googleSearch";
    var STARTING_POINT_PREVIOUS = "startingPointPrevious";
    var HINTS_PREVIOUS = "hintsPrevious";
    var DATE_PROBLEMS_PREVIOUS = "dateProblemsPrevious";
    var ORDINANCES_PREVIOUS = "ordinancesPrevious";
    var POSSIBLE_DUPLICATES_PREVIOUS = "possibleDuplicatesPrevious";
    var FIND_CLUES_PREVIOUS = "findCluesPrevious";
    var PLACE_PROBLEMS_PREVIOUS = "placeProblemsPrevious";
    var CLOSE = "close";
    var DIALOG_CLOSE = "dialogclose";
    var FIND_CLUES = "findClues";
    var POSSIBLE_DUPLICATES_REPORT = "POSSIBLE_DUPLICATES_REPORT";


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

    var LAST_CALLED = "lastCalled";

    // Forms
    var STARTING_POINT = "startingPoint";
    var STARTING_POINT_REPORT = "startingPointReport";
    var HINTS = "hints";
    var HINTS_REPORT = "hintsReport";
    var FEEDBACK = "feedback";
    var FEATURES = "features";
    var DATE_PROBLEMS = "dateProblems";
    var DATE_PROBLEMS_REPORT = "dateProblemsReport";
    var ORDINANCES = "ordinances";
    var ORDINANCES_REPORT = "ordinancesReport";
    var POSSIBLE_DUPLICATES = "possibleDuplicates";
    var POSSIBLE_DUPLICATES_REPORT = "possibleDuplicatesReport";
    var PLACE_PROBLEMS = "placeProblems";
    var PLACE_PROBLEMS_REPORT = "placeProblemsReport";
    var FIND_CLUES_REPORT = "findCluesReport";

    // Urls
    var GET_REPORT_LIST_URL = '/Home/GetReportList';
    var FIND_CLUES_URL = "/Home/FindClues";
    var FIND_CLUES_REPORT_HTML_URL = "/Home/FindCluesReportHtml";
    var FIND_CLUES_REPORT_DATA_URL = "/Home/FindCluesReportData";
    var KEEP_SESSION_ALIVE_URL = '/Home/KeepSessionAlive';
    var STARTING_POINT_URL = '/Home/StartingPoint';
    var STARTING_POINT_REPORT_HTML_URL = '/Home/StartingPointReportHtml';
    var STARTING_POINT_REPORT_DATA_URL = '/Home/StartingPointReportData';
    var HINTS_URL = '/Home/Hints';
    var HINTS_REPORT_HTML_URL = '/Home/HintsReportHtml';
    var HINTS_REPORT_DATA_URL = '/Home/HintsReportData';
    var FEEDBACK_URL = '/Home/Feedback';
    var SEND_FEEDBACK_URL = '/Home/SendFeedback';
    var FEATURES_URL = '/Home/Features';
    var POSSIBLE_DUPLICATES_URL = '/Home/PossibleDuplicates';
    var POSSIBLE_DUPLICATES_REPORT_HTML_URL = '/Home/PossibleDuplicatesReportHtml';
    var POSSIBLE_DUPLICATES_REPORT_DATA_URL = '/Home/PossibleDuplicatesReportData';
    var DATE_PROBLEMS_URL = '/Home/DateProblems';
    var DATE_PROBLEMS_REPORT_HTML_URL = '/Home/DateProblemsReportHtml';
    var DATE_PROBLEMS_REPORT_DATA_URL = '/Home/DateProblemsReportData';
    var ORDINANCES_URL = '/Home/Ordinances';
    var ORDINANCES_REPORT_HTML_URL = '/Home/OrdinancesReportHtml';
    var ORDINANCES_REPORT_DATA_URL = '/Home/OrdinancesReportData';
    var PLACE_PROBLEMS_URL = '/Home/PlaceProblems';
    var PLACE_PROBLEMS_REPORT_HTML_URL = '/Home/PlaceProblemsReportHtml';
    var PLACE_PROBLEMS_REPORT_DATA_URL = '/Home/PlaceProblemsReportData';
    var FIND_PERSON_URL = '/Home/FindPerson';
    var GOOGLE_SEARCH_URL = '/Home/GoogleSearch';
    var DISPLAY_PERSON_URLS_URL = "/Home/DisplayPersonUrls";
    var PERSON_URL_OPTIONS_URL = "/Home/PersonUrlOptions";
    var DISPLAY_PERSON_URL = "/Home/DisplayPerson";
    var FIND_PERSONS_URL = "/Home/FindPersons";
    var FIND_PERSON_OPTIONS_URL = "/Home/FindPersonOptions";
    var RETRIEVE_URL = '/Home/Retrieve';
    var RETRIEVE_DATA_URL = '/Home/RetrieveData';
    var SEARCH_CRITERIA_LIST_URL = "/Home/SearchCriteriaList";
    var RESEARCH_URL = "/Home/Research";
    var GET_PERSON_INFO = "/Home/GetPersonInfo";

    var FIND_A_GRAVE = "http://www.findagrave.com/cgi-bin/fg.cgi?page=gsr";
    var GOOGLE = "https://www.google.com/#q=";
    var ANCESTRY = "http://search.ancestry.com/cgi-bin/sse.dll?gss=angs-g&new=1&rank=1";
    var BILLION_GRAVES = "http://billiongraves.com/pages/search/#";
    var MY_HERITAGE = "http://www.myheritage.com/research?formId=master&formMode=&action=query&exactSearch=0&useTranslation=0&catId=1";
    var FIND_MY_PAST = "http://search.findmypast.com/search/united-states-records?";
    var AMERICAN_ANCESTORS = "http://www.americanancestors.org/search/database-search?";

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
        get USER_EMAIL() {
            return USER_EMAIL;
        },
        get DOMAIN_NAME() {
            return DOMAIN_NAME;
        },
        get PERSON() {
            return PERSON;
        },
        get RETRIEVE() {
            return RETRIEVE;
        },
        get FIND_PERSON() {
            return FIND_PERSON;
        },
        get GOOGLE_SEARCH() {
            return GOOGLE_SEARCH;
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
        get STARTING_POINT_REPORT_DATA_URL() {
            return STARTING_POINT_REPORT_DATA_URL;
        },
        get STARTING_POINT_HTML() {
            return STARTING_POINT_HTML;
        },
        get POSSIBLE_DUPLICATES() {
            return POSSIBLE_DUPLICATES;
        },
        get POSSIBLE_DUPLICATES_REPORT() {
            return POSSIBLE_DUPLICATES_REPORT;
        },
        get POSSIBLE_DUPLICATES_URL() {
            return POSSIBLE_DUPLICATES_URL;
        },
        get POSSIBLE_DUPLICATES_PREVIOUS() {
            return POSSIBLE_DUPLICATES_PREVIOUS;
        },
        get POSSIBLE_DUPLICATES_REPORT_HTML_URL() {
            return POSSIBLE_DUPLICATES_REPORT_HTML_URL;
        },
        get POSSIBLE_DUPLICATES_REPORT_DATA_URL() {
            return POSSIBLE_DUPLICATES_REPORT_DATA_URL;
        },
        get POSSIBLE_DUPLICATES_HTML() {
            return POSSIBLE_DUPLICATES_HTML;
        },
        get HINTS() {
            return HINTS;
        },
        get HINTS_REPORT() {
            return HINTS_REPORT;
        },
        get HINTS_URL() {
            return HINTS_URL;
        },
        get HINTS_PREVIOUS() {
            return HINTS_PREVIOUS;
        },
        get HINTS_REPORT_HTML_URL() {
            return HINTS_REPORT_HTML_URL;
        },
        get HINTS_REPORT_DATA_URL() {
            return HINTS_REPORT_DATA_URL;
        },
        get HINTS_HTML() {
            return HINTS_HTML;
        },
        get FEEDBACK() {
            return FEEDBACK;
        },
        get FEEDBACK_URL() {
            return FEEDBACK_URL;
        },
        get FEEDBACK_HTML() {
            return FEEDBACK_HTML;
        },
        get FEATURES() {
            return FEATURES;
        },
        get FEATURES_URL() {
            return FEATURES_URL;
        },
        get FEATURES_HTML() {
            return FEATURES_HTML;
        },
        get ORDINANCES() {
            return ORDINANCES;
        },
        get ORDINANCES_REPORT() {
            return ORDINANCES_REPORT;
        },
        get ORDINANCES_URL() {
            return ORDINANCES_URL;
        },
        get ORDINANCES_PREVIOUS() {
            return ORDINANCES_PREVIOUS;
        },
        get ORDINANCES_REPORT_HTML_URL() {
            return ORDINANCES_REPORT_HTML_URL;
        },
        get ORDINANCES_REPORT_DATA_URL() {
            return ORDINANCES_REPORT_DATA_URL;
        },
        get ORDINANCES_HTML() {
            return ORDINANCES_HTML;
        },
        get DATE_PROBLEMS() {
            return DATE_PROBLEMS;
        },
        get DATE_PROBLEMS_REPORT() {
            return DATE_PROBLEMS_REPORT;
        },
        get DATE_PROBLEMS_URL() {
            return DATE_PROBLEMS_URL;
        },
        get DATE_PROBLEMS_PREVIOUS() {
            return DATE_PROBLEMS_PREVIOUS;
        },
        get DATE_PROBLEMS_REPORT_HTML_URL() {
            return DATE_PROBLEMS_REPORT_HTML_URL;
        },
        get DATE_PROBLEMS_REPORT_DATA_URL() {
            return DATE_PROBLEMS_REPORT_DATA_URL;
        },
        get DATE_PROBLEMS_HTML() {
            return DATE_PROBLEMS_HTML;
        },
        get PLACE_PROBLEMS() {
            return PLACE_PROBLEMS;
        },
        get PLACE_PROBLEMS_REPORT() {
            return PLACE_PROBLEMS_REPORT;
        },
        get PLACE_PROBLEMS_URL() {
            return PLACE_PROBLEMS_URL;
        },
        get PLACE_PROBLEMS_PREVIOUS() {
            return PLACE_PROBLEMS_PREVIOUS;
        },
        get PLACE_PROBLEMS_REPORT_HTML_URL() {
            return PLACE_PROBLEMS_REPORT_HTML_URL;
        },
        get PLACE_PROBLEMS_REPORT_DATA_URL() {
            return PLACE_PROBLEMS_REPORT_DATA_URL;
        },
        get PLACE_PROBLEMS_HTML() {
            return PLACE_PROBLEMS_HTML;
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
        get FIND_CLUES_URL() {
            return FIND_CLUES_URL;
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
        get GOOGLE_SEARCH_URL() {
            return GOOGLE_SEARCH_URL;
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
        get BUTTON_ATTR() {
            return BUTTON_ATTR;
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
        get AMERICAN_ANCESTORS() {
            return AMERICAN_ANCESTORS;
        },
        get AMERICAN_ANCESTOR() {
            return AMERICAN_ANCESTOR;
        },
        get RETRIEVE_URL() {
            return RETRIEVE_URL;
        },
        get FIND_CLUES() {
            return FIND_CLUES;
        },
        get FIND_CLUES_PREVIOUS() {
            return FIND_CLUES_PREVIOUS;
        },
        get FIND_CLUES_REPORT_HTML_URL() {
            return FIND_CLUES_REPORT_HTML_URL;
        },
        get SEARCH_CRITERIA_LIST_URL() {
            return SEARCH_CRITERIA_LIST_URL;
        },
        get FIND_CLUES_REPORT_DATA_URL() {
            return FIND_CLUES_REPORT_DATA_URL;
        },
        get RESEARCH_URL() {
            return RESEARCH_URL;
        },
        get SEND_FEEDBACK_URL() {
            return SEND_FEEDBACK_URL;
        },
        get LAST_CALLED() {
            return LAST_CALLED;
        },
        get FIND_CLUES_REPORT() {
            return FIND_CLUES_REPORT;
        },
        get GET_PERSON_INFO() {
            return GET_PERSON_INFO;
        }
        

        
    }
    return constants;
});