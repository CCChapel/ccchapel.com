using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Actions
{
    /// <summary>
    /// When handler = defaultShare, the body property should contain only plain-text content.This share object will be used for simple sharing services such as SMS (text messaging), or when htmlShare content is unavailable.
    ///
    /// When handler = htmlShare, the body property can contain rich HTML-formatted text.This share object will be used for services which support rich sharing such as Email.
    /// </summary>
    public class ShareAction : Action
    {
        [JsonConverter(typeof(StringEnumConverter))]
        new public enum Handlers
        {
            [EnumMember(Value = "defaultShare")]
            DefaultShare,

            [EnumMember(Value = "htmlShare")]
            HtmlShare
        }

        /// <summary>
        /// The name of the Handler invoked by this action. Any handler name is syntactically valid, though all handlers may not be supported in all contexts. Common values include list and mediaDetail.
        /// </summary>
        new public Handlers Handler { get; set; }

        /// <summary>
        /// The body contains the share text that you wish to pass off to an external share service. Depending on the particular service chosen by the user, the url value may be passed separately or appended to the body text so that you can share a link along with your text content.
        /// </summary>
        public string Body { get; set; }
    }
}