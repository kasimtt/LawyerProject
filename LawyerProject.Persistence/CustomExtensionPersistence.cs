using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Application.Repositories.CasePdfFileRepositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Application.Repositories.FileRepositories;
using LawyerProject.Application.Repositories.UserActivityRepositories;
using LawyerProject.Application.Repositories.UserImageFileRepositories;
using LawyerProject.Application.Repositories.UserRepositories;
using LawyerProject.Domain.Entities;
using LawyerProject.Persistence.Repositories.AdvertRepositories;
using LawyerProject.Persistence.Repositories.CasePdfFileRepositories;
using LawyerProject.Persistence.Repositories.CaseRepositories;
using LawyerProject.Persistence.Repositories.FileRepositories;
using LawyerProject.Persistence.Repositories.UserActivityRepositories;
using LawyerProject.Persistence.Repositories.UserImageFileRepositories;
using LawyerProject.Persistence.Repositories.UserRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Persistence
{
    public static class CustomExtensionPersistence
    {
        public static void AddContainerWithDependenciesPersistence(this IServiceCollection services)
        {

            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();

            services.AddScoped<IAdvertReadRepository, AdvertReadRepository>();
            services.AddScoped<IAdvertWriteRepository, AdvertWriteRepository>();

            services.AddScoped<IUserActivityReadRepository, UserActivityReadRepository>();
            services.AddScoped<IUserActivityWriteRepository, UserActivityWriteRepository>();

            services.AddScoped<ICaseReadRepository,CaseReadRepository>();
            services.AddScoped<ICaseWriteRepository,CaseWriteRepository>();

            services.AddScoped<IFileReadRepository, FileReadRepository>();
            services.AddScoped<IFileWriteRepository, FileWriteRepository>();

            services.AddScoped<ICasePdfFileReadRepository, CasePdfFileReadRepository>();
            services.AddScoped<ICasePdfFileWriteRepository, CasePdfFileWriteRepository>();

            services.AddScoped<IUserImageFileReadRepository, UserImageFileReadRepository>();
            services.AddScoped<IUserImageFileWriteRepository, UserImageFileWriteRepository>();
        }
    }
}
