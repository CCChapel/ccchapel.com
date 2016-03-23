//************************************************
// MAPS
//***********************************************/
(function (CCChapel, $, undefined) {
    CCChapel.createMapInfo = function (name, address, link) {
        return '<div class="google-map__info-window">' +
                    '<div class="google-map__location-name">' + name + '</div>' +
                    '<div class="google-map__location-address">' + address + '</div>' +
                    '<div><a class="cta red" href="' + link + '" target="_blank">Get Directions</a></div>' +
               '</div>';
    }

    //************************************************
    // Public Properties
    //***********************************************/
    CCChapel.CampusLocations = [
        {
            name: 'Christ Community Chapel &ndash; Hudson Campus',
            coordinates: { lat: 41.231812, lng: -81.485023 },
            info: CCChapel.createMapInfo(
                "Christ Community Chapel &ndash; Hudson Campus",
                "750 W Streetsboro Street | Hudson, Ohio",
                "https://www.google.com/maps/dir//Christ+Community+Chapel+-+Hudson+Campus,+750+W+Streetsboro+St,+Hudson,+OH+44236,+United+States/@41.231745,-81.4859318,17z/data=!4m12!1m3!3m2!1s0x883120ef6e7919cd:0x308c6f24f427b843!2sChrist+Community+Chapel+-+Hudson+Campus!4m7!1m0!1m5!1m1!1s0x883120ef6e7919cd:0x308c6f24f427b843!2m2!1d-81.4837431!2d41.231745")
        },
        {
            name: 'Christ Community Chapel &ndash; Aurora Campus',
            coordinates: { lat: 41.323287, lng: -81.345287 },
            info: CCChapel.createMapInfo(
                "Christ Community Chapel &ndash; Aurora Campus",
                "252 N Chillicothe Road | Aurora, Ohio",
                "https://www.google.com/maps/dir//Christ+Community+Chapel+-+Aurora+Campus,+252+N+Chillicothe+Rd,+Aurora,+OH+44202,+United+States/@41.3240885,-81.3459627,17z/data=!4m12!1m3!3m2!1s0x883119d734fb37c5:0xb499c4ffae160675!2sChrist+Community+Chapel+-+Aurora+Campus!4m7!1m0!1m5!1m1!1s0x883119d734fb37c5:0xb499c4ffae160675!2m2!1d-81.343774!2d41.3240885")
        },
        {
            name: 'Christ Community Chapel &ndash; Stow Campus',
            coordinates: { lat: 41.158564, lng: -81.42059 },
            info: CCChapel.createMapInfo(
                "Christ Community Chapel &ndash; Stow Campus",
                "3900 Kent Road | Stow, Ohio",
                "https://www.google.com/maps/dir//Christ+Community+Chapel+-+Stow+Campus,+3900+Kent+Rd,+Stow,+OH+44224,+United+States/@41.1572009,-81.4232236,17z/data=!4m12!1m3!3m2!1s0x883125d3a382d885:0xc0fd9e409b822ec7!2sChrist+Community+Chapel+-+Stow+Campus!4m7!1m0!1m5!1m1!1s0x883125d3a382d885:0xc0fd9e409b822ec7!2m2!1d-81.4210349!2d41.1572009")
        },
        {
            name: 'Christ Community Chapel &ndash; Highland Square Campus',
            coordinates: { lat: 41.095619, lng: -81.544334 },
            info: CCChapel.createMapInfo(
                "Christ Community Chapel &ndash; Highland Square Campus",
                "<div class='accent'>Meeting at Portage Path Community Learning Center</div> 55 S Portage Path | Akron, Ohio",
                "https://www.google.com/maps/dir//Portage+Path+CLC+Elementary+School,+55+S+Portage+Path,+Akron,+OH+44303/@41.0955219,-81.5465821,17z/data=!4m12!1m3!3m2!1s0x8830d7b3e1a79a8d:0x436ee5a6712ac32c!2sPortage+Path+CLC+Elementary+School!4m7!1m0!1m5!1m1!1s0x8830d7b3e1a79a8d:0x436ee5a6712ac32c!2m2!1d-81.5443934!2d41.0955219")
        }
    ];

    /************************************************
    // Private Properties
    //***********************************************/
    var map;
    var bounds;

    //Marker Icon Options
    var markerIcon = {
        url: "/Content/2016-Q1/images/icons/maps/google-map-icon.png",
        size: new google.maps.Size(75, 75)
    };

    //Option Defaults
    var labelOptionDefaults = {
        content: "",
        anchor: new google.maps.Point(-40, 75),
        className: "google-map__label"
    };
    var infoWindowDefaults = {
        content: ""
    };

    //Create Info Window
    var infoWindow = new google.maps.InfoWindow({
        content: "Loading..."
    });

    /************************************************
    // Public Methods
    //***********************************************/
    CCChapel.createMap = function (elementID, options) {
        //Setup Defaults
        var defaults = {
            markers: new Array(),
            fitAllMarkers: true,
            center: "",
            zoom: 11,
            minZoom: 11,
            maxZoom: 11
        };

        options = $.extend({}, defaults, options);

        //Create Map
        mapOptions = {};
        mapOptions.zoom = options.zoom;
        if (options.center !== "") {
            mapOptions.center = options.center;
        }

        map = new google.maps.Map(document.getElementById(elementID), mapOptions);

        //Create Viewpoint Bound
        bounds = new google.maps.LatLngBounds();

        //Create Markers and Click Event
        for (var i = 0; i < options.markers.length; i++) {
            if (typeof options.markers[i].coordinates !== 'undefined') {
                //Using Coordinates
                createMarkerFromCoordinates(
                    options.markers[i].coordinates,
                    {
                        content: options.markers[i].name
                    },
                    {
                        content: options.markers[i].info
                    });
            }
            else if (typeof options.markers[i].streetAddress !== 'undefined') {
                //Using Addresses
                createMarkerFromAddress(
                    options.markers[i].streetAddress,
                    {
                        content: options.markers[i].name
                    },
                    {
                        content: options.markers[i].info
                    });
            }
        }

        //Check to fit to bounds
        if (options.fitAllMarkers === true) {
            //Fit bounds to map
            map.fitBounds(bounds);
        }

        //Check Zoom
        checkZoom(options.minZoom, options.maxZoom);
    }

    /*
    //    CCChapel.getUserLocation = function() {
    ////        var pos = {
    ////            lat: 0,
    ////            lng: 0
    ////        };
            
    //        if (navigator.geolocation) {
    //            navigator.geolocation.getCurrentPosition(function(position) {
    //                //pos.lat = position.coords.latitude;
    //                //pos.lng = position.coords.longitude;

    //                var pos = {
    //                    lat: position.coords.latitude,
    //                    lng: position.coords.longitude
    //                };
                    
    //                console.log("before");
    //                console.log(pos.lat);
    //                console.log(pos.lng);
    //                //map.setCenter(pos);
                    
    //                return pos;
    //            })(pos);

    //            console.log("after");
    //            console.log(pos.lat);
    //            console.log(pos.lng);
    //        } 
    //        else {
    //            // Browser doesn't support Geolocation
    //            //handleLocationError(false, infoWindow, map.getCenter());
    //        }
    //    */
        
    //************************************************
    // Private Methods
    //***********************************************/
    /// <summary>
    /// Checks the map's current zoom level and sets it if outside the bounds
    /// </summary>
    /// <param name="min">The minimum zoom level</param>
    /// <param name="max">The maximum zoom level</param>
    function checkZoom(min, max) {
        if (map.getZoom() > max) {
            map.setZoom(max);
        }
        else if (map.getZoom() < min) {
            map.setZoom(options.min);
        }
    }

    /// <summary>
    /// Creates a marker from Longitude and Latitude coordinates
    /// </summary>
    /// <param name="coordinates">Coordinates object</param>
    /// <param name="labelOptions">Options for Marker Label</param>
    /// <param name="infoWindowOptions">Options for Info Window</param>
    function createMarkerFromCoordinates(coordinates, labelOptions, infoWindowOptions) {
        //Create Label Options
        labelOptions = $.extend({}, labelOptionDefaults, labelOptions);

        //Create Info Window Options
        infoWindowOptions = $.extend({}, infoWindowDefaults, infoWindowOptions);

        //Create Marker
        var marker = new MarkerWithLabel({
            position: coordinates,
            draggable: false,
            map: map,
            icon: markerIcon,
            labelContent: labelOptions.content,
            labelAnchor: labelOptions.anchor,
            labelClass: labelOptions.className,
            animation: google.maps.Animation.DROP
        });

        //Add click event for InfoWindow
        google.maps.event.addListener(marker, 'click', (function (marker) {
            return function () {
                //Load proper information for clicked campus
                infoWindow.setContent(infoWindowOptions.content);

                //Open Info Window
                infoWindow.open(map, marker);
            }
        })(marker));

        //Extend bounds
        bounds.extend(
            new google.maps.LatLng(
                coordinates.lat,
                coordinates.lng)
        );
    }

    /// <summary>
    /// Creates a marker from Address
    /// </summary>
    /// <param name="position">Full address of location</param>
    /// <param name="labelOptions">Options for Marker Label</param>
    /// <param name="infoWindowOptions">Options for Info Window</param>
    function createMarkerFromAddress(address, labelOptions, infoWindowOptions) {
        //Create Label Options
        labelOptions = $.extend({}, labelOptionDefaults, labelOptions);

        //Create Info Window Options
        infoWindowOptions = $.extend({}, infoWindowDefaults, infoWindowOptions);
        
        //Create Geocoder
        var geocoder = new google.maps.Geocoder();

        //Geocode Address & Add to Map
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                var marker = new MarkerWithLabel({
                    position: results[0].geometry.location,
                    draggable: false,
                    map: map,
                    icon: markerIcon,
                    labelContent: labelOptions.content,
                    labelAnchor: labelOptions.anchor,
                    labelClass: labelOptions.className,
                    animation: google.maps.Animation.DROP
                });

                //Add click event for InfoWindow
                google.maps.event.addListener(marker, 'click', (function (marker) {
                    return function () {
                        //Load proper information for clicked campus
                        infoWindow.setContent(infoWindowOptions.content);

                        //Open Info Window
                        infoWindow.open(map, marker);
                    }
                })(marker));

                //Extend bounds
                bounds.extend(
                    new google.maps.LatLng(
                        results[0].geometry.location.lat(),
                        results[0].geometry.location.lng())
                );
                map.fitBounds(bounds);
                checkZoom(11, 11);
            } else {
                console.log('Geocode was not successful for the following reason: ' + status);
            }
        });
    }
}(window.CCChapel = window.CCChapel || {}, jQuery));