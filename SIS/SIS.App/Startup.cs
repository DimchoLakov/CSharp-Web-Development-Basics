using SIS.Framework;
using SIS.Framework.Routers;
using SIS.Framework.Services;
using SIS.WebServer;

namespace SIS.App
{
    public class Startup
    {
        public static void Main()
        {
            var dependencyContainer = new DependencyContainer();

            var controllerRouter = new ControllerRouter(dependencyContainer);
            var handlersContext = new HttpRouterHandlingContext(controllerRouter, new ResourceRouter());
            Server server = new Server(80, handlersContext);
            MvcEngine.Run(server);
        }
    }
}
