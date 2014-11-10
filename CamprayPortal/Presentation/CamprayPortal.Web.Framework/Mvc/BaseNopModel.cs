using System.Collections.Generic;
using System.Web.Mvc;

namespace CamprayPortal.Web.Framework.Mvc
{
    /// <summary>
    /// Base CamprayPortal model
    /// </summary>
    public partial class BaseNopModel
    {
        public BaseNopModel()
        {
            this.CustomProperties = new Dictionary<string, object>();
        }
        public virtual void BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
        }

        /// <summary>
        /// Use this property to store any custom value for your models. 
        /// </summary>
        public Dictionary<string, object> CustomProperties { get; set; }
    }

    /// <summary>
    /// Base CamprayPortal entity model
    /// </summary>
    public partial class BaseNopEntityModel : BaseNopModel
    {
        public virtual int Id { get; set; }
    }
}
