using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Common
{
    public partial class UrlRecordModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.System.SeNames.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.EntityId")]
        public int EntityId { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.EntityName")]
        public string EntityName { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.IsActive")]
        public bool IsActive { get; set; }

        [NopResourceDisplayName("Admin.System.SeNames.Language")]
        public string Language { get; set; }
    }
}