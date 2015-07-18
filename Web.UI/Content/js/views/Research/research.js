define(function (require) {

    var _startingPointController;
    var _findPersonController;
    var _startingPointReportController;
    var _findPersonOptionsController;
    var _possibleDuplicatesController;


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
        get findPersonOptionsController() {
            return _findPersonOptionsController;
        },
        set findPersonOptionsController(value) {
            _findPersonOptionsController = value;
        },
        get possibleDuplicatesController() {
            return _possibleDuplicatesController;
        },
        set possibleDuplicatesController(value) {
            _possibleDuplicatesController = value;
        }
    };

    return research;
});

