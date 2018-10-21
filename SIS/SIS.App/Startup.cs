using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;

namespace SIS.App
{
    public class Startup
    {
        public static void Main()
        {
            var controllerRouter = new ControllerRouter();
            var resourceRouter = new ResourceRouter();
            var handlersContext = new HttpRouterHandlingContext(controllerRouter, resourceRouter);
            Server server = new Server(8000, handlersContext);
            MvcEngine.Run(server);
        }
    }
}
