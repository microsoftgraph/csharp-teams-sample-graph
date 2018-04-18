using System.Web.Mvc;
using Unity;

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.App_Start
{
    internal class UnityFilterAttributeFilterProvider : IFilterProvider
    {
        private IUnityContainer container;

        public UnityFilterAttributeFilterProvider(IUnityContainer container)
        {
            this.container = container;
        }
    }
}