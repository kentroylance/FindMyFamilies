define(function (require) {

    var _startingPointController;
    var _findPersonController;

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
        }

    };

    return research;
});

