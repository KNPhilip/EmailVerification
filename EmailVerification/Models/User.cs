﻿namespace EmailVerification.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } = string.Empty;
        public string? VerificationToken { get; set; } = string.Empty;
        public DateTime? VerifiedAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}