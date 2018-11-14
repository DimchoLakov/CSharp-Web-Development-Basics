using System.Linq;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.App.Services
{
    public class TrackService : ITrackService
    {
        private readonly IRunesDbContext dbContext;

        public TrackService(IRunesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public AlbumTrack GetAlbumTrack(TrackDetailsViewModel viewModel)
        {
            var trackAlbum = this.dbContext
                .AlbumTracks
                .FirstOrDefault(at => at.AlbumId == viewModel.AlbumId &&
                                      at.TrackId == viewModel.TrackId);

            return trackAlbum;
        }

        public string AddTrack(CreateTrackViewModel viewModel)
        {
            var track = new Track()
            {
                Name = viewModel.Name,
                Link = viewModel.Link,
                Price = viewModel.Price,
            };

            var albumTrack = new AlbumTrack()
            {
                Track = track,
                AlbumId = viewModel.AlbumId
            };

            this.dbContext
                .AlbumTracks
                .Add(albumTrack);
            this.dbContext
                .SaveChanges();

            return albumTrack.TrackId;
        }
    }
}
