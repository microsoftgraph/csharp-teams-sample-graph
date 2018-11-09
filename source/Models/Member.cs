using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class Member
    {
        public String groupId { get; set; }
        public String upn { get; set; }
        public bool owner { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string id { get; set; }
    }

    public class User
    {
        public string id { get; set; }
    }
}