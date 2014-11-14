using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamprayPortal.Admin.Models.Common
{

    public partial class ContactUsModel : BaseNopEntityModel
    { 
        [NopResourceDisplayName("Admin.Contact.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }

        [NopResourceDisplayName("Admin.Contact.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        [NopResourceDisplayName("Admin.Contact.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        [NopResourceDisplayName("Admin.Contact.Fields.Company")]
        [AllowHtml]
        public string Company { get; set; }

        [NopResourceDisplayName("Admin.Contact.Fields.PhoneNumber")]
        [AllowHtml]
        public string PhoneNumber { get; set; }

        [NopResourceDisplayName("Admin.Contact.Fields.Content")]
        [AllowHtml]
        public string Content { get; set; }
         
    }
}