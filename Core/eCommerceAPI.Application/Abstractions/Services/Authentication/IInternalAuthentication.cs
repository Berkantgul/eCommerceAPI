using eCommerceAPI.Application.DTOs;
using eCommerceAPI.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Abstractions.Services.Authentication
{
    public interface IInternalAuthentication
    {
        Task<DTOs.Token> LoginAsync(string userNameOrEmail, string password, int accessTokenLifeTime);
        Task<DTOs.Token> RefreshTokenLoginAsync(string refreshToken);
    }
}
