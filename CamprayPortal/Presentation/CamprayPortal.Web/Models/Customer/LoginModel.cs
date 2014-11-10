using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Customer
{
    public partial class LoginModel : BaseNopModel
    {
        public bool CheckoutAsGuest { get; set; }

        [NopResourceDisplayName("Account.Login.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [NopResourceDisplayName("Account.Login.Fields.UserName")]
        [AllowHtml]
        public string Username { get; set; }

        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.Login.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        [NopResourceDisplayName("Account.Login.Fields.RememberMe")]
        public bool RememberMe { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}