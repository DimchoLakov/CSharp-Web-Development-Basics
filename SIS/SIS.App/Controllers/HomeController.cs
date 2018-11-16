using SIS.Framework.ActionResults;
using SIS.Framework.Controllers;

namespace SIS.App.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
