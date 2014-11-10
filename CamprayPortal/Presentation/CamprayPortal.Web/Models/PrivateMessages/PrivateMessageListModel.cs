using System.Collections.Generic;
using CamprayPortal.Web.Models.Common;

namespace CamprayPortal.Web.Models.PrivateMessages
{
    public partial class PrivateMessageListModel
    {
        public IList<PrivateMessageModel> Messages { get; set; }
        public PagerModel PagerModel { get; set; }
    }
}