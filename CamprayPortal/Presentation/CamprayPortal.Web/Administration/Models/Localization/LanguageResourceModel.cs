using System.Web.Mvc;
using FluentValidation.Attributes;
using CamprayPortal.Admin.Validators.Localization;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Localization
{
    [Validator(typeof(LanguageResourceValidator))]
    public partial class LanguageResourceModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.Value")]
        [AllowHtml]
        public string Value { get; set; }

        [NopResourceDisplayName("Admin.Configuration.Languages.Resources.Fields.LanguageName")]
        [AllowHtml]
        public string LanguageName { get; set; }

        public int LanguageId { get; set; }
    }
}