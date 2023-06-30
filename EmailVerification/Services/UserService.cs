namespace EmailVerification.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<string>> RegisterAsync(UserRegisterRequest request)
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

            return "User created, now please verify";
        }

        public async Task<ActionResult<string>> LoginAsync(UserRegisterRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return "Incorrect email or password.";
            }

            if (user.VerifiedAt is null)
            {
                return "User not verified.";
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

            return "User verified.";
        }

        public async Task<ActionResult<string>> ForgotPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user is null)
            {
                return "This user does not exist.";
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();

            return "You may now reset your password.";
        }

        public async Task<ActionResult<string>> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user is null || user.ResetTokenExpires < DateTime.Now)
            {
                return "Invalid Token.";
            }

            request.NewPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
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