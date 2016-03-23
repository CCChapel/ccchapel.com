//************************************************
// VIEWPORT
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Properties
    //***********************************************/

    /************************************************
    // Private Properties
    //***********************************************/
    var selector = "meta[name='viewport']";
    var locked = false;
    var lockProperty = ", maximum-scale=1.0";
    var defaults = {};

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.lockViewport = function (options) {
        //Mobile
        var content = $(selector).attr("content");
        content += lockProperty;

        $(selector).attr("content", content);

        //Desktop
        $("body").addClass("locked");

        locked = true;
    }

    CCChapel.unlockViewport = function (options) {
        //Mobile
        var content = $(selector).attr("content");
        content = content.replace(lockProperty, "");

        $(selector).attr("content", content);

        //Desktop
        $("body").removeClass("locked");

        locked = false;
    }

    CCChapel.toggleViewport = function (options) {
        if (locked == false) {
            CCChapel.lockViewport();
        }
        else {
            CCChapel.unlockViewport();
        }
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));