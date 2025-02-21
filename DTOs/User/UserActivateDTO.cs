namespace MyBookList.DTOs.User
{
    public class UserActivateDTO
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
    }
}
