using System.ComponentModel.DataAnnotations;

namespace MyBookList.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(100)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
