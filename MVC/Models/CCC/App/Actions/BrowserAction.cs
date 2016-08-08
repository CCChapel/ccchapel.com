using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Actions
{
    public partial class BrowserAction : Action
    {
        [JsonConverter(typeof(StringEnumConverter))]
        new public enum Handlers
        {
            [EnumMember(Value = "browser")]
            Browser
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Styles
        {
            [EnumMember(Value = "internal")]
            Internal,

            [EnumMember(Value = "external")]
            External
        }

        /// <summary>
        /// The name of the Handler invoked by this action. Any handler name is syntactically valid, though all handlers may not be supported in all contexts. Common values include list and mediaDetail.
        /// </summary>
        new public Handlers Handler { get; set; }

        /// <summary>
        /// When showBrowserControls = true, the app will display a control bar at the bottom of the screen containing Back, Forward, and Refresh buttons for navigating between web pages.
        ///
        /// This value is only valid when style = internal (because and external browser is outside of the app and will display its own preferred UI).
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ShowBrowserControls { get; set; }

        /// <summary>
        /// An internal browser displays web content within the context of the app, giving the experience that the user has not left the app. The web address is not displayed, and you have the option to show or hide the web navigation conrols with showBrowserControls.
        ///
        /// When style = external, the user is launched outside of the app into the device's default web browser (Chrome, Safari, etc). This is typically the wronge behavior, but is necessary for certain scenarios such as on-device donations, which must take place outside the app in order to comply with some app store policies.
        /// </summary>
        public Styles Style { get; set; }

        /// <summary>
        /// Location of the HTML content used to populate the internal web browser.
        ///
        /// If url is non-null, the app will download a separate JSON feed for the BROWSER HANDLER object, which is then responsible for specifying the contentUrl.
        ///
        /// Optionally, contentUrl can be specified directly here in the ACTION, so that a separate JSON feed download is not necessary.
        ///
        /// If both url and contentUrl are non-null, the behavior is undefined.
        /// </summary>
        public string ContentUrl { get; set; }
    }
}