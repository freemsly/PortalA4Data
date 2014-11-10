using System.Web.Routing;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Cms
{
    public partial class RenderWidgetModel : BaseNopModel
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }
    }
}