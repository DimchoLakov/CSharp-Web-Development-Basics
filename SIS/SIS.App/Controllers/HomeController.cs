using SIS.Framework.ActionResults.Interfaces;
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
