using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using Core.Utilities.Security.JWT;
using Entities.DTOs.Users;

namespace Business.Abstract.Auths
{
    public interface IAuthService
    {
        Task<IDataResult<User>> RegisterAsync(UserForRegisterDto userForRegisterDto, string password);
        Task<IDataResult<User>> LoginAsync(UserForLoginDto userForLoginDto);
        Task<IDataResult<UserForUpdateDto>> UpdateAsync(UserForUpdateDto userForUpdate);
        Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user);
        Task<IResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<IResult> PasswordResetAsync(PasswordResetDto passwordResetDto);
        Task<IResult> SendResetCodeMailAsync(string email);
        Task<IResult> IsExistAsync(string email);
    }
}