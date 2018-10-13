using IRunes.Data.Configs;
using IRunes.Models;
using Microsoft.EntityFrameworkCore;

namespace IRunes.Data
{
    public class IRunesDbContext : DbContext
    {
        public IRunesDbContext()
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Track> Tracks { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AlbumTrack> AlbumTracks { get; set; }

        public IRunesDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configs.Configuration.ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new UserConfig())
                .ApplyConfiguration(new AlbumConfig())
                .ApplyConfiguration(new TrackConfig())
                .ApplyConfiguration(new AlbumTrackConfig());
        }
    }
}
