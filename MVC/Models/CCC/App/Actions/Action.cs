using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Actions
{
    public partial class Action
    {
        /// <summary>
        /// Navigation Actions are most common, and typically move the user to a new screen.The named handler is invoked to download and process the content at the provided url.
        /// Content Actions are typically used to download and display extra content within the existing screen.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public enum ActionTypes
        {
            [EnumMember(Value = "navigation")]
            Navigation,

            [EnumMember(Value = "content")]
            Content
        };

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Handlers
        {
            [EnumMember(Value = "list")]
            List,

            [EnumMember(Value = "mediaDetail")]
            MediaDetial
        };

        /// <summary>
        /// The name of the Handler invoked by this action. Any handler name is syntactically valid, though all handlers may not be supported in all contexts. Common values include list and mediaDetail.
        /// </summary>
        public Handlers Handler { get; set; }

        ///// <summary>
        ///// Legacy value valid only on iOS. When this value is TRUE the provided handler in a smaller popover window on tablet devices.
        ///// </summary>
        //public bool PopOver { get; set; }

        /// <summary>
        /// Defines the Action Type. Can be navigation or content.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ActionTypes? Type { get; set; }

        /// <summary>
        /// Provides the source of content for this Action object. This parameter is required for all Handlers which access remote data (the common case).
        /// This parameter is optional only for Actions which do not access remote data, such as the phone handler.
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Url { get; set; }
    }
}