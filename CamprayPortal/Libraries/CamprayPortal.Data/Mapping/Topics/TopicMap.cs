using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Topics;

namespace CamprayPortal.Data.Mapping.Topics
{
    public class TopicMap : EntityTypeConfiguration<Topic>
    {
        public TopicMap()
        {
            this.ToTable("Topic");
            this.HasKey(t => t.Id);
        }
    }
}
