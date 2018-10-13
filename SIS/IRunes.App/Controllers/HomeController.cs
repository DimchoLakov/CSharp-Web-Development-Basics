using SIS.HTTP.Requests.Interfaces;
using SIS.HTTP.Responses.Interfaces;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index(IHttpRequest request)
        {
            return View("Index", request);
        }
    }
}
