using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using eCommerceAPI.Application.Abstractions.Services;
using eCommerceAPI.Application.DTOs.User;

namespace eCommerceAPI.Application.Features.Commands.AppUser.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommandRequest, CreateUserCommandResponse>
    {

        private IUserService _userService;

        public CreateUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<CreateUserCommandResponse> Handle(CreateUserCommandRequest request, CancellationToken cancellationToken)
        {
            // Burada sadece ilgili fonksiyonu çağırmam gerekiyor. Harici teknik detaylara inmemem gerekiyor.
            CreateUserResponse response= await _userService.CreateAsync(new()
            {
                Email = request.Email,
                NameSurname = request.NameSurname,
                UserName = request.UserName,
                CheckPassword = request.CheckPassword,
                Password = request.Password
            });

            return new()
            {
                Message = response.Message,
                Succceded = response.Succceded
            };
        }
    }
}
