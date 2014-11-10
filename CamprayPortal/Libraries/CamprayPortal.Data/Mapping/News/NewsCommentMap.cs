using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.News;

namespace CamprayPortal.Data.Mapping.News
{
    public partial class NewsCommentMap : EntityTypeConfiguration<NewsComment>
    {
        public NewsCommentMap()
        {
            this.ToTable("NewsComment");
            this.HasKey(pr => pr.Id);

            this.HasRequired(nc => nc.NewsItem)
                .WithMany(n => n.NewsComments)
                .HasForeignKey(nc => nc.NewsItemId);

            this.HasRequired(cc => cc.Customer)
                .WithMany()
                .HasForeignKey(cc => cc.CustomerId);
        }
    }
}