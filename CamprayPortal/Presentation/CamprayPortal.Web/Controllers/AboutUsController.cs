using System;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.News;
using CamprayPortal.Services.Helpers;
using CamprayPortal.Services.News;
using CamprayPortal.Services.Seo;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Models.AboutUs;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Controllers
{
    public class AboutUsController : BasePublicController
    {
          private readonly IDateTimeHelper _dateTimeHelper;
        private readonly INewsService _newsService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public AboutUsController(IDateTimeHelper dateTimeHelper,
            INewsService newsService, 
            IStoreContext storeContext,
            IWorkContext workContext)
        {
            _dateTimeHelper = dateTimeHelper;
            _newsService = newsService;
            _storeContext = storeContext;
            _workContext = workContext;
        }

        #region Utilities

        [NonAction]
        protected virtual void PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem");

            if (model == null)
                throw new ArgumentNullException("model");

            model.Id = newsItem.Id;
            model.MetaTitle = newsItem.MetaTitle;
            model.MetaDescription = newsItem.MetaDescription;
            model.MetaKeywords = newsItem.MetaKeywords;
            model.SeName = newsItem.GetSeName(newsItem.LanguageId, ensureTwoPublishedLanguages: false);
            model.Title = newsItem.Title;
            model.Short = newsItem.Short;
            model.Full = newsItem.Full;
            model.CreatedOn = _dateTimeHelper.ConvertToUserTime(newsItem.CreatedOnUtc, DateTimeKind.Utc);
        }

        #endregion

        public ActionResult HomePage()
        {
            return TopicModelView();
        }

        public ActionResult MessageChairman()
        {  
            return View();
        }



        public ActionResult ManagementTeam()
        {
            return View();
        }


        public ActionResult BoardMembers()
        {
            return View();
        }

        public ActionResult Investors()
        {
            return View(); 
        }

        public ActionResult WhyA4Data()
        {
            return View();
        }

        public ActionResult News(int currentpage = 1)
        {
            var newsItems = _newsService.GetAllByNewsType(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id,
                currentpage - 1,
                4, false, NewsType.News);
            var model = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x);
                    return newsModel;
                })
                .ToList();

            var pagination = new Pagination<NewsItemModel>(model, currentpage, newsItems.TotalPages);
            return View(pagination);
        }


        public ActionResult NewsDetail(int id = 0)
        {
            var newsItem = _newsService.GetNewsById(id);
            var newsModel = new NewsItemModel();
            PrepareNewsItemModel(newsModel, newsItem);
            return View(newsModel);
        }



        public ActionResult Event(int currentpage = 1) 
        {
            var newsItems = _newsService.GetAllByNewsType(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id,
                currentpage - 1,
                4, false, NewsType.Eevet);
            var model = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x);
                    return newsModel;
                })
                .ToList();

            var pagination = new Pagination<NewsItemModel>(model, currentpage, newsItems.TotalPages);
            return View(pagination);
        }


        public ActionResult EventDetail(int id = 0)
        {
            var newsItem = _newsService.GetNewsById(id);
            var newsModel = new NewsItemModel();
            PrepareNewsItemModel(newsModel, newsItem);
            return View(newsModel);
        }



        public ActionResult ContactUs()
        {
            return View();
        }

        public ActionResult Careers()
        {
            return View();
        }

    }
}