using System.ComponentModel.DataAnnotations;

namespace MyBookList.DTOs.UserBook
{
    public class UserBookCreateDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string? Status { get; set; }
        public string? StartDate { get; set; }
        public string? FinishDate { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }

        public string? Description { get; set; }
        public int? Pages { get; set; }
        public string? Genre { get; set; }
        public string Cover { get; set; }
        public string Isbn { get; set; }
    }
}
