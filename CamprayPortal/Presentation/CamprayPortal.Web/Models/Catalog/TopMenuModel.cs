using System.Collections.Generic;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Catalog
{
    public partial class TopMenuModel : BaseNopModel
    {
        public TopMenuModel()
        {
             
        }

    

        public bool BlogEnabled { get; set; }
        public bool RecentlyAddedProductsEnabled { get; set; }
        public bool ForumEnabled { get; set; }
    }
}