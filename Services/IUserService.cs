using UserAPI_Dotnet8.DTO;

namespace UserAPI_Dotnet8.Services
{
    public interface IUserService
    {
        Task<UserResponseDTO> RegisterAsync(RegisterUserDTO dto);

        Task<UserResponseDTO?> GetByIdAsync(int id);

        Task<string> LoginAsync(LoginUserDTO dto);
    }
}
