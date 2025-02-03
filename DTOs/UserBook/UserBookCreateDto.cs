using System.ComponentModel.DataAnnotations;

namespace MyBookList.DTOs.UserBook
{
    public class UserBookCreateDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int BookId { get; set; }
        [Required]
        public string? Status { get; set; }
        public string? StartDate { get; set; }
        public string? FinishDate { get; set; }
    }
}
