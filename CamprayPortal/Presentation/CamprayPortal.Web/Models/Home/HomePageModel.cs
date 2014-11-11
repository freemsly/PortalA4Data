using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Models.AboutUs;
using CamprayPortal.Web.Models.Topics;

namespace CamprayPortal.Web.Models.Home
{
    public class HomePageModel
    {
        public IList<NewsItemModel> NewsItemModels { get; set; }

        public IList<NewsItemModel> EventItemModels { get; set; }

        public TopicModel TopicModel { get; set; } 
    }
}