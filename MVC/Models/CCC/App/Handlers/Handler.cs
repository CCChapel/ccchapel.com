using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CCC.Models.App.Objects;
using Newtonsoft.Json;

namespace CCC.Models.App.Handlers
{
    public partial class Handler
    {
        /// <summary>
        /// HEADER object defining content for the top of the page, such as the page title, banner image, etc.
        /// </summary>
        [JsonProperty(Order = 1)]
        public Header Header { get; set; }
    }
}