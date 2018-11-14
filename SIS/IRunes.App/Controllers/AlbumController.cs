using System.Linq;
using System.Text;
using IRunes.App.Extensions;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using IRunes.Models;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Attributes.Method;

namespace IRunes.App.Controllers
{
    public class AlbumController : BaseController
    {
        private readonly IUserService userService;
        private readonly IAlbumService albumService;

        public AlbumController(IUserService userService, IAlbumService albumService)
        {
            this.userService = userService;
            this.albumService = albumService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var albums = this.albumService.GetAllAlbums();

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
        [HttpGet]
        public IActionResult Create()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (!this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateAlbumViewModel viewModel)
        {
            var user = (User)this.userService.GetUser(this.Identity.Username, this.Identity.Password);
            
            this.albumService.AddAlbum(viewModel, user);

            return this.All();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(AlbumDetailsViewModel viewModel)
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();

            var albumId = viewModel.AlbumId;

            var sb = new StringBuilder();
            var album = this.albumService.GetAlbum(viewModel);

            if (album == null || !this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            var tracksAsString = this.albumService.GetTracksAsString(albumId);

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
