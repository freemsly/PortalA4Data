using System;
using System.Linq;
using System.Web.Routing;
using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Domain.Localization;
using CamprayPortal.Services.Localization;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Infrastructure.Cache;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {
        private readonly LocalizationSettings _localizationSettings;
        private readonly IStoreContext _storeContext;
        private readonly ILanguageService _languageService;
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;

        public CommonController(ICacheManager cacheManager, ILanguageService languageService, IStoreContext storeContext,
            LocalizationSettings localizationSettings, IWorkContext workContext)
        {
            _cacheManager = cacheManager;
            _languageService = languageService;
            _storeContext = storeContext;
            _localizationSettings = localizationSettings;
            _workContext = workContext;
        }

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



        //language
        [ChildActionOnly]
        public ActionResult LanguageSelector()
        {
            var availableLanguages = _cacheManager.Get(string.Format(ModelCacheEventConsumer.AVAILABLE_LANGUAGES_MODEL_KEY, _storeContext.CurrentStore.Id), () =>
            {
                var result = _languageService
                    .GetAllLanguages(storeId: _storeContext.CurrentStore.Id)
                    .Select(x => new LanguageModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        FlagImageFileName = x.FlagImageFileName,
                    })
                    .ToList();
                return result;
            });

            var model = new LanguageSelectorModel()
            {
                CurrentLanguageId = _workContext.WorkingLanguage.Id,
                AvailableLanguages = availableLanguages,
                UseImages = _localizationSettings.UseImagesForLanguageSelection
            };

            if (model.AvailableLanguages.Count == 1)
                Content("");

            return PartialView(model);
        }
        public ActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageService.GetLanguageById(langid);
            if (language != null && language.Published)
            {
                _workContext.WorkingLanguage = language;
            }

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //language part in URL
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                string applicationPath = HttpContext.Request.ApplicationPath;
                if (returnUrl.IsLocalizedUrl(applicationPath, true))
                {
                    //already localized URL
                    returnUrl = returnUrl.RemoveLanguageSeoCodeFromRawUrl(applicationPath);
                }
                returnUrl = returnUrl.AddLanguageSeoCodeToRawUrl(applicationPath, _workContext.WorkingLanguage);
            }
            return Redirect(returnUrl);
        }

        #endregion

    }
}
