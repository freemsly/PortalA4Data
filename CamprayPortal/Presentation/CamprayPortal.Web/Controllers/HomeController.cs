using System;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.News;
using CamprayPortal.Services.Helpers;
using CamprayPortal.Services.News;
using CamprayPortal.Services.Seo;
using CamprayPortal.Web.Framework.Security;
using CamprayPortal.Web.Models.AboutUs;
using CamprayPortal.Web.Models.Home;

namespace CamprayPortal.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly INewsService _newsService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;

        public HomeController(IDateTimeHelper dateTimeHelper,
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

        #region Methods



        public ActionResult Index()
        {
            var homePageModel = new HomePageModel();
            var newsItems = _newsService.GetAllByNewsType(_workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id,
                0, 7, false, NewsType.News);
            var eventItems = _newsService.GetAllByNewsType(_workContext.WorkingLanguage.Id,
                _storeContext.CurrentStore.Id, 0, 7, false, NewsType.Eevet); 
            homePageModel.NewsItemModels = newsItems
                .Select(x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x);
                    return newsModel;
                })
                .ToList();
            homePageModel.EventItemModels = eventItems
               .Select(x =>
               {
                   var newsModel = new NewsItemModel();
                   PrepareNewsItemModel(newsModel, x);
                   return newsModel;
               })
               .ToList();
            return View(homePageModel);
        }


        #endregion
    }
}
