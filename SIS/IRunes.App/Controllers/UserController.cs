using SIS.HTTP.Requests.Interfaces;
using SIS.HTTP.Responses.Interfaces;

namespace IRunes.App.Controllers
{
    public class UserController : BaseController
    {
        public IHttpResponse Login(IHttpRequest request)
        {
            return View("Login", request);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return View("Register", request);
        }
    }
}
