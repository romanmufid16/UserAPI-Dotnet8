using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAPI_Dotnet8.Data;
using UserAPI_Dotnet8.DTO;
using UserAPI_Dotnet8.Entities;

namespace UserAPI_Dotnet8.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _config;

        public UserService(DataContext context, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public async Task<UserResponseDTO> RegisterAsync(RegisterUserDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new ApplicationException("Email already in use");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
            };

            user.HashedPassword = _passwordHasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return UserMapper.MapToDto(user);
        }

        public async Task<UserResponseDTO?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) throw new KeyNotFoundException("User not found");

            return UserMapper.MapToDto(user);
        }

        public async Task<string> LoginAsync(LoginUserDTO dto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Email == dto.Email);
            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password) != PasswordVerificationResult.Success)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds,
                audience: null
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
