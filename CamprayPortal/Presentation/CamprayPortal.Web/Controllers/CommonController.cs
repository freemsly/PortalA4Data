﻿using System.Web.Routing;
using System.Web.Mvc;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {


        #region Methods

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

            return View();
        }


        // GET: Common
        [ChildActionOnly]
        public ActionResult ContactUs()
        {
            return View();
        }


        [HttpPost]
        public JsonResult MessageUs(MessageUsModel model)
        {
            if (ModelState.IsValid)
            {


                return Json(1);
            }
            return Json(0);
        }


        [ChildActionOnly]
        public ActionResult Navigation()
        {
            RouteData routeData = RouteTable.Routes.GetRouteData(this.HttpContext);
            if (routeData != null)
            {
                var controller = routeData.Values["controller"];
                var action = routeData.Values["action"];
                return View(new MenuNavigation
                {
                    Action = action.ToString(),
                    Controller = controller.ToString()
                });
            }
            return View();
        }


        // GET: Common
        [ChildActionOnly]
        public ActionResult SitMap()
        { 
            return View();
        } 


        #endregion

    }
}
