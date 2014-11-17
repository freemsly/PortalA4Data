using System;
using System.Web.Mvc;
using CamprayPortal.Core.Domain.Customers;
using CamprayPortal.Core.Domain.Localization;
using CamprayPortal.Services.Authentication;
using CamprayPortal.Services.Authentication.External;
using CamprayPortal.Services.Customers;
using CamprayPortal.Services.Localization;
using CamprayPortal.Web.Models.Support;

namespace CamprayPortal.Web.Controllers
{
    public class SupportController : BasePublicController
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly ICustomerRegistrationService _customerRegistrationService;

        #endregion

        #region Ctor

        public SupportController(IAuthenticationService authenticationService,
            ILocalizationService localizationService,
            ICustomerService customerService,
            ICustomerRegistrationService customerRegistrationService
          )
        {
            this._authenticationService = authenticationService;
            this._customerService = customerService;
            this._customerRegistrationService = customerRegistrationService;
            this._localizationService = localizationService;
        }

        #endregion

        // GET: Support
        public ActionResult Benefits()
        {
            return TopicModelView();
        }

        public ActionResult CustomerPortal()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CustomerPortal(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _customerRegistrationService.ValidateCustomer(model.Username, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                    {
                        var customer = _customerService.GetCustomerByUsername(model.Username);

                        //sign in new customer
                        _authenticationService.SignIn(customer, model.RememberMe);

                            return RedirectToRoute("HomePage");
                    }
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("",
                            _localizationService.GetResource("Account.Login.WrongCredentials.CustomerNotExist"));
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("",
                            _localizationService.GetResource("Account.Login.WrongCredentials.Deleted"));
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("",
                            _localizationService.GetResource("Account.Login.WrongCredentials.NotActive"));
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("",
                            _localizationService.GetResource("Account.Login.WrongCredentials.NotRegistered"));
                        break;
                    case CustomerLoginResults.WrongPassword:
                    default:
                        ModelState.AddModelError("", _localizationService.GetResource("Account.Login.WrongCredentials"));
                        break;
                }
            }
            return View(model);
        }



        public ActionResult Logout()
        {
            //external authentication
            ExternalAuthorizerHelper.RemoveParameters();
            _authenticationService.SignOut();
            return RedirectToRoute("HomePage");
        }


        public ActionResult Documentation()
        {
            return TopicModelView();
        }

        public ActionResult Homepage()
        {
            return TopicModelView();
        }

        public ActionResult SoftwareVersion()
        {
            return TopicModelView();
        }
    }
}