using System.Linq;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using IRunes.Data;
using IRunes.Models;

namespace IRunes.App.Services
{
    public class UserService : IUserService
    {
        private readonly IRunesDbContext dbContext;

        public UserService(IRunesDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public User GetUser(string username, string hashedPassword)
        {

            var user = this.dbContext
                .Users
                .FirstOrDefault(x => x.Username == username &&
                                     x.Password == hashedPassword);

            return user;
        }

        public void AddUser(RegisterViewModel viewModel)
        {
            var user = new User()
            {
                Username = viewModel.Username,
                Password = viewModel.Password,
                Email = viewModel.Email
            };

            this.dbContext
                .Users
                .Add(user);
            this.dbContext
                .SaveChanges();
        }
    }
}
