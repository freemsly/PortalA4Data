using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class TechnologyController : BasePublicController
    {
        // GET: Technology
        public ActionResult Homepage()
        {
            return TopicModelView();
        }
    }
}