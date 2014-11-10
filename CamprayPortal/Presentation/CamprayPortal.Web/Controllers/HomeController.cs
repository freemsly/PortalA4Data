using System.Web.Mvc;
using CamprayPortal.Web.Framework.Security;

namespace CamprayPortal.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
