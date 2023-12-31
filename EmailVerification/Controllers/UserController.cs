﻿using EmailVerification.Dtos;
using EmailVerification.Services.UserService;

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
        public async Task<ActionResult<string>> Register(UserRegisterRequestDto request)
        {
            return await _userService.RegisterAsync(request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserRegisterRequestDto request)
        {
            return await _userService.LoginAsync(request);
        }

        [HttpPost("verify/{token}")]
        public async Task<ActionResult<string>> Verify(string token)
        {
            return await _userService.VerifyAsync(token);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<string>> ForgotPassword(string email)
        {
            return await _userService.ForgotPasswordAsync(email);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPassword(ResetPasswordRequestDto request)
        {
            return await _userService.ResetPasswordAsync(request);
        }
    }
}