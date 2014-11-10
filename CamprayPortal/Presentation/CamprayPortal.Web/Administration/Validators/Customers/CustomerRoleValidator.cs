using FluentValidation;
using CamprayPortal.Admin.Models.Customers;
using CamprayPortal.Services.Localization;

namespace CamprayPortal.Admin.Validators.Customers
{
    public class CustomerRoleValidator : AbstractValidator<CustomerRoleModel>
    {
        public CustomerRoleValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Customers.CustomerRoles.Fields.Name.Required"));
        }
    }
}