using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Validators.Common;
using FluentValidation.Attributes;

namespace CamprayPortal.Web.Models.Common
{
    [Validator(typeof(MessageUsValidator))]
    public class MessageUsModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompanyName { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Comments { get; set; }
    }
}