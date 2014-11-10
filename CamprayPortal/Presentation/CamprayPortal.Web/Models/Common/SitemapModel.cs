using System.Collections.Generic;
using CamprayPortal.Web.Framework.Mvc;
 
using CamprayPortal.Web.Models.Topics;

namespace CamprayPortal.Web.Models.Common
{
    public partial class SitemapModel : BaseNopModel
    {
        public SitemapModel()
        {
            
            Topics = new List<TopicModel>();
        }
        
        public IList<TopicModel> Topics { get; set; }

        public bool NewsEnabled { get; set; }
        public bool BlogEnabled { get; set; }
        public bool ForumEnabled { get; set; }
    }
}