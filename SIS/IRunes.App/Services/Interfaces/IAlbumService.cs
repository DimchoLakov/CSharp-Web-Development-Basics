using System;
using System.Collections;
using IRunes.App.ViewModels;
using IRunes.Models;

namespace IRunes.App.Services.Interfaces
{
    public interface IAlbumService
    {
        Album GetAlbum(AlbumDetailsViewModel viewModel);

        void AddAlbum(CreateAlbumViewModel viewModel, User user);

        string GetTracksAsString(string albumId);
        Album[] GetAllAlbums();
    }
}
