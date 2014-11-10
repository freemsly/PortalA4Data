using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Admin.Models.Common;
using CamprayPortal.Admin.Models.Settings;
using CamprayPortal.Admin.Models.Stores;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain;

using CamprayPortal.Core.Domain.Catalog;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Core.Domain.Customers;
using CamprayPortal.Core.Domain.Directory;

using CamprayPortal.Core.Domain.Localization;
using CamprayPortal.Core.Domain.Media;
using CamprayPortal.Core.Domain.News;

using CamprayPortal.Core.Domain.Security;
using CamprayPortal.Core.Domain.Seo;

 

using CamprayPortal.Services.Common;
using CamprayPortal.Services.Configuration;
using CamprayPortal.Services.Customers;
using CamprayPortal.Services.Directory;
using CamprayPortal.Services.Helpers;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.Logging;
using CamprayPortal.Services.Media;
 
using CamprayPortal.Services.Security;
using CamprayPortal.Services.Stores;
 
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Controllers;
using CamprayPortal.Web.Framework.Kendoui;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Framework.Mvc;
using CamprayPortal.Web.Framework.Themes;
using CamprayPortal.Web.Framework.UI.Captcha;

namespace CamprayPortal.Admin.Controllers
{
    public partial class SettingController : BaseAdminController
	{
		#region Fields

        private readonly ISettingService _settingService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IAddressService _addressService;
    
        private readonly ICurrencyService _currencyService;
        private readonly IPictureService _pictureService;
        private readonly ILocalizationService _localizationService;
        private readonly IDateTimeHelper _dateTimeHelper;
         
        private readonly IEncryptionService _encryptionService;
        private readonly IThemeProvider _themeProvider;
        private readonly ICustomerService _customerService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly IFulltextService _fulltextService;
        private readonly IMaintenanceService _maintenanceService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly IGenericAttributeService _genericAttributeService;

		#endregion

		#region Constructors

        public SettingController(ISettingService settingService,
            ICountryService countryService, IStateProvinceService stateProvinceService,
            IAddressService addressService, 
            ICurrencyService currencyService, IPictureService pictureService, 
            ILocalizationService localizationService, IDateTimeHelper dateTimeHelper,
             IEncryptionService encryptionService,
            IThemeProvider themeProvider, ICustomerService customerService, 
            ICustomerActivityService customerActivityService, IPermissionService permissionService,
            IWebHelper webHelper, IFulltextService fulltextService, 
            IMaintenanceService maintenanceService, IStoreService storeService,
            IWorkContext workContext, IGenericAttributeService genericAttributeService)
        {
            this._settingService = settingService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._addressService = addressService;
           
            this._currencyService = currencyService;
            this._pictureService = pictureService;
            this._localizationService = localizationService;
            this._dateTimeHelper = dateTimeHelper;
          
            this._encryptionService = encryptionService;
            this._themeProvider = themeProvider;
            this._customerService = customerService;
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._fulltextService = fulltextService;
            this._maintenanceService = maintenanceService;
            this._storeService = storeService;
            this._workContext = workContext;
            this._genericAttributeService = genericAttributeService;
        }

		#endregion 
        
        #region Methods
        
