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
        private readonly IWorkflowMessageService _workflowMessageService;

        public DownloadsController(IWorkContext workContext, IEmailSender emailSender,
            IEmailAccountService emailAccountService, IWebHelper webHelper,
            IWorkflowMessageService workflowMessageService)
        {
            _workContext = workContext;
            _emailSender = emailSender;
            _emailAccountService = emailAccountService;
            _webHelper = webHelper;
            _workflowMessageService = workflowMessageService;
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
                string body = String.Format("<a href='{0}'>{1}</a>", link, name);
                _workflowMessageService.SendEmailAFriendMessage(_workContext.CurrentCustomer,
                    _workContext.WorkingLanguage.Id, body);
                return Json(1);
            }
            catch (Exception exc)
            {
                return Json(2);
            }
        }
    }
}