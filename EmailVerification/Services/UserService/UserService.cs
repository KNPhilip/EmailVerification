namespace EmailVerification.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;

        public UserService(DataContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<ActionResult<string>> RegisterAsync(UserRegisterRequestDto request)
        {
            try
            {
                if (_context.Users.Any(u => u.Email == request.Email))
                {
                    return "User already exists.";
                }

                var user = new User
                {
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    VerificationToken = CreateRandomToken()
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                var mail = new EmailDto
                {
                    To = request.Email,
                    Subject = "Please verify your email",
                    Body = "<p>Hey! You've just tried to register an account - " +
                        "Please verify using this token: " +
                        $"{user.VerificationToken}\"</p>"
                };

                _emailService.SendEmail(mail);
                return "Please check your email to verify your user.";
            }
            catch (Exception e)
            {
                return $"Something went wrong: {e.Message}";
            }

        }

        public async Task<ActionResult<string>> LoginAsync(UserRegisterRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return "Incorrect email or password.";
            }

            if (user.VerifiedAt is null)
            {
                return "User not yet verified.";
            }

            return $"You logged in successfully, {request.Email}!";
        }

        public async Task<ActionResult<string>> VerifyAsync(string token)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user is null)
            {
                return "Invalid token.";
            }

            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return "User verified, you may now log in.";
        }

        public async Task<ActionResult<string>> ForgotPasswordAsync(string email)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user is null)
                {
                    return "This user does not exist.";
                }

                user.PasswordResetToken = CreateRandomToken();
                user.ResetTokenExpires = DateTime.Now.AddHours(1);
                await _context.SaveChangesAsync();

                var mail = new EmailDto
                {
                    To = email,
                    Subject = "Please verify your account",
                    Body = "<p>Hey! You've just tried to reset your password for your account - " +
                            "Please reset it using this token: " +
                            $"{user.PasswordResetToken}</p>"
                };

                _emailService.SendEmail(mail);

                return "You may now reset your password.";
            }
            catch (Exception e)
            {
                return $"Something went wrong: {e.Message}";
            }
        }

        public async Task<ActionResult<string>> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user is null || user.ResetTokenExpires < DateTime.Now)
            {
                return "Invalid Token.";
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return "Password successfully reset.";
        }

        private static string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}