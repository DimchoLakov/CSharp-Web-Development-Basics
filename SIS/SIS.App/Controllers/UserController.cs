using SIS.Framework.ActionResults;
using SIS.Framework.Attributes.Action;
using SIS.Framework.Controllers;
using SIS.Framework.Security;

namespace SIS.App.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            this.SignIn(new IdentityUser() { Username = "Pesho", Password = "123" });
            return this.View();
        }

        [Authorize]
        public IActionResult Authorized()
        {
            if (this.Identity != null)
            {
                this.Model["username"] = this.Identity.Username;
            }
            return this.View();
        }
    }
}
