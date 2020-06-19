using BarCrawlers.Data.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarCrawlers.Data.ModelSettings
{
    public class BarUserCommentSettings : IEntityTypeConfiguration<BarUserComment>
    {
        public void Configure(EntityTypeBuilder<BarUserComment> builder)
        {
            builder.HasKey(b => new { b.BarId, b.UserId });
            builder.Property(c => c.Text).IsRequired();
            builder.Property(c => c.IsFlagged).IsRequired().HasDefaultValue(false);

        }
    }
}