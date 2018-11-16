using SIS.Framework.Controllers;

namespace IRunes.App.Controllers
{
    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
        }

        protected string Error { get; set; }

        protected void ShowAppropriateButtonsBasedOnLoggedIn()
        {
            if (this.IsSignedIn())
            {
                this.Model.Data["Authenticated"] = "inline";
                this.Model.Data["NotAuthenticated"] = "none";
                this.Model.Data["showError"] = "none";
                this.Model.Data["errorMsg"] = "none";
            }
            else
            {
                this.Model.Data["Authenticated"] = "none";
                this.Model.Data["NotAuthenticated"] = "inline";
                this.Model.Data["showError"] = "none";
                this.Model.Data["errorMsg"] = "none";
            }

            if (this.Error != null)
            {
                this.Model.Data["showError"] = "inline";
                this.Model.Data["errorMsg"] = this.Error;
            }
        }

        protected bool IsSignedIn()
        {
            return this.Identity != null;
        }
    }
}
