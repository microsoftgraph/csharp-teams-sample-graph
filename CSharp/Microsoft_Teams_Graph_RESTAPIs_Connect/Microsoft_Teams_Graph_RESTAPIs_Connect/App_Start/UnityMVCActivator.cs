using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

 
   //[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Microsoft_Teams_Graph_RESTAPIs_Connect.App_Start.UnityWebActivator), "Start")]
   // [assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Microsoft_Teams_Graph_RESTAPIs_Connect.App_Start.UnityWebActivator), "Shutdown")]

namespace Microsoft_Teams_Graph_RESTAPIs_Connect.App_Start
{
   
    public static class UnityWebActivator
    {
        /// <summary>Integrates Unity when the application starts.</summary>
        public static void Start()
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            // TODO: Uncomment if you want to use PerRequestLifetimeManager
            // Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility.RegisterModule(typeof(UnityPerRequestHttpModule));
        }

        /// <summary>Disposes the Unity container when the application is shut down.</summary>
        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}