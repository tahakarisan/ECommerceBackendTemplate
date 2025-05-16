using Business.Abstract.Auths;
using Core.Entities.Concrete.Auth;
using Entities.DTOs.Users;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Auths
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private IAuthService _authService;
        private IResetPasswordCodeService _resetPasswordCodeService;
        public UsersController(IUserService userService, IResetPasswordCodeService resetPasswordCodeService, IAuthService authService = null)
        {
            _userService = userService;
            _resetPasswordCodeService = resetPasswordCodeService;
            _authService = authService;
        }
        [HttpPost("add-user")]
        public async Task<IActionResult> AddUserAsync(User user)
        {
            Core.Utilities.Results.IResult result = await _userService.AddAsync(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("update-user")]
        public async Task<IActionResult> UpdateUserAsync(User user)
        {
            Core.Utilities.Results.IResult result = await _userService.UpdateAsync(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("delete-user")]
        public async Task<IActionResult> DeleteUserAsync(User user)
        {
            Core.Utilities.Results.IResult result = await _userService.DeleteAsync(user);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPut("update-infos")]
        public async Task<IActionResult> UpdateInfosAsync(User user)
        {
            Core.Utilities.Results.IResult result = await _userService.UpdateInfosAsync(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<User> result = await _userService.GetAsync(q => q.Id == id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("get-user-by-email")]
        public async Task<IActionResult> GetByEmailAsync(string email)
        {
            Core.Utilities.Results.IDataResult<User> result = await _userService.GetUserByEmailAsync(email);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("send-password-reset-mail")]
        public async Task<IActionResult> SendEmailAsync(string email)
        {
            Core.Utilities.Results.IResult result = await _authService.SendResetCodeMailAsync(email);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("confirm-password-reset-code")]
        public async Task<IActionResult> ConfirmPasswordResetCodeAsync(ConfirmPasswordResetDto confirmPasswordResetDto)
        {
            Core.Utilities.Results.IResult result = await _resetPasswordCodeService.ConfirmResetCodeWithUserIdAsync(confirmPasswordResetDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}