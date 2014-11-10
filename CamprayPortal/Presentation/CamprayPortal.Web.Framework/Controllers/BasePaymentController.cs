using System.Collections.Generic;
using System.Web.Mvc;
 

namespace CamprayPortal.Web.Framework.Controllers
{
    /// <summary>
    /// Base controller for payment plugins
    /// </summary>
    public abstract class BasePaymentController : BasePluginController
    {
        public abstract IList<string> ValidatePaymentForm(FormCollection form);
 
    }
}
