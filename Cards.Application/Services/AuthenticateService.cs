using Cards.Domain.DTOs;
using Cards.Domain.IServices;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.IRepository;

namespace Cards.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthenticateService(IUserRepository userRepository,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }


        public async Task<TokenResponse> AuthenticateUser(string Email,string Password)
        {
            var user = await _userRepository.GetUserByUserEmailAndPassword(Email, Password);

            if (user == null)
                return null;

            //we generate token
            var tokenResponse = await _tokenService.GenerateToken(user.Id);

            return tokenResponse;
        }

        public async Task<TokenResponse> RefreshUserToken(TokenResponse tokenResponse)
        {
            var result = await _tokenService.RefreshToken(tokenResponse);

            return result;
        }
    }
}
