using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.Abstractions.Token;
using eCommerceAPI.Application.DTOs;
using eCommerceAPI.Application.DTOs.Facebook;
using eCommerceAPI.Application.Features.Commands.AppUser.GoogleLogin;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eCommerceAPI.Application.Features.Commands.AppUser.FacebookLogin
{
    public class FacebookLoginCommandHandler : IRequestHandler<FacebookLoginCommandRequest, FacebookLoginCommandResponse>
    {

        private readonly IAuthService _authService;

        public FacebookLoginCommandHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<FacebookLoginCommandResponse> Handle(FacebookLoginCommandRequest request, CancellationToken cancellationToken)
        {
            DTOs.Token response = await _authService.FacebookLoginAsync(request.AuthToken,15);
            return new()
            {
                Token = response
            };
        }
    }
}
