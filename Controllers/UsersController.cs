using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] RegisterUserDTO dto) {
            var user = await _service.RegisterAsync(dto);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO?>> GetUserById(int id)
        {
            var user = await _service.GetByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }
    }
}
