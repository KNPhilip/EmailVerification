namespace EmailVerification.Models
{
    public class UserRegisterRequest
    {
        [Required, EmailAddress]
        public required string Email { get; set; } = string.Empty;
        [Required, MinLength(6)]
        public required string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public required string ConfirmPassword { get; set; } = string.Empty;
    }
}