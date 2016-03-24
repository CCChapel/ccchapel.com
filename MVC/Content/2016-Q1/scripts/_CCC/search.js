﻿//************************************************
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

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));