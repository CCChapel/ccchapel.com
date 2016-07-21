using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CCC.Models.App.Actions
{
    public class ListAction : Action
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Styles
        {
            [EnumMember(Value = "plain")]
            Plain,

            [EnumMember(Value = "grid")]
            Grid,

            [EnumMember(Value = "carousel")]
            Carousel,

            [EnumMember(Value = "reader")]
            Reader
        }

        /// <summary>
        /// This value allows the user to navigate directly to a particular item in the list specified by the url (Example: linking directly to a particular blog post when style = reader). This value may not be supported by all List styles.
        /// </summary>
        public int SelectedIndex { get; set; }

        /// <summary>
        /// Specfies the desired display style for the list at the provided url. Can be carousel, grid, plain, reader
        /// </summary>
        public Styles Style { get; set; }
    }
}