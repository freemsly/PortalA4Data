using CamprayPortal.Core.Caching;
using CamprayPortal.Core.Domain.Configuration;
using CamprayPortal.Core.Events;
using CamprayPortal.Core.Infrastructure;
using CamprayPortal.Services.Events;

namespace CamprayPortal.Admin.Infrastructure.Cache
{
    /// <summary>
    /// Model cache event consumer (used for caching of presentation layer models)
    /// </summary>
    public partial class ModelCacheEventConsumer: 
        //settings
        IConsumer<EntityUpdated<Setting>>
    {
        /// <summary>
        /// Key for CamprayPortal.com news cache
        /// </summary>
        public const string OFFICIAL_NEWS_MODEL_KEY = "CamprayPortal.pres.admin.official.news";
        public const string OFFICIAL_NEWS_PATTERN_KEY = "CamprayPortal.pres.admin.official.news";

        private readonly ICacheManager _cacheManager;
        
        public ModelCacheEventConsumer()
        {
            //TODO inject static cache manager using constructor
            this._cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
        }

        public void HandleEvent(EntityUpdated<Setting> eventMessage)
        {
            //clear models which depend on settings
            _cacheManager.RemoveByPattern(OFFICIAL_NEWS_PATTERN_KEY); //depends on CommonSettings.HideAdvertisementsOnAdminArea
            
        }
    }
}
