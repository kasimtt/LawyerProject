using AutoMapper;
using LawyerProject.Application.Features.Commands.Adverts.CreateAdvert;
using LawyerProject.Application.Features.Commands.AppUsers.CreateUser;
using LawyerProject.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Mappers
{
    public class AppUserProfile: Profile
    {
        public AppUserProfile()
        {
            CreateMap<CreateUserCommadRequest, AppUser>();      // Password özelliğini atla
            CreateMap<AppUser, CreateUserCommadRequest>();
            
            // İki yönlü eşleme için ReverseMap() kullanmayı unutmayın
          

        }
    }
}
