using EmailVerification.Dtos;

namespace EmailVerification.Services.UserService
{
    public interface IUserService
    {
        Task<ActionResult<string>> RegisterAsync(UserRegisterRequestDto request);
        Task<ActionResult<string>> LoginAsync(UserRegisterRequestDto request);
        Task<ActionResult<string>> VerifyAsync(string token);
        Task<ActionResult<string>> ForgotPasswordAsync(string email);
        Task<ActionResult<string>> ResetPasswordAsync(ResetPasswordRequestDto request);
    }
}