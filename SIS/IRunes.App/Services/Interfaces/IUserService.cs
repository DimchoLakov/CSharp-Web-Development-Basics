using IRunes.App.ViewModels;
using IRunes.Models;

namespace IRunes.App.Services.Interfaces
{
    public interface IUserService
    {
        User GetUser(string username, string hashedPassword);

        void AddUser(RegisterViewModel viewModel);
    }
}
