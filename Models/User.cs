using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookList.Models
{
        public class User
        {
            [Key]
            public int UserId { get; set; }

            [Required]
            [MaxLength(100)]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string PasswordHash { get; set; }

            public int LibraryId { get; set; }

            [ForeignKey("LibraryId")]
            public Library Library { get; set; }
        }
    }
