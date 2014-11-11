using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class SolutionsController : BasePublicController
    {
        // GET: Solutions
        public ActionResult Applications()
        {
            return TopicModelView();
        }

        public ActionResult Benchmark()
        {
            return TopicModelView();
        }

        public ActionResult Benefit()
        {
            return TopicModelView();
        }

        public ActionResult BriefsOverview()
        {
            return TopicModelView();
        } 

        public ActionResult Homepage() 
        {
            return TopicModelView();
        }

        public ActionResult CustomerTestimonials()
        { 
            return TopicModelView();
        }

        public ActionResult CaseStudies()
        {
            return TopicModelView();
        }

        public ActionResult DifferentAarchitecture()
        {
            return TopicModelView();
        }
    }
}