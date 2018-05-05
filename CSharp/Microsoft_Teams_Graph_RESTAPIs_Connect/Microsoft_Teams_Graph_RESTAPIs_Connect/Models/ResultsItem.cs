using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class ResultsItem
    {
        // The ID and display name for the entity's radio button.
        public string Id { get; set; }
        public string Display { get; set; }

        // The properties of an entity that display in the UI.
        public Dictionary<string, object> Properties;

        public ResultsItem()
        {
            Properties = new Dictionary<string, object>();
        }
    }
}