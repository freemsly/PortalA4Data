using System.Web.Routing;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Framework.Mvc.Routes;
using CamprayPortal.Web.Framework.Seo;

namespace CamprayPortal.Web.Infrastructure
{
    public partial class GenericUrlRouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //generic URLs
            routes.MapGenericPathRoute("GenericUrl",
                                       "{generic_se_name}",
                                       new { controller = "Common", action = "GenericUrl" },
                                       new[] { "CamprayPortal.Web.Controllers" });



            routes.MapLocalizedRoute("Topic",
                         "{SeName}",
                         new { controller = "Topic", action = "TopicDetails" },
                         new[] { "CamprayPortal.Web.Controllers" });


   
   

            //the last route. it's used when none of registered routes could be used for the current request
            //but it this case we cannot process non-registered routes (/controller/action)
            //routes.MapLocalizedRoute(
            //    "PageNotFound-Wildchar",
            //    "{*url}",
            //    new { controller = "Common", action = "PageNotFound" },
            //    new[] { "CamprayPortal.Web.Controllers" });
        }

        public int Priority
        {
            get
            {
                //it should be the last route
                //we do not set it to -int.MaxValue so it could be overriden (if required)
                return -1000000;
            }
        }
    }
}
