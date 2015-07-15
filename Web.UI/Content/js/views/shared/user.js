define(function (require) {

    var constants = require('constants');

    var _id;
    var _name;

    function UserDO(id, name) {
        this.id = id;
        this.name = name;
    }

    function save() {
        if (window.localStorage) {
            var user = new UserDO(_id, _name);
            localStorage.setItem(constants.USER, JSON.stringify(user));
        }
    }

    function load() {
        if (window.localStorage) {
            var user = JSON.parse(localStorage.getItem(constants.USER));
            if (!user || !user.id || !user.name) {
                user = new UserDO();
            }
            _id = user.id;
            _name = user.name;
        }
    }

    function clear() {
        _id = "";
        _name = "";
    }

    load();

    var user = {
        get id() {
            return _id;
        },
        set id(value) {
            _id = value;
        },
        get name() {
            return _name;
        },
        set name(value) {
            _name = value;
        },
        save: function () {
            save();
        },
        clear: function () {
            clear();
        }
    };

    return user;
});

