using SIS.Framework;

namespace IRunes.App
{
    public class Launcher
    {
        public static void Main(string[] args)
        {
            WebHost.Start(new Startup());
        }
    }
}
