namespace EmailVerification.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterRequest request)
        {
            
        }
    }
}