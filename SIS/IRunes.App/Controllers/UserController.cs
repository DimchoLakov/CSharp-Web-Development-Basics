using System.Linq;
using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using IRunes.Models;
using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Attributes.Methods;
using SIS.Framework.Security;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        private readonly IHashService hashService;

        public UserController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/user/login")]
        public IActionResult Login()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }
            return this.View();
        }
        
        [HttpPost("/user/login")]
        public IActionResult DoLogin(LoginViewModel loginViewModel)
        {
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(loginViewModel.Password);

            var user = this.DbContext
                .Users
                .FirstOrDefault(x => x.Username == loginViewModel.Username &&
                                     x.Password == hashedPassword);

            if (user == null)
            {
                this.Model.Data["Error"] = "User does not exist.";
                return this.Login();
            }

            this.SignIn(new IdentityUser()
            {
                Username = user.Username,
                Password = hashedPassword,
                Email = user.Email
            });

            return this.RedirectToAction("/");
        }

        [HttpGet("/user/register")]
        public IActionResult Register()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            return View();
        }

        [HttpPost("/user/register")]
        public IActionResult DoRegister(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                return Register();
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(registerViewModel.Password);

            var userExists = this.DbContext
                .Users
                .FirstOrDefault(x => x.Username == registerViewModel.Username &&
                                                                      x.Password == hashedPassword) != null;

            if (userExists)
            {
                this.Model.Data["Error"] = $"User with username {registerViewModel.Username} already exists.";
                return this.Register();
            }

            var user = new User()
            {
                Username = registerViewModel.Username,
                Password = hashedPassword,
                Email = registerViewModel.Email
            };

            this.DbContext.Users.Add(user);
            this.DbContext.SaveChanges();
            
            return this.DoLogin(new LoginViewModel());
        }

        public IActionResult Logout()
        {
            if (!this.IsSignedIn())
            {
                this.Model.Data["Error"] = "You cannot logout if you are not logged in.";
                return this.RedirectToAction("/");
            }
            this.SignOut();

            return this.RedirectToAction("/");
        }
    }
}
