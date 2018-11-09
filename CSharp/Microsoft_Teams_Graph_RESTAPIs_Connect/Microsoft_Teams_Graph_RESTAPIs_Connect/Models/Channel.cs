using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class Channel
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string id { get; set; }

        public string displayName { get; set; }
        public string description { get; set; }
    }
}