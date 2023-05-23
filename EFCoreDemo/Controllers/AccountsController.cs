using EFCoreDemo.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly JwtHelpers jwt;

        public AccountsController(JwtHelpers jwt)
        {
            this.jwt = jwt;
        }

        [HttpPost("~/login", Name = nameof(Login))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [AllowAnonymous]
        public IActionResult Login(LoginDto login)
        {
            if (!ValidateLogin(login))
            {
                return BadRequest();
            }

            var token = jwt.GenerateToken(login.Username, 10);

            return Ok(new
            {
                token
            });
        }

        private bool ValidateLogin(LoginDto login)
        {
            return login.Username == "will";
        }
    }
}
