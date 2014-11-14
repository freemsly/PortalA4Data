using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CamprayPortal.Core.Domain.Common;

namespace CamprayPortal.Data.Mapping.Common
{
    public partial class ContactUsMap : EntityTypeConfiguration<ContactUs>
    {
        public ContactUsMap()
        {
            this.ToTable("ContactUs");
            this.HasKey(a => a.Id);
        }
    }
}
