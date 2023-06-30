namespace EmailVerification.Dtos
{
    public class UserRegisterRequestDto
    {
        [Required, EmailAddress]
        public required string Email { get; set; } = string.Empty;
        [Required, MinLength(6, ErrorMessage = "The password needs to be at least 6 characters.")]
        public required string Password { get; set; } = string.Empty;
        [Required, Compare("Password")]
        public required string ConfirmPassword { get; set; } = string.Empty;
    }
}