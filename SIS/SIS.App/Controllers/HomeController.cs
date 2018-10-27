using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Controllers;
using SIS.Framework.Security.Interfaces;

namespace SIS.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            this.SignIn(new IdentityUser() { Username = "Pesho", Password = "123" });
            this.Model["username"] = this.Identity.Username;
            return View();
        }
    }
}
