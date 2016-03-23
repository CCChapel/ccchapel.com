//<span class="css3-metro-dropdown">
//************************************************
// SELECT BOXES
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Methods
    //***********************************************/
    CCChapel.formatSelectBoxes = function () {
        $(document).ready(function () {
            //Wrap each select element in <span class="css3-metro-dropdown">
            $("select").wrap('<span class="css3-metro-dropdown"></span>');
        });
    };
}(window.CCChapel = window.CCChapel || {}, jQuery));