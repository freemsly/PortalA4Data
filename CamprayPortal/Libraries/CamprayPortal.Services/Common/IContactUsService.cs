using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Core;

namespace CamprayPortal.Services.Common
{
    public partial interface IContactUsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        void DeleteContactUs(ContactUs contactUs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUsId"></param>
        /// <returns></returns>
        ContactUs GetContactUsById(int contactUsId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        void InsertContactUs(ContactUs contactUs);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        void UpdateContactUs(ContactUs contactUs);

        /// <summary>
        /// 
        /// </summary>
      
        /// <param name="storeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        IPagedList<ContactUs> GetAllContactUs(int storeId,
            int pageIndex, int pageSize, bool showHidden = false);
    }
}