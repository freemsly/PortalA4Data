using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Directory;

namespace CamprayPortal.Data.Mapping.Directory
{
    public partial class CurrencyMap : EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            this.ToTable("Currency");
            this.HasKey(c =>c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(50);
            this.Property(c => c.CurrencyCode).IsRequired().HasMaxLength(5);
            this.Property(c => c.DisplayLocale).HasMaxLength(50);
            this.Property(c => c.CustomFormatting).HasMaxLength(50);
            this.Property(c => c.Rate).HasPrecision(18, 4);
        }
    }
}