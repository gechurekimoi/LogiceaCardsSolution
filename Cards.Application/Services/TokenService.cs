using Cards.Domain.DTOs;
using Cards.Domain.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Users.Domain.IRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Cards.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        private readonly ILogger<TokenService> _logger;

        public TokenService(IUserRepository userRepository,
            IConfiguration configuration,
            ILogger<TokenService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }


        public async Task<TokenResponse> GenerateToken(Guid Id)
        {
            try
            {
                TokenResponse tokenResponse = new TokenResponse();


                var userDetails = await _userRepository.GetUserById(Id);

                if (userDetails == null)
                    return null;

                string SigningKey = _configuration["PhoneAuthenticationToken:IssuerSigningKey"];
                string Issuer = _configuration["PhoneAuthenticationToken:ValidIssuer"];
                string Audience = _configuration["PhoneAuthenticationToken:ValidAudience"];

                List<Claim> claims = new List<Claim>();

                claims.Add(new Claim(type: "UserId", userDetails.Id.ToString()));
                claims.Add(new Claim(type: "Name", userDetails.Name.ToString()));
                claims.Add(new Claim(type: "Email", userDetails.Email.ToString()));
                claims.Add(new Claim(type: "Role", userDetails.UserRole.ToString()));




                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey));

                var signInCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(2),
                    signingCredentials: signInCredentials);

                var stringToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                if (string.IsNullOrEmpty(stringToken))
                    return null;

                var refreshToken = GenerateRefreshToken();

                userDetails.RefreshToken = refreshToken;

                var userDetails2  = await _userRepository.Update(userDetails, userDetails.Id);

                tokenResponse = new TokenResponse()
                {
                    AccessToken = stringToken,
                    RefreshToken = refreshToken
                };

                return tokenResponse;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public string GenerateRefreshToken()
        {
            try
            {
                var randomNumber = new byte[32];

                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);

                    return Convert.ToBase64String(randomNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token");
                return null;
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string SigningKey)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SigningKey)),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };

                var tokenHandler = new JwtSecurityTokenHandler();

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating refresh token");
                return null;
            }
        }

        public async Task<TokenResponse> RefreshToken(TokenResponse tokenResponse)
        {
            string signingKey = _configuration["PhoneAuthenticationToken:IssuerSigningKey"];

            var principal = GetPrincipalFromExpiredToken(tokenResponse.AccessToken, signingKey);

            var claims = principal.Claims.ToList();

            string UserId = claims.Where(p => p.Type == "UserId").FirstOrDefault().Value;

            if (string.IsNullOrEmpty(UserId))
                return null;


            return await GenerateToken(Guid.Parse(UserId));
        }
    }
}
