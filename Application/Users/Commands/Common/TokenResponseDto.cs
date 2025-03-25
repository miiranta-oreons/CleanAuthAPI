namespace Application.Users.Commands.Common
{
    public class TokenResponseDto
    {
        public required string AccessToken { get; set; }

        public string RefreshToken { get; set; }
    }
}
