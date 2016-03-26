﻿//************************************************
// VIDEOS
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Properties
    //***********************************************/

    /************************************************
    // Private Properties
    //***********************************************/
    var desktopBreakpoint = 1024;

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.isMobile = function () {
        if ($(window).width() < desktopBreakpoint) {
            return true;
        }

        return false;
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));