        [ChildActionOnly]
        public ActionResult StoreScopeConfiguration()
        {
            var allStores = _storeService.GetAllStores();
            if (allStores.Count < 2)
                return Content("");

            var model = new StoreScopeConfigurationModel();
            foreach (var s in allStores)
            {
                model.Stores.Add(new StoreModel()
                {
                    Id = s.Id,
                    Name = s.Name
                });
            }
            model.StoreId = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            return PartialView(model);
        }
        public ActionResult ChangeStoreScopeConfiguration(int storeid, string returnUrl = "")
        {
            var store = _storeService.GetStoreById(storeid);
            if (store != null || storeid == 0)
            {
                _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer,
                    SystemCustomerAttributeNames.AdminAreaStoreScopeConfiguration, storeid);
            }

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = "Admin" });

            return Redirect(returnUrl);
        }
  
        public ActionResult News()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var newsSettings = _settingService.LoadSetting<NewsSettings>(storeScope);
            var model = newsSettings.ToModel();
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Enabled_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.Enabled, storeScope);
                model.AllowNotRegisteredUsersToLeaveComments_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, storeScope);
                model.NotifyAboutNewNewsComments_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.NotifyAboutNewNewsComments, storeScope);
                model.ShowNewsOnMainPage_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.ShowNewsOnMainPage, storeScope);
                model.MainPageNewsCount_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.MainPageNewsCount, storeScope);
                model.NewsArchivePageSize_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.NewsArchivePageSize, storeScope);
                model.ShowHeaderRssUrl_OverrideForStore = _settingService.SettingExists(newsSettings, x => x.ShowHeaderRssUrl, storeScope);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult News(NewsSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var newsSettings = _settingService.LoadSetting<NewsSettings>(storeScope);
            newsSettings = model.ToEntity(newsSettings);

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.Enabled_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.Enabled, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.Enabled, storeScope);

            if (model.AllowNotRegisteredUsersToLeaveComments_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, storeScope,
                    false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.AllowNotRegisteredUsersToLeaveComments, storeScope);

            if (model.NotifyAboutNewNewsComments_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.NotifyAboutNewNewsComments, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.NotifyAboutNewNewsComments, storeScope);

            if (model.ShowNewsOnMainPage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.ShowNewsOnMainPage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.ShowNewsOnMainPage, storeScope);

            if (model.MainPageNewsCount_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.MainPageNewsCount, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.MainPageNewsCount, storeScope);

            if (model.NewsArchivePageSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.NewsArchivePageSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.NewsArchivePageSize, storeScope);

            if (model.ShowHeaderRssUrl_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(newsSettings, x => x.ShowHeaderRssUrl, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(newsSettings, x => x.ShowHeaderRssUrl, storeScope);

            //now clear settings cache
            _settingService.ClearCache();


            //activity log
            _customerActivityService.InsertActivity("EditSettings",
                _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("News");
        }
     

 

        public ActionResult Media()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var mediaSettings = _settingService.LoadSetting<MediaSettings>(storeScope);
            var model = mediaSettings.ToModel();
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.AvatarPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.AvatarPictureSize, storeScope);
                model.ProductThumbPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.ProductThumbPictureSize, storeScope);
                model.ProductDetailsPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.ProductDetailsPictureSize, storeScope);
                model.ProductThumbPictureSizeOnProductDetailsPage_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.ProductThumbPictureSizeOnProductDetailsPage, storeScope);
                model.AssociatedProductPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.AssociatedProductPictureSize, storeScope);
                model.CategoryThumbPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.CategoryThumbPictureSize, storeScope);
                model.ManufacturerThumbPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.ManufacturerThumbPictureSize, storeScope);
                model.CartThumbPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.CartThumbPictureSize, storeScope);
                model.MiniCartThumbPictureSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.MiniCartThumbPictureSize, storeScope);
                model.MaximumImageSize_OverrideForStore = _settingService.SettingExists(mediaSettings, x => x.MaximumImageSize, storeScope);
            }
            model.PicturesStoredIntoDatabase = _pictureService.StoreInDb;
            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult Media(MediaSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var mediaSettings = _settingService.LoadSetting<MediaSettings>(storeScope);
            mediaSettings = model.ToEntity(mediaSettings);

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.AvatarPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.AvatarPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.AvatarPictureSize, storeScope);
            
            if (model.ProductThumbPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.ProductThumbPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.ProductThumbPictureSize, storeScope);
            
            if (model.ProductDetailsPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.ProductDetailsPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.ProductDetailsPictureSize, storeScope);
            
            if (model.ProductThumbPictureSizeOnProductDetailsPage_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.ProductThumbPictureSizeOnProductDetailsPage, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.ProductThumbPictureSizeOnProductDetailsPage, storeScope);

            if (model.AssociatedProductPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.AssociatedProductPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.AssociatedProductPictureSize, storeScope);
            
            if (model.CategoryThumbPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.CategoryThumbPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.CategoryThumbPictureSize, storeScope);
            
            if (model.ManufacturerThumbPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.ManufacturerThumbPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.ManufacturerThumbPictureSize, storeScope);
            
            if (model.CartThumbPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.CartThumbPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.CartThumbPictureSize, storeScope);
            
            if (model.MiniCartThumbPictureSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.MiniCartThumbPictureSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.MiniCartThumbPictureSize, storeScope);

            if (model.MaximumImageSize_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(mediaSettings, x => x.MaximumImageSize, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mediaSettings, x => x.MaximumImageSize, storeScope);
                
            //now clear settings cache
            _settingService.ClearCache();
            
            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("Media");
        }
        [HttpPost, ActionName("Media")]
        [FormValueRequired("change-picture-storage")]
        public ActionResult ChangePictureStorage()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            _pictureService.StoreInDb = !_pictureService.StoreInDb;

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));
            return RedirectToAction("Media");
        }



        public ActionResult CustomerUser()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var customerSettings = _settingService.LoadSetting<CustomerSettings>(storeScope);
            var addressSettings = _settingService.LoadSetting<AddressSettings>(storeScope);
            var dateTimeSettings = _settingService.LoadSetting<DateTimeSettings>(storeScope);
            var externalAuthenticationSettings = _settingService.LoadSetting<ExternalAuthenticationSettings>(storeScope);

            //merge settings
            var model = new CustomerUserSettingsModel();
            model.CustomerSettings = customerSettings.ToModel();
            model.AddressSettings = addressSettings.ToModel();

            model.DateTimeSettings.AllowCustomersToSetTimeZone = dateTimeSettings.AllowCustomersToSetTimeZone;
            model.DateTimeSettings.DefaultStoreTimeZoneId = _dateTimeHelper.DefaultStoreTimeZone.Id;
            foreach (TimeZoneInfo timeZone in _dateTimeHelper.GetSystemTimeZones())
            {
                model.DateTimeSettings.AvailableTimeZones.Add(new SelectListItem()
                    {
                        Text = timeZone.DisplayName,
                        Value = timeZone.Id,
                        Selected = timeZone.Id.Equals(_dateTimeHelper.DefaultStoreTimeZone.Id, StringComparison.InvariantCultureIgnoreCase)
                    });
            }

            model.ExternalAuthenticationSettings.AutoRegisterEnabled = externalAuthenticationSettings.AutoRegisterEnabled;

            return View(model);
        }
        [HttpPost]
        public ActionResult CustomerUser(CustomerUserSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();


            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var customerSettings = _settingService.LoadSetting<CustomerSettings>(storeScope);
            var addressSettings = _settingService.LoadSetting<AddressSettings>(storeScope);
            var dateTimeSettings = _settingService.LoadSetting<DateTimeSettings>(storeScope);
            var externalAuthenticationSettings = _settingService.LoadSetting<ExternalAuthenticationSettings>(storeScope);

            customerSettings = model.CustomerSettings.ToEntity(customerSettings);
            _settingService.SaveSetting(customerSettings);

            addressSettings = model.AddressSettings.ToEntity(addressSettings);
            _settingService.SaveSetting(addressSettings);

            dateTimeSettings.DefaultStoreTimeZoneId = model.DateTimeSettings.DefaultStoreTimeZoneId;
            dateTimeSettings.AllowCustomersToSetTimeZone = model.DateTimeSettings.AllowCustomersToSetTimeZone;
            _settingService.SaveSetting(dateTimeSettings);

            externalAuthenticationSettings.AutoRegisterEnabled = model.ExternalAuthenticationSettings.AutoRegisterEnabled;
            _settingService.SaveSetting(externalAuthenticationSettings);

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            //selected tab
            SaveSelectedTabIndex();

            return RedirectToAction("CustomerUser");
        }






        public ActionResult GeneralCommon()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            var model = new GeneralCommonSettingsModel();
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            model.ActiveStoreScopeConfiguration = storeScope;
            //store information
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);
            model.StoreInformationSettings.StoreClosed = storeInformationSettings.StoreClosed;
            model.StoreInformationSettings.StoreClosedAllowForAdmins = storeInformationSettings.StoreClosedAllowForAdmins;
            //themes
            model.StoreInformationSettings.DefaultStoreTheme = storeInformationSettings.DefaultStoreTheme;
            model.StoreInformationSettings.AvailableStoreThemes = _themeProvider
                .GetThemeConfigurations()
                .Select(x =>
                {
                    return new GeneralCommonSettingsModel.StoreInformationSettingsModel.ThemeConfigurationModel()
                    {
                        ThemeTitle = x.ThemeTitle,
                        ThemeName = x.ThemeName,
                        PreviewImageUrl = x.PreviewImageUrl,
                        PreviewText = x.PreviewText,
                        SupportRtl = x.SupportRtl,
                        Selected = x.ThemeName.Equals(storeInformationSettings.DefaultStoreTheme, StringComparison.InvariantCultureIgnoreCase)
                    };
                })
                .ToList();
            model.StoreInformationSettings.AllowCustomerToSelectTheme = storeInformationSettings.AllowCustomerToSelectTheme;
            model.StoreInformationSettings.ResponsiveDesignSupported = storeInformationSettings.ResponsiveDesignSupported;
            //EU Cookie law
            model.StoreInformationSettings.DisplayEuCookieLawWarning = storeInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            model.StoreInformationSettings.FacebookLink = storeInformationSettings.FacebookLink;
            model.StoreInformationSettings.TwitterLink = storeInformationSettings.TwitterLink;
            model.StoreInformationSettings.YoutubeLink = storeInformationSettings.YoutubeLink;
            model.StoreInformationSettings.GooglePlusLink = storeInformationSettings.GooglePlusLink;
            //override settings
            if (storeScope > 0)
            {
                model.StoreInformationSettings.ResponsiveDesignSupported_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.ResponsiveDesignSupported, storeScope);
                model.StoreInformationSettings.StoreClosed_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.StoreClosed, storeScope);
                model.StoreInformationSettings.StoreClosedAllowForAdmins_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.StoreClosedAllowForAdmins, storeScope);
                model.StoreInformationSettings.DefaultStoreTheme_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.DefaultStoreTheme, storeScope);
                model.StoreInformationSettings.AllowCustomerToSelectTheme_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.AllowCustomerToSelectTheme, storeScope);
                model.StoreInformationSettings.DisplayEuCookieLawWarning_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.DisplayEuCookieLawWarning, storeScope);
                model.StoreInformationSettings.FacebookLink_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.FacebookLink, storeScope);
                model.StoreInformationSettings.TwitterLink_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.TwitterLink, storeScope);
                model.StoreInformationSettings.YoutubeLink_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.YoutubeLink, storeScope);
                model.StoreInformationSettings.GooglePlusLink_OverrideForStore = _settingService.SettingExists(storeInformationSettings, x => x.GooglePlusLink, storeScope);
            }

            //seo settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>(storeScope);
            model.SeoSettings.PageTitleSeparator = seoSettings.PageTitleSeparator;
            model.SeoSettings.PageTitleSeoAdjustment = (int)seoSettings.PageTitleSeoAdjustment;
            model.SeoSettings.PageTitleSeoAdjustmentValues = seoSettings.PageTitleSeoAdjustment.ToSelectList();
            model.SeoSettings.DefaultTitle = seoSettings.DefaultTitle;
            model.SeoSettings.DefaultMetaKeywords = seoSettings.DefaultMetaKeywords;
            model.SeoSettings.DefaultMetaDescription = seoSettings.DefaultMetaDescription;
            model.SeoSettings.GenerateProductMetaDescription = seoSettings.GenerateProductMetaDescription;
            model.SeoSettings.ConvertNonWesternChars = seoSettings.ConvertNonWesternChars;
            model.SeoSettings.CanonicalUrlsEnabled = seoSettings.CanonicalUrlsEnabled;
            model.SeoSettings.WwwRequirement = (int)seoSettings.WwwRequirement;
            model.SeoSettings.WwwRequirementValues = seoSettings.WwwRequirement.ToSelectList();
            model.SeoSettings.EnableJsBundling = seoSettings.EnableJsBundling;
            model.SeoSettings.EnableCssBundling = seoSettings.EnableCssBundling;
            model.SeoSettings.TwitterMetaTags = seoSettings.TwitterMetaTags;
            model.SeoSettings.OpenGraphMetaTags = seoSettings.OpenGraphMetaTags;
            //override settings
            if (storeScope > 0)
            {
                model.SeoSettings.PageTitleSeparator_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.PageTitleSeparator, storeScope);
                model.SeoSettings.PageTitleSeoAdjustment_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.PageTitleSeoAdjustment, storeScope);
                model.SeoSettings.DefaultTitle_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.DefaultTitle, storeScope);
                model.SeoSettings.DefaultMetaKeywords_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.DefaultMetaKeywords, storeScope);
                model.SeoSettings.DefaultMetaDescription_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.DefaultMetaDescription, storeScope);
                model.SeoSettings.GenerateProductMetaDescription_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.GenerateProductMetaDescription, storeScope);
                model.SeoSettings.ConvertNonWesternChars_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.ConvertNonWesternChars, storeScope);
                model.SeoSettings.CanonicalUrlsEnabled_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.CanonicalUrlsEnabled, storeScope);
                model.SeoSettings.WwwRequirement_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.WwwRequirement, storeScope);
                model.SeoSettings.EnableJsBundling_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.EnableJsBundling, storeScope);
                model.SeoSettings.EnableCssBundling_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.EnableCssBundling, storeScope);
                model.SeoSettings.TwitterMetaTags_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.TwitterMetaTags, storeScope);
                model.SeoSettings.OpenGraphMetaTags_OverrideForStore = _settingService.SettingExists(seoSettings, x => x.OpenGraphMetaTags, storeScope);
            }
            
            //security settings
            var securitySettings = _settingService.LoadSetting<SecuritySettings>(storeScope);
            var captchaSettings = _settingService.LoadSetting<CaptchaSettings>(storeScope);
            model.SecuritySettings.EncryptionKey = securitySettings.EncryptionKey;
            if (securitySettings.AdminAreaAllowedIpAddresses != null)
                for (int i = 0; i < securitySettings.AdminAreaAllowedIpAddresses.Count; i++)
                {
                    model.SecuritySettings.AdminAreaAllowedIpAddresses += securitySettings.AdminAreaAllowedIpAddresses[i];
                    if (i != securitySettings.AdminAreaAllowedIpAddresses.Count - 1)
                        model.SecuritySettings.AdminAreaAllowedIpAddresses += ",";
                }
            model.SecuritySettings.ForceSslForAllPages = securitySettings.ForceSslForAllPages;
            model.SecuritySettings.CaptchaEnabled = captchaSettings.Enabled;
            model.SecuritySettings.CaptchaShowOnLoginPage = captchaSettings.ShowOnLoginPage;
            model.SecuritySettings.CaptchaShowOnRegistrationPage = captchaSettings.ShowOnRegistrationPage;
            model.SecuritySettings.CaptchaShowOnContactUsPage = captchaSettings.ShowOnContactUsPage;
            model.SecuritySettings.CaptchaShowOnEmailWishlistToFriendPage = captchaSettings.ShowOnEmailWishlistToFriendPage;
            model.SecuritySettings.CaptchaShowOnEmailProductToFriendPage = captchaSettings.ShowOnEmailProductToFriendPage;
            model.SecuritySettings.CaptchaShowOnBlogCommentPage = captchaSettings.ShowOnBlogCommentPage;
            model.SecuritySettings.CaptchaShowOnNewsCommentPage = captchaSettings.ShowOnNewsCommentPage;
            model.SecuritySettings.CaptchaShowOnProductReviewPage = captchaSettings.ShowOnProductReviewPage;
            model.SecuritySettings.ReCaptchaPublicKey = captchaSettings.ReCaptchaPublicKey;
            model.SecuritySettings.ReCaptchaPrivateKey = captchaSettings.ReCaptchaPrivateKey;

            //PDF settings
            var pdfSettings = _settingService.LoadSetting<PdfSettings>(storeScope);
            model.PdfSettings.LetterPageSizeEnabled = pdfSettings.LetterPageSizeEnabled;
            model.PdfSettings.LogoPictureId = pdfSettings.LogoPictureId;
            model.PdfSettings.InvoiceFooterTextColumn1 = pdfSettings.InvoiceFooterTextColumn1;
            model.PdfSettings.InvoiceFooterTextColumn2 = pdfSettings.InvoiceFooterTextColumn2;
            //override settings
            if (storeScope > 0)
            {
                model.PdfSettings.LetterPageSizeEnabled_OverrideForStore = _settingService.SettingExists(pdfSettings, x => x.LetterPageSizeEnabled, storeScope);
                model.PdfSettings.LogoPictureId_OverrideForStore = _settingService.SettingExists(pdfSettings, x => x.LogoPictureId, storeScope);
                model.PdfSettings.InvoiceFooterTextColumn1_OverrideForStore = _settingService.SettingExists(pdfSettings, x => x.InvoiceFooterTextColumn1, storeScope);
                model.PdfSettings.InvoiceFooterTextColumn2_OverrideForStore = _settingService.SettingExists(pdfSettings, x => x.InvoiceFooterTextColumn2, storeScope);
            }

            //localization
            var localizationSettings = _settingService.LoadSetting<LocalizationSettings>(storeScope);
            model.LocalizationSettings.UseImagesForLanguageSelection = localizationSettings.UseImagesForLanguageSelection;
            model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled = localizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
            model.LocalizationSettings.AutomaticallyDetectLanguage = localizationSettings.AutomaticallyDetectLanguage;
            model.LocalizationSettings.LoadAllLocaleRecordsOnStartup = localizationSettings.LoadAllLocaleRecordsOnStartup;
            model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup = localizationSettings.LoadAllLocalizedPropertiesOnStartup;
            model.LocalizationSettings.LoadAllUrlRecordsOnStartup = localizationSettings.LoadAllUrlRecordsOnStartup;

            //full-text support
            var commonSettings = _settingService.LoadSetting<CommonSettings>(storeScope);
            model.FullTextSettings.Supported = _fulltextService.IsFullTextSupported();
            model.FullTextSettings.Enabled = commonSettings.UseFullTextSearch;
            model.FullTextSettings.SearchMode = (int)commonSettings.FullTextMode;
            model.FullTextSettings.SearchModeValues = commonSettings.FullTextMode.ToSelectList();


            return View(model);
        }
        [HttpPost]
        [FormValueRequired("save")]
        public ActionResult GeneralCommon(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();


            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            //store information settings
            var storeInformationSettings = _settingService.LoadSetting<StoreInformationSettings>(storeScope);
            storeInformationSettings.StoreClosed = model.StoreInformationSettings.StoreClosed;
            storeInformationSettings.StoreClosedAllowForAdmins = model.StoreInformationSettings.StoreClosedAllowForAdmins;
            storeInformationSettings.DefaultStoreTheme = model.StoreInformationSettings.DefaultStoreTheme;
            storeInformationSettings.AllowCustomerToSelectTheme = model.StoreInformationSettings.AllowCustomerToSelectTheme;
            storeInformationSettings.ResponsiveDesignSupported = model.StoreInformationSettings.ResponsiveDesignSupported;
            //EU Cookie law
            storeInformationSettings.DisplayEuCookieLawWarning = model.StoreInformationSettings.DisplayEuCookieLawWarning;
            //social pages
            storeInformationSettings.FacebookLink = model.StoreInformationSettings.FacebookLink;
            storeInformationSettings.TwitterLink = model.StoreInformationSettings.TwitterLink;
            storeInformationSettings.YoutubeLink = model.StoreInformationSettings.YoutubeLink;
            storeInformationSettings.GooglePlusLink = model.StoreInformationSettings.GooglePlusLink;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.StoreInformationSettings.ResponsiveDesignSupported_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.ResponsiveDesignSupported, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.ResponsiveDesignSupported, storeScope);

            if (model.StoreInformationSettings.StoreClosed_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.StoreClosed, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.StoreClosed, storeScope);

            if (model.StoreInformationSettings.StoreClosedAllowForAdmins_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.StoreClosedAllowForAdmins, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.StoreClosedAllowForAdmins, storeScope);

            if (model.StoreInformationSettings.DefaultStoreTheme_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.DefaultStoreTheme, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.DefaultStoreTheme, storeScope);

            if (model.StoreInformationSettings.AllowCustomerToSelectTheme_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.AllowCustomerToSelectTheme, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.AllowCustomerToSelectTheme, storeScope);
            
            if (model.StoreInformationSettings.DisplayEuCookieLawWarning_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.DisplayEuCookieLawWarning, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.DisplayEuCookieLawWarning, storeScope);

            if (model.StoreInformationSettings.FacebookLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.FacebookLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.FacebookLink, storeScope);

            if (model.StoreInformationSettings.TwitterLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.TwitterLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.TwitterLink, storeScope);

            if (model.StoreInformationSettings.YoutubeLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.YoutubeLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.YoutubeLink, storeScope);

            if (model.StoreInformationSettings.GooglePlusLink_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(storeInformationSettings, x => x.GooglePlusLink, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(storeInformationSettings, x => x.GooglePlusLink, storeScope);
            
            //now clear settings cache
            _settingService.ClearCache();



            //seo settings
            var seoSettings = _settingService.LoadSetting<SeoSettings>(storeScope);
            seoSettings.PageTitleSeparator = model.SeoSettings.PageTitleSeparator;
            seoSettings.PageTitleSeoAdjustment = (PageTitleSeoAdjustment)model.SeoSettings.PageTitleSeoAdjustment;
            seoSettings.DefaultTitle = model.SeoSettings.DefaultTitle;
            seoSettings.DefaultMetaKeywords = model.SeoSettings.DefaultMetaKeywords;
            seoSettings.DefaultMetaDescription = model.SeoSettings.DefaultMetaDescription;
            seoSettings.GenerateProductMetaDescription = model.SeoSettings.GenerateProductMetaDescription;
            seoSettings.ConvertNonWesternChars = model.SeoSettings.ConvertNonWesternChars;
            seoSettings.CanonicalUrlsEnabled = model.SeoSettings.CanonicalUrlsEnabled;
            seoSettings.WwwRequirement = (WwwRequirement)model.SeoSettings.WwwRequirement;
            seoSettings.EnableJsBundling = model.SeoSettings.EnableJsBundling;
            seoSettings.EnableCssBundling = model.SeoSettings.EnableCssBundling;
            seoSettings.TwitterMetaTags = model.SeoSettings.TwitterMetaTags;
            seoSettings.OpenGraphMetaTags = model.SeoSettings.OpenGraphMetaTags;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            if (model.SeoSettings.PageTitleSeparator_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.PageTitleSeparator, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.PageTitleSeparator, storeScope);
            
            if (model.SeoSettings.PageTitleSeoAdjustment_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.PageTitleSeoAdjustment, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.PageTitleSeoAdjustment, storeScope);
            
            if (model.SeoSettings.DefaultTitle_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.DefaultTitle, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.DefaultTitle, storeScope);
            
            if (model.SeoSettings.DefaultMetaKeywords_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.DefaultMetaKeywords, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.DefaultMetaKeywords, storeScope);
            
            if (model.SeoSettings.DefaultMetaDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.DefaultMetaDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.DefaultMetaDescription, storeScope);

            if (model.SeoSettings.GenerateProductMetaDescription_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.GenerateProductMetaDescription, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.GenerateProductMetaDescription, storeScope);
            
            if (model.SeoSettings.ConvertNonWesternChars_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.ConvertNonWesternChars, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.ConvertNonWesternChars, storeScope);
            
            if (model.SeoSettings.CanonicalUrlsEnabled_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.CanonicalUrlsEnabled, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.CanonicalUrlsEnabled, storeScope);

            if (model.SeoSettings.WwwRequirement_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.WwwRequirement, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.WwwRequirement, storeScope);
            
            if (model.SeoSettings.EnableJsBundling_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.EnableJsBundling, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.EnableJsBundling, storeScope);

            if (model.SeoSettings.EnableCssBundling_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.EnableCssBundling, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.EnableCssBundling, storeScope);

            if (model.SeoSettings.TwitterMetaTags_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.TwitterMetaTags, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.TwitterMetaTags, storeScope);

            if (model.SeoSettings.OpenGraphMetaTags_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(seoSettings, x => x.OpenGraphMetaTags, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(seoSettings, x => x.OpenGraphMetaTags, storeScope);

            //now clear settings cache
            _settingService.ClearCache();



            //security settings
            var securitySettings = _settingService.LoadSetting<SecuritySettings>(storeScope);
            var captchaSettings = _settingService.LoadSetting<CaptchaSettings>(storeScope);
            if (securitySettings.AdminAreaAllowedIpAddresses == null)
                securitySettings.AdminAreaAllowedIpAddresses = new List<string>();
            securitySettings.AdminAreaAllowedIpAddresses.Clear();
            if (!String.IsNullOrEmpty(model.SecuritySettings.AdminAreaAllowedIpAddresses))
                foreach (string s in model.SecuritySettings.AdminAreaAllowedIpAddresses.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    if (!String.IsNullOrWhiteSpace(s))
                        securitySettings.AdminAreaAllowedIpAddresses.Add(s.Trim());
            securitySettings.ForceSslForAllPages = model.SecuritySettings.ForceSslForAllPages;
            _settingService.SaveSetting(securitySettings);
            captchaSettings.Enabled = model.SecuritySettings.CaptchaEnabled;
            captchaSettings.ShowOnLoginPage = model.SecuritySettings.CaptchaShowOnLoginPage;
            captchaSettings.ShowOnRegistrationPage = model.SecuritySettings.CaptchaShowOnRegistrationPage;
            captchaSettings.ShowOnContactUsPage = model.SecuritySettings.CaptchaShowOnContactUsPage;
            captchaSettings.ShowOnEmailWishlistToFriendPage = model.SecuritySettings.CaptchaShowOnEmailWishlistToFriendPage;
            captchaSettings.ShowOnEmailProductToFriendPage = model.SecuritySettings.CaptchaShowOnEmailProductToFriendPage;
            captchaSettings.ShowOnBlogCommentPage = model.SecuritySettings.CaptchaShowOnBlogCommentPage;
            captchaSettings.ShowOnNewsCommentPage = model.SecuritySettings.CaptchaShowOnNewsCommentPage;
            captchaSettings.ShowOnProductReviewPage = model.SecuritySettings.CaptchaShowOnProductReviewPage;
            captchaSettings.ReCaptchaPublicKey = model.SecuritySettings.ReCaptchaPublicKey;
            captchaSettings.ReCaptchaPrivateKey = model.SecuritySettings.ReCaptchaPrivateKey;
            _settingService.SaveSetting(captchaSettings);
            if (captchaSettings.Enabled &&
                (String.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPublicKey) || String.IsNullOrWhiteSpace(captchaSettings.ReCaptchaPrivateKey)))
            {
                //captcha is enabled but the keys are not entered
                ErrorNotification("Captcha is enabled but the appropriate keys are not entered");
            }

            //PDF settings
            var pdfSettings = _settingService.LoadSetting<PdfSettings>(storeScope);
            pdfSettings.LetterPageSizeEnabled = model.PdfSettings.LetterPageSizeEnabled;
            pdfSettings.LogoPictureId = model.PdfSettings.LogoPictureId;
            pdfSettings.InvoiceFooterTextColumn1 = model.PdfSettings.InvoiceFooterTextColumn1;
            pdfSettings.InvoiceFooterTextColumn2 = model.PdfSettings.InvoiceFooterTextColumn2;
            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            
            if (model.PdfSettings.LetterPageSizeEnabled_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pdfSettings, x => x.LetterPageSizeEnabled, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pdfSettings, x => x.LetterPageSizeEnabled, storeScope);

            if (model.PdfSettings.LogoPictureId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pdfSettings, x => x.LogoPictureId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pdfSettings, x => x.LogoPictureId, storeScope);

            if (model.PdfSettings.InvoiceFooterTextColumn1_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pdfSettings, x => x.InvoiceFooterTextColumn1, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pdfSettings, x => x.InvoiceFooterTextColumn1, storeScope);

            if (model.PdfSettings.InvoiceFooterTextColumn2_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(pdfSettings, x => x.InvoiceFooterTextColumn2, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(pdfSettings, x => x.InvoiceFooterTextColumn2, storeScope);

            //now clear settings cache
            _settingService.ClearCache();




            //localization settings
            var localizationSettings = _settingService.LoadSetting<LocalizationSettings>(storeScope);
            localizationSettings.UseImagesForLanguageSelection = model.LocalizationSettings.UseImagesForLanguageSelection;
            if (localizationSettings.SeoFriendlyUrlsForLanguagesEnabled != model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                localizationSettings.SeoFriendlyUrlsForLanguagesEnabled = model.LocalizationSettings.SeoFriendlyUrlsForLanguagesEnabled;
                //clear cached values of routes
                System.Web.Routing.RouteTable.Routes.ClearSeoFriendlyUrlsCachedValueForRoutes();
            }
            localizationSettings.AutomaticallyDetectLanguage = model.LocalizationSettings.AutomaticallyDetectLanguage;
            localizationSettings.LoadAllLocaleRecordsOnStartup = model.LocalizationSettings.LoadAllLocaleRecordsOnStartup;
            localizationSettings.LoadAllLocalizedPropertiesOnStartup = model.LocalizationSettings.LoadAllLocalizedPropertiesOnStartup;
            localizationSettings.LoadAllUrlRecordsOnStartup = model.LocalizationSettings.LoadAllUrlRecordsOnStartup;
            _settingService.SaveSetting(localizationSettings);

            //full-text
            var commonSettings = _settingService.LoadSetting<CommonSettings>(storeScope);
            commonSettings.FullTextMode = (FulltextSearchMode)model.FullTextSettings.SearchMode;
            _settingService.SaveSetting(commonSettings);

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            SuccessNotification(_localizationService.GetResource("Admin.Configuration.Updated"));

            //selected tab
            SaveSelectedTabIndex();

            return RedirectToAction("GeneralCommon");
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("changeencryptionkey")]
        public ActionResult ChangeEncryptionKey(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            //set page timeout to 5 minutes
            this.Server.ScriptTimeout = 300;

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var securitySettings = _settingService.LoadSetting<SecuritySettings>(storeScope);

            try
            {
                if (model.SecuritySettings.EncryptionKey == null)
                    model.SecuritySettings.EncryptionKey = "";

                model.SecuritySettings.EncryptionKey = model.SecuritySettings.EncryptionKey.Trim();

                var newEncryptionPrivateKey = model.SecuritySettings.EncryptionKey;
                if (String.IsNullOrEmpty(newEncryptionPrivateKey) || newEncryptionPrivateKey.Length != 16)
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TooShort"));

                string oldEncryptionPrivateKey = securitySettings.EncryptionKey;
                if (oldEncryptionPrivateKey == newEncryptionPrivateKey)
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.TheSame"));
                 
                //update user information
                //optimization - load only users with PasswordFormat.Encrypted
                var customers = _customerService.GetAllCustomersByPasswordFormat(PasswordFormat.Encrypted);
                foreach (var customer in customers)
                {
                    string decryptedPassword = _encryptionService.DecryptText(customer.Password, oldEncryptionPrivateKey);
                    string encryptedPassword = _encryptionService.EncryptText(decryptedPassword, newEncryptionPrivateKey);

                    customer.Password = encryptedPassword;
                    _customerService.UpdateCustomer(customer);
                }

                securitySettings.EncryptionKey = newEncryptionPrivateKey;
                _settingService.SaveSetting(securitySettings);
                SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.EncryptionKey.Changed"));
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            //selected tab
            SaveSelectedTabIndex();

            return RedirectToAction("GeneralCommon");
        }
        [HttpPost, ActionName("GeneralCommon")]
        [FormValueRequired("togglefulltext")]
        public ActionResult ToggleFullText(GeneralCommonSettingsModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var commonSettings = _settingService.LoadSetting<CommonSettings>(storeScope);
            try
            {
                if (! _fulltextService.IsFullTextSupported())
                    throw new NopException(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.NotSupported"));

                if (commonSettings.UseFullTextSearch)
                {
                    _fulltextService.DisableFullText();

                    commonSettings.UseFullTextSearch = false;
                    _settingService.SaveSetting(commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Disabled"));
                }
                else
                {
                    _fulltextService.EnableFullText();

                    commonSettings.UseFullTextSearch = true;
                    _settingService.SaveSetting(commonSettings);

                    SuccessNotification(_localizationService.GetResource("Admin.Configuration.Settings.GeneralCommon.FullTextSettings.Enabled"));
                }
            }
            catch (Exception exc)
            {
                ErrorNotification(exc);
            }

            //selected tab
            SaveSelectedTabIndex();

            return RedirectToAction("GeneralCommon");
        }




        //all settings
        public ActionResult AllSettings()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();
            
            return View();
        }
        [HttpPost]
        public ActionResult AllSettings(DataSourceRequest command,
            CamprayPortal.Web.Framework.Kendoui.Filter filter = null, IEnumerable<Sort> sort = null)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var settings = _settingService
                .GetAllSettings()
                .Select(x =>
                            {
                                string storeName = "";
                                if (x.StoreId == 0)
                                {
                                    storeName = _localizationService.GetResource("Admin.Configuration.Settings.AllSettings.Fields.StoreName.AllStores");
                                }
                                else
                                {
                                    var store = _storeService.GetStoreById(x.StoreId);
                                    storeName = store != null ? store.Name : "Unknown";
                                }
                                var settingModel = new SettingModel()
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Value = x.Value,
                                    Store = storeName,
                                    StoreId = x.StoreId
                                };
                                return settingModel;
                            })
                .AsQueryable()
                .Filter(filter)
                .Sort(sort);

            var gridModel = new DataSourceResult
            {
                Data = settings.PagedForCommand(command).ToList(),
                Total = settings.Count()
            };

            return Json(gridModel);
        }
        [HttpPost]
        public ActionResult SettingUpdate(SettingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }

            var setting = _settingService.GetSettingById(model.Id);
            if (setting == null)
                return Content("No setting could be loaded with the specified ID");

            var storeId = model.StoreId;

            if (!setting.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase) ||
                setting.StoreId != storeId)
            {
                //setting name or store has been changed
                _settingService.DeleteSetting(setting);
            }

            _settingService.SetSetting(model.Name, model.Value, storeId);

            //activity log
            _customerActivityService.InsertActivity("EditSettings", _localizationService.GetResource("ActivityLog.EditSettings"));

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult SettingAdd([Bind(Exclude = "Id")] SettingModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            if (!ModelState.IsValid)
            {
                return Json(new DataSourceResult() { Errors = ModelState.SerializeErrors() });
            }
            var storeId = model.StoreId;
            _settingService.SetSetting(model.Name, model.Value, storeId);

            //activity log
            _customerActivityService.InsertActivity("AddNewSetting", _localizationService.GetResource("ActivityLog.AddNewSetting"), model.Name);

            return new NullJsonResult();
        }
        [HttpPost]
        public ActionResult SettingDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageSettings))
                return AccessDeniedView();

            var setting = _settingService.GetSettingById(id);
            if (setting == null)
                throw new ArgumentException("No setting found with the specified id");
            _settingService.DeleteSetting(setting);

            //activity log
            _customerActivityService.InsertActivity("DeleteSetting", _localizationService.GetResource("ActivityLog.DeleteSetting"), setting.Name);

            return new NullJsonResult();
        }

        #endregion
    }
}
