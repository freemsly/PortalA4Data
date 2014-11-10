using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Stores;

namespace CamprayPortal.Data.Mapping.Stores
{
    public partial class StoreMappingMap : EntityTypeConfiguration<StoreMapping>
    {
        public StoreMappingMap()
        {
            this.ToTable("StoreMapping");
            this.HasKey(sm => sm.Id);

            this.Property(sm => sm.EntityName).IsRequired().HasMaxLength(400);

            this.HasRequired(sm => sm.Store)
                .WithMany()
                .HasForeignKey(sm => sm.StoreId)
                .WillCascadeOnDelete(true);
        }
    }
}