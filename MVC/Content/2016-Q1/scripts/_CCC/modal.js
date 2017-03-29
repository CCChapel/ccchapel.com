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
    CCChapel.openModal = function (options, callback) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        if (options.fullScreen == true) {
            $(options.cssClass).addClass("full");
        }
        else {
            $(options.cssClass).removeClass("full");
        }

        $(options.cssClass).fadeIn(options.transitionDuration, function () {
            if (callback !== undefined) {
                callback();
            }
        });

        if (options.lockViewport == true) {
            CCChapel.lockViewport();
        }

        modalOpen = true;
    }

    CCChapel.closeModal = function (options, callback) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        $(options.cssClass).fadeOut(options.transitionDuration, function () {
            console.log(callback);

            if (callback !== undefined) {
                callback();
            }
        });

        //Clean out content
        this.clearModalContent();

        CCChapel.unlockViewport();

        modalOpen = false;
    }

    CCChapel.toggleModal = function (options, callback) {
        //Setup Defaults
        options = $.extend({}, defaults, options);

        if (modalOpen == true) {
            CCChapel.closeModal(options, callback);
        }
        else {
            CCChapel.openModal(options, callback);
        }
    }

    CCChapel.clearModalContent = function() {
        $(modalContentSelector).html("");
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