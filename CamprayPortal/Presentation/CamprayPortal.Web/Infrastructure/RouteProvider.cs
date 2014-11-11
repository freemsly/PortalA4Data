using System.Web.Mvc;
using System.Web.Routing;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Framework.Mvc.Routes;

namespace CamprayPortal.Web.Infrastructure
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //We reordered our routes so the most used ones are on top. It can improve performance.

            //home page
            routes.MapLocalizedRoute("HomePage",
                            "",
                            new { controller = "Home", action = "Index" },
                            new[] { "CamprayPortal.Web.Controllers" });

            //install
            routes.MapRoute("Installation",
                            "install",
                            new { controller = "Install", action = "Index" },
                            new[] { "CamprayPortal.Web.Controllers" });
            
            //page not found
            routes.MapLocalizedRoute("PageNotFound",
                            "page-not-found",
                            new { controller = "Common", action = "PageNotFound" },
                            new[] { "CamprayPortal.Web.Controllers" });

        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
