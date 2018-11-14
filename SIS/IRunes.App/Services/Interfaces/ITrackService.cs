using IRunes.App.ViewModels;
using IRunes.Models;

namespace IRunes.App.Services.Interfaces
{
    public interface ITrackService
    {
        AlbumTrack GetAlbumTrack(TrackDetailsViewModel viewModel);

        string AddTrack(CreateTrackViewModel viewModel);
    }
}
