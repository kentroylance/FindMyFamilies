define(function(require) {
    
    var $ = require('jquery');
    require('jqueryUiOptions');

    var msgBox = {
        originalMessage: window.message,
        originalQuestion: window.question,

        message: function(messageText, messageTitle, messageCallback, options) {

            var title = "Information",
                message = "",
                callback;

            if (messageCallback !== undefined) {
                if (Object.prototype.toString.call(messageCallback) === "[object Function]") {
                    callback = messageCallback;
                }
            }

            if (!messageTitle) {
                messageTitle = title;
            }
            if (!messageText) {
                messageText = message;
            }
            messageText = "<i class=\"fa fmf-exclamation24 msg-icons\"></i>" + messageText;
            try {
                var $dialogContainer = $('#msgBox');
                var $detachedChildren = $dialogContainer.children().detach();
                $('<div id="msgBox"></div>').html(messageText).dialog({
                    title: messageTitle,
                    width: 550,
                    open: function () {
                        $detachedChildren.appendTo($dialogContainer);
                    },
                    close: function(event, ui) {
                        event.preventDefault();
                        if (callback !== undefined) {
                            callback();
                        } else {
                            $(this).dialog('destroy').remove();
                        }
                    },
                    buttons: {
                        "0": {
                            id: 'ok',
                            text: 'OK',
                            icons: { primary: "okIcon" },
                            click: function(event) {
                                event.preventDefault();
                                $(this).dialog("close");
                            },

                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                        }
                    }
                });

                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }

        },

        warning: function (messageText, messageTitle, messageCallback, options) {

            var title = "Warning",
                message = "",
                callback;

            if (messageCallback !== undefined) {
                if (Object.prototype.toString.call(messageCallback) === "[object Function]") {
                    callback = messageCallback;
                }
            }

            if (!messageTitle) {
                messageTitle = title;
            }
            if (!messageText) {
                messageText = message;
            }
            messageText = "<i class=\"fa fmf-warning24 msg-icons\"></i>" + messageText;
            try {
                var $dialogContainer = $('#msgBox');
                var $detachedChildren = $dialogContainer.children().detach();
                $('<div id="msgBox"></div>').html(messageText).dialog({
                    title: messageTitle,
                    width: 550,
                    open: function () {
                        $detachedChildren.appendTo($dialogContainer);
                    },
                    close: function (event, ui) {
                        event.preventDefault();
                        if (callback !== undefined) {
                            callback();
                        } else {
                            $(this).dialog('destroy').remove();
                        }
                    },
                    buttons: {
                        "0": {
                            id: 'ok',
                            text: 'OK',
                            icons: { primary: "okIcon" },
                            click: function (event) {
                                event.preventDefault();
                                $(this).dialog("close");
                            },

                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                        }
                    }
                });

                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }

        },

        error: function (messageText, messageTitle, messageCallback, options) {

            var title = "Error",
                message = "",
                callback;

            if (messageCallback !== undefined) {
                if (Object.prototype.toString.call(messageCallback) === "[object Function]") {
                    callback = messageCallback;
                }
            }

            if (!messageTitle) {
                messageTitle = title;
            }
            if (!messageText) {
                messageText = message;
            }
            messageText = "<i class=\"fa fmf-warning24 msg-icons\"></i>" + messageText;
            try {
                var $dialogContainer = $('#msgBox');
                var $detachedChildren = $dialogContainer.children().detach();
                $('<div id="msgBox"></div>').html(messageText).dialog({
                    title: messageTitle,
                    width: 550,
                    open: function () {
                        $detachedChildren.appendTo($dialogContainer);
                    },
                    close: function (event, ui) {
                        event.preventDefault();
                        if (callback !== undefined) {
                            callback();
                        } else {
                            $(this).dialog('destroy').remove();
                        }
                    },
                    buttons: {
                        "0": {
                            id: 'ok',
                            text: 'OK',
                            icons: { primary: "okIcon" },
                            click: function (event) {
                                event.preventDefault();
                                $(this).dialog("close");
                            },

                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                        }
                    }
                });

                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }

        },

        question: function (messageText, messageTitle, callback, options) {

            var title = "Question",
                message = "",
                result;

            if (!messageTitle) {
                messageTitle = title;
            }
            if (!messageText) {
                messageText = message;
            }
            messageText = "<i class=\"fa fmf-question24 msg-icons\" ></i>" + messageText;
            try {
                var $dialogContainer = $('#msgBox');
                var $detachedChildren = $dialogContainer.children().detach();
                $('<div id="msgBox"></div>').html(messageText).dialog({
                    title: messageTitle,
                    width: 550,
                    open: function () {
                        $detachedChildren.appendTo($dialogContainer);
                    },
                    buttons: {
                        "0": {
                            id: 'yes',
                            text: 'Yes',
                            icons: { primary: "okIcon" },
                            click: function(event) {
                                event.preventDefault();
                                result = true;
                                if (callback !== undefined && callback !== "") {
                                    if (typeof (callback) === "function") {
                                        callback(result);
                                        $(this).dialog("close");
                                    } else {
                                        msgBox.message("Not a function !");
                                    }

                                }
                                if (callback === "" || callback === undefined) {
                                    msgBox.message("Please define a callback function");
                                }
                            },

                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-green"
                        },
                        "1": {
                            id: 'no',
                            text: 'No',
                            icons: { primary: "cancelIcon" },
                            click: function(event) {
                                event.preventDefault();
                                result = false;
                                if (callback !== undefined && callback !== "") {
                                    if (typeof (callback) === "function") {

                                        callback(result);
                                        $(this).dialog("close");
                                    } else {

                                        msgBox.message("Not a function !");
                                    }
                                }
                                if (callback === "" || callback === undefined) {
                                    msgBox.message("Please define a callback function");
                                }
                            },
                            "class": "btn-u btn-brd btn-brd-hover rounded btn-u-blue"
                        }
                    }
                });

                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }
        },

        ReplaceMessage: function() {
            window.message = msgBox.message;
        },
        ReplaceQuestion: function() {
            window.question = msgBox.question;
        },
        RestoreMessage: function() {
            window.message = msgBox.originalMessage;
        },
        RestoreQuestion: function() {
            window.question = msgBox.originalQuestion;
        }
    };

    return msgBox;
});

//# sourceURL=msgBox.js