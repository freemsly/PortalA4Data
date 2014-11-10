using CamprayPortal.Core.Domain.Customers;
using CamprayPortal.Core.Domain.Directory;
using CamprayPortal.Core.Domain.Localization;
 

namespace CamprayPortal.Core
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// Gets or sets the current customer
        /// </summary>
        Customer CurrentCustomer { get; set; }
        /// <summary>
        /// Gets or sets the original customer (in case the current one is impersonated)
        /// </summary>
     

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        Language WorkingLanguage { get; set; }
        /// <summary>
        /// Get or set current user working currency
        /// </summary>
        Currency WorkingCurrency { get; set; }
      

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        bool IsAdmin { get; set; }
    }
}
