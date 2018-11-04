using System.Collections.Generic;
using System.Linq;
using IRunes.App.Extensions;
using IRunes.App.ViewModels;
using IRunes.Models;
using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;

namespace IRunes.App.Controllers
{
    public class TrackController : BaseController
    {
        [Authorize]
        [HttpGet("track/create")]
        public IActionResult Create(TrackAlbumIdViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            this.Model.Data.Add("AlbumId", viewModel.AlbumId);
            return this.View();
        }

        [HttpPost("track/create")]
        public IActionResult DoCreate(CreateTrackViewModel viewModel)
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

            this.DbContext
                .AlbumTracks
                .Add(albumTrack);
            this.DbContext
                .SaveChanges();

            var trackDetailsViewModel = new TrackDetailsViewModel()
            {
                AlbumId = viewModel.AlbumId,
                TrackId = albumTrack.TrackId
            };

            return this.Details(trackDetailsViewModel);
        }

        [Authorize]
        [HttpGet("track/details")]
        public IActionResult Details(TrackDetailsViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var trackAlbum = this.DbContext
                .AlbumTracks
                .FirstOrDefault(at => at.AlbumId == viewModel.AlbumId &&
                                      at.TrackId == viewModel.TrackId);

            var trackLink = trackAlbum.Track.Link.DecodeUrl().Replace("watch?v=", "embed/");

            this.Model.Data.Add("Link", trackLink);
            this.Model.Data.Add("Name", trackAlbum.Track.Name);
            this.Model.Data.Add("Price", trackAlbum.Track.Price);
            this.Model.Data.Add("BackToAlbum", trackAlbum.AlbumId);

            return this.View();
        }
    }
}
