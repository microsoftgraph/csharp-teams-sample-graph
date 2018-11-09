using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class TeamMessagingSettings
    {
        public Boolean allowUserEditMessages { get; set; }
        public Boolean allowUserDeleteMessages { get; set; }
        public Boolean allowOwnerDeleteMessages { get; set; }
        public Boolean allowTeamMentions { get; set; }
        public Boolean allowChannelMentions { get; set; }
    }
}