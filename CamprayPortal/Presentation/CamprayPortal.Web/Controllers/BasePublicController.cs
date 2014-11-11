using System;
using System.Web.Mvc;
using System.Web.Routing;
using CamprayPortal.Core.Domain.Topics;
using CamprayPortal.Core.Infrastructure;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.Seo;
using CamprayPortal.Services.Topics;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Controllers;
using CamprayPortal.Web.Framework.Security;
using CamprayPortal.Web.Framework.Seo;
using CamprayPortal.Web.Models.Topics;

namespace CamprayPortal.Web.Controllers
{

    public abstract partial class BasePublicController : BaseController
    {
        protected virtual ActionResult InvokeHttp404()
        {
            // Call target Controller and pass the routeData.
            IController errorController = EngineContext.Current.Resolve<CommonController>();

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Common");
            routeData.Values.Add("action", "PageNotFound");

            errorController.Execute(new RequestContext(this.HttpContext, routeData));

            return new EmptyResult();
        }


        [NonAction]
        protected virtual TopicModel PrepareTopicModel(Topic topic)
        {
            if (topic == null)
                throw new ArgumentNullException("topic");

            var model = new TopicModel()
            {
                Id = topic.Id,
                SystemName = topic.SystemName,
                IncludeInSitemap = topic.IncludeInSitemap,
                IsPasswordProtected = topic.IsPasswordProtected,
                Title = topic.IsPasswordProtected ? "" : topic.GetLocalized(x => x.Title),
                Body = topic.IsPasswordProtected ? "" : topic.GetLocalized(x => x.Body),
                MetaKeywords = topic.GetLocalized(x => x.MetaKeywords),
                MetaDescription = topic.GetLocalized(x => x.MetaDescription),
                MetaTitle = topic.GetLocalized(x => x.MetaTitle),
                SeName = topic.GetSeName(),
            };
            return model;
        }

        [NonAction]
        protected virtual TopicModel TopicModelContent(string key = null)
        {
            var topicService = EngineContext.Current.Resolve<ITopicService>();
            var topickey = key;
            if (key == null)
            {
                var routeData = RouteTable.Routes.GetRouteData(this.HttpContext);
                if (routeData != null)
                {
                    var controller = routeData.Values["controller"];
                    var action = routeData.Values["action"];
                    topickey = string.Format("{0}-{1}", controller, action);
                }
            }
            var topic = topicService.GetTopicBySystemName(topickey);
            if (null != topic)
            {
                //SEO
                ViewBag.Description = topic.MetaDescription;
                ViewBag.Keywords = topic.MetaKeywords;
                ViewBag.Generator = topic.MetaTitle;

                return PrepareTopicModel(topic);
            }
            return new TopicModel();
        }


        protected virtual ActionResult TopicModelView(string key = null)
        {
            return View(TopicModelContent(key));
        }

    }
}
