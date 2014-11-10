using System.Collections.Generic;
using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.News
{
    public partial class NewsItemListModel : BaseNopModel
    {
        public NewsItemListModel()
        {
            AvailableStores = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.ContentManagement.News.NewsItems.List.SearchStore")]
        public int SearchStoreId { get; set; }
        public IList<SelectListItem> AvailableStores { get; set; }
    }
}