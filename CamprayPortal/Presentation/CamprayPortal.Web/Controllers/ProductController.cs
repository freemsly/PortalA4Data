using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.Customers;

namespace CamprayPortal.Web.Controllers
{
    public class ProductController : BasePublicController
    {
        private readonly IWorkContext _workContext;

        public ProductController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        // GET: Product
        public ActionResult FeatureBriefs()
        {
            return TopicModelView();
        }

        public ActionResult Homepage()
        {
            return TopicModelView();
        }


        public ActionResult JetAppliance()
        {
            return TopicModelView();
        }


        public ActionResult JetVM()
        {
            return TopicModelView();
        }

        public ActionResult ApplicationNotes()
        {
            return TopicModelView();
        }


        public ActionResult ProductPayment()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return RedirectToRoute("Login");
            return View();
        }


    }
}