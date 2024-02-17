using Cards.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.IServices
{
    public interface IAuthenticateService
    {
        Task<TokenResponse> AuthenticateUser(string Email, string Password);
        Task<TokenResponse> RefreshUserToken(TokenResponse tokenResponse);
    }
}
