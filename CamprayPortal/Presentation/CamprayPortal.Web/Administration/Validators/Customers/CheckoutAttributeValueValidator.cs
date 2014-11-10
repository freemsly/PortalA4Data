using FluentValidation;
using CamprayPortal.Admin.Models.Customers;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Customers
{
    public class CustomerAttributeValueValidator : AbstractValidator<CustomerAttributeValueModel>
    {
        public CustomerAttributeValueValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerAttributes.Values.Fields.Name.Required"));
        }
    }
}