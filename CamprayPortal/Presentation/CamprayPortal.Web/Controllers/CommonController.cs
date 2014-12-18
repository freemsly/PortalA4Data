using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Routing;
using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Core.Domain.Localization;
using CamprayPortal.Services.Common;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.News;
using CamprayPortal.Services.Topics;
using CamprayPortal.Web.Framework;
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
        private readonly IContactUsService _contactUsService;
        private readonly ILocalizationService _localizationService;
        private readonly INewsService _newsService;
        private readonly ITopicService _topicService;

        public CommonController(ICacheManager cacheManager, ILanguageService languageService, IStoreContext storeContext,
            LocalizationSettings localizationSettings, IWorkContext workContext, IContactUsService contactUsService,
            ILocalizationService localizationService, INewsService newsService, ITopicService topicService)
        {
            _cacheManager = cacheManager;
            _languageService = languageService;
            _storeContext = storeContext;
            _localizationSettings = localizationSettings;
            _workContext = workContext;
            _contactUsService = contactUsService;
            _localizationService = localizationService;
            _newsService = newsService;
            _topicService = topicService;
        }


        public string ClearHtmlFormate(string html)
        {
            if (String.IsNullOrEmpty(html))
                return String.Empty;
            var regex1 =
                new Regex(@"<script[\s\S]+</script *>",
                    RegexOptions.IgnoreCase);
            var regex2 =
                new Regex(@" href *= *[\s\S]*script *:",
                    RegexOptions.IgnoreCase);
            var regex3 = new Regex(@" no[\s\S]*=",
                RegexOptions.IgnoreCase);
            var regex4 =
                new Regex(@"<iframe[\s\S]+</iframe *>",
                    RegexOptions.IgnoreCase);
            var regex5 =
                new Regex(@"<frameset[\s\S]+</frameset *>",
                    RegexOptions.IgnoreCase);
            var regex6 = new Regex(@"\<img[^\>]+\>",
                RegexOptions.IgnoreCase);
            var regex7 = new Regex(@"</p>",
                RegexOptions.IgnoreCase);
            var regex8 = new Regex(@"<p>",
                RegexOptions.IgnoreCase);
            var regex9 = new Regex(@"<[^>]*>",
                RegexOptions.IgnoreCase);
            html = regex1.Replace(html, String.Empty); //过滤<script></script>标记 
            html = regex2.Replace(html, String.Empty); //过滤href=javascript: (<A>) 属性 
            html = regex3.Replace(html, " _disibledevent="); //过滤其它控件的on...事件 
            html = regex4.Replace(html, String.Empty); //过滤iframe 
            html = regex5.Replace(html, String.Empty); //过滤frameset 
            html = regex6.Replace(html, String.Empty); //过滤frameset 
            html = regex7.Replace(html, String.Empty); //过滤frameset 
            html = regex8.Replace(html, String.Empty); //过滤frameset 
            html = regex9.Replace(html, String.Empty);
            html = html.Replace("</strong>", String.Empty);
            html = html.Replace("<strong>", String.Empty);
            return html;
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
                var contactus = new ContactUs()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Company = model.CompanyName,
                    Content = model.Comments,
                    Email = model.EmailAddress,
                    PhoneNumber = model.PhoneNumber
                };
                _contactUsService.InsertContactUs(contactus);
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
            var availableLanguages =
                _cacheManager.Get(
                    string.Format(ModelCacheEventConsumer.AVAILABLE_LANGUAGES_MODEL_KEY, _storeContext.CurrentStore.Id),
                    () =>
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



        public ActionResult SearchResult(string searchkey = null, int currentpage = 1)
        {
            const int pagesplit = 5;
            var lanaugeid = _workContext.WorkingLanguage.Id;
            var availablecontent =
                _cacheManager.Get(string.Format(ModelCacheEventConsumer.PRODUCT_MANUFACTURERS_PATTERN_KEY, lanaugeid),
                    () =>
                    {
                        var newsitems = _newsService.GetAllNews(lanaugeid, _storeContext.CurrentStore.Id,
                            0, int.MaxValue);
                        IList<SearchResultItem> searchResultItems = newsitems.Select(nitem => new SearchResultItem
                        {
                            Title = nitem.Title,
                            Content = ClearHtmlFormate(nitem.Full),
                            Url = Url.Action("NewsDetail", "AboutUs", new {id = nitem.Id}),
                            ContentType = ContentType.News
                        }).ToList();



                        var topics = _topicService.GetAllTopics(_storeContext.CurrentStore.Id);
                        foreach (var top in topics)
                        {
                            var seritem = new SearchResultItem
                            {
                                Title = top.GetLocalized(x => x.Title),
                                Content = ClearHtmlFormate(top.GetLocalized(x => x.Body)),
                                ContentType = ContentType.Topic
                            };

                            switch (top.SystemName)
                            {
                                case "Homepage":
                                    seritem.Url = "/";
                                    break;
                                case "Solutions-BenchmarkSpeedTime":
                                case "Solutions-BenchmarkMobile":
                                case "Solutions-BriefsOverview":
                                    seritem.Url = Url.Action("Benchmark", "Solutions");
                                    break;
                                default:
                                {
                                    if (top.SystemName.Contains("-"))
                                    {
                                        var ca = top.SystemName.Split('-');
                                        seritem.Url = Url.Action(ca[1], ca[0]);
                                    }
                                }
                                    break;
                            }
                            searchResultItems.Add(seritem);
                        }
                        return searchResultItems;
                    });

            var query = availablecontent.Where(d => true);
            var searchkeyword = _localizationService.GetResource("searchresults.key");
            if (String.IsNullOrEmpty(searchkey) || searchkey == searchkeyword)
            {
                ViewBag.Searchkey = searchkeyword;
            }
            else
            {
                var searchFullText = System.Configuration.ConfigurationManager.AppSettings["SearchFullText"];
                if (bool.Parse(searchFullText))
                {
                    query =
                        query.Where(
                            d =>
                                (d.Title != null && d.Title.Contains(searchkey)) ||
                                (d.Content != null && d.Content.Contains(searchkey)));
                }
                else
                {
                    query =
                        query.Where(
                            d => d.Content != null && d.Content.Contains(searchkey));
                }

                ViewBag.Searchkey = searchkey;
            }

            var total = (query.Count()/pagesplit) + (query.Count()%pagesplit != 0 ? 1 : 0);
            var items = query.Skip(pagesplit*(currentpage - 1)).Take(pagesplit).ToList();

            var pagination = new Pagination<SearchResultItem>(items, currentpage, total);
            return View(pagination);
        }


        public ActionResult PrivacyPolicy()
        {
            return TopicModelView();
        }


        public ActionResult TermsofService()
        {
            return TopicModelView();
        }
  
        #endregion

    }



}
