namespace EmailVerification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterRequest request)
        {
            return await _userService.RegisterAsync(request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserRegisterRequest request)
        {
            return await _userService.LoginAsync(request);
        }

        [HttpPost("verify")]
        public async Task<ActionResult<string>> Verify(string token)
        {
            return await _userService.VerifyAsync(token);
        }
    }
}