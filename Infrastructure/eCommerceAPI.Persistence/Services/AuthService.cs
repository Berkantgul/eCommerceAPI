using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.DTOs;
using eCommerceAPI.Application.DTOs.Facebook;
using eCommerceAPI.Application.DTOs.User;
using eCommerceAPI.Application.Exceptions;
using eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using eCommerceAPI.Application.Features.Commands.AppUser.LoginUser;
using eCommerceAPI.Domain.Entities.Identity;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using F = eCommerceAPI.Domain.Entities.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using Microsoft.EntityFrameworkCore;

namespace eCommerceAPI.Persistence.Services
{
    internal class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly ITokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<F::AppUser> _signInManager;
        private readonly IUserService _userService;

        public AuthService(IHttpClientFactory httpClientFactory, UserManager<AppUser> userManager, ITokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
            _userService = userService;
        }

        async Task<Token> CreateExternalUserAsync(AppUser appUser, string email, string name, UserLoginInfo info, int accessTokenLifeTime)
        {
            bool result = appUser != null;
            if (appUser == null)
            {
                appUser = await _userManager.FindByEmailAsync(email);
                if (appUser == null)
                {
                    appUser = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = email,
                        UserName = email,
                        NameSurname = name
                    };
                    var identityResult = await _userManager.CreateAsync(appUser);
                    result = identityResult.Succeeded;
                }
            }
            if (result)
            {
                await _userManager.AddLoginAsync(appUser, info);
                Token token = _tokenHandler.CreateToken(900,appUser);
                await _userService.UpdateRefreshToken(token.RefreshToken, appUser, token.Expiration, 300);
                return token;

            }
            throw new Exception("Invalid External Login");
        }
        public async Task<Token> FacebookLoginAsync(string authToken, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:Client_ID"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:Client_Secret"]}&grant_type=client_credentials");


            // Json verisini Deserialize etmem gerekiyor.
            FacebookAccessTokenResponse_DTO? facebookAccessTokenResponse = JsonSerializer.Deserialize<FacebookAccessTokenResponse_DTO>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&  access_token={facebookAccessTokenResponse?.AccessTken}");

            FacebookUserAccessTokenValidation_DTO? validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidation_DTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid != null)
            {
                string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,name&access_token={authToken}");

                FacebookUserInfoResponse_DTO? facebookUserInfoResponse = JsonSerializer.Deserialize<FacebookUserInfoResponse_DTO>(userInfoResponse);

                UserLoginInfo userLoginInfo = new("FACEBOOK", validation.Data.UserId, "FACEBOOK");
                Domain.Entities.Identity.AppUser appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);

                // CreateExternalUserAsync
                return await CreateExternalUserAsync(appUser, facebookUserInfoResponse.Email, facebookUserInfoResponse.Name, userLoginInfo, accessTokenLifeTime);

            }
            throw new Exception("Invalid External authentication.");
        }

        public async Task<Token> GoogleLoginAsync(string idToken, int accessTokenLifeTime)
        {
            ValidationSettings? settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string>() { _configuration["ExternalLoginSettings:Google:GoogleClientId"] }
            };

            Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            UserLoginInfo userLoginInfo = new("GOOGLE", payload.Subject, "GOOGLE");
            Domain.Entities.Identity.AppUser appUser = await _userManager.FindByLoginAsync(userLoginInfo.LoginProvider, userLoginInfo.ProviderKey);
            return await CreateExternalUserAsync(appUser, payload.Email, payload.Email, userLoginInfo, accessTokenLifeTime);

        }

        public async Task<Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime)
        {
            F::AppUser user = await _userManager.FindByNameAsync(userNameOrEmail);
            if (user == null)
                user = await _userManager.FindByEmailAsync(userNameOrEmail);

            if (user == null)
                throw new NotFoundUserException();

            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                // Yetkilendirme 
                Token token = _tokenHandler.CreateToken(accessTokenLifeTime,user);
                // Refresh Token
                await _userService.UpdateRefreshToken(token.RefreshToken, user, token.Expiration, 300);
                return token;

            }
            else
                throw new NotFailLoginException();
        }

        public async Task<Token> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                Token token = _tokenHandler.CreateToken(900,user);
                await _userService.UpdateRefreshToken(token.RefreshToken,user,token.Expiration, 300);
                return token;
            }
            else
                throw new NotFoundUserException();

        }
    }
}

