﻿using System;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Infrastructure;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.Seo;
using CamprayPortal.Services.Topics;
using CamprayPortal.Web.Framework.UI.Paging;
using CamprayPortal.Web.Infrastructure.Cache;
using CamprayPortal.Web.Models.Boards;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Extensions
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// BBCode editor
        /// </summary>
        /// <typeparam name="TModel">Model</typeparam>
        /// <param name="html">HTML Helper</param>
        /// <param name="name">Name</param>
        /// <returns>Editor</returns>
        public static MvcHtmlString BBCodeEditor<TModel>(this HtmlHelper<TModel> html, string name)
        {
            var sb = new StringBuilder();

            var storeLocation = EngineContext.Current.Resolve<IWebHelper>().GetStoreLocation();
            string bbEditorWebRoot = String.Format("{0}Content/", storeLocation);

            sb.AppendFormat("<script src=\"{0}Content/BBEditor/ed.js\" type=\"text/javascript\"></script>", storeLocation);
            sb.Append(Environment.NewLine);
            sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    var webRoot = '{0}';", bbEditorWebRoot);
            sb.Append(Environment.NewLine);
            sb.AppendFormat("    edToolbar('{0}');", name);
            sb.Append(Environment.NewLine);
            sb.Append("</script>");
            sb.Append(Environment.NewLine);

            return MvcHtmlString.Create(sb.ToString());
        }

        //we have two pagers:
        //The first one can have custom routes
        //The second one just adds query string parameter
        public static MvcHtmlString Pager<TModel>(this HtmlHelper<TModel> html, PagerModel model)
        {
            if (model.TotalRecords == 0)
                return null;

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

            var links = new StringBuilder();
            if (model.ShowTotalSummary && (model.TotalPages > 0))
            {
                links.Append("<li class=\"total-summary\">");
                links.Append(string.Format(model.CurrentPageText, model.PageIndex + 1, model.TotalPages, model.TotalRecords));
                links.Append("</li>");
            }
            if (model.ShowPagerItems && (model.TotalPages > 1))
            {
                if (model.ShowFirst)
                {
                    //first page
                    if ((model.PageIndex >= 3) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.page = 1;

                        links.Append("<li class=\"first-page\">");
                        if (model.UseRouteLinks)
                        {
                            links.Append(html.RouteLink(model.FirstButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.FirstPageTitle") }));
                        }
                        else
                        {
                            links.Append(html.ActionLink(model.FirstButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.FirstPageTitle") }));
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowPrevious)
                {
                    //previous page
                    if (model.PageIndex > 0)
                    {
                        model.RouteValues.page = (model.PageIndex);

                        links.Append("<li class=\"previous-page\">");
                        if (model.UseRouteLinks)
                        {
                            links.Append(html.RouteLink(model.PreviousButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.PreviousPageTitle") }));
                        }
                        else
                        {
                            links.Append(html.ActionLink(model.PreviousButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.PreviousPageTitle") }));
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowIndividualPages)
                {
                    //individual pages
                    int firstIndividualPageIndex = model.GetFirstIndividualPageIndex();
                    int lastIndividualPageIndex = model.GetLastIndividualPageIndex();
                    for (int i = firstIndividualPageIndex; i <= lastIndividualPageIndex; i++)
                    {
                        if (model.PageIndex == i)
                        {
                            links.AppendFormat("<li class=\"current-page\"><span>{0}</span></li>", (i + 1));
                        }
                        else
                        {
                            model.RouteValues.page = (i + 1);

                            links.Append("<li class=\"individual-page\">");
                            if (model.UseRouteLinks)
                            {
                                links.Append(html.RouteLink((i + 1).ToString(), model.RouteActionName, (object)model.RouteValues, new { title = String.Format(localizationService.GetResource("Pager.PageLinkTitle"), (i + 1)) }));
                            }
                            else
                            {
                                links.Append(html.ActionLink((i + 1).ToString(), model.RouteActionName, (object)model.RouteValues, new { title = String.Format(localizationService.GetResource("Pager.PageLinkTitle"), (i + 1)) }));
                            }
                            links.Append("</li>");
                        }
                    }
                }
                if (model.ShowNext)
                {
                    //next page
                    if ((model.PageIndex + 1) < model.TotalPages)
                    {
                        model.RouteValues.page = (model.PageIndex + 2);

                        links.Append("<li class=\"next-page\">");
                        if (model.UseRouteLinks)
                        {
                            links.Append(html.RouteLink(model.NextButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.NextPageTitle") }));
                        }
                        else
                        {
                            links.Append(html.ActionLink(model.NextButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.NextPageTitle") }));
                        }
                        links.Append("</li>");
                    }
                }
                if (model.ShowLast)
                {
                    //last page
                    if (((model.PageIndex + 3) < model.TotalPages) && (model.TotalPages > model.IndividualPagesDisplayedCount))
                    {
                        model.RouteValues.page = model.TotalPages;

                        links.Append("<li class=\"last-page\">");
                        if (model.UseRouteLinks)
                        {
                            links.Append(html.RouteLink(model.LastButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.LastPageTitle") }));
                        }
                        else
                        {
                            links.Append(html.ActionLink(model.LastButtonText, model.RouteActionName, (object)model.RouteValues, new { title = localizationService.GetResource("Pager.LastPageTitle") }));
                        }
                        links.Append("</li>");
                    }
                }
            }
            var result = links.ToString();
            if (!String.IsNullOrEmpty(result))
            {
                result = "<ul>" + result + "</ul>";
            }
            return MvcHtmlString.Create(result);
        }
     
        public static Pager Pager(this HtmlHelper helper, IPageableModel pagination)
        {
            return new Pager(pagination, helper.ViewContext);
        }
        public static Pager Pager(this HtmlHelper helper, string viewDataKey)
        {
            var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IPageableModel;

            if (dataSource == null)
            {
                throw new InvalidOperationException(string.Format("Item in ViewData with key '{0}' is not an IPagination.",
                                                                  viewDataKey));
            }

            return helper.Pager(dataSource);
        }

        /// <summary>
        /// Get topic system name
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="html">HTML helper</param>
        /// <param name="systemName">System name</param>
        /// <returns>Topic SEO Name</returns>
        public static string GetTopicSeName<T>(this HtmlHelper<T> html, string systemName)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var storeContext = EngineContext.Current.Resolve<IStoreContext>();

            //static cache manager
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
            var cacheKey = string.Format(ModelCacheEventConsumer.TOPIC_SENAME_BY_SYSTEMNAME, systemName, workContext.WorkingLanguage.Id, storeContext.CurrentStore.Id);
            var cachedSeName = cacheManager.Get(cacheKey, () =>
            {
                var topicService = EngineContext.Current.Resolve<ITopicService>();
                var topic = topicService.GetTopicBySystemName(systemName, storeContext.CurrentStore.Id);
                if (topic == null)
                    return "";

                return topic.GetSeName();
            });
            return cachedSeName;
        }

    }
}
