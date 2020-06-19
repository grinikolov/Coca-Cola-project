using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class LocationSettings : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Lattitude).IsRequired();
            builder.Property(i => i.Longtitude).IsRequired();

        }
    }
}