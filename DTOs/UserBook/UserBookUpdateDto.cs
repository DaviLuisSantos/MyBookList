namespace MyBookList.DTOs.UserBook
{
    public class UserBookUpdateDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string StartDate {  get; set; }
        public string? EndDate { get; set; }
    }
}
