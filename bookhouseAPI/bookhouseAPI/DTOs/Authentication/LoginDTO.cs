using System.ComponentModel.DataAnnotations;

namespace bookhouseAPI.DTOs.Authentication
{
    public class LoginDTO
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
