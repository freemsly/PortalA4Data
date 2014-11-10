using FluentValidation;
using CamprayPortal.Admin.Models.Affiliates;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Affiliates
{
    public class AffiliateValidator : AbstractValidator<AffiliateModel>
    {
        public AffiliateValidator(ILocalizationService localizationService)
        {
        }
    }
}