using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Common;

namespace CamprayPortal.Data.Mapping.Common
{
    public partial class GenericAttributeMap : EntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");
            this.HasKey(ga => ga.Id);

            this.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Value).IsRequired();
        }
    }
}