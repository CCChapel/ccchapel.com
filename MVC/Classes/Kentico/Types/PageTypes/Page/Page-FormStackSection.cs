using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CMS.DocumentEngine.Types
{
    /// <summary>
    /// Represents a content item of type FormStackSection.
    /// </summary>
    public partial class FormStackSection : TreeNode
    {
        /// <summary>
        /// Creates the necessary script markup to insert the FormStack form
        /// </summary>
        public string FormHtml
        {
            get
            {
                //<script type="text/javascript" src="https://ccchapel.formstack.com/forms/js.php/@Html.Raw(Model.SectionFormStackUrl)?no_style=1"></script>
                //<noscript>Please enable JavaScript to use this page</noscript>

                //Create string
                string html = "";

                //Create <script></script>
                var script = new TagBuilder("script");
                script.MergeAttribute("type", "text/javascript");
                script.MergeAttribute("src",
                    string.Format("https://ccchapel.formstack.com/forms/js.php/{0}?nojquery=1&nojqueryui=1&no_style=1&no_style_strict=1", SectionFormStackUrl));

                //Add <script>
                html += script.ToString();

                //Create <noscript>
                var noscript = new TagBuilder("noscript");
                noscript.InnerHtml = "Please enable JavaScript to use this page.";

                //Add <noscript>
                html += noscript.ToString();

                return html;
            }
        }
        public string SectionContentWithForm
        {
            get
            {
                //Replace {form} with Form HTML
                return SectionContent.Replace("{form}", FormHtml);
            }
        }
    }
}