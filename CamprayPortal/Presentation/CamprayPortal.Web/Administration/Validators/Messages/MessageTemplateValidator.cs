using FluentValidation;
using CamprayPortal.Admin.Models.Messages;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Messages
{
    public class MessageTemplateValidator : AbstractValidator<MessageTemplateModel>
    {
        public MessageTemplateValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Subject).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Subject.Required"));
            RuleFor(x => x.Body).NotEmpty().WithMessage(localizationService.GetResource("Admin.ContentManagement.MessageTemplates.Fields.Body.Required"));
        }
    }
}