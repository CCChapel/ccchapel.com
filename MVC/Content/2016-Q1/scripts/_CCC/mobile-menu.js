//************************************************
// VIDEOS
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Properties
    //***********************************************/

    /************************************************
    // Private Properties
    //***********************************************/
    var ItemClass = ".menu__items";
    var menuClass = ".banner__menu";
    var itemsClass = ".menu__items";
    var itemClass = ".menu__item";
    var trigger = "#nav-icon";

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.openMobileMenu = function (callback) {
        //if (CCChapel.isMobile() == true) {
            //animate icon
            $(trigger).addClass("close");

            //show items
            $(itemsClass).show();

            //toggle screen lock
            $("body").addClass("hide-overflow");
            $("body").addClass("lock-position");

            CCChapel.openModal();

            //toggle menu
            $(menuClass).slideDown(250, function () {
                //blur backgrounds after menu displays
                //$(".notifications, .body, .footer").toggleClass("blur");   
                callback();
            });
        //}
    }

    CCChapel.hideMenuItems = function () {
        $(ItemClass).hide();
    }

    CCChapel.showMenuItems = function () {
        $(ItemClass).show();
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));