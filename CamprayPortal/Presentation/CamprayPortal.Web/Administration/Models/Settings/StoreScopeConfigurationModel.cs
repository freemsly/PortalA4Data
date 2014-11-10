using System.Collections.Generic;
using CamprayPortal.Admin.Models.Stores;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Settings
{
    public partial class StoreScopeConfigurationModel : BaseNopModel
    {
        public StoreScopeConfigurationModel()
        {
            Stores = new List<StoreModel>();
        }

        public int StoreId { get; set; }
        public IList<StoreModel> Stores { get; set; }
    }
}