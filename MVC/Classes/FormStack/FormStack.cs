using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FormStack
{
    public class FormStackForm
    {
        /// <summary>
        /// The value of the URL set in FormStack at Settings > General.
        /// </summary>
        public string FormUrl { get; set; }

        /// <summary>
        /// Creates the necessary script markup to insert the FormStack form
        /// </summary>
        public string FormHtml
        {
            get
            {
                //<script type="text/javascript" src="https://ccchapel.formstack.com/forms/js.php/@Html.Raw(Model.SectionFormStackUrl)?no_style=1"></script>
                //<noscript>Please enable JavaScript to use this page</noscript>
                //<script>CCChapel.formatSelectBoxes();CCChapel.formstackAutopopulate();</script>

                //Create string
                string html = "";

                //Create <script></script>
                var script = new TagBuilder("script");
                script.MergeAttribute("type", "text/javascript");
                script.MergeAttribute("src",
                    string.Format("https://ccchapel.formstack.com/forms/js.php/{0}?nojquery=1&nojqueryui=1&no_style=1&no_style_strict=1", FormUrl));

                //Add <script>
                html += script.ToString();

                //Create <noscript>
                var noscript = new TagBuilder("noscript");
                noscript.InnerHtml = "Please enable JavaScript to use this page.";

                //Add <noscript>
                html += noscript.ToString();

                //Select Boxes Script
                var customScript = new TagBuilder("script");
                customScript.InnerHtml = "CCChapel.formatSelectBoxes();CCChapel.formstackAutopopulate();";

                //Add <script>
                html += customScript.ToString();

                return html;
            }
        }

        #region Constructors
        public FormStackForm() { }

        public FormStackForm(string formUrl)
        {
            FormUrl = formUrl;
        }
        #endregion
    }
}