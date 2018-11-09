using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class Team
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TeamGuestSettings guestSettings { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TeamMemberSettings memberSettings { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TeamMessagingSettings messagingSettings { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TeamFunSettings funSettings { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string id { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string displayName { get; set; }
    }

    public class ResultList<T>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public T[] value { get; set; }
    }
}