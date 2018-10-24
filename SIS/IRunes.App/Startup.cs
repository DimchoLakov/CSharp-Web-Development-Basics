using IRunes.App.Controllers;
using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.HTTP.Enums;
using SIS.WebServer;
using SIS.WebServer.Api;
using SIS.WebServer.Routing;

namespace IRunes.App
{
    public class Startup
    {
        public static void Main()
        {
            // TODO: Modify IRunes.App to run correctly with SIS.Framework

            ///////////////// NOT WORKING CORRECTLY AT THE MOMENT!


            //var serverRoutingTable = new ServerRoutingTable();

            ////// GET Requests

            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/home/index"] = request => new HomeController().Index(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/login"] = request => new UserController().Login(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/logout"] = request => new UserController().Logout(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/users/register"] = request => new UserController().Register(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/all"] = request => new AlbumController().ShowAlbums(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/create"] = request => new AlbumController().CreateAlbum(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/albums/details"] = request => new AlbumController().AlbumDetails(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/create"] = request => new AlbumController().CreateTrack(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Get]["/tracks/details"] = request => new AlbumController().TrackDetails(request);

            ////// POST Requests

            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/login"] = request => new UserController().DoLogin(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/users/register"] = request => new UserController().DoRegister(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/albums/create"] = request => new AlbumController().DoCreateAlbum(request);
            //serverRoutingTable.Routes[HttpRequestMethod.Post]["/tracks/create"] = request => new AlbumController().DoCreateTrack(request);


            var dependencyContainer = new DependencyContainer();
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();

            var controllerRouter = new ControllerRouter(dependencyContainer);
            var handlersContext = new HttpRouterHandlingContext(controllerRouter, new ResourceRouter());
            var server = new Server(80, handlersContext);
            MvcEngine.Run(server);
        }
    }
}
