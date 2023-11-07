﻿using LawyerProject.Application.DTOs.UserDtos;
using LawyerProject.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LawyerProject.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<CreateUserResponseDto> CreateAsync(CreateUserDto createUserDto );
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenDate,int addOnAccessTokenDate);
    }
}