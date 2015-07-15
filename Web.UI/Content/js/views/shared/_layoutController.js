define(function(require) {
    var $ = require('jquery');
    var lazyRequire = require('lazyRequire');
    var requireOnce = lazyRequire.once();
    var msgBox;

    var _layoutController = {
        handleBootstrap: function($) {
            /*Tooltips*/
            $(".tooltips").tooltip();
            $(".tooltips-show").tooltip("show");
            $(".tooltips-hide").tooltip("hide");
            $(".tooltips-toggle").tooltip("toggle");
            $(".tooltips-destroy").tooltip("destroy");

            /*Popovers*/
            $(".popovers").popover();
            $(".popovers-show").popover("show");
            $(".popovers-hide").popover("hide");
            $(".popovers-toggle").popover("toggle");
            $(".popovers-destroy").popover("destroy");
        },

        //Sidebar Navigation Toggle
        handleToggle: function($) {
            $(".list-toggle").on("click", function() {
                $(this).toggleClass("active");
            });
        },

        //Fixed Header
        handleHeader: function($) {
            $(window).scroll(function() {
                if ($(window).scrollTop() > 100) {
                    $(".header-fixed .header-sticky").addClass("header-fixed-shrink");
                } else {
                    $(".header-fixed .header-sticky").removeClass("header-fixed-shrink");
                }
            });
        },

        //Header Mega Menu
        handleMegaMenu: function($) {
            $(document).on("click", ".mega-menu .dropdown-menu", function(e) {
                e.stopPropagation();
            });
        }

    };

    $("#subscribe").on("click", function(e) {
            event.preventDefault();
            requireOnce(['msgBox'], function(MsgBox) {
                msgBox = MsgBox;
                msgBox.message("Error!Occured");
                    if (document.forms[0].checkValidity()) {
                        var email = document.getElementById("email").value;
                        $.ajax({
                            url: "/Home/SubscribeEmail",
                            data: { "email": email },
                            success: function(data) {
                                if ((data != null) && (data != "")) {
                                    msgBox.message(data);
                                } else {
                                    msgBox.message("Thanks for subscribing to our monthly newsletter.");
                                }
                                document.getElementById("email").value = "";
                                document.getElementById("email").focus();
                            }
                        }).fail(function(xhr, textStatus, error) {
                            msgBox.message("Error!Occured");
                        });
                    }
                },
                function() {
                }
            );

            return false;
        }
    );


    //    $("#subscribe").on("click", function (e) {
    //
    //            event.preventDefault();
    //            requireOnce(['msgBox', 'css!/Content/css/lib/common/jquery-ui-1.11.2', 'jqueryui', 'dialog',  'cookie', 'spinner', 'system', 'user', ], function (MsgBox) {
    //                    msgBox = MsgBox;
    //            },
    //
    //                function() {
    //                    msgBox.message("Thanks for subscribing to our monthly newsletter.");
    //                    if (document.forms[0].checkValidity()) {
    //                        var email = document.getElementById("email").value;
    //                        $.ajax({
    //                            url: "/Home/SubscribeEmail",
    //                            data: { "email": email },
    //                            success: function(data) {
    //                                if ((data != null) && (data != "")) {
    //                                    msgBox.message(data);
    //                                } else {
    //                                    msgBox.message("Thanks for subscribing to our monthly newsletter.");
    //                                }
    //                                document.getElementById("email").value = "";
    //                                document.getElementById("email").focus();
    //                            }
    //                        }).fail(function(xhr, textStatus, error) {
    //                            msgBox.message("Error!Occured");
    //                        });
    //                    }
    //                }
    //            );
    //
    //            return false;
    //        }
    //    );


    _layoutController.handleBootstrap($);
    _layoutController.handleToggle($);
    _layoutController.handleHeader($);
    _layoutController.handleMegaMenu($);


    return _layoutController;


});