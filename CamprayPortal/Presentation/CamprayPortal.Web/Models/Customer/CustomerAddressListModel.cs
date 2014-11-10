using System.Collections.Generic;
using CamprayPortal.Web.Framework.Mvc;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Models.Customer
{
    public partial class CustomerAddressListModel : BaseNopModel
    {
        public CustomerAddressListModel()
        {
            Addresses = new List<AddressModel>();
        }

        public IList<AddressModel> Addresses { get; set; }
        public CustomerNavigationModel NavigationModel { get; set; }
    }
}