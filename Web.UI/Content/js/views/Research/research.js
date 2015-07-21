define(function (require) {

    var _startingPointController;
    var _findPersonController;
    var _hintsController;
    var _incompleteOrdinancesController;
    var _startingPointReportController;
    var _possibleDuplicatesController;
    var _retrieveController;
    var _dateProblemsController;


    var research = {
        get startingPointController() {
            return _startingPointController;
        },
        set startingPointController(value) {
            _startingPointController = value;
        },
        get findPersonController() {
            return _findPersonController;
        },
        set findPersonController(value) {
            _findPersonController = value;
        },
        get startingPointReportController() {
            return _startingPointReportController;
        },
        set startingPointReportController(value) {
            _startingPointReportController = value;
        },
        get possibleDuplicatesController() {
            return _possibleDuplicatesController;
        },
        set possibleDuplicatesController(value) {
            _possibleDuplicatesController = value;
        },
        get hintsController() {
            return _hintsController;
        },
        set hintsController(value) {
            _hintsController = value;
        },
        get dateProblemsController() {
            return _dateProblemsController;
        },
        set dateProblemsController(value) {
            _dateProblemsController = value;
        },
        get incompleteOrdinancesController() {
            return _incompleteOrdinancesController;
        },
        set incompleteOrdinancesController(value) {
            _incompleteOrdinancesController = value;
        },
        get retrieveController() {
            return _retrieveController;
        },
        set retrieveController(value) {
            _retrieveController = value;
        }

    };

    return research;
});

