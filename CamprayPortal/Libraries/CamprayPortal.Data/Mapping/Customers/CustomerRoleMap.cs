using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Customers;

namespace CamprayPortal.Data.Mapping.Customers
{
    public partial class CustomerRoleMap : EntityTypeConfiguration<CustomerRole>
    {
        public CustomerRoleMap()
        {
            this.ToTable("CustomerRole");
            this.HasKey(cr => cr.Id);
            this.Property(cr => cr.Name).IsRequired().HasMaxLength(255);
            this.Property(cr => cr.SystemName).HasMaxLength(255);
        }
    }
}