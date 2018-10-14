using System.Collections.Generic;
using System.IO;
using ByTheCake.App.Services;
using ByTheCake.App.Services.Interfaces;
using SIS.HTTP.Enums;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer.Results;

namespace ByTheCake.App.Controllers
{
    public abstract class BaseController
    {
        protected BaseController()
        {
            this.UserCookieService = new UserCookieService();
        }

        protected IUserCookieService UserCookieService { get; }

        protected IHttpResponse View(string viewName, Dictionary<string, string> viewBag = null)
        {
            if (viewBag == null)
            {
                viewBag = new Dictionary<string, string>();
            }

            var allContent = GetViewContent(viewName, viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.BadRequest);
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            var viewBag = new Dictionary<string, string>();
            viewBag.Add("Error", errorMessage);
            var allContent = this.GetViewContent("Error", viewBag);

            return new HtmlResult(allContent, HttpResponseStatusCode.InternalServerError);
        }

        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return null;
            }

            var cookie = request.Cookies.GetCookie(".auth-cakes");
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected string GetViewContent(string viewName, Dictionary<string, string> viewBag)
        {
            var layout = File.ReadAllText("Views/_Layout.html");
            var content = File.ReadAllText("Views/" + viewName + ".html");
            foreach (var item in viewBag)
            {
                content = content.Replace($"@Model.{item.Key}", item.Value.ToString());
            }

            var allContent = layout.Replace("@RenderBody()", content);
            return allContent;
        }
    }
}
