define(function(require) {

    var $ = require('jquery');
    var user = require('user');
    var constants = require('constants');
    require('vendor/jquery.spin');
    var lazyRequire = require('lazyRequire');
    var requireOnce = lazyRequire.once();

    var _familySearchSystem;
    var _debugging;
    var _jqueryui;
    var _count = 0;

    var _target = document.getElementById(constants.DEFAULT_SPINNER_AREA);

    function stopSpinner(force) {
        _count--;
        if (force || _count === 0) {
            //            _target.spin();
            $('#' + _target.id).spin(false);
            _count = 0;
        } else if (_count < 0) {
            _count = 0;
        }
    }

    function startSpinner(force) {
        //    alert(_count + " " + _target.id);
        if (force || _count === 0) {
            //spinner = new Spinner(options).spin(_target);
            $('#' + _target.id).spin();
            _count = 0;
        }

        _count++;
    }

    function initSpinner(spinnerArea, stop) {
        if (_target && (_target.id !== spinnerArea)) {
            if (_target && (_target.id !== spinnerArea)) {
                stopSpinner(true);
            }
            _target = document.getElementById(spinnerArea);
            _count = 0;
        }
        if (stop) {
            stopSpinner(true);
        } else {
            startSpinner(true);
        }
    }

    function setCookie(name, value, days) {
        var expires;
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        } else {
            expires = "";
        }
        document.cookie = name + "=" + value + expires + "; path=/";
    }

    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) {
                return c.substring(nameEQ.length, c.length);
            }
        }
        return null;
    }

    function deleteCookie(name) {
        setCookie(name, "", -1);
    }

    function getFamilySearchSystem() {
        if (!_familySearchSystem) {
            _familySearchSystem = getCookie("FamilySearchSystem");
        }
        return _familySearchSystem;
    }

    function isDebugging() {
        if (!_debugging) {
            var debuggingWeb = getCookie("debugging");
            _debugging = false;
            if (debuggingWeb === "True") {
                _debugging = true;
            }
        }
        return _debugging;
    }


    function KeepSessionAlive() {
        $.get(constants.KEEP_SESSION_ALIVE_URL, function(data) {
        });

        setTimeout(KeepSessionAlive, 1140000);
    }

    // Run function for a first time
 //   KeepSessionAlive();

    window.onerror = function(errorText, url, lineNumber) {
        var report = {
            errorText: errorText,
            url: url,
            lineNumber: lineNumber
        };

        jQuery.ajax({
            type: "POST",
            url: "/error/record",
            dataType: 'json',
            data: report,
            cache: false
        });
        stopSpinner(true);
    };

    var logger = {
        force: false,
        log: function(obj) {
            if (window.location.hostname === domainName) {
                if (window.myLogger.force === true) {
                    window.myLogger.original.apply(this, arguments);
                }
            } else {
                window.myLogger.original.apply(this, arguments);
            }
        },
        forceLogging: function(force) {
            window.myLogger.force = force;
        },
        original: function() {
            return window.myLogger.original;
        },
        init: function() {
            window.myLogger.original = console.log;
            console.log = window.myLogger.log;
        }
    };
    window.myLogger = logger;

    var defaultHandler = function(jqXHR, textStatus, errorThrown) {
        //        var data = JSON.parse(jqXHR.responseText);
        //        if (data && data.Url) {
        //            window.location = data.url;
        //        } else {
        //            // TODO: redirect to a fixed error location, alert, etc
        //        }
        stopSpinner(true);
        window.location.reload();
    };

    $.ajaxSetup({
        'type': 'GET',
        'async': true,
        'cache': true,
        'beforeSend': function() {
            startSpinner();
        },
        'complete': function() {
            stopSpinner();
        },
        'error': function(jqXHR, textStatus, errorThrown) {
            stopSpinner(true);
            if (jqXHR.status === 0) {
                alert("Your session has timed out.  After selecting Ok, please relogin");
            } else if (jqXHR.status == 404) {
                alert("Requested page not found. [404].  After selecting Ok, please relogin");
            } else if (jqXHR.status == 500) {
                alert("Internal Server Error [500].  After selecting Ok, please relogin");
            } else if (textStatus === 'parsererror') {
                alert("Requested JSON parse failed.  After selecting Ok, please relogin");
            } else if (textStatus === 'timeout') {
                alert("Time out error.  After selecting Ok, please relogin");
            } else if (textStatus === 'abort') {
                alert("Ajax request aborted.  After selecting Ok, please relogin");
            } else {
                alert('Uncaught Error.\n' + jqXHR.responseText);
            }
            window.location.reload();
            if (this[jqXHR.status]) {
                this[jqXHR.status](jqXHR, textStatus, errorThrown);
            } else {
                // No handlers were found, handle the situation in a global manner here
            }
        },
        '401': defaultHandler,
        '403': defaultHandler,
        '500': defaultHandler
    });

    requirejs.onError = function (err) {
        alert(err.requireType + '. modules: ' + err.requireModules);
        throw err;
    };

