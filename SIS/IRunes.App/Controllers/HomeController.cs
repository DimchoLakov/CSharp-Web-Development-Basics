using SIS.Framework.ActionResults;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            this.ShowAppropriateButtonsBasedOnLoggedIn();
            if (this.IsSignedIn())
            {
                this.Model.Data["Username"] = this.Identity.Username;
            }
            return View();
        }
    }
}
