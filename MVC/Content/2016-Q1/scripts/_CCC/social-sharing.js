//************************************************
// SOCIAL SHARING
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Methods
    //***********************************************/
    var selector = "[data-type='social-share']";

    var shareUrl;
    var width;
    var height;
    var windowName;

    CCChapel.setupSocialSharing = function () {
        $(document).ready(function () {
            $(selector).click(function (e) {
                e.preventDefault();

                var service = $(this).attr("data-service").toLowerCase();

                switch (service) {
                    case "facebook":
                        width = 626;
                        height = 436;
                        windowName = "FacebookShare";

                        shareUrl = "https://www.facebook.com/sharer/sharer.php?u=" +
                            encodeURIComponent(location.href);
                        break;
                    case "twitter":
                        width = 550;
                        height = 520;
                        windowName = "TwitterShare";

                        shareUrl = "https://twitter.com/share?url=" +
                            encodeURIComponent(location.href);
                        break;
                    case "google+":
                        width = 840;
                        height = 464;
                        windowName = "GoogleShare";

                        shareUrl = "https://plus.google.com/share?url=" +
                            encodeURIComponent(location.href);
                        break;
                    case "pinterest":
                        width = 450;
                        height = 430;
                        windowName = "PinterestShare";

                        shareUrl = "http://pinterest.com/pin/create/button/?url=" +
                            encodeURIComponent(location.href) +
                            "&media=" + encodeURIComponent($("meta[property='og:image']").attr("content"));
                        break;
                    case "email":
                        shareUrl = "mailto:?subject=Check%20This%20Out&body=" +
                            encodeURIComponent(location.href);
                        $(this).attr("href", shareUrl);
                        return;
                        break;
                }

                window.open(shareUrl, windowName, "width=" + width + "," + "height=" + height);
            });
        });
    };
}(window.CCChapel = window.CCChapel || {}, jQuery));