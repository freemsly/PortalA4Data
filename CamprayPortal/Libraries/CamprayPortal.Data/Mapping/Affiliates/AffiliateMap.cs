using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Affiliates;

namespace CamprayPortal.Data.Mapping.Affiliates
{
    public partial class AffiliateMap : EntityTypeConfiguration<Affiliate>
    {
        public AffiliateMap()
        {
            this.ToTable("Affiliate");
            this.HasKey(a => a.Id);

            this.HasRequired(a => a.Address).WithMany().HasForeignKey(x => x.AddressId).WillCascadeOnDelete(false);
        }
    }
}