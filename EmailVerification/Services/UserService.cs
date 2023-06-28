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
                VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64))
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "User successfully created!";
        }
    }
}