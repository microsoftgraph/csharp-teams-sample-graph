using System.Web;
using System.Web.Mvc;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
