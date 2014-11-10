using System.Collections.Generic;
using System.Web.Mvc;
using FluentValidation.Attributes;
using CamprayPortal.Admin.Models.Stores;
using CamprayPortal.Admin.Validators.Localization;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Localization
{
    [Validator(typeof(LanguageValidator))]
    public partial class LanguageModel : BaseNopEntityModel
    {
        public LanguageModel()
        {
            FlagFileNames = new List<string>();
            AvailableCurrencies = new List<SelectListItem>();
        }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        [AllowHtml]
        public string LanguageCulture { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        [AllowHtml]
        public string UniqueSeoCode { get; set; }
        
        //flags
        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        [AllowHtml]
        public string FlagImageFileName { get; set; }
        public IList<string> FlagFileNames { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        //default currency
        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
        [AllowHtml]
        public int DefaultCurrencyId { get; set; }
        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }


        //Store mapping
        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.LimitedToStores")]
        public bool LimitedToStores { get; set; }
        [NopResourceDisplayName("Admin.Configuration.Languages.Fields.AvailableStores")]
        public List<StoreModel> AvailableStores { get; set; }
        public int[] SelectedStoreIds { get; set; }

    }
}