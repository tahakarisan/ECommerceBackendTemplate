using Business.Abstract.Auths;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Auths
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IAuthService _authService;
        private IUserService _userService;
        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserForLoginDto userForLoginDto)
        {
            Core.Utilities.Results.IDataResult<Core.Entities.Concrete.Auth.User> userToLogin = await _authService.LoginAsync(userForLoginDto);
            if (!userToLogin.Success)
            {
                return BadRequest(userToLogin.Message);
            }
            Core.Utilities.Results.IDataResult<Core.Utilities.Security.JWT.AccessToken> result = await _authService.CreateAccessTokenAsync(userToLogin.Data);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserForRegisterDto userForRegisterDto)
        {
            Core.Utilities.Results.IResult userExists = await _authService.IsExistAsync(userForRegisterDto.Email);
            if (!userExists.Success)
            {
                return BadRequest(userExists.Message);
            }
            Core.Utilities.Results.IDataResult<Core.Entities.Concrete.Auth.User> registerResult = await _authService.RegisterAsync(userForRegisterDto, userForRegisterDto.Password);
            Core.Utilities.Results.IDataResult<Core.Utilities.Security.JWT.AccessToken> result = await _authService.CreateAccessTokenAsync(registerResult.Data);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync(UserForUpdateDto userForUpdate)
        {
            await _authService.UpdateAsync(userForUpdate);
            Core.Utilities.Results.IDataResult<Core.Entities.Concrete.Auth.User> user = await _userService.GetAsync(q => q.Id == userForUpdate.UserId);
            Core.Utilities.Results.IDataResult<Core.Utilities.Security.JWT.AccessToken> result = await _authService.CreateAccessTokenAsync(user.Data); if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            Core.Utilities.Results.IResult result = await _authService.ChangePasswordAsync(changePasswordDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> PasswordResetAsync(PasswordResetDto passwordResetDto)
        {
            Core.Utilities.Results.IResult result = await _authService.PasswordResetAsync(passwordResetDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}

