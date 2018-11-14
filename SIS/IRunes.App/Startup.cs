using IRunes.App.Services;
using IRunes.App.Services.Interfaces;
using SIS.Framework.Api;
using SIS.Framework.Services;

namespace IRunes.App
{
    public class Startup : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<IHashService, HashService>();
            dependencyContainer.RegisterDependency<IUserCookieService, UserCookieService>();
            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<IAlbumService, AlbumService>();
            dependencyContainer.RegisterDependency<ITrackService, TrackService>();
        }
    }
}
