using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserAPI_Dotnet8.DTO;
using UserAPI_Dotnet8.Services;

namespace UserAPI_Dotnet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;
        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] RegisterUserDTO dto)
        {
            var user = await _service.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginUserDTO dto)
        {
            var token = await _service.LoginAsync(dto);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<ActionResult<UserResponseDTO>> GetMyUser()
        {
            // Ambil User ID dari JWT Claim
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            var user = await _service.GetByIdAsync(userId);
            return Ok(user);
        }
    }
}
