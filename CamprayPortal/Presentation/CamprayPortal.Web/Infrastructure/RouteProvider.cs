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
             
                      //RegisterResult  
            routes.MapLocalizedRoute("RegisterResult",
                            "Support/",
                            new { controller = "Support", action = "RegisterResult" },
                            new[] { "CamprayPortal.Web.Controllers" });

            routes.MapLocalizedRoute("AccountActivation",
                     "customer/activation",
                     new { controller = "Support", action = "AccountActivation" },
                     new[] { "CamprayPortal.Web.Controllers" });

            //login
            routes.MapLocalizedRoute("Login",
                            "login/",
                            new { controller = "Support", action = "CustomerPortal" },
                            new[] { "CamprayPortal.Web.Controllers" });

            //change language (AJAX link)
            routes.MapLocalizedRoute("ChangeLanguage",
                            "changelanguage/{langid}",
                            new { controller = "Common", action = "SetLanguage" },
                            new { langid = @"\d+" },
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
