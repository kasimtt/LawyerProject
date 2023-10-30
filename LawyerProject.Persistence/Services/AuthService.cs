using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using LawyerProject.Application.Abstractions.Services;
using LawyerProject.Application.Abstractions.Services.Authentication;
using LawyerProject.Application.Abstractions.Token;
using LawyerProject.Application.DTOs.TokenDtos;
using LawyerProject.Application.Exceptions;
using LawyerProject.Application.Features.Commands.AppUsers.LoginUser;
using LawyerProject.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LawyerProject.Persistence.Services
{
    public class AuthService : IAuthService
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
        }


        async Task<Token> CreateUserExternalAsync(AppUser user, string name, string email, string givenName, string familyName, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = user != null;
            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new AppUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = name,
                        FirstName = givenName,
                        LastName = familyName,
                    };
                    var IdentityResult = await _userManager.CreateAsync(user);
                    result = IdentityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(user, info);  //AspNetUserlogine kaydedilecek
            }
            else
                throw new InvalidExternalAuthentication();

            Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);

            return token;


        }

        public async Task<Token> GoogleLoginAsync(string idToken, string provider, int accessTokenLifeTime)
        {
            var setting = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { _configuration["ExternalLoginSetting:Google:Client_Id"] }
            };

            var payLoad = await GoogleJsonWebSignature.ValidateAsync(idToken, setting);
            var info = new UserLoginInfo(provider, payLoad.Subject, provider);
            AppUser user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            Token token = await CreateUserExternalAsync(user, payLoad.Name, payLoad.Email, payLoad.GivenName, payLoad.FamilyName, info, accessTokenLifeTime);

            return token;

        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userNameOrEmail);
            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (result.Succeeded) //authentication başarılı
            {
                Token token = _tokenHandler.CreateAccessToken(accessTokenLifeTime);
                return   token ;
            }
            throw new AuthenticationErrorException();
        }
    }
}
