using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IRunes.App.Extensions;
using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        private IHashService hashService;
        private IRunesDbContext dbContext;
        private Dictionary<string, string> viewBag;

        public UserController()
        {
            this.hashService = new HashService();
            this.dbContext = new IRunesDbContext();
            this.viewBag = new Dictionary<string, string>();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                var username = this.GetUsernameFromSession(request);
                this.viewBag.Add("Username", username);
                return View("Index", request, viewBag);
            }
            return View("Login", request);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return View("Index", request);
            }
            return View("Register", request);
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            if (this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var user = this.dbContext.Users.FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return View("Index", request);
            }

            var view = this.View("Index", request);

            this.SignInUser(username, view, request);
            
            viewBag.Add("Username", username);

            return View("Index", request, viewBag);
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();
            var email = request.FormData["email"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) || string.IsNullOrWhiteSpace(email) || password != confirmPassword)
            {
                return View("/users/register", request);
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(password);

            var user = new User()
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.dbContext.Users.Add(user);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequestError(e.Message, request);
            }

            return this.DoLogin(request);

            //return View("Index", request);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            this.LogoutUser(request);
            return View("Index", request);
        }

        public IHttpResponse ShowAlbums(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var albums = this.dbContext
                .Albums
                .Select(a => $"<a href=\"/albums/details?id={a.Id}\">{a.Name}</a><br />")
                .ToArray();

            var albumsToString = string.Join(string.Empty, albums);
            var result = !string.IsNullOrWhiteSpace(albumsToString) ? albumsToString : "There are currently no albums.";
            
            viewBag.Add("Albums", result);

            return View("AllAlbums", request, viewBag);
        }

        public IHttpResponse CreateAlbum(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return View("Index", request);
            }
            return View("AlbumCreate", request);
        }

        public IHttpResponse DoCreateAlbum(IHttpRequest request)
        {
            var albumName = request.FormData["name"].ToString();
            var albumCover = request.FormData["cover"].ToString();

            var username = this.GetUsernameFromSession(request);
            var userId = this.dbContext.Users.FirstOrDefault(x => x.Username == username)?.Id;
            if (userId == null || !this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var album = new Album()
            {
                Name = albumName,
                Cover = albumCover,
                UserId = userId
            };

            this.dbContext.Albums.Add(album);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestError(e.Message, request);
            }

            return this.ShowAlbums(request);
        }

        public IHttpResponse AlbumDetails(IHttpRequest request)
        {
            var albumId = string.Empty;
            try
            {
                albumId = request.QueryData["id"].ToString();
            }
            catch (Exception)
            {
                albumId = request.QueryData["albumDetailsId"].ToString();
            }

            var album = this.dbContext.Albums.FirstOrDefault(x => x.Id == albumId);
            if (album == null || !this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var sb = new StringBuilder();

            sb.Append("<ol>");

            var tracks = this.dbContext.AlbumTracks
                .Where(x => x.AlbumId == albumId)
                .Select(x => $"<li><a href=\"/tracks/details?albumId={albumId}&trackId={x.Track.Id}\">{x.Track.Name}</a></li>")
                .ToArray();

            var tracksAsString = string.Join(string.Empty, tracks);
            var result = !string.IsNullOrWhiteSpace(tracksAsString) ? tracksAsString : "There are currently no tracks.";

            sb.Append(result);
            sb.Append("</ol>");
                
            viewBag.Add("Cover", album.Cover.DecodeUrl());
            viewBag.Add("Name", album.Name);
            viewBag.Add("Price", "$" + album.Price.ToString("f2"));
            viewBag.Add("AlbumId", albumId);
            viewBag.Add("Tracks", sb.ToString());

            return View("AlbumDetails", request, viewBag);
        }

        public IHttpResponse CreateTrack(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return View("Index", request);
            }

            var albumId = request.QueryData["albumId"].ToString();

            this.viewBag.Add("AlbumId", albumId);
            this.viewBag.Add("BackToAlbum", $"<a href=\"/albums/details?id={albumId}\">Back To Album</a>");

            return View("TrackCreate", request, viewBag);
        }

        public IHttpResponse DoCreateTrack(IHttpRequest request)
        {
            var trackName = request.FormData["name"].ToString().Trim();
            var link = request.FormData["link"].ToString().Trim();
            var priceAsString = request.FormData["price"].ToString().Trim();

            if (!decimal.TryParse(priceAsString, out decimal price))
            {
                return View("TrackCreate", request);
            }

            var track = new Track()
            {
                Name = trackName,
                Link = link,
                Price = price
            };

            var albumId = request.QueryData["albumId"].ToString();
            var albumTrack = new AlbumTrack()
            {
                Track = track,
                AlbumId = albumId
            };

            this.dbContext.AlbumTracks.Add(albumTrack);

            try
            {
                this.dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message, request);
            }

            request.QueryData["albumDetailsId"] = albumId;

            return this.AlbumDetails(request);
        }

        public IHttpResponse TrackDetails(IHttpRequest request)
        {
            var trackId = request.QueryData["trackId"].ToString();
            var albumId = request.QueryData["albumId"].ToString();

            var track = this.dbContext.Tracks.FirstOrDefault(x => x.Id == trackId);

            if (track == null)
            {
                request.QueryData["albumDetailsId"] = albumId;
                return View("AlbumDetails", request);
            }

            var decodedLink = track.Link.DecodeUrl();
            decodedLink = decodedLink.Replace("watch?v=", "embed/");

            viewBag.Add("Link", decodedLink + "/");
            viewBag.Add("Name", track.Name);
            viewBag.Add("Price", "$" + track.Price.ToString("f2"));
            viewBag.Add("BackToAlbum", $"<a href=\"/albums/details?id={albumId}\">Back To Album</a>");

            return View("TrackDetails", request, viewBag);
        }
    }
}
