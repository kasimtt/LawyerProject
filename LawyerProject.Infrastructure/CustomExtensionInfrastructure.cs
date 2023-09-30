using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Infrastructure.Services.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace LawyerProject.Infrastructure
{
    public static class CustomExtensionInfrastructure
    {
        public static void AddContainerWithDependenciesInfrastucture(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
        }

        public static void AddStorage<T> (this IServiceCollection services) where T : class, IStorage 
        {
          services.AddScoped<IStorage,T>();
        }
    }
}
