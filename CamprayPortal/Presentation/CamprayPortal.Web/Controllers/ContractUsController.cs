using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public class ContractUsController : BasePublicController
    {
        public ActionResult HomePage()  
        {
            return TopicModelView();
        }

    }
}