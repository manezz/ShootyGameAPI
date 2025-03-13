using ShootyGameAPI.Helpers;

namespace ShootyGameAPI.DTOs
{
    public class SignInResponse
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PlayerTag { get; set; }
        public int Money { get; set; }
        public Role Role { get; set; }
        public string Token { get; set; }
    }
}
