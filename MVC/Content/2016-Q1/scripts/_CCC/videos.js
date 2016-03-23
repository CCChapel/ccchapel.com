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
    var modalSelector = ".modal-content";

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.showVideo = function (options) {
        //Setup Defaults
        var defaults = {
            videoMarkup: ""
        };

        options = $.extend({}, defaults, options);

        //Load Video
        //var videoMarkup =
        //    '<div class="center-vertically"><iframe src="https://player.vimeo.com/video/157907090?badge=0&autopause=0&player_id=0" width="1280" height="720" frameborder="0" title="The Promise (Part 4) - God Cuts a Covenant" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe></div>';
        //    //'<div class="center-vertically"><iframe src="https://player.vimeo.com/video/153663975?autoplay=1&color=28708a&title=0&byline=0&portrait=0" width="100%" height="100%" frameborder="0" webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe></div>';

        if (options.videoMarkup != "") {
            //Show Modal
            CCChapel.openModal({ fullScreen: true });

            //Insert Video
            $(modalSelector).html(options.videoMarkup);
        }
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));