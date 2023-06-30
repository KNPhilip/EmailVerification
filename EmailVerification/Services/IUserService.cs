namespace EmailVerification.Services
{
    public interface IUserService
    {
        Task<ActionResult<string>> RegisterAsync(UserRegisterRequest request);
        Task<ActionResult<string>> LoginAsync(UserRegisterRequest request);
        Task<ActionResult<string>> VerifyAsync(string token);
        Task<ActionResult<string>> ForgotPasswordAsync(string email);
    }
}