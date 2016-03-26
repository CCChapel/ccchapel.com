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
                DesktopSearch.toggle();
            });

            $(".modal").click(function () {
                DesktopSearch.close();
            })

            //            //Field Loses Focus
            //            $(this.SearchField).focusout(function() {
            //                if (!$(DesktopSearch.IconClass).is(":focus")) {
            //                    console.log( $(DesktopSearch.IconClass).is(":focus") );
            //                    
            //                    DesktopSearch.close(); 
            //                }
            //                else {
            //                    console.log("icon clicked");
            //                }
            //            });
        },
        toggle: function () {
            //Toggle Icon
            $(this.IconClass).toggleClass("open");

            //Toggle Menu
            $(this.MenuItemsClass).toggle();

            //Toggle Width
            $(this.ItemClass).toggleClass("one-tenth").toggleClass("one-whole");
            $(this.IconClass).toggleClass("desk--one-whole").toggleClass("desk--one-tenth");

            //Toggle Banner Height
            $(this.BannerClass).toggleClass("fullHeight");

            //Clear Search Field
            $(this.FieldClass).val("");

            //Toggle Search Field
            $(this.FieldClass).toggleClass("show");

            //Toggle Modal
            CCChapel.toggleModal();

            //Set focus if visible
            if ($(this.FieldClass).hasClass("show")) {
                $(this.SearchField).focus();
            }
        },
        open: function () {
            //Show Icon
            $(this.IconClass).addClass("open");

            //Show Menu
            $(this.MenuItemsClass).hide();

            //Set Width
            $(this.ItemClass).removeClass("one-tenth").addClass("one-whole");
            $(this.IconClass).removeClass("desk--one-whole").addClass("desk--one-tenth");

            //Set Banner Height
            $(this.BannerClass).addClass("fullHeight");

            //Clear Search Field
            $(this.FieldClass).val("");

            //Show Search Field
            $(this.FieldClass).addClass("show");

            //Show Modal
            CCChapel.openModal();

            //Set focus
            $(this.SearchField).focus();
        },
        close: function () {
            //Hide Icon
            $(this.IconClass).removeClass("open");

            //Show Menu
            $(this.MenuItemsClass).show();

            //Hide Width
            $(this.ItemClass).addClass("one-tenth").removeClass("one-whole");
            $(this.IconClass).addClass("desk--one-whole").removeClass("desk--one-tenth");

            //Hide Banner Height
            $(this.BannerClass).removeClass("fullHeight");

            //Hide Modal
            CCChapel.closeModal();

            //Hide Search Field
            $(this.FieldClass).removeClass("show");

            //Clear Search Field
            $(this.FieldClass).val("");
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

            if (offset <= 0) {
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

            //Hide Current Campus in Other List
            $(this.ListClass).children("li").children(campusSelector).parent().hide();

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
                DesktopSearch.open();
            });
        },
        portableSetup: function () {
            $(this.CssClass).click(function () {
                //Open Menu
                MobileMenu.open();

                //Setup Focus
                $(DesktopSearch.SearchField).focus();
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

        //Functions
        setup: function () {
            //Toggle open and close
            $(this.Trigger).click(function () {
                MobileMenu.toggleMenu();
            });
        },
        toggleMenu: function () {
            //animate icon
            $(this.Trigger).toggleClass("close");

            //show items
            $(this.ItemsClass).show();

            //toggle screen lock
            $("body").toggleClass("hide-overflow");
            $("body").toggleClass("lock-position");

            CCChapel.toggleModal();

            //toggle menu
            $(this.MenuClass).slideToggle(250, function () {
                //blur backgrounds after menu displays
                //$(".notifications, .body, .footer").toggleClass("blur");  
            });

            //clear search
            $("#menu-search").val("");
        },
        open: function () {
            //animate icon
            $(this.Trigger).addClass("close");

            //show items
            $(this.ItemsClass).show();

            //toggle screen lock
            $("body").addClass("hide-overflow");
            $("body").addClass("lock-position");

            CCChapel.openModal();

            //toggle menu
            $(this.MenuClass).slideDown(250, function () {
                //blur backgrounds after menu displays
                //$(".notifications, .body, .footer").toggleClass("blur");   
            });
        },
        close: function () {
            //animate icon
            $(this.Trigger).removeClass("close");

            //toggle screen lock
            $("body").removeClass("hide-overflow");
            $("body").removeClass("lock-position");

            CCChapel.closeModal();

            //toggle menu
            $(this.MenuClass).slideUp(250, function () {
                //blur backgrounds after menu displays
                //$(".notifications, .body, .footer").toggleClass("blur");   
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

            //Homepage Campus Section
            HomepageCampusSection.desktopSetup();

            //Content Search Setup
            ContentSearch.desktopSetup();
        }

        //PORTABLE FUNCTIONS
        if ($(window).width() < desktopBreakpoint) {
            //Mobile Campus List
            CampusSelect.setup();

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