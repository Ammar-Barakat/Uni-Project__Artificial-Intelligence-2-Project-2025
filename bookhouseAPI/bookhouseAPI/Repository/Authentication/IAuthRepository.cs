using bookhouseAPI.DTOs.Authentication;

namespace bookhouseAPI.Repository.Authentication
{
    public interface IAuthRepository
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthDTO> LoginAsync(LoginDTO dto);
    }
}
