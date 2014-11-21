using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CamprayPortal.Admin.Models.Common;
using CamprayPortal.Admin.Models.Logging;
using CamprayPortal.Core;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Services.Common;
using CamprayPortal.Services.Helpers;
using CamprayPortal.Services.Localization;
using CamprayPortal.Services.Logging;
using CamprayPortal.Services.Security;
using CamprayPortal.Web.Framework.Kendoui;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Admin.Controllers
{
    public partial class ActivityLogController : BaseAdminController
    {
        #region Fields

        private readonly ICustomerActivityService _customerActivityService;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly IContactUsService _contactUsService;
        private readonly IStoreContext _storeContext;
        #endregion Fields

        #region Constructors

        public ActivityLogController(ICustomerActivityService customerActivityService,
            IDateTimeHelper dateTimeHelper, ILocalizationService localizationService,
            IPermissionService permissionService, IContactUsService contactUsService, IStoreContext storeContext)
		{
            this._customerActivityService = customerActivityService;
            this._dateTimeHelper = dateTimeHelper;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            _contactUsService = contactUsService;
            _storeContext = storeContext;
		}

		#endregion 

        #region Activity log types

        public ActionResult ListTypes()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();

            var model = _customerActivityService
                .GetAllActivityTypes()
                .Select(x => x.ToModel())
                .ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveTypes(FormCollection form)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();

            string formKey = "checkbox_activity_types";
            var checkedActivityTypes = form[formKey] != null ? form[formKey].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList() : new List<int>();
            
            var activityTypes = _customerActivityService.GetAllActivityTypes();
            foreach (var activityType in activityTypes)
            {
                activityType.Enabled = checkedActivityTypes.Contains(activityType.Id);
                _customerActivityService.UpdateActivityType(activityType);
            }
            SuccessNotification(_localizationService.GetResource("Admin.Configuration.ActivityLog.ActivityLogType.Updated"));
            return RedirectToAction("ListTypes");
        }

        #endregion
        
        #region Activity log
        
        public ActionResult ListLogs()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            return View();
        }

        [HttpPost]
        public ActionResult ListLogs(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            var activityLog = _contactUsService.GetAllContactUs(0, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult()
            {
                Data = activityLog.Select(al => new ContactUs
                {
                    Id = al.Id,
                    FirstName = al.FirstName,
                    LastName = al.LastName,
                    Email = al.Email,
                    Company = al.Company,
                    PhoneNumber = al.PhoneNumber,
                    CreatedOnUtc = al.CreatedOnUtc,
                    Content = al.Content.Length > 50 ? al.Content.Substring(0,49) + "..." : al.Content,
                }).ToList(),
                Total = activityLog.TotalCount
            };
            return Json(gridModel);
        }


        public ActionResult AcivityLogDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();

            var activityLog = _contactUsService.GetContactUsById(id);
            if (activityLog == null)
            {
                throw new ArgumentException("No activity log found with the specified id");
            }
            _contactUsService.DeleteContactUs(activityLog);

            return new NullJsonResult();
        }

        public ActionResult ClearAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();

            _customerActivityService.ClearAllActivities();
            return RedirectToAction("ListLogs");
        }

        //edit
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageAffiliates))
                return AccessDeniedView();

            var contactUs = _contactUsService.GetContactUsById(id);
            if (contactUs == null)
                //No affiliate found with the specified id
                return RedirectToAction("List");

            var model = contactUs.ToModel();
    
            return View(model);
        }
         
        #endregion

    }
}
