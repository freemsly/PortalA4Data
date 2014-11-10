using FluentValidation;
using CamprayPortal.Admin.Models.Localization;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Localization
{
    public class LanguageResourceValidator : AbstractValidator<LanguageResourceModel>
    {
        public LanguageResourceValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Name.Required"));
            RuleFor(x => x.Value).NotEmpty().WithMessage(localizationService.GetResource("Admin.Configuration.Languages.Resources.Fields.Value.Required"));
        }
    }
}