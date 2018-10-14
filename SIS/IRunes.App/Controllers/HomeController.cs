using System.Collections.Generic;
using IRunes.Data;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;

namespace IRunes.App.Controllers
{
    public class HomeController : BaseController
    {
        private IRunesDbContext dbContext;

        public HomeController()
        {
            this.dbContext = new IRunesDbContext();
        }

        public IHttpResponse Index(IHttpRequest request)
        {
            var viewBag = new Dictionary<string, string>();
            if (this.IsAuthenticated(request))
            {
                var username = request.Session.GetParameter("username");
                viewBag.Add("Username", username.ToString());
            }
            return View("Index", request, viewBag);
        }
    }
}
