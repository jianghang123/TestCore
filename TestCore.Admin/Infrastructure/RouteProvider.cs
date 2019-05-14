using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using TestCore.Common.Routing;

namespace TestCore.Admin.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        #region Methods

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routeBuilder">Route builder</param>
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            //areas
            routeBuilder.MapRoute(name: "areaRoute", template: "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            //default
            routeBuilder.MapRoute("Default", "{controller=login}/{action=index}/{id?}");
            //error
            routeBuilder.MapRoute(name: "Error", template: "error", defaults: new
            {
                controller = "Common",
                action = "Error",
            });
            //page not found
            routeBuilder.MapRoute(name: "PageNotFound", template: "page-not-found", defaults: new
            {
                controller = "Common",
                action = "PageNotFound",
            });
            //page no permission
            routeBuilder.MapRoute(name: "PageNoPermission", template: "page-no-permission", defaults: new
            {
                controller = "Common",
                action = "NoPermission",
            });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => -1000000;

        #endregion
    }
}
