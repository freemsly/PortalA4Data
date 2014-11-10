using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Domain;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Core.Domain.Customers;
using CamprayPortal.Core.Domain.Localization;
using CamprayPortal.Core.Domain.Messages;
using CamprayPortal.Core.Domain.News;
using CamprayPortal.Services.Common;
using CamprayPortal.Services.Customers;
using CamprayPortal.Services.Directory;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.Logging;
using CamprayPortal.Services.Messages;
using CamprayPortal.Services.Security;
using CamprayPortal.Services.Seo;
using CamprayPortal.Services.Topics;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Framework.Security;
using CamprayPortal.Web.Framework.Themes;
using CamprayPortal.Web.Framework.UI.Captcha;
using CamprayPortal.Web.Infrastructure.Cache;
using CamprayPortal.Web.Models.Common;
using CamprayPortal.Web.Models.Topics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CamprayPortal.Web.Controllers
{
    public partial class CommonController : BasePublicController
    {
        #region Fields
 
        private readonly ITopicService _topicService;
        private readonly ILanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ISitemapGenerator _sitemapGenerator;
        private readonly IThemeContext _themeContext;
        private readonly IThemeProvider _themeProvider;
       
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWebHelper _webHelper;
        private readonly IPermissionService _permissionService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerActivityService _customerActivityService;

        private readonly CustomerSettings _customerSettings;
         
        private readonly StoreInformationSettings _storeInformationSettings;
        private readonly EmailAccountSettings _emailAccountSettings;
        private readonly CommonSettings _commonSettings;
       
        private readonly NewsSettings _newsSettings;
       
        private readonly LocalizationSettings _localizationSettings;
        private readonly CaptchaSettings _captchaSettings;

        #endregion

        #region Constructors

        public CommonController( 
            ITopicService topicService,
            ILanguageService languageService,
            ICurrencyService currencyService,
            ILocalizationService localizationService,
            IWorkContext workContext, 
            IStoreContext storeContext,
            IQueuedEmailService queuedEmailService, 
            IEmailAccountService emailAccountService,
            ISitemapGenerator sitemapGenerator,
            IThemeContext themeContext,
            IThemeProvider themeProvider,
           
            IGenericAttributeService genericAttributeService, 
            IWebHelper webHelper,
            IPermissionService permissionService,
            ICacheManager cacheManager,
            ICustomerActivityService customerActivityService,
            CustomerSettings customerSettings, 
         
            StoreInformationSettings storeInformationSettings,
            EmailAccountSettings emailAccountSettings,
            CommonSettings commonSettings, 
          
            NewsSettings newsSettings,
           
            LocalizationSettings localizationSettings, 
            CaptchaSettings captchaSettings)
        {
           
            this._topicService = topicService;
            this._languageService = languageService;
            this._currencyService = currencyService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._queuedEmailService = queuedEmailService;
            this._emailAccountService = emailAccountService;
            this._sitemapGenerator = sitemapGenerator;
            this._themeContext = themeContext;
            this._themeProvider = themeProvider;
         
            this._genericAttributeService = genericAttributeService;
            this._webHelper = webHelper;
            this._permissionService = permissionService;
            this._cacheManager = cacheManager;
            this._customerActivityService = customerActivityService;

            this._customerSettings = customerSettings;
           
            this._storeInformationSettings = storeInformationSettings;
            this._emailAccountSettings = emailAccountSettings;
            this._commonSettings = commonSettings;
           
            this._newsSettings = newsSettings;
          
            this._localizationSettings = localizationSettings;
            this._captchaSettings = captchaSettings;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected virtual int GetUnreadPrivateMessages()
        {
            var result = 0;
            var customer = _workContext.CurrentCustomer;
            

            return result;
        }

        #endregion

        #region Methods

        //page not found
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;

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

        //currency
        [ChildActionOnly]
        public ActionResult CurrencySelector()
        {
            var availableCurrencies = _cacheManager.Get(string.Format(ModelCacheEventConsumer.AVAILABLE_CURRENCIES_MODEL_KEY, _workContext.WorkingLanguage.Id, _storeContext.CurrentStore.Id), () =>
            {
                var result = _currencyService
                    .GetAllCurrencies(storeId: _storeContext.CurrentStore.Id)
                    .Select(x =>
                    {
                        //currency char
                        var currencySymbol = "";
                        if (!string.IsNullOrEmpty(x.DisplayLocale))
                            currencySymbol = new RegionInfo(x.DisplayLocale).CurrencySymbol;
                        else
                            currencySymbol = x.CurrencyCode;
                        //model
                        var currencyModel = new CurrencyModel()
                        {
                            Id = x.Id,
                            Name = x.GetLocalized(y => y.Name),
                            CurrencySymbol = currencySymbol
                        };
                        return currencyModel;
                    })
                    .ToList();
                return result;
            });

            var model = new CurrencySelectorModel()
            {
                CurrentCurrencyId = _workContext.WorkingCurrency.Id,
                AvailableCurrencies = availableCurrencies
            };

            if (model.AvailableCurrencies.Count == 1)
                Content("");

            return PartialView(model);
        }
        public ActionResult SetCurrency(int customerCurrency, string returnUrl = "")
        {
            var currency = _currencyService.GetCurrencyById(customerCurrency);
            if (currency != null)
                _workContext.WorkingCurrency = currency;

            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");
            
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }
         
        //footer
        [ChildActionOnly]
        public ActionResult JavaScriptDisabledWarning()
        {
            if (!_commonSettings.DisplayJavaScriptDisabledWarning)
                return Content("");

            return PartialView();
        }

        //header links
        [ChildActionOnly]
        public ActionResult HeaderLinks()
        {
            var customer = _workContext.CurrentCustomer;

            var unreadMessageCount = GetUnreadPrivateMessages();
            var unreadMessage = string.Empty;
            var alertMessage = string.Empty;
            if (unreadMessageCount > 0)
            {
                unreadMessage = string.Format(_localizationService.GetResource("PrivateMessages.TotalUnread"), unreadMessageCount);

                //notifications here
                if ( 
                    !customer.GetAttribute<bool>(SystemCustomerAttributeNames.NotifiedAboutNewPrivateMessages, _storeContext.CurrentStore.Id))
                {
                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.NotifiedAboutNewPrivateMessages, true, _storeContext.CurrentStore.Id);
                    alertMessage = string.Format(_localizationService.GetResource("PrivateMessages.YouHaveUnreadPM"), unreadMessageCount);
                }
            }

            var model = new HeaderLinksModel()
            {
                IsAuthenticated = customer.IsRegistered(),
                CustomerEmailUsername = customer.IsRegistered() ? (_customerSettings.UsernamesEnabled ? customer.Username : customer.Email) : "",
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                WishlistEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
               
                UnreadPrivateMessages = unreadMessage,
                AlertMessage = alertMessage,
            };
            

            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult AdminHeaderLinks()
        {
            var customer = _workContext.CurrentCustomer;

            var model = new AdminHeaderLinksModel()
            {
                ImpersonatedCustomerEmailUsername = customer.IsRegistered() ? (_customerSettings.UsernamesEnabled ? customer.Username : customer.Email) : "",
              
                DisplayAdminLink = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel),
            };

            return PartialView(model);
        }

        //footer
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var model = new FooterModel()
            {
                StoreName = _storeContext.CurrentStore.GetLocalized(x => x.Name),
                WishlistEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableWishlist),
                ShoppingCartEnabled = _permissionService.Authorize(StandardPermissionProvider.EnableShoppingCart),
                SitemapEnabled = _commonSettings.SitemapEnabled,
                WorkingLanguageId = _workContext.WorkingLanguage.Id,
                FacebookLink = _storeInformationSettings.FacebookLink,
                TwitterLink = _storeInformationSettings.TwitterLink,
                YoutubeLink = _storeInformationSettings.YoutubeLink,
                GooglePlusLink = _storeInformationSettings.GooglePlusLink,
               
                NewsEnabled = _newsSettings.Enabled
                 
            };

            return PartialView(model);
        }


        //contact us page
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult ContactUs()
        {
            var model = new ContactUsModel()
            {
                Email = _workContext.CurrentCustomer.Email,
                FullName = _workContext.CurrentCustomer.GetFullName(),
                DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage
            };
            return View(model);
        }
        [HttpPost, ActionName("ContactUs")]
        [CaptchaValidator]
        public ActionResult ContactUsSend(ContactUsModel model, bool captchaValid)
        {
            //validate CAPTCHA
            if (_captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage && !captchaValid)
            {
                ModelState.AddModelError("", _localizationService.GetResource("Common.WrongCaptcha"));
            }

            if (ModelState.IsValid)
            {
                string email = model.Email.Trim();
                string fullName = model.FullName;
                string subject = string.Format(_localizationService.GetResource("ContactUs.EmailSubject"), _storeContext.CurrentStore.GetLocalized(x => x.Name));

                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    throw new Exception("No email account could be loaded");

                string from = null;
                string fromName = null;
                string body = Core.Html.HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);
                //required for some SMTP servers
                if (_commonSettings.UseSystemEmailForContactUsForm)
                {
                    from = emailAccount.Email;
                    fromName = emailAccount.DisplayName;
                    body = string.Format("<strong>From</strong>: {0} - {1}<br /><br />{2}", 
                        Server.HtmlEncode(fullName), 
                        Server.HtmlEncode(email), body);
                }
                else
                {
                    from = email;
                    fromName = fullName;
                }
                _queuedEmailService.InsertQueuedEmail(new QueuedEmail()
                {
                    From = from,
                    FromName = fromName,
                    To = emailAccount.Email,
                    ToName = emailAccount.DisplayName,
                    ReplyTo = email,
                    ReplyToName = fullName,
                    Priority = 5,
                    Subject = subject,
                    Body = body,
                    CreatedOnUtc = DateTime.UtcNow,
                    EmailAccountId = emailAccount.Id
                });
                
                model.SuccessfullySent = true;
                model.Result = _localizationService.GetResource("ContactUs.YourEnquiryHasBeenSent");

                //activity log
                _customerActivityService.InsertActivity("PublicStore.ContactUs", _localizationService.GetResource("ActivityLog.PublicStore.ContactUs"));

                return View(model);
            }

            model.DisplayCaptcha = _captchaSettings.Enabled && _captchaSettings.ShowOnContactUsPage;
            return View(model);
        }

        //sitemap page
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Sitemap()
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

            var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
               .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.SITEMAP_PAGE_MODEL_KEY, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var cachedModel = _cacheManager.Get(cacheKey, () =>
            {
                var model = new SitemapModel()
                {
                  
                    NewsEnabled = _newsSettings.Enabled
                };
              
               
                if (_commonSettings.SitemapIncludeTopics)
                {
                    var topics = _topicService.GetAllTopics(_storeContext.CurrentStore.Id)
                        .ToList()
                        .FindAll(t => t.IncludeInSitemap);
                    model.Topics = topics.Select(topic => new TopicModel()
                    {
                        Id = topic.Id,
                        SystemName = topic.SystemName,
                        IncludeInSitemap = topic.IncludeInSitemap,
                        IsPasswordProtected = topic.IsPasswordProtected,
                        Title = topic.GetLocalized(x => x.Title),
                    })
                    .ToList();
                }
                return model;
            });

            return View(cachedModel);
        }

        //SEO sitemap page
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult SitemapXml()
        {
            if (!_commonSettings.SitemapEnabled)
                return RedirectToRoute("HomePage");

             var customerRolesIds = _workContext.CurrentCustomer.CustomerRoles
               .Where(cr => cr.Active).Select(cr => cr.Id).ToList();
            string cacheKey = string.Format(ModelCacheEventConsumer.SITEMAP_SEO_MODEL_KEY, _workContext.WorkingLanguage.Id, string.Join(",", customerRolesIds), _storeContext.CurrentStore.Id);
            var siteMap = _cacheManager.Get(cacheKey, () =>
            {
                return _sitemapGenerator.Generate(this.Url);
            });
            return Content(siteMap, "text/xml");
        }

        //store theme
        [ChildActionOnly]
        public ActionResult StoreThemeSelector()
        {
            if (!_storeInformationSettings.AllowCustomerToSelectTheme)
                return Content("");

            var model = new StoreThemeSelectorModel();
            var currentTheme = _themeProvider.GetThemeConfiguration(_themeContext.WorkingThemeName);
            model.CurrentStoreTheme = new StoreThemeModel()
            {
                Name = currentTheme.ThemeName,
                Title = currentTheme.ThemeTitle
            };
            model.AvailableStoreThemes = _themeProvider.GetThemeConfigurations()
                .Select(x =>
                {
                    return new StoreThemeModel()
                    {
                        Name = x.ThemeName,
                        Title = x.ThemeTitle
                    };
                })
                .ToList();
            return PartialView(model);
        }
        public ActionResult SetStoreTheme(string themeName, string returnUrl = "")
        {
            _themeContext.WorkingThemeName = themeName;
            
            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                returnUrl = Url.RouteUrl("HomePage");

            return Redirect(returnUrl);
        }

        //favicon
        [ChildActionOnly]
        public ActionResult Favicon()
        {
            //try loading a store specific favicon
            var faviconFileName = string.Format("favicon-{0}.ico", _storeContext.CurrentStore.Id);
            var localFaviconPath = System.IO.Path.Combine(Request.PhysicalApplicationPath, faviconFileName);
            if (!System.IO.File.Exists(localFaviconPath))
            {
                //try loading a generic favicon
                faviconFileName = "favicon.ico";
                localFaviconPath = System.IO.Path.Combine(Request.PhysicalApplicationPath, faviconFileName);
                if (!System.IO.File.Exists(localFaviconPath))
                {
                    return Content("");
                }
            }

            var model = new FaviconModel()
            {
                FaviconUrl = _webHelper.GetStoreLocation() + faviconFileName
            };
            return PartialView(model);
        }
        
        //EU Cookie law
        [ChildActionOnly]
        public ActionResult EuCookieLaw()
        {
            if (!_storeInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Content("");

            //ignore search engines because some pages could be indexed with the EU cookie as description
            if (_workContext.CurrentCustomer.IsSearchEngineAccount())
                return Content("");

            if (_workContext.CurrentCustomer.GetAttribute<bool>("EuCookieLaw.Accepted", _storeContext.CurrentStore.Id))
                //already accepted
                return Content("");

            return PartialView();
        }
        [HttpPost]
        public ActionResult EuCookieLawAccept()
        {
            if (!_storeInformationSettings.DisplayEuCookieLawWarning)
                //disabled
                return Json(new { stored = false });

            //save setting
            _genericAttributeService.SaveAttribute(_workContext.CurrentCustomer, "EuCookieLaw.Accepted", true, _storeContext.CurrentStore.Id);
            return Json(new { stored = true });
        }

        public ActionResult RobotsTextFile()
        {
            var disallowPaths = new List<string>()
                                    {
                                        "/bin/",
                                        "/content/files/",
                                        "/content/files/exportimport/",
                                        "/country/getstatesbycountryid",
                                        "/install",
                                        "/setproductreviewhelpfulness",
                                    };
            var localizableDisallowPaths = new List<string>()
                                               {
                                                   "/addproducttocart/catalog/",
                                                   "/addproducttocart/details/",
                                                   "/boards/forumwatch",
                                                   "/boards/postedit",
                                                   "/boards/postdelete",
                                                   "/boards/postcreate",
                                                   "/boards/topicedit",
                                                   "/boards/topicdelete",
                                                   "/boards/topiccreate",
                                                   "/boards/topicmove",
                                                   "/boards/topicwatch",
                                                   "/cart",
                                                   "/checkout",
                                                   "/checkout/billingaddress",
                                                   "/checkout/completed",
                                                   "/checkout/confirm",
                                                   "/checkout/shippingaddress",
                                                   "/checkout/shippingmethod",
                                                   "/checkout/paymentinfo",
                                                   "/checkout/paymentmethod",
                                                   "/clearcomparelist",
                                                   "/compareproducts",
                                                   "/customer/avatar",
                                                   "/customer/activation",
                                                   "/customer/addresses",
                                                   "/customer/backinstocksubscriptions",
                                                   "/customer/changepassword",
                                                   "/customer/checkusernameavailability",
                                                   "/customer/downloadableproducts",
                                                   "/customer/forumsubscriptions",
                                                   "/customer/info",
                                                   "/customer/orders",
                                                   "/customer/returnrequests",
                                                   "/customer/rewardpoints",
                                                   "/deletepm",
                                                   "/emailwishlist",
                                                   "/inboxupdate",
                                                   "/newsletter/subscriptionactivation",
                                                   "/onepagecheckout",
                                                   "/orderdetails",
                                                   "/passwordrecovery/confirm",
                                                   "/poll/vote",
                                                   "/privatemessages",
                                                   "/returnrequest",
                                                   "/sendpm",
                                                   "/sentupdate",
                                                   "/shoppingcart/productdetails_attributechange",
                                                   "/subscribenewsletter",
                                                   "/topic/authenticate",
                                                   "/viewpm",
                                                   "/uploadfileproductattribute",
                                                   "/uploadfilecheckoutattribute",
                                                   "/wishlist",
                                               };


            const string newLine = "\r\n"; //Environment.NewLine
            var sb = new StringBuilder();
            sb.Append("User-agent: *");
            sb.Append(newLine);
            //sitemaps
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //URLs are localizable. Append SEO code
                foreach (var language in _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id))
                {
                    sb.AppendFormat("Sitemap: {0}{1}/sitemap.xml", _storeContext.CurrentStore.Url, language.UniqueSeoCode);
                    sb.Append(newLine);
                }
            }
            else
            {
                //localizable paths (without SEO code)
                sb.AppendFormat("Sitemap: {0}sitemap.xml", _storeContext.CurrentStore.Url);
                sb.Append(newLine);
            }

            //usual paths
            foreach (var path in disallowPaths)
            {
                sb.AppendFormat("Disallow: {0}", path);
                sb.Append(newLine);
            }
            //localizable paths (without SEO code)
            foreach (var path in localizableDisallowPaths)
            {
                sb.AppendFormat("Disallow: {0}", path);
                sb.Append(newLine);
            }
            if (_localizationSettings.SeoFriendlyUrlsForLanguagesEnabled)
            {
                //URLs are localizable. Append SEO code
                foreach (var language in _languageService.GetAllLanguages(storeId: _storeContext.CurrentStore.Id))
                {
                    foreach (var path in localizableDisallowPaths)
                    {
                        sb.AppendFormat("Disallow: {0}{1}", language.UniqueSeoCode, path);
                        sb.Append(newLine);
                    }
                }
            }

            Response.ContentType = "text/plain";
            Response.Write(sb.ToString());
            return null;
        }

        public ActionResult GenericUrl()
        {
            //seems that no entity was found
            return InvokeHttp404();
        }

        //store is closed
        public ActionResult StoreClosed()
        {
            return View();
        }

        #endregion
    }
}
