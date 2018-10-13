using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IRunes.Data.Configs
{
    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        public void Configure(EntityTypeBuilder<Track> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired();

            builder
                .Property(x => x.Link)
                .IsRequired();

            builder
                .Property(x => x.Price)
                .IsRequired();
        }
    }
}
