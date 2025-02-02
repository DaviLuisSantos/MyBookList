using System.ComponentModel.DataAnnotations;

namespace MyBookList.Models
{
    
        public class Book
        {
            [Key]
            public int BookId { get; set; }

            [Required]
            [MaxLength(200)]
            public string Title { get; set; }

            [Required]
            [MaxLength(150)]
            public string Author { get; set; }

            [MaxLength(2000)]
            public string? Description { get; set; }

            public int? Pages { get; set; }
            public string? Genre { get; set; }
            public string? Cover { get; set; }
            public string Isbn {  get; set; }
        }
    
}
