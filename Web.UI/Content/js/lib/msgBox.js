define(function(require) {
    
    var $ = require('jquery');
    require('jqueryUiOptions');

    var msgBox = {
        originalMessage: window.message,
        originalQuestion: window.question,

        message: function(messageText, messageTitle, messageCallback, options) {

            var title = "Information",
                message = "",
                callback,
                tittleBgColor = "#275F98",
                tittleFontColor = "#ffffff",
                messageBgColor = "#ffffff",
                messageFontColor = "#000000";

            if (options !== undefined) {
                if (options.messageFontColor !== undefined && options.messageFontColor !== "") {
                    messageFontColor = options.messageFontColor;
                }
                if (options.messageBgColor !== undefined && options.messageBgColor !== "") {
                    messageBgColor = options.messageBgColor;
                }
                if (options.tittleBgColor !== undefined && options.tittleBgColor !== "") {
                    tittleBgColor = options.tittleBgColor;
                }
                if (options.tittleFontColor !== undefined && options.tittleFontColor !== "") {
                    tittleFontColor = options.tittleFontColor;
                }
            }

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
            messageText = "<i class=\"fmf24-exclamation\" style=\" font-size: 22px; vertical-align: middle; margin-right: .25em; margin-top : -0.225em\"></i>" + messageText;
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
                var image = "cus-accept";
//                $('#msgBox').parent().children(".ui-dialog-titlebar").prepend('<span style="float:left; margin-top: 7px; margin-right: .5em;" class="fa cus-About"></span>');
                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");


//                $(".ui-widget-content").css("color", messageFontColor);
//                $(".ui-widget-content").css("background", messageBgColor);
//                $(".ui-widget-header").css("background", tittleBgColor);
//                $(".ui-dialog-title").css("color", tittleFontColor);

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }

        },

        warning: function (messageText, messageTitle, messageCallback, options) {

            var title = "Warning",
                message = "",
                callback,
                tittleBgColor = "#275F98",
                tittleFontColor = "#ffffff",
                messageBgColor = "#ffffff",
                messageFontColor = "#000000";

            if (options !== undefined) {
                if (options.messageFontColor !== undefined && options.messageFontColor !== "") {
                    messageFontColor = options.messageFontColor;
                }
                if (options.messageBgColor !== undefined && options.messageBgColor !== "") {
                    messageBgColor = options.messageBgColor;
                }
                if (options.tittleBgColor !== undefined && options.tittleBgColor !== "") {
                    tittleBgColor = options.tittleBgColor;
                }
                if (options.tittleFontColor !== undefined && options.tittleFontColor !== "") {
                    tittleFontColor = options.tittleFontColor;
                }
            }

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
            messageText = "<i class=\"fmf24-warning\" style=\" font-size: 22px; vertical-align: middle; margin-right: .25em; margin-top : -0.225em\"></i>" + messageText;
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
                var image = "cus-accept";
                //                $('#msgBox').parent().children(".ui-dialog-titlebar").prepend('<span style="float:left; margin-top: 7px; margin-right: .5em;" class="fa cus-About"></span>');
                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");


                //                $(".ui-widget-content").css("color", messageFontColor);
                //                $(".ui-widget-content").css("background", messageBgColor);
                //                $(".ui-widget-header").css("background", tittleBgColor);
                //                $(".ui-dialog-title").css("color", tittleFontColor);

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }

        },

        question: function (messageText, messageTitle, callback, options) {

            var title = "Question",
                message = "",
                tittleBgColor = "#275F98",
                tittleFontColor = "#ffffff",
                messageBgColor = "#ffffff",
                messageFontColor = "#000000",
                result;

            if (options !== undefined) {
                if (options.messageFontColor !== undefined && options.messageFontColor !== "") {
                    messageFontColor = options.messageFontColor;
                }
                if (options.messageBgColor !== undefined && options.messageBgColor !== "") {
                    messageBgColor = options.messageBgColor;
                }

                if (options.tittleBgColor !== undefined && options.tittleBgColor !== "") {
                    tittleBgColor = options.tittleBgColor;
                }

                if (options.tittleFontColor !== undefined && options.tittleFontColor !== "") {
                    tittleFontColor = options.tittleFontColor;
                }
            }

            if (!messageTitle) {
                messageTitle = title;
            }
            if (!messageText) {
                messageText = message;
            }
            messageText = "<i class=\"fmf24-question\" style=\" font-size: 22px; vertical-align: middle; margin-right: .25em; margin-top : -0.225em\"></i>" + messageText;
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

                var image = "cus-accept";
//                $('#msgBox').parent().children(".ui-dialog-titlebar").prepend('<span style="float:left; margin-top: 2px; margin-right: .2em;" class="fa fmf24-question"></span>');
                $('#msgBox').dialog("open");
                $('#msgBox').dialog("option", "position", "center");

//                $(".ui-widget-content").css("color", messageFontColor);
//                $(".ui-widget-content").css("background", messageBgColor);
//                $(".ui-widget-header").css("background", tittleBgColor);
//                $(".ui-dialog-title").css("color", tittleFontColor);

                msgBox.byPassjQueryUI();

            } catch (error) {
                window.console.log(error);
            }
        },

        byPassjQueryUI: function() {
            $(".ui-button-text-only .ui-button-text").css("padding-left", "20px");
            $(".ui-button-text-only .ui-button-text").css("padding-right", "20px");
            $(".ui-button-text-only .ui-button-text").css("padding-top", "5px");
            $(".ui-button-text-only .ui-button-text").css("padding-bottom", "5px");
            $(".ui-button-text-only .ui-button-text").css("font-size", "13px");

            $(".ui-dialog").css("border-width", "1px");
            $(".ui-dialog").css("border-style", "solid");
            $(".ui-dialog").css("border-color", "#76A0F8");
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