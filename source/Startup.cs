using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Microsoft_Teams_Graph_RESTAPIs_Connect.Startup))]

namespace Microsoft_Teams_Graph_RESTAPIs_Connect
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
