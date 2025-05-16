using Core.Utilities.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class ServiceRegistration
    {
        public static void AddCoreRegistration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IFileHelperService, FileHelperManager>();
        }
    }
}
