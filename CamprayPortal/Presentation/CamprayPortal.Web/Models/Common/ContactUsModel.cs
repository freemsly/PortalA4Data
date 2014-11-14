using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CamprayPortal.Web.Models.Common
{
    public partial class ContactUsModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Address.Contact.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Address.Contact.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Address.Contact.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [NopResourceDisplayName("Address.Contact.Company")]
        [AllowHtml]
        public string Company { get; set; }

        [NopResourceDisplayName("Address.Contact.PhoneNumber")]
        [AllowHtml]
        public string PhoneNumber { get; set; }

        [NopResourceDisplayName("Address.Contact.Content")]
        [AllowHtml]
        public string Content { get; set; }
         
    }
}