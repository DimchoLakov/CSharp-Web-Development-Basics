using IRunes.App.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace IRunes.App
{
    public class Startup
    {
        public static void Main()
        {
            var serverRoutingTable = new ServerRoutingTable();

            //// GET Requests

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UserController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UserController().Register(request);

            //// POST Requests

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}
