using System.Collections.Generic;
using System.Web.Mvc;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.Boards
{
    public partial class TopicMoveModel : BaseNopEntityModel
    {
        public TopicMoveModel()
        {
            ForumList = new List<SelectListItem>();
        }

        public int ForumSelected { get; set; }
        public string TopicSeName { get; set; }

        public IEnumerable<SelectListItem> ForumList { get; set; }
    }
}