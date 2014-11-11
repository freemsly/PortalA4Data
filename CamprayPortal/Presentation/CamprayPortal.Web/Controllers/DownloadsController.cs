using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class DownloadsController : BasePublicController
    {
        // GET: Downloads
        public ActionResult Homepage()
        {
            return TopicModelView();
        }
    }
}