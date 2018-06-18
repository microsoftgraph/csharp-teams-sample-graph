using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class TeamsApp
    {
        public string id { get; set; }
        public string name { get; set; }
        public string version { get; set; }
        public string isBlocked { get; set; }
        public string installedState { get; set; }
        public string context { get; set; }
    }
}