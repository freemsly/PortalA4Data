using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Logging
{
    public partial class ActivityLogTypeModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Name")]
        public string Name { get; set; }
        [NopResourceDisplayName("Admin.Configuration.ActivityLog.ActivityLogType.Fields.Enabled")]
        public bool Enabled { get; set; }
    }
}