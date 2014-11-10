using System;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Core;
 
using CamprayPortal.Core.Domain.Common;
 
using CamprayPortal.Services.Topics;

namespace CamprayPortal.Services.Seo
{
    /// <summary>
    /// Represents a sitemap generator
    /// </summary>
    public partial class SitemapGenerator : BaseSitemapGenerator, ISitemapGenerator
    {
        private readonly IStoreContext _storeContext;
       
        private readonly ITopicService _topicService;
        private readonly CommonSettings _commonSettings;

        public SitemapGenerator(IStoreContext storeContext,
            ITopicService topicService, 
            CommonSettings commonSettings)
        {
            this._storeContext = storeContext;
         
            this._topicService = topicService;
            this._commonSettings = commonSettings;
        }

        /// <summary>
        /// Method that is overridden, that handles creation of child urls.
        /// Use the method WriteUrlLocation() within this method.
        /// </summary>
        /// <param name="urlHelper">URL helper</param>
        protected override void GenerateUrlNodes(UrlHelper urlHelper)
        {
            if (_commonSettings.SitemapIncludeCategories)
            {
                WriteCategories(urlHelper, 0);
            }

            if (_commonSettings.SitemapIncludeManufacturers)
            {
                WriteManufacturers(urlHelper);
            }

            if (_commonSettings.SitemapIncludeProducts)
            {
                WriteProducts(urlHelper);
            }

            if (_commonSettings.SitemapIncludeTopics)
            {
                WriteTopics(urlHelper);
            }
        }

        private void WriteCategories(UrlHelper urlHelper, int parentCategoryId)
        {
           
        }

        private void WriteManufacturers(UrlHelper urlHelper)
        {
          
        }

        private void WriteProducts(UrlHelper urlHelper)
        {
            
        }

        private void WriteTopics(UrlHelper urlHelper)
        {
            var topics = _topicService.GetAllTopics(_storeContext.CurrentStore.Id).ToList().FindAll(t => t.IncludeInSitemap);
            foreach (var topic in topics)
            {
                var url = urlHelper.RouteUrl("Topic", new { SeName = topic.GetSeName() }, "http");
                var updateFrequency = UpdateFrequency.Weekly;
                var updateTime = DateTime.UtcNow;
                WriteUrlLocation(url, updateFrequency, updateTime);
            }
        }
    }
}
