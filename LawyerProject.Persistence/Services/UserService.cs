using AutoMapper;
using LawyerProject.Application.Abstractions.Services;
using LawyerProject.Application.DTOs.UserDtos;
using LawyerProject.Application.Exceptions;
using LawyerProject.Application.Features.Commands.AppUsers.CreateUser;
using LawyerProject.Application.Helpers;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Persistence.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<CreateUserResponseDto> CreateAsync(CreateUserDto createUserDto)
        {
            AppUser appUser = _mapper.Map<AppUser>(createUserDto);
            appUser.Id = Guid.NewGuid().ToString();



            IdentityResult result = await _userManager.CreateAsync(appUser, createUserDto.Password);

            CreateUserResponseDto dto = new CreateUserResponseDto();
            dto.Success = result.Succeeded;
            if (result.Succeeded)
            {
                dto.Message = "başarıyla kayıt yapılmıştır";
            }
            else
                foreach (var error in result.Errors)
                {
                    dto.Message += $"{error.Code} - {error.Description}\n";
                }

            return dto;




        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate, int addOnAccessTokenDate)
        {

            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenDate.AddMinutes(addOnAccessTokenDate);
                await _userManager.UpdateAsync(user);
            }
            else
                throw new NotFoundUserException();
        }

        public async Task<GetUserDto> GetUserByUserNameAsync(string userNameOrEmail)
        {
            AppUser User = await _userManager.FindByNameAsync(userNameOrEmail);
            if (User == null)
                User = await _userManager.FindByEmailAsync(userNameOrEmail);
            if (User == null)
                throw new NotFoundUserException();

            GetUserDto dto = _mapper.Map<GetUserDto>(User);

            return dto;



        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);
                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user);
                else
                    throw new PasswordChangeFailedException();
            }
        }
    }
}
