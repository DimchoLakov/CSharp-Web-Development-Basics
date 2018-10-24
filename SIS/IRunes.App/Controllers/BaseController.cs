using System.Collections.Generic;
using System.IO;
using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using SIS.Framework.Controllers;
using SIS.HTTP.Cookies;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;

namespace IRunes.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private const string AuthKey = ".auth-IRunes";
        private const int CookieExpiryDays = 7;

        protected BaseController()
        {
            this.UserCookieService = new UserCookieService();
        }

        protected IUserCookieService UserCookieService { get; }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(AuthKey))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(AuthKey);
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected IHttpResponse View(string viewName, IHttpRequest request, Dictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }

            var allContent = GetViewContent(viewName, request, viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError(string errorMessage, IHttpRequest request)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", request, viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage, IHttpRequest request)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", request, viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.InternalServerError);
        }

        protected string GetViewContent(string viewName, IHttpRequest request, Dictionary<string, string> viewBag)
        {
            var layout = string.Empty;
            var content = string.Empty;

            if (this.IsAuthenticated(request))
            {
                layout = File.ReadAllText("../../../Views/LoggedIn/_Layout.html");
                content = File.ReadAllText("../../../Views/LoggedIn/" + viewName + ".html");
            }
            else
            {
                layout = File.ReadAllText("../../../Views/LoggedOut/_Layout.html");
                content = File.ReadAllText("../../../Views/LoggedOut/" + viewName + ".html");
            }

            foreach (var item in viewBag)
            {
                content = content.Replace($"@Model.{item.Key}", item.Value.ToString());
            }

            var allContent = layout.Replace("@RenderBody()", content);
            return allContent;
        }

        public void SignInUser(string username, IHttpRequest request, IHttpResponse response)
        {
            request.Session.AddParameter("username", username);
            var userCookieValue = this.UserCookieService.GetUserCookie(username);
            response.Cookies.Add(new HttpCookie(AuthKey, userCookieValue, CookieExpiryDays));
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            //return request.Session.ContainsParameter("username");

            return request.Cookies.ContainsCookie(AuthKey);
        }

        public string GetUsernameFromSession(IHttpRequest request)
        {
            return request.Session.GetParameter("username").ToString();
        }

        public void LogoutUser(IHttpRequest request, IHttpResponse response)
        {
            request.Session.ClearParameters();

            var cookie = request.Cookies.GetCookie(AuthKey);
            cookie.Delete();
            response.AddCookie(cookie);
        }
    }
}
