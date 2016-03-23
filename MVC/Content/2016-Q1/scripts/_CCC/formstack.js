//************************************************
// FORMSTACK
//***********************************************/
(function (CCChapel, $, undefined) {
    //************************************************
    // Public Methods
    //***********************************************/
    CCChapel.formstackAutopopulate = function () {
        $(document).ready(function () {
            //Load Campus
            var currentCampus = CCChapel.CurrentCampus;
            currentCampus = currentCampus.charAt(0).toUpperCase() + currentCampus.slice(1) + " Campus";
            $("#field40375789").val(currentCampus);

            //Load Query String Values
            var qs = getQueryString();

            $.each(qs, function (key, value) {
                var selector = "#" + key;
                $(selector).val(value);
            });
        });
    };
}(window.CCChapel = window.CCChapel || {}, jQuery));