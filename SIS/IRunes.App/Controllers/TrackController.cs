using IRunes.App.Extensions;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace IRunes.App.Controllers
{
    public class TrackController : BaseController
    {
        private readonly ITrackService trackService;

        public TrackController(ITrackService trackService)
        {
            this.trackService = trackService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(TrackAlbumIdViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            this.Model.Data.Add("AlbumId", viewModel.AlbumId);
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateTrackViewModel viewModel)
        {
            var trackId = this.trackService.AddTrack(viewModel);

            var trackDetailsViewModel = new TrackDetailsViewModel()
            {
                AlbumId = viewModel.AlbumId,
                TrackId = trackId
            };

            return this.Details(trackDetailsViewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(TrackDetailsViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var albumTrack = this.trackService.GetAlbumTrack(viewModel);
            var trackLink = albumTrack.Track.Link.DecodeUrl().Replace("watch?v=", "embed/");

            this.Model.Data.Add("Link", trackLink);
            this.Model.Data.Add("Name", albumTrack.Track.Name);
            this.Model.Data.Add("Price", albumTrack.Track.Price);
            this.Model.Data.Add("BackToAlbum", albumTrack.AlbumId);

            return this.View();
        }
    }
}
