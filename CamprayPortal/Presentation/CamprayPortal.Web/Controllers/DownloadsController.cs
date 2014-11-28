using System;
using System.Linq;
using System.Web;
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
                var filename = url.Substring(url.LastIndexOf('/') + 1, url.Length - url.LastIndexOf('/') - 1);
                var downurl = _webHelper.GetHost(false) + "Downloads/FileDownLoad?filename=" + HttpUtility.UrlEncode(filename);
                if (String.IsNullOrWhiteSpace(_workContext.CurrentCustomer.Email))
                    return Json(2);
                string body = String.Format("<a href='{0}'>{1}</a>", downurl, name);
                _workflowMessageService.SendEmailAFriendMessage(_workContext.CurrentCustomer,
                    _workContext.WorkingLanguage.Id, body); 
                return Json(1);
            }
            catch (Exception exc)
            {
                return Json(2);
            }
        }


        public FileResult FileDownLoad(string filename)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
            {
                Response.Redirect("/login?returnUrl=/Downloads/FileDownLoad?filename=" + filename);
                return null;
            }

            return File(Server.MapPath("~/Content/Images/uploaded/" + filename), "application/octet-stream", filename);
        }
    }
}