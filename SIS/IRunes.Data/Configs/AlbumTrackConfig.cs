using IRunes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IRunes.Data.Configs
{
    public class AlbumTrackConfig : IEntityTypeConfiguration<AlbumTrack>
    {
        public void Configure(EntityTypeBuilder<AlbumTrack> builder)
        {
            builder
                .HasKey(x => new { x.AlbumId, x.TrackId });

            builder
                .HasOne(x => x.Album)
                .WithMany(x => x.AlbumTracks)
                .HasForeignKey(x => x.AlbumId);

            builder
                .HasOne(x => x.Track)
                .WithMany(x => x.TrackAlbums)
                .HasForeignKey(x => x.TrackId);
        }
    }
}
