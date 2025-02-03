using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookList.Models
{
        public class UserBook
        {
            [Key]
            public int UserBookId { get; set; }

            public int UserId { get; set; }

            [ForeignKey("UserId")]
            public User User { get; set; }
            public int BookId { get; set; }

            [ForeignKey("BookId")]
            public Book Book { get; set; }

            public DateOnly? DateStarted { get; set; }

            public DateOnly? DateFinished { get; set; }

            public string? Status { get; set; }
        }
    }