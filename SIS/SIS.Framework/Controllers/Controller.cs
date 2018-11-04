using System;
using System.IO;
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

        public IIdentity Identity
            => (IIdentity)this.Request.Session.GetParameter(SessionParameterAuth);

        private ViewEngine ViewEngine { get; } = new ViewEngine();

        protected IViewable View([CallerMemberName] string actionName = "")
        {
            var controllerName = ControllerUtilities.GetControllerName(this);
            string viewContent = null;

            try
            {
                viewContent = this.ViewEngine.GetViewContent(controllerName, actionName);
            }
            catch (FileNotFoundException e)
            {
                this.Model.Data["Error"] = e.Message;

                viewContent = this.ViewEngine.GetErrorContent();
            }

            var renderedContent = this.ViewEngine.RenderHtml(viewContent, this.Model.Data);
            return new ViewResult(new View(renderedContent));
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

        public bool IsSignedIn()
        {
            return this.Identity != null;
        }
    }
}
