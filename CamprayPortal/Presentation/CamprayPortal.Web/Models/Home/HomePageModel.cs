using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Models.AboutUs;

namespace CamprayPortal.Web.Models.Home
{
    public class HomePageModel
    {
        public IList<NewsItemModel> NewsItemModels { get; set; }
    }
}