using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Common;

namespace CamprayPortal.Data.Mapping.Common
{
    public partial class SearchTermMap : EntityTypeConfiguration<SearchTerm>
    {
        public SearchTermMap()
        {
            this.ToTable("SearchTerm");
            this.HasKey(st => st.Id);
        }
    }
}
