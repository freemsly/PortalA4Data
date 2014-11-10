using FluentValidation;
using CamprayPortal.Admin.Models.Customers;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Customers
{
    public class CustomerAttributeValidator : AbstractValidator<CustomerAttributeModel>
    {
        public CustomerAttributeValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerAttributes.Fields.Name.Required"));
        }
    }
}