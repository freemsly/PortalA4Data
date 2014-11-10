using System.Web.Mvc;
using FluentValidation.Attributes;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;
using CamprayPortal.Web.Validators.Customer;

namespace CamprayPortal.Web.Models.Customer
{
    [Validator(typeof(PasswordRecoveryValidator))]
    public partial class PasswordRecoveryModel : BaseNopModel
    {
        [AllowHtml]
        [NopResourceDisplayName("Account.PasswordRecovery.Email")]
        public string Email { get; set; }

        public string Result { get; set; }
    }
}