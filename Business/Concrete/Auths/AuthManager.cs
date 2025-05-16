using Business.Abstract.Auths;
using Business.Constants;
using Business.Utilities.Mail;
using Core.Entities.Concrete.Auth;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Users;

namespace Business.Concrete.Auths
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;
        private IResetPasswordCodeService _resetPasswordCodeService;
        private IMailService _mailService;
        private IBasketDal _basketDal;
        public AuthManager(IUserService userService, ITokenHelper tokenHelper, IResetPasswordCodeService resetPasswordCodeService, IMailService mailService, IBasketDal basketDal)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _resetPasswordCodeService = resetPasswordCodeService;
            _mailService = mailService;
            _basketDal = basketDal;
        }
        public async Task<IDataResult<User>> RegisterAsync(UserForRegisterDto userForRegisterDto, string password)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            User user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            IResult result = await _userService.AddAsync(user);
            if (result.Success)
            {
                await _basketDal.AddAsync(new Basket { UserId = user.Id });
            }
            return result.Success ? new SuccessDataResult<User>(user, Messages.UserRegistered) : new ErrorDataResult<User>(user, Messages.UserNotRegistered);
        }
        public async Task<IDataResult<User>> LoginAsync(UserForLoginDto userForLoginDto)
        {
            User userToCheck = await _userService.GetByMailAsync(userForLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck, true, Messages.SuccessfulLogin);
        }
        public async Task<IDataResult<UserForUpdateDto>> UpdateAsync(UserForUpdateDto userForUpdate)
        {
            bool isExists = await _userService.IsExistAsync(q => q.Id == userForUpdate.UserId);
            if (!isExists)
            {
                return new ErrorDataResult<UserForUpdateDto>(userForUpdate, "Kullanıcı Güncellenemedi!");
            }

            User user = new User
            {
                Id = userForUpdate.UserId,
                Email = userForUpdate.Email,
                FirstName = userForUpdate.FirstName,
                LastName = userForUpdate.LastName,
            };

            IResult result = await _userService.UpdateAsync(user);

            return result.Success ? new SuccessDataResult<UserForUpdateDto>(userForUpdate, "Kullanıcı Güncellendi.") : new ErrorDataResult<UserForUpdateDto>(userForUpdate, "Kullanıcı Güncellenemedi!");
        }
        public async Task<IDataResult<AccessToken>> CreateAccessTokenAsync(User user)
        {
            List<OperationClaim> claims = await _userService.GetClaimsAsync(user);
            AccessToken accessToken = _tokenHelper.CreateToken(user, claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
        }
        public async Task<IResult> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            byte[] passwordHash, passwordSalt;
            User userToCheck = (await _userService.GetAsync(q => q.Id == changePasswordDto.UserId)).Data;
            if (userToCheck == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            if (!HashingHelper.VerifyPasswordHash(changePasswordDto.OldPassword, userToCheck.PasswordHash, userToCheck.PasswordSalt))
            {
                return new ErrorResult(Messages.PasswordError);
            }
            HashingHelper.CreatePasswordHash(changePasswordDto.NewPassword, out passwordHash, out passwordSalt);
            userToCheck.PasswordHash = passwordHash;
            userToCheck.PasswordSalt = passwordSalt;
            IResult result = await _userService.UpdateAsync(userToCheck);
            return result.Success ? new SuccessResult("Parola Değişti.") : new ErrorResult("Parola Değiştirilemedi!");
        }
        public async Task<IResult> PasswordResetAsync(PasswordResetDto passwordResetDto)
        {
            await _resetPasswordCodeService.ConfirmResetCodeAsync(passwordResetDto.Code);
            IDataResult<ResetPasswordCode> resetPassword = await _resetPasswordCodeService.GetByCodeAsync(passwordResetDto.Code);
            resetPassword.Data.IsActive = false;

            byte[] passwordHash, passwordSalt;
            User userToCheck = (await _userService.GetAsync(q => q.Id == passwordResetDto.UserId)).Data;
            if (userToCheck == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            HashingHelper.CreatePasswordHash(passwordResetDto.NewPassword, out passwordHash, out passwordSalt);
            userToCheck.PasswordHash = passwordHash;
            userToCheck.PasswordSalt = passwordSalt;
            IResult result = await _userService.UpdateAsync(userToCheck);
            await _resetPasswordCodeService.UpdateAsync(resetPassword.Data);
            return result.Success ? new SuccessResult("Parola Değişti.") : new ErrorResult("Parola Değiştirilemedi!");
        }
        public async Task<IResult> SendResetCodeMailAsync(string email)
        {
            string code = Guid.NewGuid().ToString();

            User user = (await _userService.GetUserByEmailAsync(email)).Data;

            if (user == null) { return new ErrorResult("Kullanıcı Bulunamadı!"); }

            ResetPasswordCode resetPasswordCode = new ResetPasswordCode
            {
                Code = code,
                IsActive = true,
                UserEmail = email,
                CreatedAt = DateTime.Now,
                EndDate = DateTime.Now.AddHours(3),
                UserId = user.Id
            };
            if ((await _resetPasswordCodeService.AddAsync(resetPasswordCode)).Success)
            {
                IResult result = await _mailService.SendPasswordResetMailAsync(resetPasswordCode);
                if (result.Success) { return new SuccessResult("Mail Gönderilmiştir."); }
            };
            return new ErrorResult("Bir hata oluştu");
        }
        public async Task<IResult> IsExistAsync(string email)
        {
            bool result = await _userService.IsExistAsync(q => q.Email == email);
            return result ? new SuccessResult("Kullanıcı Bulundu.") : new ErrorResult("Kullanıcı Bulunamadı!");
        }
    }
}