//    requireQueue([
//            'app',
//            'apps/home/initialize',
//            'apps/entities/initialize',
//            'apps/cti/initialize'
//    ], function (App) {
//        App.start();
//    });
    var _callback;

    function load(queue, results) {
        if (queue.length) {
            require([queue.shift()], function (result) {
                results.push(result);
                load(queue, results);
            });
        } else {
            _callback.apply(null, results);
        }
    }

    function requireQueue(modules, callback) {
        _callback = callback;
        load(modules, []);
    };


    if (!user || !user.id || !user.name) {
        user.id = getCookie(constants.USER_ID);
        user.name = getCookie(constants.USER_NAME);
    }

    function openForm(form, image, spinnerTarget) {
        form.parent().children(".ui-dialog-titlebar").prepend('<span style="float:left; margin-top: 1px; margin-right: .3em;" class="fa ' + image + '"></span>');
        form.dialog({ minHeight: 0 });
        form.dialog({ open: function() { $(this).parent().css("padding", "0px") } });
        form.dialog("open");
        form.dialog("option", "position", "center");
        if (spinnerTarget) {
            initSpinner(spinnerTarget, true);
        } else {
            stopSpinner(true);
        }
    }

    function initJqueryui() {
        if (!jqueryui) {
            requireOnce(['jqueryui', 'css!/Content/css/lib/common/jquery-ui-1.11.2'], function() {
                    $.extend($.ui.dialog.prototype.options, {
                        height: "auto",
                        autoOpen: false,
                        modal: true,
                        resizable: false,
                        minHeight: 0,
                        closeOnEscape: true,
                        close: function(event, ui) {
                            event.preventDefault();
                            $(this).dialog('destroy').remove();
                        },
                        buttons: {
                            "0": {
                                id: 'close',
                                text: 'Close',
                                icons: { primary: "closeIcon" },
                                click: function(event) {
                                    event.preventDefault();
                                    $(this).dialog("close");
                                },
                                "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                            }
                        }
                    });
                }, function() {}
            );
        }

    }


    function isAuthenticated() {
        var authenticated = true;
        var tokenHourExpire = getCookie("TokenHourExpire");
        var token24HourExpire = getCookie("Token24HourExpire");
        if (tokenHourExpire && token24HourExpire) {
            var tokenHourExpireDate = new Date(tokenHourExpire);
            var token24HourExpireDate = new Date(token24HourExpire);
            var now = new Date();
            if (now > tokenHourExpireDate) {
                authenticated = false;
            }
            if (now > token24HourExpireDate) {
                authenticated = false;
            }
        } else {
            authenticated = false;
        }

        return authenticated;
    };


    function relogin() {
       stopSpinner(true);
        alert("Sorry, your login session has expired, please relogin.");
        window.location.reload();
        return false;
    };

    function clearStorage() {
        if (window.localStorage) {
            localStorage.clear();
        }
    }

    var domReady = require('domReady');
    domReady(function() {
        if (isAuthenticated()) {
            document.getElementById("displayPerson").innerHTML = user.name + "<b>&nbsp;-&nbsp;" + user.id + "</b>&nbsp;";
        } else {
            var loginInfoLi = document.getElementById("loginInfo");
            if (loginInfoLi) {
                loginInfoLi.className = 'hidden';
            }
            var loginInfoDividerLi = document.getElementById("loginInfoDivider");
            if (loginInfoDividerLi) {
                loginInfoDividerLi.className = 'hidden';
            }
            var logoutLi = document.getElementById("logout");
            if (logoutLi) {
                logoutLi.className = 'hidden';
            }
            var loginLi = document.getElementById("login");
            if (loginLi) {
                loginLi.className = 'unhidden';
            }
        }
    });

    var system = {
        startSpinner: function(force) {
            startSpinner(force);
        },
        stopSpinner: function(force) {
            stopSpinner(force);
        },
        initSpinner: function(spinnerArea, stop) {
            initSpinner(spinnerArea, stop);
        },
        get target() {
            return _target;
        },
        set target(value) {
            _target = value;
        },
        get domainName() {
            return _domainName;
        },
        clearStorage: function() {
            clearStorage();
        },
        relogin: function() {
            relogin();
        },
        isAuthenticated: function() {
            return isAuthenticated();
        },
        keepSessionAlive: function() {
            keepSessionAlive();
        },
        familySearchSystem: function () {
            return getFamilySearchSystem();
        },
        debugging: function() {
            isDebugging();
        },
        openForm: function(form, image, spinnerTarget) {
            openForm(form, image, spinnerTarget);
        },
        initJqueryui: function() {
            initJqueryui();
        },
        requireQueue: function(modules, callback) {
            requireQueue(modules, callback);
        },
        getCookie: function (name) {
            return getCookie(name);
        },
        setCookie: function (name, value, days) {
            setCookie(name, value, days);
        },
        deleteCookie: function (name) {
            deleteCookie(name);
        }
    };

    return system;
});