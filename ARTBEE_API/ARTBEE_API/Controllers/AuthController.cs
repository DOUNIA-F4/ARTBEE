using ARTBEE_API.Interfaces;
using ARTBEE_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARTBEE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> Register([FromForm] SignUp signUp, IFormFile? image, string role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auth.SignUp(signUp, image, role);

            if (!result.IsAuthenticated)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _auth.Login(login);
            if (!result.IsAuthenticated)
            {
                return BadRequest("something went wrong");
            }

            return Ok(result.Token);


        }
    }
}
