using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class PartnerController : BasePublicController
    {
        // GET: Partner
        public ActionResult Homepage()
        {
            return TopicModelView();
        }
    }
}