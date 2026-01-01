namespace SchoolManagementSystem.Application.DTOs.Account
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}