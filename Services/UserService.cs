using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserAPI_Dotnet8.Data;
using UserAPI_Dotnet8.DTO;
using UserAPI_Dotnet8.Entities;

namespace UserAPI_Dotnet8.Services
{
    public class UserService: IUserService
    {
        private readonly DataContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(DataContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserResponseDTO> RegisterAsync(RegisterUserDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                throw new BadHttpRequestException("Email already in use");
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
            if (user == null) throw new BadHttpRequestException("User not found", 400);

            return UserMapper.MapToDto(user);
        }
    }
}
