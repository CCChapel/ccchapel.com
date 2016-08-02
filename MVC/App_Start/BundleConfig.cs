using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace MVC
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            /**********************************************************
             * SCRIPTS
             **********************************************************/
            #region AngularJS 
            //var angularCdnPath = "https://ajax.googleapis.com/ajax/libs/angularjs/1.5.5/angular.min.js";

            //bundles.Add(new ScriptBundle("~/bundles/angular", angularCdnPath).Include("~/Content/2016-Q1/scripts/angular/angular-{version}.min.js"));
            #endregion

            #region jQuery
            //Add link to jQuery on the CDN
            var jqueryCdnPath = "https://code.jquery.com/jquery-2.2.0.min.js";
            var jqueryUiCdnPath = "https://code.jquery.com/ui/1.11.4/jquery-ui.min.js";

            bundles.Add(new ScriptBundle("~/bundles/jquery", jqueryCdnPath).Include("~/Content/2016-Q1/scripts/jquery/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui", jqueryUiCdnPath).Include("~/Content/2016-Q1/scripts/jquery/jquery-ui-{version}.js"));
            #endregion

            #region Google
            var googleMapsCdnPath = "https://maps.googleapis.com/maps/api/js?key=AIzaSyAz-PEj0orqPeMuXxor8KRCanI843fOYJY";

            bundles.Add(new ScriptBundle("~/bundles/google/maps", googleMapsCdnPath));
            bundles.Add(new ScriptBundle("~/bundles/google/maps/marker").Include("~/Content/2016-Q1/scripts/google/maps/markerwithlabel.js"));
            #endregion

            #region Custom Scripts
            bundles.Add(new ScriptBundle("~/bundles/scripts").IncludeDirectory("~/Content/2016-Q1/scripts/_CCC/", "*.js", true));
            #endregion

            /**********************************************************
             * STYLES
             **********************************************************/
            #region FontAwesome
            var fontAwesomeCdnPath = "https://maxcdn.bootstrapcdn.com/font-awesome/4.6.3/css/font-awesome.min.css";
            bundles.Add(new StyleBundle("~/bundles/font-awesome", fontAwesomeCdnPath).Include("~/Content/2016-Q1/styles/font-awesome/font-awesome-{version}.css"));
            #endregion

            #region Custom Styles
            bundles.Add(new StyleBundle("~/bundles/styles").Include("~/Content/2016-Q1/styles/_CCC/styles.css"));
            #endregion

            BundleTable.EnableOptimizations = true;
        }
    }
}