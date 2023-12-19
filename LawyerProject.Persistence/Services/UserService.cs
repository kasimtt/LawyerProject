using AutoMapper;
using LawyerProject.Application.Abstractions.Services;
using LawyerProject.Application.DTOs.AdvertsDtos;
using LawyerProject.Application.DTOs.CasesDtos;
using LawyerProject.Application.DTOs.UserDtos;
using LawyerProject.Application.Exceptions;
using LawyerProject.Application.Features.Commands.AppUsers.CreateUser;
using LawyerProject.Application.Helpers;
using LawyerProject.Application.Repositories.AdvertRepositories;
using LawyerProject.Application.Repositories.CaseRepositories;
using LawyerProject.Domain.Entities;
using LawyerProject.Domain.Entities.Identity;
using LawyerProject.Persistence.Context;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICaseReadRepository _caseReadRepository;
        private readonly IAdvertReadRepository _advertRepository;

        public UserService(UserManager<AppUser> userManager, IMapper mapper, LawyerProjectContext context, ICaseReadRepository caseReadRepository, IAdvertReadRepository advertRepository)
        {
            _userManager = userManager;
            _mapper = mapper;
            _caseReadRepository = caseReadRepository;
            _advertRepository = advertRepository;
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

        public async Task<GetUserDetailsDto> GetUserDetailsAsync(string userNameOrEmail)
        {



            AppUser User = await _userManager.FindByNameAsync(userNameOrEmail);
            if (User == null)
                User = await _userManager.FindByEmailAsync(userNameOrEmail);
            if (User == null)
                throw new NotFoundUserException();

            IEnumerable<Case> _cases = _caseReadRepository.GetWhere(c => c.IdUserFK == User.Id).ToList();
            IEnumerable<Advert> _adverts = _advertRepository.GetWhere(c => c.IdUserFK == User.Id).ToList();

            IEnumerable<GetAdvertDtoWithoutUser> advertDto = _mapper.Map<IEnumerable<Advert>, IEnumerable<GetAdvertDtoWithoutUser>>(_adverts).ToList();
            IEnumerable<GetCaseDto> caseDto = _mapper.Map<IEnumerable<Case>, IEnumerable<GetCaseDto>>(_cases).ToList();




            GetUserDetailsDto dto = _mapper.Map<GetUserDetailsDto>(User);
            dto.Cases = caseDto;
            dto.Adverties = advertDto;

            return dto;
        }

        public async Task<List<ListUser>> GetAllUsersAsync(int page, int size)
        {
            var users = await _userManager.Users
                .Skip(page * size)
                .Take(size)
                .ToListAsync();

            return users.Select(user => new ListUser
            {
                Id = user.Id,
                Email = user.Email,
                NameSurname = user.FirstName + user.LastName,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            }).ToList();
        }

        public int TotalUsersCount => _userManager.Users.Count();
        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, roles);
            }
        }

        public async Task<string[]> GetRolesToUserAsync(string userId)
        {
            AppUser user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.ToArray();
            }
            return new string[] { };
        }
    }
}
