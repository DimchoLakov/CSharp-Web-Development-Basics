using IRunes.Data;
using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string AuthKey = ".auth-IRunes";
        private const int CookieExpiryDays = 7;

        protected BaseController()
        {
            this.DbContext = new IRunesDbContext();
        }

        protected IRunesDbContext DbContext { get; }

        protected void ShowAppropriateButtonsBasedOnLoggedIn()
        {
            if (this.IsSignedIn())
            {
                this.Model.Data["Authenticated"] = "inline";
                this.Model.Data["NotAuthenticated"] = "none";
            }
            else
            {
                this.Model.Data["Authenticated"] = "none";
                this.Model.Data["NotAuthenticated"] = "inline";
            }
        }
    }
}
