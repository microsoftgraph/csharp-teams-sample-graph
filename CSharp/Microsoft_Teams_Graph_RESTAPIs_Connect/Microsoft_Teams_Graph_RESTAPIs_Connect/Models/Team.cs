using Newtonsoft.Json;

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
    }
}