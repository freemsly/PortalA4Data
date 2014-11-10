using CamprayPortal.Admin.Models.Common;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Customers
{
    public partial class CustomerAddressModel : BaseNopModel
    {
        public int CustomerId { get; set; }

        public AddressModel Address { get; set; }
    }
}