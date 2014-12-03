using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class ProductController : BasePublicController
    {
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
            return View(); 
        }


        
        
    }
}