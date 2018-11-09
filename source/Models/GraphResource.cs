using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.Models
{
    public class CreateChannel
    {
        public string displayName { get; set; }
        public string description { get; set; }
    }

    public class PostMessage
    {
        public RootMessage rootMessage { get; set; }
    }

    public class RootMessage
    {
        public MessageBody body { get; set; }
    }

    public class MessageBody
    {
        public string content { get; set; }
    }
}