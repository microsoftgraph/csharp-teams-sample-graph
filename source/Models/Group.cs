using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class Group
    {
        public String displayName { get; set; }
        public String mailNickname { get; set; }
        public String description { get; set; }
        public String[] groupTypes { get; set; }
        public Boolean mailEnabled { get; set; }
        public Boolean securityEnabled { get; set; }
        public String visibility { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string id { get; set; }
    }
}