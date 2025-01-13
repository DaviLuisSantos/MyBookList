using System.ComponentModel.DataAnnotations;
namespace MyBookList.Models
{
        public class Library
        {
            [Key]
            public int LibraryId { get; set; }

            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [MaxLength(250)]
            public string? Address { get; set; }
        }
}
