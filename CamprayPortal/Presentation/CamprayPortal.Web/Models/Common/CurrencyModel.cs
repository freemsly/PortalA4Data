using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Common
{
    public partial class CurrencyModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string CurrencySymbol { get; set; }
    }
}