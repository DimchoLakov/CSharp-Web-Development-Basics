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
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] = request => new UserController().ShowAlbums(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request => new UserController().CreateAlbum(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => new UserController().AlbumDetails(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => new UserController().CreateTrack(request);
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/logout"] = request => new UserController().Logout(request);

            //// POST Requests

            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UserController().DoLogin(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UserController().DoRegister(request);
            serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request => new UserController().DoCreateAlbum(request);

            var server = new Server(80, serverRoutingTable);
            server.Run();
        }
    }
}
