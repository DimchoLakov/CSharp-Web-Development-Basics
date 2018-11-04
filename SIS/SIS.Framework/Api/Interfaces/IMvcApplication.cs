using SIS.Framework.Services;

namespace SIS.Framework.Api.Interfaces
{
    public interface IMvcApplication
    {
        void Configure();

        void ConfigureServices(IDependencyContainer dependencyContainer);
    }
}
