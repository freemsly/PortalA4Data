﻿using System;
using System.Web.Mvc;
using CamprayPortal.Core;
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
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;
        #endregion

        #region Ctor

        public SupportController(IAuthenticationService authenticationService,
            ILocalizationService localizationService,
            ICustomerService customerService,
            ICustomerRegistrationService customerRegistrationService, IWorkContext workContext, CustomerSettings customerSettings)
        {
            this._authenticationService = authenticationService;
            this._customerService = customerService;
            this._customerRegistrationService = customerRegistrationService;
            _workContext = workContext;
            _customerSettings = customerSettings;
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
        public ActionResult CustomerPortal(LoginModel model, string returnUrl)
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

                        if (customer.IsAdmin())
                            return Redirect("/admin");
                        if (!String.IsNullOrEmpty(returnUrl))
                            return Redirect(returnUrl);
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



        public ActionResult Logout(string returnUrl)
        {
            //external authentication
            ExternalAuthorizerHelper.RemoveParameters();
            _authenticationService.SignOut();
            if (!String.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);
            return RedirectToRoute("HomePage");
        }



        public ActionResult Register()
        {
            var model = new RegisterModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl) 
        {
            if (_workContext.CurrentCustomer.IsRegistered())
            {
                //Already registered customer. 
                _authenticationService.SignOut();

                //Save a new record
                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
            }
            var customer = _workContext.CurrentCustomer;

            if (ModelState.IsValid)
            {

                bool isApproved = _customerSettings.UserRegistrationType == UserRegistrationType.Standard;
                var registrationRequest = new CustomerRegistrationRequest(customer, model.Email,
                    model.Email, model.Password, _customerSettings.DefaultPasswordFormat, isApproved);
                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                if (registrationResult.Success)
                {

                    //login customer now
                    if (isApproved)
                        _authenticationService.SignIn(customer, true);
                    return Redirect("/");
                }
                else
                {
                    foreach (var error in registrationResult.Errors)
                        ModelState.AddModelError("", error);
                }
            }
            return View(model);
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