namespace Investo.Api.Models
{
    public class AuthResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}