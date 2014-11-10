using System;
using System.Net;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;
using CamprayPortal.Admin.Infrastructure.Cache;
using CamprayPortal.Admin.Models.Home;
using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Services.Configuration;

namespace CamprayPortal.Admin.Controllers
{
    public partial class HomeController : BaseAdminController
    {
        #region Fields
        private readonly IStoreContext _storeContext;
        private readonly CommonSettings _commonSettings;
        private readonly ISettingService _settingService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        public HomeController(IStoreContext storeContext, 
            CommonSettings commonSettings, 
            ISettingService settingService,
            IWorkContext workContext,
            ICacheManager cacheManager)
        {
            this._storeContext = storeContext;
            this._commonSettings = commonSettings;
            this._settingService = settingService;
            this._workContext = workContext;
            this._cacheManager= cacheManager;
        }

        #endregion

        #region Methods

        public ActionResult Index()
        {
            var model = new DashboardModel();
           
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult CamprayPortalNews()
        {
            try
            {
                string feedUrl = string.Format("http://www.CamprayPortal.com/NewsRSS.aspx?Version={0}&Localhost={1}&HideAdvertisements={2}&StoreURL={3}",
                    NopVersion.CurrentVersion, 
                    Request.Url.IsLoopback, 
                    _commonSettings.HideAdvertisementsOnAdminArea,
                    _storeContext.CurrentStore.Url)
                    .ToLowerInvariant();

                var rssData = _cacheManager.Get(ModelCacheEventConsumer.OFFICIAL_NEWS_MODEL_KEY, () =>
                {
                    //specify timeout (5 secs)
                    var request = WebRequest.Create(feedUrl);
                    request.Timeout = 5000;
                    using (var response = request.GetResponse())
                    using (var reader = XmlReader.Create(response.GetResponseStream()))
                    {
                        return SyndicationFeed.Load(reader);
                    }
                });
 
                return PartialView(rssData);
            }
            catch (Exception)
            {
                return Content("");
            }
        }

        [HttpPost]
        public ActionResult CamprayPortalNewsHideAdv()
        {
            _commonSettings.HideAdvertisementsOnAdminArea = !_commonSettings.HideAdvertisementsOnAdminArea;
            _settingService.SaveSetting(_commonSettings);
            return Content("Setting changed");
        }

        #endregion
    }
}
