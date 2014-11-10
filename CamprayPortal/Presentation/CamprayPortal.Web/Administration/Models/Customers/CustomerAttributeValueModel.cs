using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using CamprayPortal.Admin.Validators.Customers;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Localization;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Customers
{
    [Validator(typeof(CustomerAttributeValueValidator))]
    public partial class CustomerAttributeValueModel : BaseNopEntityModel, ILocalizedModel<CustomerAttributeValueLocalizedModel>
    {
        public CustomerAttributeValueModel()
        {
            Locales = new List<CustomerAttributeValueLocalizedModel>();
        }

        public int CustomerAttributeId { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.IsPreSelected")]
        public bool IsPreSelected { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.DisplayOrder")]
        public int DisplayOrder {get;set;}

        public IList<CustomerAttributeValueLocalizedModel> Locales { get; set; }

    }

    public partial class CustomerAttributeValueLocalizedModel : ILocalizedModelLocal
    {
        public int LanguageId { get; set; }

        [NopResourceDisplayName("Admin.Customers.CustomerAttributes.Values.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }
    }
}