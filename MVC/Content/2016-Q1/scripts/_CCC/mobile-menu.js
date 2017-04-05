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
    //var ItemClass = ".menu__items";
    var menuClass = ".banner__menu";
    var itemsClass = ".menu__items";
    var itemClass = ".menu__item";
    var trigger = "#nav-icon";
    var isOpen = false;

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.initializeMobileMenu = function (callback) {
        //Toggle open and close
        $(trigger).click(function () {
            CCChapel.toggleMobileMenu();
        });
    }
    
    CCChapel.openMobileMenu = function (callback) {
        //animate icon
        $(trigger).addClass("close");

        //toggle screen lock
        $("body").addClass("hide-overflow");
        $("body").addClass("lock-position");

        CCChapel.openModal({}, function () {
            //toggle menu
            $(".banner__menu").show();

            //show items
            $(itemsClass).show();

            CCChapel.showMenuItems({ duration: 500 });

            //$(itemClass)
            //    .css('opacity', 0)
            //    .slideDown(500)
            //    .animate(
            //        { opacity: 1 },
            //        { queue: false, duration: 500 }
            //    );

            isOpen = true;

            if (callback !== undefined) {
                callback();
            }
        });
    }

    CCChapel.closeMobileMenu = function (callback) {
        $(trigger).removeClass("close");

        $("body").removeClass("hide-overflow");
        $("body").removeClass("lock-position");

        CCChapel.closeModal({}, function () {
            $(".banner__menu").hide();

            CCChapel.hideMenuItems({ duration: 500 });

            isOpen = false;

            //clear search
            $("#menu-search").val("");

            if (callback !== undefined) {
                callback();
            }
        })
    }

    CCChapel.toggleMobileMenu = function (callback) {
        if (isOpen === true) {
            CCChapel.closeMobileMenu(callback);
        }
        else {
            CCChapel.openMobileMenu(callback);
        }
    }

    CCChapel.hideMenuItems = function (options) {
        $(itemsClass).fadeOut(options);
    }

    CCChapel.showMenuItems = function (options) {
        $(itemsClass).hide().fadeIn(options);
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));