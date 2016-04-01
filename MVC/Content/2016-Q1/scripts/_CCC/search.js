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
    var apiUrl = location.protocol + "//" + location.hostname + "/api/search/";
    var defaults = {
        maxResults: undefined
    }
    var itemClass = ".menu__search";
    var fieldClass = ".menu__search-field";
    var searchField = "#menu-search";
    var iconClass = ".menu__search-icon";
    var bannerClass = ".banner";

    var query;

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

    CCChapel.initializeSearch = function () {
        $(document).ready(function () {
            initializeAjaxSearch();

            //Setup Enter to Load Search Page
            $(searchField).keyup(function (e) {
                if (e.keyCode == 13) {
                    query = $(searchField).val();

                    if (query.length > 0) {
                        window.location = getQueryLink(query);
                    }
                }
            });
        });
    }

    //************************************************
    // Private Methods
    //***********************************************/
    function initializeAjaxSearch() {
        var timer;
        var delay = 600;
        var loading = false;

        //Setup AJAX Results
        $(searchField).on('input', function () {
            if (CCChapel.isMobile() != true) {
                if (loading == false) {
                    showLoading();

                    loading = true;
                }

                window.clearTimeout(timer);

                timer = window.setTimeout(function () {
                    query = $(searchField).val();

                    if (query.length > 0) {
                        var jqxhr = CCChapel.getSearchResults(query);

                        jqxhr.done(function (data) {
                            loading = false;

                            if (data.totalItemCount > 0) {
                                displaySearchResults(data);
                            }
                            else {
                                displayNoResults();
                                console.log(data);
                            }
                        })
                        .fail(function (jqxhr, textStatus, error) {
                            loading = false;

                            var err = textStatus + ", " + error;
                            console.log("Request Failed: " + err);

                            displayNoResults();
                        });
                    }
                    else {
                        loading = false;

                        CCChapel.clearModalContent();
                    }
                }, delay);
            }
        });
    }

    function showLoading() {
        CCChapel.clearModalContent();

        var html = $("<div></div>").addClass("search-results");
        var contentWrapper = $("<div></div>").addClass("content-wrapper");

        var searching = $("<div></div>").html("Searching&hellip;");

        var spinnerContainer = $("<div></div>");
        var spinner = $("<i></i>").addClass("fa fa-circle-o-notch fa-spin fa-2x");
        spinnerContainer.append(spinner);

        var sectionTitle =
            $("<div></div>")
                .addClass("section-title font-white")
                .append(searching)
                .append(spinner);

        contentWrapper.append(sectionTitle);
        html.append(contentWrapper);
        $(".modal-content").append(html);
    }

    function getQueryLink(query) {
        return "/search?query=" + encodeURI(query.replace(" ", "+"));
    }

    function displayNoResults() {
        //Clear out old results
        CCChapel.clearModalContent();

        var html = $("<div></div>").addClass("search-results");
        var contentWrapper = $("<div></div>").addClass("content-wrapper");

        var sectionTitle =
            $("<div></div>")
                .addClass("section-title font-white")
                .html("Hmm&hellip; We didn&rsquo;t find anything for <i>" + query + "</i>");
        contentWrapper.append(sectionTitle);

        var sectionDescription =
            $("<div></div>")
                .addClass("section-description font-white")
                .html('Maybe try searching for something else, or feel free to <a class="font-white" href="/contact-us">contact us directly</a>.');
        contentWrapper.append(sectionDescription);

        html.append(contentWrapper);
        $(".modal-content").append(html);
    }

    function displaySearchResults(results) {
        //Clear out old results
        CCChapel.clearModalContent();

        var html = $("<div></div>").addClass("search-results");
        var contentWrapper = $("<div></div>").addClass("content-wrapper");

        $.each(results.items, function (i, item) {
            var result = $("<div></div>").addClass("search-results__item");

            var link = $("<a></a>").attr("href", item.url);
            var title = $("<div></div>").addClass("search-results__item-title").html(item.title);
            var description = $("<div></div>").addClass("search-results__item-description").html(item.description);
            link.append(title, description);

            var horizontalRule = $("<hr />");

            result.append(link);
            contentWrapper.append(result.add(horizontalRule));
        });

        if (results.totalItemCount > 3) {
            var more = $("<div></div>").addClass("center");
            var moreLink = $("<a></a>").addClass("cta auto red").attr("href", getQueryLink(results.query)).html('More Results&nbsp;<i class="fa fa-chevron-right"></i>');

            more.append(moreLink);
            contentWrapper.append(more);
        }

        html.append(contentWrapper);
        $(".modal-content").append(html);

        //Clean Up
        while ($(window).height() < $(".search-results").outerHeight()) {
            $(".modal-content .search-results__item").last().remove();
            $(".modal-content .search-results").children(".content-wrapper").children("hr").last().remove();
        }
    }
}(window.CCChapel = window.CCChapel || {}, jQuery));



