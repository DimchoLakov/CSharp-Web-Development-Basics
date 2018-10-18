using SIS.Framework;
using SIS.Framework.Routers;
using SIS.WebServer;

namespace SIS.App
{
    public class Startup
    {
        public static void Main()
        {
            Server server = new Server(8000, new ControllerRouter());
            MvcEngine.Run(server);
        }
    }
}
