using FluentValidation;
using CamprayPortal.Admin.Models.Topics;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Topics
{
    public class TopicValidator : AbstractValidator<TopicModel>
    {
        public TopicValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.SystemName).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.Topics.Fields.SystemName.Required"));
        }
    }
}