using System.ComponentModel.DataAnnotations;

namespace MyBookList.DTOs.Book;

public class BookCreateDto
{
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
