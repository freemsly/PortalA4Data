using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;
using CamprayPortal.Web.Validators.Common;
using FluentValidation.Attributes;

namespace CamprayPortal.Web.Models.Support
{
    [Validator(typeof(RegisterValidator))]
    public partial class RegisterModel : BaseNopModel
    {
   
        [NopResourceDisplayName("Account.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }


        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.Fields.ConfirmPassword")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        [NopResourceDisplayName("Account.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }
        [NopResourceDisplayName("Account.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Account.Fields.Company")]
        [AllowHtml]
        public string Company { get; set; }

        [NopResourceDisplayName("Account.Fields.StreetAddress")]
        [AllowHtml]
        public string StreetAddress { get; set; }

    }
}