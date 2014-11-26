using System;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.Customers;
using CamprayPortal.Services.Messages;

namespace CamprayPortal.Web.Controllers
{
    public class DownloadsController : BasePublicController
    {
        private readonly IWorkContext _workContext;
        private readonly IEmailSender _emailSender;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IWebHelper _webHelper;

        public DownloadsController(IWorkContext workContext, IEmailSender emailSender,
            IEmailAccountService emailAccountService, IWebHelper webHelper)
        {
            _workContext = workContext;
            _emailSender = emailSender;
            _emailAccountService = emailAccountService;
            _webHelper = webHelper;
        }

        // GET: Downloads
        public ActionResult Homepage()
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return RedirectToRoute("Login", new {returnUrl = Url.Action("Homepage")});
            return TopicModelView();
        }


        [HttpPost]
        public JsonResult SendEmail(string name, string url)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
                return Json(0);

            var emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
            if (emailAccount == null)
                return Json(2);
            try
            {
                string link;
                if (url.Contains("http"))
                {
                    link = url;
                }
                else
                {
                    var localhost = _webHelper.GetHost(false);
                    url = url.IndexOf('/') == 0 ? url.Substring(1, url.Length - 1) : url;
                    link = localhost + url;
                }

                if (String.IsNullOrWhiteSpace(_workContext.CurrentCustomer.Email))
                    return Json(2);
                const string subject = "A4 Data DownLoad Center";
                string body = String.Format("Please click <a href='{0}'>{1}</a>  download file !", link, name);
                _emailSender.SendEmail(emailAccount, subject, body, emailAccount.Email, emailAccount.DisplayName,
                    _workContext.CurrentCustomer.Email, null);
                return Json(1);
            }
            catch (Exception exc)
            {
                return Json(2);
            }
        }
    }
}