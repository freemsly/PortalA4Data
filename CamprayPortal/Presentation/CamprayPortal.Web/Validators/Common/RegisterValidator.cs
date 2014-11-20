using CamprayPortal.Services.Localization;
using CamprayPortal.Web.Models.Support;
using FluentValidation;

namespace CamprayPortal.Web.Validators.Common
{
    public class RegisterValidator : AbstractValidator<RegisterModel>
    {
        public RegisterValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Email.Required"));
            RuleFor(x => x.FirstName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.FirstName.Required"));
            RuleFor(x => x.LastName).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.LastName.Required"));
            RuleFor(x => x.Email).EmailAddress().WithMessage(localizationService.GetResource("Common.WrongEmail"));
            RuleFor(x => x.Company).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Company.Required"));
            RuleFor(x => x.Password).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.Password.Required"));
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage(localizationService.GetResource("Account.Fields.ConfirmPassword.Required"));
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage(localizationService.GetResource("Account.Fields.Password.EnteredPasswordsDoNotMatch"));
        }
    }
}