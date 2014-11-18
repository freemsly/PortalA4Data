using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.Customers;

namespace CamprayPortal.Web.Controllers
{
    public class DownloadsController : BasePublicController
    {
        private readonly IWorkContext _workContext;

        public DownloadsController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        // GET: Downloads
        public ActionResult Homepage()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return RedirectToRoute("Login");
            return TopicModelView();
        }
    }
}