using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Common
{
    public class SearchResultItem : BaseNopEntityModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public ContentType ContentType { get; set; }
    }

    public enum ContentType
    {
        News,
        Topic
    }
}