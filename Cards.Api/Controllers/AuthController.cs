using Cards.Api.Models;
using Cards.Domain.DTOs;
using Cards.Domain.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cards.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthenticateService _authenticateService;

        public AuthController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginUser(LoginDTO login)
        {
            var response = await _authenticateService.AuthenticateUser(login.Email, login.Password);

            if (response == null)
                return Unauthorized("User not found");

            return Ok(response);
        }

        [HttpPost]
        [Route("RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshUserToken(TokenResponse token)
        {
            var response = _authenticateService.RefreshUserToken(token);

            if (response == null)
                return Unauthorized("User not found");

            return Ok(response);
        }

    }
}
