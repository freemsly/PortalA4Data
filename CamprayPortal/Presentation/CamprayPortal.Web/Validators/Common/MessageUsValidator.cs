using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CamprayPortal.Web.Models.Common;
using FluentValidation;

namespace CamprayPortal.Web.Validators.Common
{
    public class MessageUsValidator : AbstractValidator<MessageUsModel>
    {
        public MessageUsValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.CompanyName).NotEmpty();
            RuleFor(x => x.EmailAddress).NotEmpty();
        }
    }
}