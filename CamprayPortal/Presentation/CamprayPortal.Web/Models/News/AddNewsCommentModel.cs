using System.Web.Mvc;
using CamprayPortal.Web.Framework;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Models.News
{
    public partial class AddNewsCommentModel : BaseNopModel
    {
        [NopResourceDisplayName("News.Comments.CommentTitle")]
        [AllowHtml]
        public string CommentTitle { get; set; }

        [NopResourceDisplayName("News.Comments.CommentText")]
        [AllowHtml]
        public string CommentText { get; set; }

        public bool DisplayCaptcha { get; set; }
    }
}