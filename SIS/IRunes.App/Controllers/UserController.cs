using IRunes.App.Services.Interfaces;
using IRunes.App.ViewModels;
using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Method;
using SIS.Framework.Security;
using SIS.HTTP.Cookies;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        private const string AuthKey = ".auth-IRunes";
        private const int CookieExpiryDays = 7;

        private readonly IHashService hashService;
        private readonly IUserService userService;

        public UserController(IHashService hashService, IUserService userService)
        {
            this.hashService = hashService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }
            return this.View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(loginViewModel.Password);

            var user = this.userService.GetUser(loginViewModel.Username, hashedPassword);

            if (user == null)
            {
                this.Error = $"Wrong username or password.";
                return this.Login();
            }

            this.SignIn(new IdentityUser()
            {
                Username = user.Username,
                Password = hashedPassword,
                Email = user.Email
            });

            //var userCookie = this.userCookieService.GetUserCookie(user.Username);
            //this.Request.Cookies.Add(new HttpCookie(AuthKey, userCookie, CookieExpiryDays));
            //this.Cookies.Add(new HttpCookie(AuthKey, userCookie, CookieExpiryDays));

            return this.RedirectToAction("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (this.IsSignedIn())
            {
                return this.RedirectToAction("/");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                this.Error = $"Passwords do not match.";
                return this.Register();
            }

            var hashedPassword = this.hashService.ComputeSha256Hash(registerViewModel.Password);

            var user = this.userService.GetUser(registerViewModel.Username, hashedPassword);
            
            if (user != null)
            {
                this.Error = $"User with username {registerViewModel.Username} already exists.";
                return this.Register();
            }

            registerViewModel.Password = hashedPassword;
            registerViewModel.ConfirmPassword = hashedPassword;

            this.userService.AddUser(registerViewModel);
            
            return this.Login(new LoginViewModel());
        }

        public IActionResult Logout()
        {
            if (!this.IsSignedIn())
            {
                this.Error = "You cannot logout if you are not logged in.";
                return this.RedirectToAction("/");
            }
            this.SignOut();

            //var cookieExists = this.Request.Cookies.ContainsCookie(AuthKey);
            //if (cookieExists)
            //{
            //    var cookie = this.Request.Cookies.GetCookie(AuthKey);
            //    cookie.Delete();
            //    this.Cookies.Add(cookie);
            //}

            return this.RedirectToAction("/");
        }
    }
}
