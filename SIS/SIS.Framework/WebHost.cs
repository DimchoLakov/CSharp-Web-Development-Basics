using SIS.Framework.Api.Interfaces;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.WebServer;
using SIS.WebServer.Api.Interfaces;

namespace SIS.Framework
{
    public static class WebHost
    {
        private const int HostingPort = 80;

        public static void Start(IMvcApplication application)
        {
            IDependencyContainer container = new DependencyContainer();
            application.ConfigureServices(container);

            IHttpHandlingContext controllerRouter = new HttpRouterHandlingContext(new ControllerRouter(container), new ResourceRouter());

            application.Configure();

            Server server = new Server(HostingPort, controllerRouter);
            MvcEngine.Run(server);
        }
    }
}
