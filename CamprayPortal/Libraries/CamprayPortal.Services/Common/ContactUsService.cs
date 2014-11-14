using CamprayPortal.Core;
using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Data;
using CamprayPortal.Core.Domain.Common;
using CamprayPortal.Services.Events;
using System;

namespace CamprayPortal.Services.Common
{
    public partial class ContactUsServicec : IContactUsService
    {
        #region Constants
        private const string ContactUsEs_By_ID_Key = "CamprayPortal.contactus.id-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ContactUsEs_Pattern_Key = "CamprayPortal.contactus.";

        #endregion

        #region Fields

        private readonly IRepository<ContactUs> _contactUsRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheManager"></param>
        /// <param name="contactUsRepository"></param>
        /// <param name="eventPublisher"></param>
        public ContactUsServicec(ICacheManager cacheManager, IRepository<ContactUs> contactUsRepository,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager; 
            this._eventPublisher = eventPublisher;
            this._contactUsRepository = contactUsRepository;
        }

        #endregion
        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        public virtual void DeleteContactUs(ContactUs contactUs)
        {
            if (contactUs == null)
                throw new ArgumentNullException("contactUs");

            _contactUsRepository.Delete(contactUs);

            //cache
            _cacheManager.RemoveByPattern(ContactUsEs_Pattern_Key);

            //event notification
            _eventPublisher.EntityDeleted(contactUs);
        }
 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUsId"></param>
        /// <returns></returns>
        public virtual ContactUs GetContactUsById(int contactUsId)
        {
            if (contactUsId == 0)
                return null;

            string key = string.Format(ContactUsEs_By_ID_Key, contactUsId);
            return _cacheManager.Get(key, () => { return _contactUsRepository.GetById(contactUsId); });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        public virtual void InsertContactUs(ContactUs contactUs)
        {
            if (contactUs == null)
                throw new ArgumentNullException("contactUs");

            contactUs.CreatedOnUtc = DateTime.UtcNow;

            _contactUsRepository.Insert(contactUs);

            //cache
            _cacheManager.RemoveByPattern(ContactUsEs_Pattern_Key);

            //event notification
            _eventPublisher.EntityInserted(contactUs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contactUs"></param>
        public virtual void UpdateContactUs(ContactUs contactUs)
        {
            if (contactUs == null)
                throw new ArgumentNullException("contactUs");


            _contactUsRepository.Update(contactUs);

            //cache
            _cacheManager.RemoveByPattern(ContactUsEs_Pattern_Key);

            //event notification
            _eventPublisher.EntityUpdated(contactUs);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        public virtual IPagedList<ContactUs> GetAllContactUs(int storeId,
            int pageIndex, int pageSize, bool showHidden = false)
        {
            var query = _contactUsRepository.Table;

            var contacts = new PagedList<ContactUs>(query, pageIndex, pageSize);
            return contacts;
        }

        #endregion
    }
}
