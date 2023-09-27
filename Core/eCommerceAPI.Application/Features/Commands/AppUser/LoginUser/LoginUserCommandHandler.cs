using T = eCommerceAPI.Application.DTOs;
using eCommerceAPI.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F = eCommerceAPI.Domain.Entities.Identity;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.Abstractions.Services;

namespace eCommerceAPI.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly IAuthService _authService;

        public LoginUserCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            DTOs.Token response = await _authService.LoginAsync(request.UsernameOrEmail,request.Password,900);
            return new LoginUserCommandSuccessResponse()
            {
                Token = response,
            };
        }
    }
}
