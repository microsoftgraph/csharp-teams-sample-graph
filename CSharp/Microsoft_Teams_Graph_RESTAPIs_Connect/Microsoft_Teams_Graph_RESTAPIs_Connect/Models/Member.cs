using Newtonsoft.Json;
using System;

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
}