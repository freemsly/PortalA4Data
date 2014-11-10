using System.Collections.Generic;
using CamprayPortal.Admin.Models.Localization;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Models.Common
{
    public partial class LanguageSelectorModel : BaseNopModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }
    }
}