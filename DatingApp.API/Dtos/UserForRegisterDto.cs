using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {/* to sluzy jako translacja stringow: usernname i pass na class */
        [Required]
        public string  Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "Password mmust be between 4 and 8 chars")]
        public string Password { get; set; }
    }
    
}