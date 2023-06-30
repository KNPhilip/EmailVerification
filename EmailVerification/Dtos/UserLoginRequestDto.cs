namespace EmailVerification.Dtos
{
    public class UserLoginRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; } = string.Empty;
        [Required]
        public required string Password { get; set; } = string.Empty;
    }
}