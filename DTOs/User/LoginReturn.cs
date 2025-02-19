namespace MyBookList.DTOs.User
{
    public class LoginReturn
    {
        public Guid Id { get; set; }
        public string token { get; set; }
        public string Username { get; set; }
    }
}
