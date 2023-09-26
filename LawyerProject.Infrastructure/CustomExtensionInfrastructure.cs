using LawyerProject.Application.Services;
using LawyerProject.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Infrastructure
{
    public static class CustomExtensionInfrastructure
    {
        public static void AddContainerWithDependenciesInfrastucture(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }
    }
}
