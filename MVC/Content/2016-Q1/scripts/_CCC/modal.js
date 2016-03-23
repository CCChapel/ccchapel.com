//************************************************
// MODAL
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Properties
    //***********************************************/

    /************************************************
    // Private Properties
    //***********************************************/
    var modalCloseSelector = ".modal-close";
    var modalContentSelector = ".modal-content";
    var modalOpen = false;

    var defaults = {
        cssClass: ".modal",
        fullScreen: false,
        transitionDuration: 250,
        lockViewport: true
    }

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.openModal = function (options) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        if (options.fullScreen == true) {
            $(options.cssClass).addClass("full");
        }
        else {
            $(options.cssClass).removeClass("full");
        }

        $(options.cssClass).fadeIn(options.transitionDuration);

        if (options.lockViewport == true) {
            CCChapel.lockViewport();
        }

        modalOpen = true;
    }

    CCChapel.closeModal = function (options) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        $(options.cssClass).fadeOut(options.transitionDuration);

        //Clean out content
        $(modalContentSelector).html("");

        CCChapel.unlockViewport();

        modalOpen = false;
    }

    CCChapel.toggleModal = function (options) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        if (modalOpen == true) {
            CCChapel.closeModal(options);
        }
        else {
            CCChapel.openModal(options);
        }
    }

    CCChapel.setupModal = function () {
        $(document).ready(function () {
            //Add close button functionality
            $(modalCloseSelector).click(function (e) {
                CCChapel.closeModal();
            });
        });
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));