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

            ////show items
            //$(itemsClass).show();

            //toggle screen lock
            $("body").addClass("hide-overflow");
            $("body").addClass("lock-position");

            CCChapel.openModal({}, function () {
                //toggle menu
                $(".banner__menu").show();

                $(".menu__item").animate({
                    opacity: 1,
                    height: 'auto'
                }, 500);

                callback();
            });

            ////toggle menu
            //$(menuClass).slideDown(250, function () {
            //    //blur backgrounds after menu displays
            //    //$(".notifications, .body, .footer").toggleClass("blur");   
            //    callback();
            //});
        //}
    }

    CCChapel.hideMenuItems = function (options) {
        $(ItemClass).fadeOut(options);
    }

    CCChapel.showMenuItems = function (options) {
        $(ItemClass).fadeIn(options);
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));