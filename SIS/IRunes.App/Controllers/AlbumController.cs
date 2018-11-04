using System.Linq;
using System.Text;
using IRunes.App.Extensions;
using IRunes.App.ViewModels;
using IRunes.Models;
using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Methods;

namespace IRunes.App.Controllers
{
    public class AlbumController : BaseController
    {
        [Authorize]
        [HttpGet("album/all")]
        public IActionResult All()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var albums = this.DbContext.Albums.ToArray();

            if (albums.Any())
            {
                var listOfAlbums = string.Empty;
                foreach (var album in albums)
                {
                    var albumHtml = $@"<p><a href=""/album/details?albumId={album.Id}"">{album.Name}</a></p>";
                    listOfAlbums += albumHtml;
                }
                this.Model["Albums"] = listOfAlbums;
            }
            else
            {
                this.Model["Albums"] = "There are currently no albums.";
            }

            return this.View();
        }

        [Authorize]
        [HttpGet("album/create")]
        public IActionResult Create()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (!this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }
            return this.View();
        }

        [HttpPost("album/create")]
        public IActionResult DoCreate(CreateAlbumViewModel viewModel)
        {
            var user = this.DbContext
                .Users
                .FirstOrDefault(x => x.Username == this.Identity.Username &&
                                     x.Password == this.Identity.Password);
            
            var album = new Album()
            {
                Name = viewModel.Name,
                Cover = viewModel.Cover,
                User = user,
                UserId = user.Id
            };

            this.DbContext
                .Albums
                .Add(album);

            this.DbContext
                .SaveChanges();

            return this.All();
        }

        [Authorize]
        [HttpGet("album/details")]
        public IActionResult Details(AlbumDetailsViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var albumId = viewModel.AlbumId;

            var sb = new StringBuilder();

            var album = this.DbContext.Albums.FirstOrDefault(x => x.Id == albumId);
            if (album == null || !this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            var tracks = this.DbContext
                .AlbumTracks
                .Where(x => x.AlbumId == albumId)
                .Select(x => $"<li class=\"list-group-item\"><a href=\"/track/details?albumId={albumId}&trackId={x.Track.Id}\"><strong>{x.Track.Name}</strong></a></li>")
                .ToArray();

            var tracksAsString = string.Join(string.Empty, tracks);
            var result = !string.IsNullOrWhiteSpace(tracksAsString) ? tracksAsString : "There are currently no tracks.";

            sb.Append(result);

            this.Model.Data.Add("Cover", album.Cover.DecodeUrl());
            this.Model.Data.Add("Name", album.Name);
            this.Model.Data.Add("Price", "$" + album.Price.ToString("f2"));
            this.Model.Data.Add("AlbumId", albumId);
            this.Model.Data.Add("Tracks", sb.ToString());
            return this.View();
        }
    }
}
