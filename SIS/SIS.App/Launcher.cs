using SIS.Framework;

namespace SIS.App
{
    public class Launcher
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
