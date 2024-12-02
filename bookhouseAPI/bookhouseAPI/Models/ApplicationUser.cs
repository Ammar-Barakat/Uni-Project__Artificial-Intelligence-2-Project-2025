using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing;
using System.ComponentModel.DataAnnotations;

namespace bookhouseAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required, MaxLength(100)]
        public string FirstName { get; set; }
        [Required, MaxLength(100)]
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public byte[]? ProfilePicture { get; set; }
    }
}
