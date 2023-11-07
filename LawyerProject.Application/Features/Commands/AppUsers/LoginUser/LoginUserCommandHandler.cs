using LawyerProject.Application.Abstractions.Services;
using LawyerProject.Application.Abstractions.Token;
using LawyerProject.Application.DTOs.TokenDtos;
using LawyerProject.Application.Exceptions;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Features.Commands.AppUsers.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {

           Token token = await _authService.LoginAsync(request.UserNameOrEmail, request.Password, 10);


            return new LoginUserCommandResponse { Token  = token };
            

        }
    }
}
