using CamprayPortal.Web.Framework.Mvc;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Models.Customer
{
    public partial class CustomerAddressEditModel : BaseNopModel
    {
        public CustomerAddressEditModel()
        {
            this.Address = new AddressModel();
        }
        public AddressModel Address { get; set; }
        public CustomerNavigationModel NavigationModel { get; set; }
    }
}