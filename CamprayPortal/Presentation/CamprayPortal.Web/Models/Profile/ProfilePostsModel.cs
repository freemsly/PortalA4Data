using System.Collections.Generic;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Models.Profile
{
    public partial class ProfilePostsModel
    {
        public IList<PostsModel> Posts { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}