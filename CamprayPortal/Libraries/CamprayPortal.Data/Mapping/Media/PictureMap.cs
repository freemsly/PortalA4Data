using System.Data.Entity.ModelConfiguration;
using CamprayPortal.Core.Domain.Media;

namespace CamprayPortal.Data.Mapping.Media
{
    public partial class PictureMap : EntityTypeConfiguration<Picture>
    {
        public PictureMap()
        {
            this.ToTable("Picture");
            this.HasKey(p => p.Id);
            this.Property(p => p.PictureBinary).IsMaxLength();
            this.Property(p => p.MimeType).IsRequired().HasMaxLength(40);
            this.Property(p => p.SeoFilename).HasMaxLength(300);
        }
    }
}