"use strict";

//************************************************
// SETUP
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Private Properties
    //***********************************************/
    var desktopBreakpoint = 1024;
    var currentCampusSelector = "#currentCampus";

    //************************************************
    // Public Properties
    //***********************************************/
    CCChapel.CurrentCampus = "";

    //************************************************
    // Private Methods
    //***********************************************/
    /*** CAMPUS LOCATION MAP ***/
    var CampusLocationMap = {
        //Properties
        ListClass: ".campus-info__icon-list",
        MapClass: ".campus-info__map",
        ButtonClass: "#toggle-campus-map",

        //Functions
        setup: function () {
            $(this.ButtonClass).click(function (e) {
                e.preventDefault();

                if ($(window).width() > 1024) {
                    CampusLocationMap.setSize();
                }

                CampusLocationMap.toggle();
            });
        },
        drawMap: function () {

        },
        toggle: function () {
            $(this.ButtonClass).toggleClass("active");
            $(this.ListClass).fadeToggle();
            $(this.MapClass).fadeToggle();

            CCChapel.createMap("campus-info__map", {
                markers: CCChapel.CampusLocations,
                fitAllMarkers: true
            });
        },
        setSize: function () {
            //Get Current List Size
            var height = $(this.ListClass).height();
            var width = $(this.ListClass).width();

            $(this.MapClass).height(height);
            $(this.MapClass).width(width);
        }
    }

    /*** DESKTOP SEARCH ***/
    var DesktopSearch = {
        //Properties
        ItemClass: ".menu__search",
        FieldClass: ".menu__search-field",
        SearchField: "#menu-search",
        IconClass: ".menu__search-icon",
        BannerClass: ".banner",
        MenuItemsClass: ".menu__items", //CCChapel.MobileMenu.ItemsClass,

        //Functions
        setup: function () {
            //Icon Click
            $(this.IconClass).click(function () {
                //Open Mobile Menu
                CCChapel.openMobileMenu(function () {
                    //Set focus to search field
                    $("#menu-search").focus();
                });
            });
        }
    }

    /*** DESKTOP STICKY MENU ***/
    var DesktopStickyMenu = {
        //Properties
        CssClass: ".banner",
        StartingHeight: "64px",
        EndingHeight: "41px",
        BodySelector: ".body :first-child:visible",             //Use :first-child & :visible because Notifications may not always be there
        BodyStartingPosition: "0",
        HeaderClass: ".header",

        //Functions
        setup: function () {
            //Set Heights
            this.StartingHeight = $(this.CssClass).height();
            this.EndingHeight = Math.round(this.StartingHeight * .64);

            //Set BodyStartingPosition

            this.BodyStartingPosition = $(this.BodySelector).position().top;

            //Setup Scroll Event
            $(window).scroll(function () {
                DesktopStickyMenu.adjustStickyMenu();
            });

            //Initial Page Check
            $(document).ready(function () {
                //We only care if we're not at the top of the screen
                if ($(window).scrollTop != 0) {
                    DesktopStickyMenu.adjustStickyMenu();
                }
            });
        },
        adjustStickyMenu: function () {
            //Scale Banner
            var bodyOffet = $(this.BodySelector).offset().top;
            var windowPosition = $(window).scrollTop();
            var offset = Math.round(bodyOffet - windowPosition);

            if (offset < 0) {
                $(this.HeaderClass).addClass("sticky")
            }
            else {
                $(this.HeaderClass).removeClass("sticky");
            }
        }
    }

    /*** CAMPUS SECTION ***/
    var HomepageCampusSection = {
        //Properties
        ListClass: ".campus-info__other-list",
        CampusesClass: ".campus-info__other-list a",
        DetailsClass: ".campus-info__current",
        TitleClass: ".campus-info__other-title",
        StartingCampus: "",

        //Functions
        desktopSetup: function () {
            var campusSelector = "[data-campus='" + CCChapel.CurrentCampus + "']";

            //Show Current Campus Details
            $(this.DetailsClass).hide();
            this.StartingCampus = this.DetailsClass + campusSelector;
            $(this.StartingCampus).show();

            //Remove Current Campus in Other List
            $(this.ListClass).children("li").children(campusSelector).parent().remove();

            //Setup Hover Effect
            $(this.CampusesClass).hover(
                //Over
                function () {
                    var campus = $(this).attr("data-campus");
                    var selector = HomepageCampusSection.DetailsClass + "[data-campus='" + campus + "']";

                    //Hide All
                    $(HomepageCampusSection.DetailsClass).hide();

                    //Show Hovered One
                    $(selector).show();
                },

                //Out
                function () {
                    //Hide Hovered
                    $(HomepageCampusSection.DetailsClass).hide();

                    //Show Original
                    $(HomepageCampusSection.StartingCampus).show();
                }
            );
        },
        portableSetup: function () {
            var campusSelector = "[data-campus='" + CCChapel.CurrentCampus + "']";

            //Show Current Campus Details
            $(this.DetailsClass).hide();
            this.StartingCampus = this.DetailsClass + campusSelector;
            $(this.StartingCampus).show();

            //Remove Current Campus in Other List
            $(this.ListClass).children("li").children(campusSelector).parent().remove();

            //Setup click event
            $(this.TitleClass).click(function () {
                $(".campus-info__other-list").slideToggle();
            });
        }
    }

    /*** CONTENT SEARCH ***/
    var ContentSearch = {
        //Properties
        CssClass: ".search__input",

        //Functions
        desktopSetup: function () {
            $(this.CssClass).click(function () {
                //DesktopSearch.open();
                $(".menu__search-icon").click();
            });
        },
        portableSetup: function () {
            $(this.CssClass).click(function () {
                ////Open Menu
                //MobileMenu.open();

                ////Setup Focus
                //$(DesktopSearch.SearchField).focus();

                $(".menu__search-icon").click();
            });
        }
    }

    /*** CAMPUS SELECT ***/
    var CampusSelect = {
        //Properties
        CssClass: ".campus-select__list",
        ToggleTrigger: ".campus-select__campus-marker",

        //Functions
        toggleMobileList: function () {
            $(this.CssClass).slideToggle(250);
        },
        setup: function () {
            $(this.ToggleTrigger).click(function () {
                CampusSelect.toggleMobileList();
            });
        }
    }

    /*** MOBILE MENU ***/
    var MobileMenu = {
        //Properties
        MenuClass: ".banner__menu",
        ItemsClass: ".menu__items",
        ItemClass: ".menu__item",
        Trigger: "#nav-icon",
        //isOpen: false,

        //Functions
        setup: function () {
            //Toggle open and close
            $(this.Trigger).click(function () {
                MobileMenu.toggleMenu();
            });
        },
        toggleMenu: function () {
            //Check if menu is visible
            if ($(this.MenuClass).css("display") !== "block") {
                this.open();

                //this.isOpen = true;
            }
            else {
                this.close();

                //this.isOpen = false;
            }

            //clear search
            $("#menu-search").val("");
        },
        open: function () {
            //animate icon
            $(this.Trigger).addClass("close");

            //show items
            //$(this.ItemsClass).show();

            //toggle screen lock
            $("body").addClass("hide-overflow");
            $("body").addClass("lock-position");

            CCChapel.openModal({}, function () {
                //toggle menu
                $(".banner__menu").show();

                $(".menu__item").animate({
                    opacity: 1,
                    height: 'toggle'
                }, 500);
            });
        },
        close: function () {
            //animate icon
            $(this.Trigger).removeClass("close");

            //toggle screen lock
            $("body").removeClass("hide-overflow");
            $("body").removeClass("lock-position");

            CCChapel.closeModal({}, function () {
                //toggle menu
                $(".banner__menu").hide();

                $(".menu__item").animate({
                    opacity: 0,
                    height: 'toggle'
                }, 500);
            });

            //clear search
            $("#menu-search").val("");
        }
    }

    //************************************************
    // Public Methods
    //***********************************************/
    CCChapel.initialize = function () {
        //Initialize Current Campus Value
        CCChapel.CurrentCampus = $(currentCampusSelector).val();

        CCChapel.initializeSearch();

        CCChapel.setupCampusLinks();

        CampusLocationMap.setup();
        //        SearchFields.setup();
        CCChapel.setupModal();

        CCChapel.setupWebAppLinks();

        CCChapel.setupSocialSharing();

        //DESKTOP FUNCTIONS
        if ($(window).width() >= desktopBreakpoint) {
            //Desktop Search
            DesktopSearch.setup();

            //Desktop Sticky Menu
            DesktopStickyMenu.setup();

            //Mobile Menu
            MobileMenu.setup();

            //Homepage Campus Section
            HomepageCampusSection.desktopSetup();

            //Content Search Setup
            ContentSearch.desktopSetup();
        }

        //PORTABLE FUNCTIONS
        if ($(window).width() < desktopBreakpoint) {
            //Mobile Campus List
            CampusSelect.setup();

            //Desktop Sticky Menu
            DesktopStickyMenu.setup();

            //Mobile Menu
            MobileMenu.setup();

            //Homepage Campus Section
            HomepageCampusSection.portableSetup();

            //Content Search Setup
            ContentSearch.portableSetup();
        }
    };
}(window.CCChapel = window.CCChapel || {}, jQuery));


$(document).ready(function () {
    CCChapel.initialize();
})