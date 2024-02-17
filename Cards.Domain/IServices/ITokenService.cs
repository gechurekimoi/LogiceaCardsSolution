using Cards.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cards.Domain.IServices
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateToken(Guid Id);
        Task<TokenResponse> RefreshToken(TokenResponse tokenResponse);
    }
}
