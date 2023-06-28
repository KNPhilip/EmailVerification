namespace EmailVerification.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
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
                VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64))
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User successfully created!";
        }
    }
}