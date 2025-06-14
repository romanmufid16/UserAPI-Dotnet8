using UserAPI_Dotnet8.Entities;

namespace UserAPI_Dotnet8.DTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public static class UserMapper
    {
        public static UserResponseDTO MapToDto(User user) => new UserResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
        };
    }
}

