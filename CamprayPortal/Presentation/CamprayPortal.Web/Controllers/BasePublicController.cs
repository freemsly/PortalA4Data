using System.Web.Mvc;
using System.Web.Routing;
using CamprayPortal.Core.Infrastructure;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Controllers;
using CamprayPortal.Web.Framework.Security;
using CamprayPortal.Web.Framework.Seo;

namespace CamprayPortal.Web.Controllers
{
    [CheckAffiliate]
    [StoreClosed]
    [PublicStoreAllowNavigation]
    [LanguageSeoCode]
    [NopHttpsRequirement(SslRequirement.NoMatter)]
    [WwwRequirement]
    public abstract partial class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = EngineContext.Current.Resolve<CamprayPortal.Web.Controllers.CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }

    }
}
