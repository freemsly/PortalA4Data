using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Customer
{
    public partial class BackInStockSubscriptionModel : BaseNopEntityModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SeName { get; set; }
    }
}
