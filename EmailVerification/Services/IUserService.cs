namespace EmailVerification.Services
{
    public interface IUserService
    {
        Task<ActionResult<string>> RegisterAsync(UserRegisterRequest request);
    }
}