using AutoMapper;
using LawyerProject.Application.Exceptions;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Commands.AppUsers.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommadRequest, CreateUserCommandResponse>
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<CreateUserCommandResponse> Handle(CreateUserCommadRequest request, CancellationToken cancellationToken)
        {
            AppUser appUser = _mapper.Map<AppUser>(request);
            appUser.Id = Guid.NewGuid().ToString();
            
            

            IdentityResult result = await _userManager.CreateAsync(appUser,request.Password); ;

            return new CreateUserCommandResponse
            {
                Success = result.Succeeded,
                Message = "şimdilik boş kalsın sonra bakarız"
            };

         




            //   throw new UserCreateFailedException();




        }
    }
}
