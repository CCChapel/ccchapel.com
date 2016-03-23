//************************************************
// Current Campus
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Methods
    //***********************************************/
    CCChapel.CurrentCampus = getCurrentCampus(); //"Hudson"; //$(selector).val();

    function getCurrentCampus() {
        var selector = "#currentCampus";
        var campus = "default";

        $(document).ready(function () {
            campus = $("#currentCampus").val();

            CCChapel.CurrentCampus = campus;
            //alert(campus);
        })

        return campus;
    }
}(window.CCChapel = window.CCChapel || {}, jQuery));