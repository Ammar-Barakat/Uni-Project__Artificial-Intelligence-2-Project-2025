using bookhouseAPI.DTOs.Authentication;

namespace bookhouseAPI.Repository.AuthenticationRepository
{
    public interface IAuthRepository
    {
        Task<AuthDTO> RegisterAsync(RegisterDTO dto);
        Task<AuthDTO> LoginAsync(LoginDTO dto);
    }
}
