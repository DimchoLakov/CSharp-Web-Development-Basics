using System.Linq;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.App.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IRunesDbContext dbContext;

        public AlbumService(IRunesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Album GetAlbum(AlbumDetailsViewModel viewModel)
        {
            var album = this.dbContext.Albums.FirstOrDefault(a => a.Id == viewModel.AlbumId);

            return album;
        }

        public void AddAlbum(CreateAlbumViewModel viewModel, User user)
        {
            var album = new Album()
            {
                Name = viewModel.Name,
                Cover = viewModel.Cover,
                UserId = user.Id
            };

            this.dbContext
                .Albums
                .Add(album);

            this.dbContext
                .SaveChanges();
        }

        public string GetTracksAsString(string albumId)
        {
            var tracks = this.dbContext
                .AlbumTracks
                .Where(x => x.AlbumId == albumId)
                .Select(x => $"<li class=\"list-group-item\"><a href=\"/track/details?albumId={albumId}&trackId={x.Track.Id}\"><strong>{x.Track.Name}</strong></a></li>")
                .ToArray();

            var tracksAsString = string.Join(string.Empty, tracks);

            return tracksAsString;
        }

        public Album[] GetAllAlbums()
        {
            return this.dbContext.Albums.ToArray();
        }
    }
}
