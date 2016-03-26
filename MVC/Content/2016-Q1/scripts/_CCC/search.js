//************************************************
// SEARCH
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Properties
    //***********************************************/

    /************************************************
    // Private Properties
    //***********************************************/
    var apiUrl = "api/search/";
    var defaults = {
        maxResults: undefined
    }
    var itemClass = ".menu__search";
    var fieldClass = ".menu__search-field";
    var searchField = "#menu-search";
    var iconClass = ".menu__search-icon";
    var bannerClass = ".banner";

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.getSearchResults = function(query, options) {
        var url = apiUrl + encodeURI(query);

        options = $.extend({}, defaults, options);

        if (options.maxResults !== undefined) {
            url += "?maxResults=" + options.maxResults;
        }

        return $.getJSON(url);
    }

    CCChapel.openSearch = function () {
        if (CCChapel.isMobile() == true) {
            CCChapel.openMobileMenu();
        }
        else {
            $(iconClass).click();
        }
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));