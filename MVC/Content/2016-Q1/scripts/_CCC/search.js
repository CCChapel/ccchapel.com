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
    var apiUrl = "api/search?query=";

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.getSearchResults = function(query) {
        var url = apiUrl + encodeURI(query.replace(" ", "+"));
        console.log(url);

        return $.getJSON(url);
    }

    //************************************************
    // Private Methods
    //***********************************************/
}(window.CCChapel = window.CCChapel || {}, jQuery));