using ByTheCake.App.Controllers;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace ByTheCake.App
{
    public class Launcher
    {
        public static void Main()
        {
            ServerRoutingTable serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/index"] = request => new HomeController().Index(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/register"] = request => new AccountController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/hello"] = request => new HomeController().Hello(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/profile"] = request => new AccountController().Profile(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/add"] = request => new CakeController().AddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/add"] = request => new CakeController().DoAddCake(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/search"] = request => new CakeController().Search(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/cakeById"] = request => new CakeController().CakeById(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/cakeById"] = request => new CakeController().CakeById(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/orders"] = request => new AccountController().ShowOrders(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/orderDetails"] = request => new CakeController().OrderDetails(request);
            
            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
