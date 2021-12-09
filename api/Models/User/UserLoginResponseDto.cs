namespace api.Models.User
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; }
        
        public string RefreshToken { get; set; }
    }
}