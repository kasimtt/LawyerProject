using LawyerProject.Application.Abstractions.Storage;
using LawyerProject.Application.Abstractions.Token;
using LawyerProject.Infrastructure.Services.Storage;
using T=  LawyerProject.Infrastructure.Services.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LawyerProject.Infrastructure
{
    public static class CustomExtensionInfrastructure
    {
        public static void AddContainerWithDependenciesInfrastucture(this IServiceCollection services)
        {
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ITokenHandler, T.TokenHandler>();
        }

        public static void AddStorage<T> (this IServiceCollection services) where T : BaseStorage, IStorage 
        {
          services.AddScoped<IStorage,T>();
        }
    }
}
