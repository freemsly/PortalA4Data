using System.Web.Mvc;
using CamprayPortal.Web.Models.Solutions;

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
            var model = new BenchmarkViewModel
            {
                Overview = TopicModelContent("Solutions-BenchmarkOverview"),
                Mobile = TopicModelContent("Solutions-BenchmarkMobile"),
                SpeedTime = TopicModelContent("Solutions-BenchmarkSpeedTime")
            };
            return View(model);
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