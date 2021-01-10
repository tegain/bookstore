using System.ComponentModel.DataAnnotations;

namespace BookStore_API.Dto
{
    public class UserDto
    {
        
    }

    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}