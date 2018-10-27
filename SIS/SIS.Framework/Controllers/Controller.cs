using System.Runtime.CompilerServices;
using SIS.Framework.ActionResults;
using SIS.Framework.ActionResults.Interfaces;
using SIS.Framework.Models;
using SIS.Framework.Security.Interfaces;
using SIS.Framework.Utilities;
using SIS.Framework.Views;
using SIS.HTTP.Requests;

namespace SIS.Framework.Controllers
{
    public abstract class Controller
    {
        private const string SessionParameterAuth = "auth";

        protected Controller()
        {
            this.Model = new ViewModel();
            this.ModelState = new Model();
        }

        public ViewModel Model { get; set; }

        public IHttpRequest Request { get; set; }

        public Model ModelState { get; set; }

        public IIdentity Identity => (IIdentity)this.Request.Session.GetParameter(SessionParameterAuth);

        protected IViewable View([CallerMemberName] string caller = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);

            var fullyQualifiedName = ControllerUtilities.GetViewFullQualifiedName(controllerName, caller);

            var view = new View(fullyQualifiedName, Model.Data); // TODO DOUBLE CHECK!!!!

            return new ViewResult(view);
        }

        protected IRedirectable RedirectToAction(string redirectUrl)
        {
            return new RedirectResult(redirectUrl);
        }

        public void SignIn(IIdentity auth)
        {
            this.Request.Session.AddParameter(SessionParameterAuth, auth);
        }

        public void SignOut()
        {
            this.Request.Session.ClearParameters();
        }
    }
}
