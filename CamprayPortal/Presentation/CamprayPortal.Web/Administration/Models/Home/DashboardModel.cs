using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Home
{
    public partial class DashboardModel : BaseNopModel
    {
        public bool IsLoggedInAsVendor { get; set; }
    }
}